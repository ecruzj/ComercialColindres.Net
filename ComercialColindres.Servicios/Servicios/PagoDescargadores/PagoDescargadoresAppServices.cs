using System;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Helpers;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ServidorCore.EntornoDatos;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios
{
    public class PagoDescargadoresAppServices : IPagoDescargadoresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        private const string _codigoCorrelativo_PagoDescarga = "PD";
        readonly IDescargadoresDomainServices _descargadoresDomainServices;

        public PagoDescargadoresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                            IDescargadoresDomainServices descargadoresDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _descargadoresDomainServices = descargadoresDomainServices;
        }

        public List<PagoDescargadoresDTO> Post(PostPagosDescargas request)
        {
            List<PagoDescargadoresDTO> pagoDescargadoresDto = new List<PagoDescargadoresDTO>();

            var pagoDescargadores = _unidadDeTrabajo.PagoDescargadores.Find(request.PagoDescargas.PagoDescargaId);

            if (pagoDescargadores != null)
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = "N/A", NumeroEnvio = "N/A", ValidationErrorMessage = "PagoPrestamoId YA Existe" };
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            var cuadrilla = _unidadDeTrabajo.Cuadrillas.Find(request.PagoDescargas.CuadrillaId);

            if (cuadrilla == null)
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = "N/A", NumeroEnvio = "N/A", ValidationErrorMessage = "CuadrillaId NO Existe" };
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            bool isOpenDescargadoresPayment = _unidadDeTrabajo.PagoDescargadores.Any(d => d.Cuadrilla.PlantaId == cuadrilla.PlantaId && d.Estado != Estados.CERRADO);

            if (isOpenDescargadoresPayment)
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = "N/A", NumeroEnvio = "N/A", ValidationErrorMessage = "Existe un Pago de Descargas en Proceso, debe Cerrarlo!" };
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            pagoDescargadores = new PagoDescargadores("??", cuadrilla.CuadrillaId, request.RequestUserInfo.UserId, request.PagoDescargas.FechaPago);

            List<string> mensajesValidacion = pagoDescargadores.GetValidationErrors().ToList();
            if (mensajesValidacion.Any())
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = "N/A", NumeroEnvio = "N/A",  ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(mensajesValidacion)};
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            pagoDescargadores.CodigoPagoDescarga = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.RequestUserInfo.SucursalId, _codigoCorrelativo_PagoDescarga, string.Empty, true);
            _unidadDeTrabajo.PagoDescargadores.Add(pagoDescargadores);

            pagoDescargadoresDto = AssignDescargasToPayment(request.Descargas, pagoDescargadores, cuadrilla);

            if (!pagoDescargadoresDto.Any())
            {
                var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "PagoDescargas");
                _unidadDeTrabajo.Commit(transaccion);

                RemoverCachePagoDescargas();
            }

            return pagoDescargadoresDto;
        }

        private List<PagoDescargadoresDTO> AssignDescargasToPayment(List<DescargadoresDTO> descargasRequest, PagoDescargadores pagoDescargadores, Cuadrillas cuadrilla)
        {
            List<PagoDescargadoresDTO> pagoDescargadoresDto = new List<PagoDescargadoresDTO>();

            bool isShippingNumberRequired = cuadrilla.ClientePlanta.IsShippingNumberRequired();
            List<string> descargas = descargasRequest.Select(n => isShippingNumberRequired ? n.NumeroEnvio : n.CodigoBoleta).ToList();
            List<Boletas> descargaBoletas = _unidadDeTrabajo.Boletas.Where(d => d.PlantaId == cuadrilla.ClientePlanta.PlantaId 
                                                                           && descargas.Contains(isShippingNumberRequired ? d.NumeroEnvio : d.CodigoBoleta)).ToList();
            List<DescargasPorAdelantado> descargasPorAdelanto = _unidadDeTrabajo.DescargasPorAdelantado.Where(p => p.PlantaId == cuadrilla.ClientePlanta.PlantaId).ToList();
            List<AjusteTipo> tipoAjustes = _unidadDeTrabajo.AjusteTipos.ToList();
            AjusteTipo ajustePorDescarga = tipoAjustes.FirstOrDefault(d => d.Descripcion == "Descarga Pendiente");

            string numeroEnvio = string.Empty;
            string codigoBoleta = string.Empty;

            foreach (DescargadoresDTO descargaDto in descargasRequest)
            {
                numeroEnvio = isShippingNumberRequired ? descargaDto.NumeroEnvio : "N/A";
                codigoBoleta = !isShippingNumberRequired ? descargaDto.CodigoBoleta : "N/A";

                Boletas boleta = descargaBoletas.FirstOrDefault(d => isShippingNumberRequired
                                                                                ? d.NumeroEnvio == descargaDto.NumeroEnvio
                                                                                : d.CodigoBoleta == descargaDto.CodigoBoleta);
                DescargasPorAdelantado descargaPorAdelanto = descargasPorAdelanto.FirstOrDefault(d => isShippingNumberRequired
                                                                                ? d.NumeroEnvio == descargaDto.NumeroEnvio
                                                                                : d.CodigoBoleta == descargaDto.CodigoBoleta);

                if (!_descargadoresDomainServices.TryAssigneDescargaToPay(pagoDescargadores, boleta, ajustePorDescarga, descargaPorAdelanto, descargaDto.NumeroEnvio, descargaDto.CodigoBoleta, descargaDto.PagoDescarga, out string errorMessage))
                {
                    PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = codigoBoleta, NumeroEnvio = numeroEnvio, ValidationErrorMessage = errorMessage };
                    pagoDescargadoresDto.Add(pagoDescargador);
                }
            }

            return pagoDescargadoresDto;
        }

        public PagoDescargadoresDTO Get(GetPagoDescargadoresUltimo request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Prestamos, "GetPagoDescargadoresUltimo", request.SucursalId, request.Fecha.Year.ToString());
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var prefijoControl = _codigoCorrelativo_PagoDescarga + DateTime.Now.ToString("yy") + "-";
                var codigoPagoDescargas = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_PagoDescarga, prefijoControl, false);
                return new PagoDescargadoresDTO { CodigoPagoDescarga = codigoPagoDescargas };
            });

            return retorno;
        }

        public BusquedaPagoDescargadoresDTO Get(GetByValorPagosDescargas request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.PagoDescargas, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = PagoDescargasEspecificaciones.FiltrarPagoDescargadoresBusqueda(request.Filtro);
                List<PagoDescargadores> datos = _unidadDeTrabajo.PagoDescargadores.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PagoDescargadores>, IEnumerable<PagoDescargadoresDTO>>(datosPaginados.Items as IEnumerable<PagoDescargadores>);

                var dto = new BusquedaPagoDescargadoresDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<PagoDescargadoresDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public List<PagoDescargadoresDTO> Put(PutPagoDescargas request)
        {
            List<PagoDescargadoresDTO> pagoDescargadoresDto = new List<PagoDescargadoresDTO>();

            var pagoDescargas = _unidadDeTrabajo.PagoDescargadores.Find(request.PagoDescargas.PagoDescargaId);

            if (pagoDescargas == null)
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { CodigoBoleta = "N/A", NumeroEnvio = "N/A", ValidationErrorMessage = "PagoDescargaId No Existe!" };
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            var descargasPendientes = _unidadDeTrabajo.Descargadores.Where(p => p.CuadrillaId == pagoDescargas.CuadrillaId && 
                                                                           p.Estado == Estados.ACTIVO &&
                                                                          !p.EsDescargaPorAdelanto &&
                                                                           p.PagoDescargaId == null);

            if (descargasPendientes == null || !descargasPendientes.Any())
            {
                PagoDescargadoresDTO pagoDescargador = new PagoDescargadoresDTO { ValidationErrorMessage = $"No existen Descargas pendientes para la Cuadrilla de {pagoDescargas.Cuadrilla.NombreEncargado}" };
                pagoDescargadoresDto.Add(pagoDescargador);

                return pagoDescargadoresDto;
            }

            foreach (var descarga in descargasPendientes)
            {
                descarga.AsignarDescargaAPago(pagoDescargas);
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "ActualizarPagoDescargas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePagoDescargas();

            return pagoDescargadoresDto;
        }

        public EliminarResponseDTO Delete(DeletePagoDescargas request)
        {
            var pagoDescargas = _unidadDeTrabajo.PagoDescargadores.Find(request.PagoDescargaId);

            if (pagoDescargas == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "PagoDescargasId NO Existe"
                };
            }

            var mensajesValidacion = pagoDescargas.GetValidationErrorsDelete().ToList();
            if (mensajesValidacion.Any())
            {
                return new EliminarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            pagoDescargas.EliminarPagoDescargas();
            _unidadDeTrabajo.PagoDescargadores.Remove(pagoDescargas);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "EliminarPagoDescargadores");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePagoDescargas();

            return new EliminarResponseDTO();
        }

        private void RemoverCachePagoDescargas()
        {
            var listaKey = new List<string>
            {
                KeyCache.Descargadores,
                KeyCache.PagoDescargas,
                KeyCache.PagoDescargasDetalle,
                KeyCache.DescargasPorAdelantado
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
