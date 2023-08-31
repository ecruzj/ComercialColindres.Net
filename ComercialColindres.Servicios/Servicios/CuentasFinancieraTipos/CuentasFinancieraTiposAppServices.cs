using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class CuentasFinancieraTiposAppServices : ICuentasFinancieraTiposAppServices
    {
        ComercialColindresContext _unidadDeTrabajo;

        public CuentasFinancieraTiposAppServices(ComercialColindresContext unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public List<CuentasFinancieraTiposDTO> Get(GetAllTiposCuentasFinancieras request)
        {
            var datos = _unidadDeTrabajo.CuentasFinancieraTipos.ToList();
            var dto = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<CuentasFinancieraTipos>, IEnumerable<CuentasFinancieraTiposDTO>>(datos);
            return dto.ToList();
        }
    }
}
