using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaDetallesAppServices
    {
        List<BoletaDetallesDTO> Get(GetBoletasDetallePorBoletaId request);
        ActualizarResponseDTO Post(PostBoletasDetalle request);
    }
}
