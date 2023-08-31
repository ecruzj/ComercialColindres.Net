using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IEquiposAppServices
    {
        List<EquiposDTO> Get(GetEquiposPorProveedorId request);

        List<EquiposDTO> Get(GetEquiposPorValorBusqueda request);

        ActualizarResponseDTO Post(PostEquipos request);
    }
}
