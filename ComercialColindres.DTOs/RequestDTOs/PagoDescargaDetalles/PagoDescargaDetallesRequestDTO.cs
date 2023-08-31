using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles
{
    [Route("/pago-descargas-detalle/busqueda/{pagoDescargasId}", "GET")]
    public class GetPagoDescargasDetallePorPagoDescargaId : IReturn<List<PagoDescargaDetallesDTO>>
    {
        public int PagoDescargasId { get; set; }
    }

    [Route("/pago-descargas-detalle/", "POST")]
    public class PostPagoDescargasDetalle : IReturn<ActualizarResponseDTO>
    {
        public int PagoDescargaId { get; set; }
        public List<PagoDescargaDetallesDTO> PagoDescargaDetalle { get; set; }
        public string UserId { get; set; }
    }
}
