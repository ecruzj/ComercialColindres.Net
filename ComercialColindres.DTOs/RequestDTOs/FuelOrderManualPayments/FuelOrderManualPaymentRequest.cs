using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments
{
    [Route("/fuel-order-manual-payments/{fuelOrderId}", "GET")]
    public class GetFuelOrderManualPayments : RequestBase, IReturn<List<FuelOrderManualPaymentDto>>
    {
        public int FuelOrderId { get; set; }
    }

    [Route("/fuel-order-manual-payments/", "POST")]
    public class PostFuelOrderManualPayments : RequestBase, IReturn<FuelOrderManualPaymentDto>
    {
        public int FuelOrderId { get; set; }
        public List<FuelOrderManualPaymentDto> FuelOrderManualPayments { get; set; }
    }
}
