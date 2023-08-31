using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class SucursalesAppServices : ISucursalesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public SucursalesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<SucursalesDTO> Get(GetAllSucursales request)
        {
            var datos = _unidadDeTrabajo.Sucursales.ToList();
            var dto = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Sucursales>, IEnumerable<SucursalesDTO>>(datos);
            return dto.ToList();
        }

        public SucursalesDTO Get(GetSucursal request)
        {
            var dato = _unidadDeTrabajo.Sucursales.Find(request.SucursalId);
            if (dato == null)
            {
                return new SucursalesDTO
                {
                    MensajeError = "Sucursal Id NO existe"
                };
            }
            var dto = AutomapperTypeAdapter.ProyectarComo<Sucursales, SucursalesDTO>(dato);
            return dto;
        }

        public ActualizarResponseDTO Put(PutSucursal request)
        {
            var dato = _unidadDeTrabajo.Sucursales.Find(request.Sucursal.SucursalId);
            if (dato == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Sucursal Id NO existe"
                };
            }
            dato.Actualizar(request.Sucursal.CodigoSucursal, request.Sucursal.Nombre, request.Sucursal.Direccion, request.Sucursal.Telefonos);
            _unidadDeTrabajo.SaveChanges();
            return new ActualizarResponseDTO();
        }
    }
}
