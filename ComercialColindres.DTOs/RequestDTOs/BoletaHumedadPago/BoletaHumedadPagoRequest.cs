using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago
{
    [Route("/boletas-humedad-pago/{boletaId}", "GET")]
    public class GetHumidityPaymentByBoleta : RequestBase, IReturn<List<BoletaHumedadPagoDto>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boletas-humedad-pago/payment", "GET")]
    public class GetHumidityPayment : RequestBase, IReturn<BoletaHumedadPagoDto>
    {
        public int BoletaHumedadId { get; set; }
    }

    [Route("/boletas-humedad-pago/", "POST")]
    public class PostBoletaHumedadPago : RequestBase, IReturn<BoletaHumedadPagoDto>
    {
        public int BoletaId { get; set; }
        public List<BoletaHumedadPagoDto> BoletasHumedadPago { get; set; }
    }

    [Route("/boletas-humedad-pago/", "DELETE")]
    public class DeleteBoletaHumedadPago : RequestBase, IReturn<BoletaHumedadPagoDto>
    {
        public int BoletaHumedadPagoId { get; set; }
    }
}
