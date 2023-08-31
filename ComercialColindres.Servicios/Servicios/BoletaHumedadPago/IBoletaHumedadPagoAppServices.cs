using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaHumedadPagoAppServices
    {
        BoletaHumedadPagoDto CreateBoletaHumidityPayment(PostBoletaHumedadPago request);
        BoletaHumedadPagoDto DeleteBoletaHumidityPay(DeleteBoletaHumedadPago request);
        List<BoletaHumedadPagoDto> GetHumidityPaymentByBoleta(GetHumidityPaymentByBoleta request);
        BoletaHumedadPagoDto GetHumidityPayment(GetHumidityPayment request);
    }
}
