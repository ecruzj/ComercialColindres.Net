using ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios.FuelOrderManualPayments
{
    public interface IFuelOrderManualPaymentAppServices
    {
        List<FuelOrderManualPaymentDto> GetFuelOrderManualPayments(GetFuelOrderManualPayments request);
        FuelOrderManualPaymentDto SaveFuelOrderManualPayments(PostFuelOrderManualPayments request);
    }
}
