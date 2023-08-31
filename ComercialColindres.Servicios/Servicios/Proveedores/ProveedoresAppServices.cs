using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using ServidorCore.EntornoDatos;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ComercialColindres.Servicios.Servicios
{
    public class ProveedoresAppServices : IProveedoresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public ProveedoresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public EliminarResponseDTO Delete(DeleteProveedor request)
        {
            var proveedor = _unidadDeTrabajo.Proveedores.Find(request.ProveedorId);
            if (proveedor == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "proveedor Id No Existe"
                };
            }

            var mensajesValidacionEliminar = proveedor.GetValidationErrorsDelete();
            if (mensajesValidacionEliminar.Any())
            {
                return new EliminarResponseDTO
                {
                    //MensajeError =  Utilitarios.CrearMensajeValidacion(mensajesValidacionEliminar)
                };
            }

            _unidadDeTrabajo.Proveedores.Remove(proveedor);
            _unidadDeTrabajo.SaveChanges();

            RemoverCacheproveedores();

            return new EliminarResponseDTO();
        }

        public List<ProveedoresDTO> Get(GetProveedoresPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Proveedores, "GetProveedoresPorValorBusqueda", request.ValorBusqueda);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = ProveedoresEspecificaciones.FiltrarProveedoresBusqueda(request.ValorBusqueda);
                var datos = _unidadDeTrabajo.Proveedores.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Proveedores>, IEnumerable<ProveedoresDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public BusquedaProveedoresDTO Get(GetByValorProveedores request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Proveedores, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = ProveedoresEspecificaciones.FiltrarProveedoresBusqueda(request.Filtro);
                List<Proveedores> datos = _unidadDeTrabajo.Proveedores.Where(especificacion.EvalFunc).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Proveedores>, IEnumerable<ProveedoresDTO>>(datosPaginados.Items as IEnumerable<Proveedores>);

                var dto = new BusquedaProveedoresDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<ProveedoresDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public ProveedoresDTO Get(GetProveedor request)
        {
            var cacheKey = string.Format("{0}{1}", KeyCache.Proveedores, request.ProveedorId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var dato = _unidadDeTrabajo.Proveedores.Find(request.ProveedorId);
                if (dato == null)
                {
                    return new ProveedoresDTO
                    {
                        MensajeError = "proveedor d NO existe"
                    };
                }
                var dto = AutomapperTypeAdapter.ProyectarComo<Proveedores, ProveedoresDTO>(dato);
                return dto;
            });
            return retorno;
        }
        
        public ActualizarResponseDTO Post(PostProveedor request)
        {
            var proveedor = _unidadDeTrabajo.Proveedores.Find(request.Proveedor.ProveedorId);

            if (proveedor != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "proveedorId Existe"
                };
            }

            var validacionesDocumentacion = VerificarDocumentos(request.Proveedor.RTN, request.Proveedor.CedulaNo, request.Proveedor.ProveedorId, false);

            if (validacionesDocumentacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionesDocumentacion)
                };
            }

            proveedor = new Proveedores(request.Proveedor.RTN, request.Proveedor.Nombre, request.Proveedor.CedulaNo, request.Proveedor.Direccion, 
                                   request.Proveedor.Telefonos, request.Proveedor.CorreoElectronico);

            var mensajesValidacion = proveedor.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.Proveedores.Add(proveedor);
            _unidadDeTrabajo.SaveChanges();

            RemoverCacheproveedores();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutProveedor request)
        {
            var proveedor = _unidadDeTrabajo.Proveedores.Find(request.Proveedor.ProveedorId);
            if (proveedor == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "proveedorId No Existe"
                };
            }

            var validacionesDocumentacion = VerificarDocumentos(request.Proveedor.RTN, request.Proveedor.CedulaNo, request.Proveedor.ProveedorId, true);

            if (validacionesDocumentacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionesDocumentacion)
                };
            }

            proveedor.ActualizarProveedor(request.Proveedor.CedulaNo, request.Proveedor.RTN, request.Proveedor.Nombre, request.Proveedor.Direccion, request.Proveedor.Telefonos,
                                      request.Proveedor.CorreoElectronico, request.Proveedor.Estado);
            
            var mensajesValidacion = proveedor.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.SaveChanges();

            RemoverCacheproveedores();

            return new ActualizarResponseDTO();
        }

        private List<string> VerificarDocumentos(string rtn, string cedulaNo, int proveedorId, bool esActualizacion)
        {
            var listaErrores = new List<string>();
            var existeRTN = false;
            var existeCedula = false;

            if (esActualizacion)
            {
                existeRTN = !string.IsNullOrWhiteSpace(rtn) ? _unidadDeTrabajo.Proveedores.Any(r => r.RTN == rtn && r.ProveedorId != proveedorId) : false;
                existeCedula = !string.IsNullOrWhiteSpace(cedulaNo) ? _unidadDeTrabajo.Proveedores.Any(c => c.CedulaNo == cedulaNo && c.ProveedorId != proveedorId) : false;
            }
            else
            {
                existeRTN = _unidadDeTrabajo.Proveedores.Any(r => r.RTN == rtn);
                existeCedula = _unidadDeTrabajo.Proveedores.Any(c => c.CedulaNo == cedulaNo);                
            }

            if (existeRTN)
            {
                var mensaje = string.Format("El RTN {0} ya existe", rtn);
                listaErrores.Add(mensaje);
            }

            if (existeCedula)
            {
                var mensaje = string.Format("La Cedula {0} ya existe", cedulaNo);
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        void RemoverCacheproveedores()
        {
            var cacheKey = string.Format("{0}", KeyCache.Proveedores);
            _cacheAdapter.Remove(cacheKey);
        }        
    }
}
