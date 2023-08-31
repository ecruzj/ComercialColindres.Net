using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaDetalles
{
    [Route("/boleta-destalles/busqueda/{boletaId}", "GET")]
    public class GetBoletasDetallePorBoletaId : IReturn<List<BoletaDetallesDTO>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boleta-destalles/", "POST")]
    public class PostBoletasDetalle : IReturn<ActualizarResponseDTO>
    {
        public int BoletaId { get; set; }
        public string UserId { get; set; }
        public List<BoletaDetallesDTO> BoletasDetalle { get; set; }
    }
}
