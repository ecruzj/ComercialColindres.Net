using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual
{
    [Route("/boleta-deduccion-manual/{boletaId}", "GET")]
    public class GetBoletaDeduccionesManualByBoletaId : IReturn<List<BoletaDeduccionManualDto>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boleta-deduccion-manual/", "POST")]
    public class PostBoletadeduccionesManual : RequestBase, IReturn<BoletaDeduccionManualDto>
    {
        public int BoletaId { get; set; }
        public List<BoletaDeduccionManualDto> BoletaDeduccionesManual { get; set; }
    }
}
