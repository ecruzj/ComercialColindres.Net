using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    [Route("/ordenes-combustible-imag/{ordenCombustibleId}", "GET")]
    public class GetOrdenCombustibleImg : RequestBase, IReturn<OrdenCombustibleImgDto>
    {
        public int OrdenCombustibleId { get; set; }
    }
}
