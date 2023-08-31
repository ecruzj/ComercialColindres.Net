using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa
{
    [Route("/maestro-biomasa/", "GET")]
    public class GetAllMaestroBiomasa : IReturn<List<MaestroBiomasaDTO>>
    {
    }
}
