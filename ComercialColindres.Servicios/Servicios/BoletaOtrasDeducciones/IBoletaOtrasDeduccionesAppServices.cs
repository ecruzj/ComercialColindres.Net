using ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaOtrasDeduccionesAppServices
    {
        List<BoletaOtrasDeduccionesDTO> Get(GetBoletaOtrasDeduccionesPorBoletaId request);
        ActualizarResponseDTO Post(PostBoletaOtrasDeducciones request);
    }
}
