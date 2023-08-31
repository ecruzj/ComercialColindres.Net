using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Helpers;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.ReglasNegocio.DomainServices.Invoice;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using ServidorCore.EntornoDatos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class FacturasAppServices : IFacturasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        readonly IInvoiceDomainServices _invoiceDomainServices;

        public FacturasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, InvoiceDomainServices invoiceDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _invoiceDomainServices = invoiceDomainServices;
        }

        public ActualizarResponseDTO Put(PutFacturaAnular request)
        {
            var factura = _unidadDeTrabajo.Facturas.Find(request.FacturaId);
            if (factura == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "FacturaId NO Existe"
                };
            }
            
            var mensajesValidacionEliminar = factura.Anular();
            if (!string.IsNullOrWhiteSpace(mensajesValidacionEliminar))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = mensajesValidacionEliminar
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "AnularFactura");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }
                        
        public BusquedaFacturasDTO Get(GetByValorFacturas request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Facturas, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = FacturasEspecificaciones.Filtrar(request.Filtro);
                List<Factura> datos = _unidadDeTrabajo.Facturas.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Factura>, IEnumerable<FacturasDTO>>(datosPaginados.Items as IEnumerable<Factura>);

                var dto = new BusquedaFacturasDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<FacturasDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public FacturasDTO Get(GetFactura request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Facturas, "GetFactura", request.FacturaId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var dato = _unidadDeTrabajo.Facturas.Find(request.FacturaId);
                if (dato == null)
                {
                    return new FacturasDTO
                    {
                        MensajeError = "FacturaId NO existe"
                    };
                }
                var dto = AutomapperTypeAdapter.ProyectarComo<Factura, FacturasDTO>(dato);
                return dto;
            });
            return retorno;
        }

        public ActualizarResponseDTO Post(PostFactura request)
        {
            var nuevoDato = _unidadDeTrabajo.Facturas.Find(request.Factura.FacturaId);

            if (nuevoDato != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "FacturaId Existe"
                };
            }

            var existeNumeroFactura = _unidadDeTrabajo.Facturas.FirstOrDefault(r => r.NumeroFactura == request.Factura.NumeroFactura);
            if (existeNumeroFactura != null) { return new ActualizarResponseDTO { MensajeError = "El # Factura YA Existe, no se puede agregar nuevamente" }; }

            if (request.Factura.RequiereOrdenCompra)
            {
                bool existProductOrder = _unidadDeTrabajo.Facturas.Any(p => p.OrdenCompra == request.Factura.OrdenCompra && p.Estado != Estados.ANULADO);
                if (existProductOrder) { return new ActualizarResponseDTO { MensajeError = "#Orden de Compra ya existe en otra Factura" }; }
            }

            if (request.Factura.IsExonerated)
            {
                bool existExoneration = _unidadDeTrabajo.Facturas.Any(e => e.ExonerationNo == request.Factura.ExonerationNo);
                if (existExoneration) { return new ActualizarResponseDTO { MensajeError = "#Exoneración ya existe en otra Factura" }; }
            }

            var facturaCategoria = _unidadDeTrabajo.FacturasCategorias.Find(request.Factura.FacturaCategoriaId);

            if (facturaCategoria == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "FacturaCategoriaId NO existe"
                };
            }

            ClientePlantas planta = _unidadDeTrabajo.ClientePlantas.Find(request.Factura.PlantaId);
            string proformaNo = planta.RequiresProForm
                                ? CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.RequestUserInfo.SucursalId, "PF", "PF" + DateTime.Now.ToString("yy") + "-", false)
                                : string.Empty;

            nuevoDato = new Factura(request.Factura.SucursalId, facturaCategoria, planta, request.Factura.SubPlantaId, request.Factura.OrdenCompra, request.Factura.Semana,
                                    request.Factura.NumeroFactura, proformaNo, request.Factura.Fecha, request.Factura.Total, request.Factura.ExonerationNo,
                                    request.Factura.TaxPercent, request.Factura.IsForeignCurrency, request.Factura.LocalCurrencyAmount, request.Factura.Observaciones, request.Factura.HasUnitPriceItem);

            var mensajesValidacion = nuevoDato.GetValidationErrors();

            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            nuevoDato.ProFormaNo = planta.RequiresProForm
                                ? CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.RequestUserInfo.SucursalId, "PF", "PF" + DateTime.Now.ToString("yy") + "-", true)
                                : string.Empty;

            _unidadDeTrabajo.Facturas.Add(nuevoDato);

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateInvoice");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }
        
        public FacturasDTO UpdateInvoice(UpdateInfoInvoice request)
        {
            Factura invoice = _unidadDeTrabajo.Facturas.Find(request.Factura.FacturaId);
            FacturasDTO invoiceRequest = request.Factura;

            if (invoice == null) return new FacturasDTO { MensajeError = "FacturaId no existe" };

            List<Factura> invoices = _unidadDeTrabajo.Facturas.ToList();
            ClientePlantas factory = _unidadDeTrabajo.ClientePlantas.Find(invoiceRequest.PlantaId);
            SubPlanta subFactory = _unidadDeTrabajo.SubPlantas.FirstOrDefault(s => s.SubPlantaId == invoiceRequest.SubPlantaId);

            if (!_invoiceDomainServices.CanUpdateInvoice(invoice, invoices, invoiceRequest.NumeroFactura, factory, subFactory, out string errorMessage))
            {
                return new FacturasDTO { MensajeError = errorMessage };
            }

            if (!_invoiceDomainServices.IsAblePurchaseOrder(invoiceRequest.OrdenCompra, invoice, invoices, factory, out errorMessage))
            {
                return new FacturasDTO { MensajeError = errorMessage };
            }

            if (!_invoiceDomainServices.IsAbleExemptionNo(invoiceRequest.ExonerationNo, invoice, invoices, factory, out errorMessage))
            {
                return new FacturasDTO { MensajeError = errorMessage };
            }

            string proformaNo = string.Empty;
            bool isTempProform = false;
            if (string.IsNullOrWhiteSpace(invoice.ProFormaNo) && factory.RequiresProForm)
            {
                invoiceRequest.ProFormaNo = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.RequestUserInfo.SucursalId, "PF", "PF" + DateTime.Now.ToString("yy") + "-", false);
                isTempProform = true;
            }

            invoice.UpdateInfo(factory, subFactory, invoiceRequest.OrdenCompra, invoiceRequest.Semana, invoiceRequest.NumeroFactura, invoiceRequest.ProFormaNo, invoiceRequest.Fecha, invoiceRequest.Total,
                               invoiceRequest.ExonerationNo, invoiceRequest.TaxPercent, invoiceRequest.IsForeignCurrency, invoiceRequest.LocalCurrencyAmount, invoiceRequest.Observaciones, invoiceRequest.HasUnitPriceItem);

            IEnumerable<string> validations = invoice.GetValidationErrors();
            if (validations.Any())
            {
                return new FacturasDTO { MensajeError = validations.FirstOrDefault() };
            }

            if (factory.RequiresProForm && isTempProform)
            {
                invoice.ProFormaNo = factory.RequiresProForm
                                    ? CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.RequestUserInfo.SucursalId, "PF", "PF" + DateTime.Now.ToString("yy") + "-", true)
                                    : string.Empty;
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "UpdateInvoiceInfo");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new FacturasDTO();
        }

        public FacturasDTO ActiveInvoice(ActiveInvoiceById request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Factura invoice = _unidadDeTrabajo.Facturas.Find(request.InvoiceId);

            if (invoice == null)
            {
                return new FacturasDTO { ValidationErrorMessage = "FacturaId NO EXISTE!" };
            }

            if (invoice.Estado != Estados.NUEVO)
            {
                return new FacturasDTO { ValidationErrorMessage = "El estado debe ser NUEVO!" };
            }

            if (!invoice.TryActiveInvoice(out string errorMessage))
            {
                return new FacturasDTO { ValidationErrorMessage = errorMessage };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "ActiveInvoice");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new FacturasDTO();
        }

        public EliminarResponseDTO Delete(DeleteFactura request)
        {
            var dato = _unidadDeTrabajo.Facturas.Find(request.FacturaId);
            if (dato == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "FacturaId No Existe"
                };
            }

            var mensajesValidacionEliminar = dato.GetValidationErrorsDelete();

            if (mensajesValidacionEliminar.Any())
            {
                return new EliminarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacionEliminar)
                };
            }

            _unidadDeTrabajo.Facturas.Remove(dato);
            _unidadDeTrabajo.SaveChanges();
            
            RemoverCache();

            return new EliminarResponseDTO();
        }

        void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Facturas,
                KeyCache.FacturaDetalleBoletas,
                KeyCache.FacturaDetalleItems
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
