using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.Boletas
{
    [Route("/boletas-imag/{boletaId}", "GET")]
    public class GetBoletaImg : RequestBase, IReturn<BoletaImgDto>
    {
        public int BoletaId { get; set; }
    }
}
