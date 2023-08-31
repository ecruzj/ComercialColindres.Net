using ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments;
using ComercialColindres.Servicios.Servicios.FuelOrderManualPayments;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FuelOrderManualPaymentRestService : IService
    {
        IFuelOrderManualPaymentAppServices _fuelOrderManualPaymentAppServices;

        public FuelOrderManualPaymentRestService(FuelOrderManualPaymentAppServices fuelOrderManualPaymentAppServices)
        {
            _fuelOrderManualPaymentAppServices = fuelOrderManualPaymentAppServices;
        }

        public object Get(GetFuelOrderManualPayments request)
        {
            return _fuelOrderManualPaymentAppServices.GetFuelOrderManualPayments(request);
        }

        public object Post(PostFuelOrderManualPayments request)
        {
            return _fuelOrderManualPaymentAppServices.SaveFuelOrderManualPayments(request);
        }
    }
}