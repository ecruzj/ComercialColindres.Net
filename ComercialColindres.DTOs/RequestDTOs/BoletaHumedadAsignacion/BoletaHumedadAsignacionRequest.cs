using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion
{
    [Route("/boletas-humedad-asignacion/", "GET")]
    public class GetBoletasHumedadByVendor : RequestBase, IReturn<List<BoletaHumedadAsignacionDto>>
    {
        public int VendorId { get; set; }
        public int BoletaId { get; set; }
    }
}
