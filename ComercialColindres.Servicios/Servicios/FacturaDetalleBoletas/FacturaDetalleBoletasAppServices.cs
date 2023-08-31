using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalle;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Datos.Recursos;
using ServidorCore.Aplicacion;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Datos.Entorno.DataCore.Setting;

namespace ComercialColindres.Servicios.Servicios
{
    public class FacturaDetalleBoletasAppServices : IFacturaDetalleBoletasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        readonly IFacturaDetalleBoletaDomainServices _facturaDetalleBoletaDomainServices;

        public FacturaDetalleBoletasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                                IFacturaDetalleBoletaDomainServices facturaDetalleBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _facturaDetalleBoletaDomainServices = facturaDetalleBoletaDomainServices;
        }

        public List<FacturaDetalleBoletasDTO> Put(PutValidarDetalleBoletasMasivo request)
        {
            var listaFacturaDetalleBoleta = new List<FacturaDetalleBoletasDTO>();
            Factura factura = _unidadDeTrabajo.Facturas.Find(request.FacturaId);

            if (factura == null)
            {
                listaFacturaDetalleBoleta.Add(new FacturaDetalleBoletasDTO
                {
                    MensajeError = "FacturaId NO Existe!"
                });

                return listaFacturaDetalleBoleta;
            }

            bool invoiceRequiredShipmentNumber = IsShippingNumberRequired(factura.PlantaId);
            var listadoBoletas = _unidadDeTrabajo.Boletas.Where(b => b.PlantaId == factura.PlantaId).ToList();

            foreach (var boleta in request.DetalleBoletas)
            {
                var datoBoleta = invoiceRequiredShipmentNumber
                                 ? listadoBoletas.FirstOrDefault(b => b.NumeroEnvio == boleta.NumeroEnvio)
                                 : listadoBoletas.FirstOrDefault(b => b.CodigoBoleta == boleta.CodigoBoleta);
                var mensajeValidacion = ValidarBoleta(datoBoleta, factura);

                if (!string.IsNullOrWhiteSpace(mensajeValidacion))
                {
                    var validacion = new FacturaDetalleBoletasDTO
                    {
                        CodigoBoleta = boleta.CodigoBoleta,
                        MensajeError = mensajeValidacion
                    };

                    listaFacturaDetalleBoleta.Add(validacion);
                }
                else
                {
                    var nuevaBoleta = new FacturaDetalleBoletasDTO
                    {
                        BoletaId = datoBoleta.BoletaId,
                        CodigoBoleta = datoBoleta.CodigoBoleta,
                        PrecioVenta = boleta.PrecioVenta
                    };

                    listaFacturaDetalleBoleta.Add(nuevaBoleta);
                }
            }

            return listaFacturaDetalleBoleta;
        }

        private string ValidarBoleta(Boletas datoBoleta, Factura factura)
        {
            var mensajeValidacion = string.Empty;

            if (datoBoleta == null) return string.Empty;

            if (datoBoleta.PlantaId != factura.PlantaId)
            {
                return mensajeValidacion = string.Format("Planta incorrecta, {0}", datoBoleta.ClientePlanta.NombrePlanta);
            }

            if (datoBoleta.FacturaDetalleBoletas.Any(f => f.FacturaId != factura.FacturaId))
            {
                return mensajeValidacion = "Ya existe en Factura";
            }

            return mensajeValidacion;    
        }

        public List<FacturaDetalleBoletasDTO> Get(GetDetalleBoletasPorFacturaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.FacturaDetalleBoletas, "GetBoletasPorFacturaId", request.FacturaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<FacturaDetalleBoletas> datos = _unidadDeTrabajo.FacturaDetalleBoletas.Where(d => d.FacturaId == request.FacturaId)
                                                                            .OrderByDescending(o => o.Boleta.CategoriaProductoId).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<FacturaDetalleBoletas>, IEnumerable<FacturaDetalleBoletasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }


        public List<FacturaDetalleBoletasDTO> SaveInvoiceDetailBoletas(PostDetalleBoletas request)
        {
            var factura = _unidadDeTrabajo.Facturas.Find(request.FacturaId);
            List<FacturaDetalleBoletasDTO> errorMessages = new List<FacturaDetalleBoletasDTO>();

            if (factura == null)
            {
                errorMessages.Add(CreateErrorMessage("FacturaId NO EXISTE!", string.Empty, string.Empty));
                return errorMessages;
            }

            List<FacturaDetalleBoletas> boletasDetalle = factura.FacturaDetalleBoletas.ToList();
                        
            if (!_facturaDetalleBoletaDomainServices.CanAssignBoletasDetail(factura, out string errorMessage))
            {
                errorMessages.Add(CreateErrorMessage(errorMessage, string.Empty, string.Empty));
                return errorMessages;
            }
                        
            bool invoiceRequiredShipmentNumber = IsShippingNumberRequired(factura.PlantaId);

            //Verificar si removieron Items
            foreach (var itemBoleta in boletasDetalle)
            {
                var boleta = invoiceRequiredShipmentNumber
                             ? request.DetalleBoletas.FirstOrDefault(c => c.NumeroEnvio == itemBoleta.NumeroEnvio)
                             : request.DetalleBoletas.FirstOrDefault(c => c.CodigoBoleta == itemBoleta.CodigoBoleta);

                if (boleta == null)
                {
                    factura.FacturaDetalleBoletas.Remove(itemBoleta);
                }
            }

            List<Boletas> boletas = _unidadDeTrabajo.Boletas.Where(b => b.PlantaId == factura.PlantaId).ToList();
            List<FacturaDetalleBoletas> invoiceDetailBoletas = _unidadDeTrabajo.FacturaDetalleBoletas.Where(p => p.PlantaId == factura.PlantaId).ToList();

            foreach (FacturaDetalleBoletasDTO boletaDetailRequest in request.DetalleBoletas)
            {
                if (invoiceRequiredShipmentNumber)
                {
                    if (string.IsNullOrWhiteSpace(boletaDetailRequest.NumeroEnvio))
                    {
                        errorMessages.Add(CreateErrorMessage("#Envío Requerido", boletaDetailRequest.CodigoBoleta, boletaDetailRequest.NumeroEnvio));
                        continue;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(boletaDetailRequest.CodigoBoleta))
                    {
                        errorMessages.Add(CreateErrorMessage("CodigoBoleta Requerido", boletaDetailRequest.CodigoBoleta, boletaDetailRequest.NumeroEnvio));
                        continue;
                    }
                }

                string validation = string.Empty;
                if (IsBoletaAssignedToInvoice(invoiceDetailBoletas, boletaDetailRequest, factura, invoiceRequiredShipmentNumber, out validation))
                {
                    errorMessages.Add(CreateErrorMessage(validation, boletaDetailRequest.CodigoBoleta, boletaDetailRequest.NumeroEnvio));
                    continue;
                }

                bool isBoletaAssignedToThisInvoce = invoiceRequiredShipmentNumber
                                                    ? invoiceDetailBoletas.Any(b => b.NumeroEnvio == boletaDetailRequest.NumeroEnvio)
                                                    : invoiceDetailBoletas.Any(b => b.CodigoBoleta == boletaDetailRequest.CodigoBoleta);

                if (!isBoletaAssignedToThisInvoce)
                {
                    Boletas boleta = invoiceRequiredShipmentNumber
                                     ? boletas.FirstOrDefault(b => b.NumeroEnvio == boletaDetailRequest.NumeroEnvio)
                                     : boletas.FirstOrDefault(b => b.CodigoBoleta == boletaDetailRequest.CodigoBoleta);
                    
                    FacturaDetalleBoletas newBoletaDetail;
                    if (boleta != null)
                    {
                        if (boleta.GetFacturaIdAssigned() != 0) continue;
                        newBoletaDetail = new FacturaDetalleBoletas(factura, boleta.BoletaId, boletaDetailRequest.NumeroEnvio, boletaDetailRequest.CodigoBoleta, boletaDetailRequest.PesoProducto, boletaDetailRequest.FechaIngreso, boletaDetailRequest.UnitPrice);
                    }
                    else
                    {
                        newBoletaDetail = new FacturaDetalleBoletas(factura, null, boletaDetailRequest.NumeroEnvio, boletaDetailRequest.CodigoBoleta, boletaDetailRequest.PesoProducto, boletaDetailRequest.FechaIngreso, boletaDetailRequest.UnitPrice);
                    }

                    IEnumerable<string> validations = newBoletaDetail.GetValidationErrors();

                    if (validations.Any())
                    {
                        errorMessages.Add(CreateErrorMessage(Utilitarios.CrearMensajeValidacion(validations), newBoletaDetail.CodigoBoleta, newBoletaDetail.NumeroEnvio));
                        continue;
                    }

                    factura.AddInvoiceDetailBoleta(newBoletaDetail);
                }
            }
                       
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateInvoiceDetailBoletas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheFacturaDetalleBoletas();

            return errorMessages;
        }

        private bool IsBoletaAssignedToInvoice(List<FacturaDetalleBoletas> invoiceDetailBoletas, FacturaDetalleBoletasDTO boletaDetailRequest, Factura invoice, bool invoiceRequiredShipmentNumber, out string validation)
        {
            FacturaDetalleBoletas boletaDetail = invoiceRequiredShipmentNumber
                                                 ? invoiceDetailBoletas.FirstOrDefault(b => b.NumeroEnvio == boletaDetailRequest.NumeroEnvio && b.FacturaId != invoice.FacturaId)
                                                 : invoiceDetailBoletas.FirstOrDefault(b => b.CodigoBoleta == boletaDetailRequest.CodigoBoleta && b.FacturaId != invoice.FacturaId);

            if (boletaDetail != null)
            {
                validation = invoiceRequiredShipmentNumber
                             ? $"#Envio ya existe en Factura {boletaDetail.Factura.NumeroFactura}"
                             : $"CodigoBoleta ya existe en Factura {boletaDetail.Factura.NumeroFactura}";
                return true;
            }

            validation = string.Empty;
            return false;
        }

        private FacturaDetalleBoletasDTO CreateErrorMessage(string message, string codigoBoleta, string numeroEnvio)
        {
            return new FacturaDetalleBoletasDTO
            {
                CodigoBoleta = string.IsNullOrWhiteSpace(codigoBoleta) ? "N/A" : codigoBoleta,
                NumeroEnvio = string.IsNullOrWhiteSpace(numeroEnvio) ? "N/A" : numeroEnvio,
                ValidationErrorMessage = message
            };
        }

        private bool IsShippingNumberRequired(int plantaId)
        {
            string result = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("NumeroEnvio", plantaId.ToString(), string.Empty);

            return !string.IsNullOrWhiteSpace(result) ? result == "1" ? true : false : false;
        }

        private void RemoverCacheFacturaDetalleBoletas()
        {
            var listaKey = new List<string>
            {
                KeyCache.Facturas,
                KeyCache.FacturaDetalleBoletas,
                KeyCache.OrdenescompraProducto,
                KeyCache.OrdenescompraDetalleBoleta
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
