using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle
{
    [Route("/ajuste-boleta-detalle/{ajusteBoletaId}", "GET")]
    public class GetAjusteBoletaDetalleByAjusteBoletaId : RequestBase, IReturn<List<AjusteBoletaDetalleDto>>
    {
        public int AjusteBoletaId { get; set; }
    }

    [Route("/ajuste-boleta-detalle/by-vendor/", "GET")]
    public class GetAjusteBoletaDetalleByVendorId : RequestBase, IReturn<List<AjusteBoletaDetalleDto>>
    {
        public int VendorId { get; set; }
        public int BoletaId { get; set; }
    }
    [Route("/ajuste-boleta-detalle/", "POST")]
    public class PostAjusteBoletaDetalle : RequestBase, IReturn<AjusteBoletaDetalleDto>
    {
        public int AjusteBoletaId { get; set; }
        public List<AjusteBoletaDetalleDto> AjusteBoletaDetalles { get; set; }
    }
}
