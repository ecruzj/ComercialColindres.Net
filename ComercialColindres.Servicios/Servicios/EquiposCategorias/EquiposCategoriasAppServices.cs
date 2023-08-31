using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.EquiposCategorias;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using System;

namespace ComercialColindres.Servicios.Servicios
{
    public class EquiposCategoriasAppServices : IEquiposCategoriasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public EquiposCategoriasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public EquiposCategoriasDTO Get(GetEquipoCategoriaPorEquipoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.EquiposCategorias, "GetEquipoCategoria", request.EquipoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var equipo = _unidadDeTrabajo.Equipos.FirstOrDefault(e => e.EquipoId == request.EquipoId);
                var categoriaId = 0;

                if (equipo != null)
                {
                    categoriaId = equipo.EquipoCategoriaId;
                }

                var datos = _unidadDeTrabajo.EquiposCategorias.FirstOrDefault(c => c.EquipoCategoriaId == categoriaId);
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarComo<EquiposCategorias, EquiposCategoriasDTO>(datos);

                return datosDTO;
            });
            return datosConsulta;
        }

        public List<EquiposCategoriasDTO> Get(GetAllEquiposCategorias request)
        {
            var cacheKey = string.Format("{0}-{1}", KeyCache.EquiposCategorias, "GetAllEquiposCategorias");
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.EquiposCategorias.ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<EquiposCategorias>, IEnumerable<EquiposCategoriasDTO>>(datos);

                return datosDTO.ToList();
            });
            return datosConsulta;
        }
    }
}
