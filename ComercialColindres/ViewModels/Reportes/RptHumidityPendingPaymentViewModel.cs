using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptHumidityPendingPaymentViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptHumidityPendingPaymentViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }
        
        public List<RptHumidityPendingPaymentDto> HumiditiesPendingPayment
        {
            get { return _humiditiesPendingPayment; }
            set
            {
                _humiditiesPendingPayment = value;
                RaisePropertyChanged(nameof(HumiditiesPendingPayment));
            }
        }
        private List<RptHumidityPendingPaymentDto> _humiditiesPendingPayment;
        
        public int TotalHumidities
        {
            get { return _totalHumidities; }
            set
            {
                _totalHumidities = value;
                RaisePropertyChanged(nameof(TotalHumidities));
            }
        }
        private int _totalHumidities;
        
        public decimal TotalPendingPayment
        {
            get { return _totalPendingPayment; }
            set
            {
                _totalPendingPayment = value;
                RaisePropertyChanged(nameof(TotalPendingPayment));
            }
        }
        private decimal _totalPendingPayment;

        public RelayCommand CommandGetReportInformation { get; set; }
        public RelayCommand CommandPrintReport { get; set; }

        private void InicializarComandos()
        {
            CommandGetReportInformation = new RelayCommand(GetReportInformation);
            CommandPrintReport = new RelayCommand(PrintReport);
        }

        private void PrintReport()
        {
            if (HumiditiesPendingPayment == null) return;

            if (!HumiditiesPendingPayment.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                return;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte Humedades Pendientes de Cobrar";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "RptHumidityPendingPayment.rdlc";
                manejadorReporte.AgregarParametro("UserId", _serviciosComunes.GetRequestUserInfo().UserId);
                manejadorReporte.NombreDataSet = "HumidityPendingPaymentDs";
                manejadorReporte.Datos = HumiditiesPendingPayment;
                manejadorReporte.MostarReporte();
            });
        }

        private void GetReportInformation()
        {
            HumiditiesPendingPayment = new List<RptHumidityPendingPaymentDto>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetHumidityPendingPayment();
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                HumiditiesPendingPayment = res;                
                TotalHumidities = res.Count;
                TotalPendingPayment = res.Sum(t => t.Total);

                RaisePropertyChanged(nameof(HumiditiesPendingPayment));
                RaisePropertyChanged(nameof(TotalHumidities));
                RaisePropertyChanged(nameof(TotalPendingPayment));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
    }
}
