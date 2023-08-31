using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.Enumeraciones;
using System;

namespace ComercialColindres.Clases
{
    public interface IServiciosComunes
    {
        void MostrarNotificacion(EventType tipoNotificacion, string titulo, string mensaje);
        void MostrarNotificacion(EventType tipoNotificacion, string mensaje);
        string ObtenerRutaReporte();
        string ObtenerUriApiService();
        RequestUserInfo GetRequestUserInfo();
        string GetPathBoletasImg();
        string GetPathFuelOrder();
        void LogError(string message, Exception exception);
        RptBoletaPaymentPendingResumenDto GetPendingBoletasPayment(RptBoletaPaymentPendingResumenDto info);
    }
}
