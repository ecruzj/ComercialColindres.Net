using ComercialColindres.DTOs.RequestDTOs.FacturasCategorias;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IFacturasCategoriasAppServices
    {
        List<FacturasCategoriasDTO> Get(GetAllFacturasCategorias request);
    }
}
