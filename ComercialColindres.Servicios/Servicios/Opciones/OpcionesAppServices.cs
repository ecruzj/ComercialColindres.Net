using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.Opciones;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class OpcionesAppServices : IOpcionesAppServices
    {
        readonly ComercialColindresContext _unidadDeTrabajo;
        readonly ICacheAdapter _cacheAdapter;

        public OpcionesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            this._cacheAdapter = cacheAdapter;
            this._unidadDeTrabajo = unidadDeTrabajo;
        }

        public EliminarResponseDTO Delete(DeleteOpcion request)
        {
            var dato = _unidadDeTrabajo.Opciones.Find(request.OpcionId);
            if (dato == null)
            {
                return new EliminarResponseDTO { MensajeError = "OpcionId NO existe" };
            }

            var mensajesValidacion = dato.GetValidationErrorsDelete();
            if (mensajesValidacion.Any())
            {
                return new EliminarResponseDTO { MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
            }
            _unidadDeTrabajo.Opciones.Remove(dato);
            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new EliminarResponseDTO();
        }

        // <inheritDoc/>
        public List<OpcionesDTO> Get(FindOpciones request)
        {
            var cacheKey = string.Format("{0}-{1}", KeyCache.OpcionesSistema, request.Filtro);

            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var opcionEspecificacion = OpcionesEspecificaciones.FiltroBusqueda(request.Filtro);
                var datos = _unidadDeTrabajo.Opciones.Where(opcionEspecificacion.EvalPredicate).OrderBy(o => o.Nombre);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Opciones>, IEnumerable<OpcionesDTO>>(datos);

                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostOpcion request)
        {
            var dato = _unidadDeTrabajo.Opciones.Find(request.Opcion.OpcionId);
            if (dato != null)
            {
                return new ActualizarResponseDTO { MensajeError = "OpcionId YA existe" };
            }

            dato = new Opciones(request.Opcion.Nombre, request.Opcion.TipoAcceso, request.Opcion.TipoPropiedad);
            var mensajesValidacion = dato.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO { MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
            }

            _unidadDeTrabajo.Opciones.Add(dato);
            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutOpcion request)
        {
            var dato = _unidadDeTrabajo.Opciones.Find(request.Opcion.OpcionId);
            if (dato == null)
            {
                return new ActualizarResponseDTO { MensajeError = "OpcionId NO existe" };
            }

            dato.Nombre = request.Opcion.Nombre;
            dato.TipoAcceso = request.Opcion.TipoAcceso;
            dato.TipoPropiedad = request.Opcion.TipoPropiedad;
            var mensajesValidacion = dato.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO { MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
            }

            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.OpcionesSistema,
                KeyCache.Usuarios
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
