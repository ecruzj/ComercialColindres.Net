using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;

namespace ComercialColindres.Servicios.Servicios
{
    public class EquiposAppServices : IEquiposAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public EquiposAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<EquiposDTO> Get(GetEquiposPorProveedorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Equipos, "GetEquiposPorClienteId", request.ProveedorId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Equipos> datos = _unidadDeTrabajo.Equipos.Where(cb => cb.ProveedorId == request.ProveedorId).OrderByDescending(o => o.PlacaCabezal).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Equipos>, IEnumerable<EquiposDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<EquiposDTO> Get(GetEquiposPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Conductores, "GetEquiposPorValorBusqueda", request.ValorBusqueda, request.ProveedorId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = EquiposEspecificaciones.FiltrarPorProveedor(request.ValorBusqueda, request.ProveedorId);
                var datos = _unidadDeTrabajo.Equipos.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Equipos>, IEnumerable<EquiposDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public ActualizarResponseDTO Post(PostEquipos request)
        {
            var cliente = _unidadDeTrabajo.Proveedores.Find(request.ProveedorId);

            if (cliente == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El Cliente no existe"
                };
            }

            var listaEquipos = cliente.Equipos.ToList();

            //Verificar si removieron Items
            foreach (var itemEquipo in listaEquipos)
            {
                var equipo = request.Equipos
                                   .FirstOrDefault(c => c.EquipoId == itemEquipo.EquipoId);
                
                if (equipo == null)
                {
                    cliente.Equipos.Remove(itemEquipo);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var itemEquipo in request.Equipos)
            {
                var equipo = listaEquipos
                                     .FirstOrDefault(c => c.EquipoId == itemEquipo.EquipoId);

                var existePlacaCabezal = _unidadDeTrabajo.Equipos.Any(p => p.PlacaCabezal == itemEquipo.PlacaCabezal && 
                                                                      p.Proveedor != null && p.EquipoId != itemEquipo.EquipoId);

                if (existePlacaCabezal)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = string.Format("Ya existe un Equipo con la Placa {0}", itemEquipo.PlacaCabezal)
                    };
                }

                //Hubo una Actualizacion
                if (equipo != null)
                {
                    equipo.ActualizarEquipo(itemEquipo.EquipoCategoriaId, itemEquipo.PlacaCabezal);
                    mensajesValidacion = equipo.GetValidationErrors();

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
                    var nuevoEquipo = new Equipos(itemEquipo.EquipoCategoriaId, request.ProveedorId, itemEquipo.PlacaCabezal);
                    mensajesValidacion = nuevoEquipo.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.Equipos.Add(nuevoEquipo);
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
                KeyCache.Equipos
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
