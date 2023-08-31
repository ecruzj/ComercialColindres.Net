using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones
{
    [Route("/boleta-otras-deducciones/busqueda/{boletaId}", "GET")]
    public class GetBoletaOtrasDeduccionesPorBoletaId : IReturn<List<BoletaOtrasDeduccionesDTO>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boleta-otras-deducciones/", "POST")]
    public class PostBoletaOtrasDeducciones : IReturn<ActualizarResponseDTO>
    {
        public int BoletaId { get; set; }
        public string UserId { get; set; }
        public int SucursalId { get; set; }
        public List<BoletaOtrasDeduccionesDTO> BoletaOtrasDeducciones { get; set; }
    }
}
