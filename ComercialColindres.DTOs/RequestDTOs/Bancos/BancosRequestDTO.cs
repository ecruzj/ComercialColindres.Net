using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Bancos
{
    [Route("/bancos/", "GET")]
    public class GetAllBancos : IReturn<List<BancosDTO>>
    {
    }    
}
