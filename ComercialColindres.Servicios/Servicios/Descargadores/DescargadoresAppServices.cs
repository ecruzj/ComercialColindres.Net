using System;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ServidorCore.EntornoDatos;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class DescargadoresAppServices : IDescargadoresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletasDetalleDomainServices _boletasDetalleDomainSerives;

        public DescargadoresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, 
                                        IBoletasDetalleDomainServices boletasDetalleDomainSerives)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletasDetalleDomainSerives = boletasDetalleDomainSerives;
        }

        public BusquedaDescargadoresDTO Get(GetByValorDescargadores request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Descargadores, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = DescargadoresEspecificaciones.FiltrarDescargas(request.Filtro);
                List<Descargadores> datos = _unidadDeTrabajo.Descargadores.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Descargadores>, IEnumerable<DescargadoresDTO>>(datosPaginados.Items as IEnumerable<Descargadores>);

                var dto = new BusquedaDescargadoresDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<DescargadoresDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public List<DescargadoresDTO> Get(GetDescargasPorCuadrillaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Descargadores, "GetDescargadoresPorCuadrillaId", request.CuadrillaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Descargadores> datos = _unidadDeTrabajo.Descargadores.Where(d => d.CuadrillaId == request.CuadrillaId && d.Estado == Estados.ACTIVO)
                                                                          .OrderByDescending(o => o.FechaDescarga).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Descargadores>, IEnumerable<DescargadoresDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<DescargadoresDTO> Get(GetDescargasAplicaPagoPorCuadrillaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Descargadores, "GetDescargasAplicaPagoPorCuadrillaId", request.CuadrillaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Descargadores> datos = _unidadDeTrabajo.Descargadores.Where(d => d.CuadrillaId == request.CuadrillaId && 
                                                                                 d.Cuadrilla.AplicaPago == true &&
                                                                                 d.Estado == Estados.ACTIVO)
                                                                          .OrderByDescending(o => o.FechaDescarga).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Descargadores>, IEnumerable<DescargadoresDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
        
        public List<DescargadoresDTO> Get(GetDescargasPorPagoDescargaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Descargadores, "GetDescargasPorPagoDescargaId", request.PagoDescargaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Descargadores> datos = _unidadDeTrabajo.Descargadores.Where(d => d.PagoDescargaId == request.PagoDescargaId)
                                                                            .OrderByDescending(o => o.FechaTransaccion).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Descargadores>, IEnumerable<DescargadoresDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostDescargadores request)
        {            
            var descargaRequest = request.Descarga;

            if (descargaRequest.CuadrillaId == 0) throw new ArgumentNullException("CuadrillaId");
            if (descargaRequest.BoletaId == 0) throw new ArgumentNullException("BoletaId");
            if (descargaRequest.PrecioDescarga == 0) throw new ArgumentNullException("PrecioDescarga");

            var descarga = _unidadDeTrabajo.Descargadores.Find(descargaRequest.DescargadaId);

            if (descarga != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La DescargaId ya existe"
                };
            }

            var boleta = _unidadDeTrabajo.Boletas.FirstOrDefault(b => b.BoletaId == descargaRequest.BoletaId);

            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La BoletaId no existe"
                };
            }

            if (boleta.Descargador != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Ya existe una orden de descarga asociada a la boleta"
                };
            }

            var cuadrilla = _unidadDeTrabajo.Cuadrillas.FirstOrDefault(c => c.CuadrillaId == descargaRequest.CuadrillaId);

            if (cuadrilla == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La CuadrillaId no existe"
                };
            }

            if (cuadrilla.PlantaId != boleta.PlantaId)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("La Cuadrilla no pertenece a la planta {0}", boleta.ClientePlanta.NombrePlanta)
                };
            }

            descarga = new Descargadores(descargaRequest.BoletaId, descargaRequest.CuadrillaId, descargaRequest.PrecioDescarga, boleta.FechaSalida, false);

            var mensajesValidacion = descarga.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            _unidadDeTrabajo.Descargadores.Add(descarga);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CreacionDescargas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheDescargadores();

            return new ActualizarResponseDTO();
        }

        public EliminarResponseDTO Put(PutDescargaAnular request)
        {
            var descarga = _unidadDeTrabajo.Descargadores.Where(d => d.DescargadaId == request.DescargadaId).FirstOrDefault();

            if (descarga == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "DescargaId NO Existe"
                };
            }

            descarga.EliminarDescarga();

            _unidadDeTrabajo.Descargadores.Remove(descarga);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "AnularDescarga");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheDescargadores();

            return new EliminarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutDescarga request)
        {
            var descarga = _unidadDeTrabajo.Descargadores.Where(d => d.DescargadaId == request.Descarga.DescargadaId).FirstOrDefault();

            if (descarga == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "DescargaId NO Existe"
                };
            }

            if (descarga.Boleta != null)
            {
                if (descarga.Boleta.Estado != Estados.ACTIVO)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = "El Estado de la Boleta debe ser ACTIVO"
                    };
                }

                var aplicaNuevoPrecioDescarga = ((descarga.Boleta.ObtenerTotalAPagar() + descarga.PrecioDescarga) - request.Descarga.PrecioDescarga) > 0;

                if (!aplicaNuevoPrecioDescarga)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = "El Precio de la Descarga Excede el Total a Pagar de la Boleta"
                    };
                }
            }

            descarga.ActualizarDescarga(request.Descarga.BoletaId, request.Descarga.CuadrillaId, request.Descarga.PrecioDescarga, request.Descarga.FechaDescarga);

            var mensajesValidacion = descarga.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizacionDescarga");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheDescargadores();

            return new ActualizarResponseDTO();
        }

        private void RemoverCacheDescargadores()
        {
            var listaKey = new List<string>
            {
                KeyCache.Descargadores,
                KeyCache.DescargasPorAdelantado,
                KeyCache.BoletasDetalles,
                KeyCache.Boletas
            };
            _cacheAdapter.Remove(listaKey);
        }        
    }
}
