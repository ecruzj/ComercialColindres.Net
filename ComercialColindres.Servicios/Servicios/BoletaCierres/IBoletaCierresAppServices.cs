using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaCierresAppServices
    {
        List<BoletaCierresDTO> Get(GetBoletasCierrePorBoletaId request);
        ActualizarResponseDTO Post(PostBoletasCierre request);
        List<BoletaCierresDTO> CloseBoletaMasive(CloseBoletaMasive request);
    }
}
