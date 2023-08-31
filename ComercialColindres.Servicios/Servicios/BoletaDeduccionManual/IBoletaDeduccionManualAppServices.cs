using ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaDeduccionManualAppServices
    {
        List<BoletaDeduccionManualDto> GetBoletaDeduccionesManualById(GetBoletaDeduccionesManualByBoletaId request);
        BoletaDeduccionManualDto SaveBoletaDeduccionesManual(PostBoletadeduccionesManual request);
    }
}
