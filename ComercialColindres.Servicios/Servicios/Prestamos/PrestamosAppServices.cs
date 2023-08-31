using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Helpers;
using ComercialColindres.Datos.Recursos;
using System;

namespace ComercialColindres.Servicios.Servicios
{
    public class PrestamosAppServices : IPrestamosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        private const string _codigoCorrelativo_Prestamos = "PR";

        public PrestamosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public ActualizarResponseDTO Put(PutPrestamoAnular request)
        {
            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.PrestamoId);

            if (prestamo == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PrestamoId NO Existe"
                };
            }

            var mensajeValidacion = prestamo.Anular();
            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = mensajeValidacion
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "AnulacionPrestamos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePrestamos();

            return new ActualizarResponseDTO();
        }

        public PrestamosDTO Get(GetPrestamoUltimo request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Prestamos, "GetPrestamoUltimo", request.SucursalId, request.Fecha.Year.ToString());
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var prefijoControl = _codigoCorrelativo_Prestamos + DateTime.Now.ToString("yy") + "-";
                var codigoPrestamo = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_Prestamos, prefijoControl, false);
                return new PrestamosDTO { CodigoPrestamo = codigoPrestamo };
            });
            return retorno;
        }

        public BusquedaPrestamosDTO Get(GetByValorPrestamos request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Prestamos, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = PrestamosEspecificaciones.FiltrarPrestamosBusqueda(request.Filtro);
                List<Prestamos> datos = _unidadDeTrabajo.Prestamos.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Prestamos>, IEnumerable<PrestamosDTO>>(datosPaginados.Items as IEnumerable<Prestamos>);

                var dto = new BusquedaPrestamosDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<PrestamosDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public List<PrestamosDTO> Get(GetPrestamoPorProveedorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Prestamos, "GetPrestamoPorProveedorId", request.ProveedorId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Prestamos> datos = _unidadDeTrabajo.Prestamos.Where(p => p.ProveedorId == request.ProveedorId && p.Estado == Estados.ACTIVO)
                                                                            .OrderByDescending(o => o.MontoPrestamo).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Prestamos>, IEnumerable<PrestamosDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostPrestamo request)
        {
            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.Prestamo.PrestamoId);
            var prestamoRequest = request.Prestamo;

            if (prestamo != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PrestaId YA existe"
                };
            }

            var sucursal = _unidadDeTrabajo.Sucursales.Find(request.Prestamo.SucursalId);

            if (sucursal == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "SucursalId NO Existe"
                };
            }

            prestamo = new Prestamos("???", prestamoRequest.SucursalId, prestamoRequest.ProveedorId, prestamoRequest.FechaCreacion, prestamoRequest.AutorizadoPor, prestamoRequest.PorcentajeInteres,
                                     prestamoRequest.MontoPrestamo, prestamoRequest.Observaciones, prestamoRequest.EsInteresMensual);

            var mensajesValidacion = prestamo.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            prestamo.CodigoPrestamo = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, prestamo.SucursalId, _codigoCorrelativo_Prestamos, string.Empty, true);
            _unidadDeTrabajo.Prestamos.Add(prestamo);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CreacionPrestamos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePrestamos();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutPrestamo request)
        {
            var prestamoRequest = request.Prestamo;
            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.Prestamo.PrestamoId);
            
            if (prestamo == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PrestamoId NO Existe"
                };
            }

            if (prestamo.Estado == Estados.ENPROCESO || prestamo.Estado == Estados.CERRADO || prestamo.Estado == Estados.ANULADO || prestamo.Estado == Estados.ACTIVO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El estado del Prestado debe ser NUEVO"
                };
            }

            prestamo.ActualizarPrestamo(prestamoRequest.SucursalId, prestamoRequest.ProveedorId, prestamoRequest.FechaCreacion, prestamoRequest.AutorizadoPor, prestamoRequest.PorcentajeInteres,
                                     prestamoRequest.MontoPrestamo, prestamoRequest.Observaciones);

            var mensajesValidacion = prestamo.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizacionPrestamos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePrestamos();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutActivarPrestamo request)
        {
            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.PrestamoId);

            if (prestamo == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El PrestamoId NO Existe"
                };
            }

            var mensajeValidacion = prestamo.Activar();
            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = mensajeValidacion
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActivarPrestamo");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCachePrestamos();

            return new ActualizarResponseDTO();
        }

        private void RemoverCachePrestamos()
        {
            var cacheKey = string.Format("{0}", KeyCache.Prestamos);
            _cacheAdapter.Remove(cacheKey);
        }        
    }
}
