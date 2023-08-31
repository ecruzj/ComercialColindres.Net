using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaHumedadPagoRestService : IService
    {
        IBoletaHumedadPagoAppServices _boletaHumedadPagoAppServices;

        public BoletaHumedadPagoRestService(IBoletaHumedadPagoAppServices boletaHumedadPagoAppServices)
        {
            _boletaHumedadPagoAppServices = boletaHumedadPagoAppServices;
        }

        public object Get(GetHumidityPaymentByBoleta request)
        {
            return _boletaHumedadPagoAppServices.GetHumidityPaymentByBoleta(request);
        }

        public object Get(GetHumidityPayment request)
        {
            return _boletaHumedadPagoAppServices.GetHumidityPayment(request);
        }


        public object Post(PostBoletaHumedadPago request)
        {
            return _boletaHumedadPagoAppServices.CreateBoletaHumidityPayment(request);
        }

        public object Delete(DeleteBoletaHumedadPago request)
        {
            return _boletaHumedadPagoAppServices.DeleteBoletaHumidityPay(request);
        }
    }
}