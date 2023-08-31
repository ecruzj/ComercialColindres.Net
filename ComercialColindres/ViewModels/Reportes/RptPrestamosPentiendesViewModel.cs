using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptPrestamosPentiendesViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptPrestamosPentiendesViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }
        
        public RptPrestamosPendientesDTO PrestamosPendientes
        {
            get { return _prestamosPendientes; }
            set
            {
                _prestamosPendientes = value;
                RaisePropertyChanged("PrestamosPendientes");
            }
        }
        private RptPrestamosPendientesDTO _prestamosPendientes;
        
        public RelayCommand ComandoObtenerInformacionReporte { get; set; }
        public RelayCommand ComandoImprimir { get; set; }

        private void InicializarComandos()
        {
            ComandoObtenerInformacionReporte = new RelayCommand(ObtenerInformacionReporte);
            ComandoImprimir = new RelayCommand(Imprimir);
        }

        private void ObtenerInformacionReporte()
        {
            PrestamosPendientes = new RptPrestamosPendientesDTO();
            
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetPrestamosPendientes();

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.ListaPrestamosPendientes.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                PrestamosPendientes = res;
                PrestamosPendientes.SaldoPendiente = PrestamosPendientes.ObtenerSaldoPendiente();
                RaisePropertyChanged("PrestamosPendientes");

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void Imprimir()
        {
            if (PrestamosPendientes == null) return;

            if (!PrestamosPendientes.ListaPrestamosPendientes.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                return;
            }

            ImprimirReporte();
        }

        private void ImprimirReporte()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte Prestamos Pendientes de Cobrar";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.NombreDataSet = "PrestamosPendientes";                        
                manejadorReporte.Datos = PrestamosPendientes.ListaPrestamosPendientes;
                manejadorReporte.ArchivoReporte = "RptPrestamosPendientes.rdlc";
                manejadorReporte.MostarReporte();
            });
        }
    }
}
