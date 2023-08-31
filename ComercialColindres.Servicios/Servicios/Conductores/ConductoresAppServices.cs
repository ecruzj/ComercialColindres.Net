using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;

namespace ComercialColindres.Servicios.Servicios
{
    public class ConductoresAppServices : IConductoresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public ConductoresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }
        
        public List<ConductoresDTO> Get(GetConductoresPorProveedorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Conductores, "GetConductoresPorClienteId", request.ProveedorId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Conductores> datos = _unidadDeTrabajo.Conductores.Where(cb => cb.ProveedorId == request.ProveedorId).OrderByDescending(o => o.Nombre).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Conductores>, IEnumerable<ConductoresDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<ConductoresDTO> Get(GetConductoresPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Conductores, "GetConductoresPorValorBusqueda", request.ValorBusqueda, request.ProveedorId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = ConductoresEspecificaciones.FiltrarPorProveedor(request.ValorBusqueda, request.ProveedorId);
                var datos = _unidadDeTrabajo.Conductores.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Conductores>, IEnumerable<ConductoresDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public ActualizarResponseDTO Post(PostConductores request)
        {
            var cliente = _unidadDeTrabajo.Proveedores.Find(request.ProveedorId);

            if (cliente == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El Cliente no existe"
                };
            }

            var listaConductores = cliente.Conductores.ToList();

            //Verificar si removieron Items
            foreach (var itemConductor in listaConductores)
            {
                var conductor = request.Conductores
                                   .FirstOrDefault(c => c.ConductorId == itemConductor.ConductorId);

                if (conductor == null)
                {
                    cliente.Conductores.Remove(itemConductor);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var itemConductor in request.Conductores)
            {
                var conductor = listaConductores
                                     .FirstOrDefault(c => c.ConductorId == itemConductor.ConductorId);

                //Hubo una Actualizacion
                if (conductor != null)
                {
                    conductor.ActualizarConductor(itemConductor.Nombre, itemConductor.Telefonos);
                    mensajesValidacion = conductor.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                else
                {
                    //Se agrego una nueva cuenta
                    var nuevoConductor = new Conductores(itemConductor.Nombre, request.ProveedorId, itemConductor.Telefonos);
                    mensajesValidacion = nuevoConductor.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.Conductores.Add(nuevoConductor);
                }
            }

            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new ActualizarResponseDTO();
        }
        
        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Conductores
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
