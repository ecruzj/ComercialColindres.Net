using ComercialColindres.Clases;
using ComercialColindres.CrossViewModel;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptBoletaPaymentPendingViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptBoletaPaymentPendingViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }
        
        public RptBoletaPaymentPendingResumenDto ReportBoletasPaymentPending
        {
            get { return _reportBoletasPaymentPending; }
            set
            {
                _reportBoletasPaymentPending = value;
                RaisePropertyChanged(nameof(ReportBoletasPaymentPending));
            }
        }
        private RptBoletaPaymentPendingResumenDto _reportBoletasPaymentPending;

        public List<AutoCompleteEntry> Proveedores
        {
            get { return _proveedores; }
            set
            {
                _proveedores = value;
                RaisePropertyChanged(nameof(Proveedores));
            }
        }
        private List<AutoCompleteEntry> _proveedores;

        public AutoCompleteEntry ProveedorSeleccionado
        {
            get { return _proveedorSeleccionado; }
            set
            {
                _proveedorSeleccionado = value;
                RaisePropertyChanged(nameof(ProveedorSeleccionado));
            }
        }
        private AutoCompleteEntry _proveedorSeleccionado;

        public RelayCommand ComandoObtenerInformacionReporte { get; set; }
        public RelayCommand<string> ComandoBuscarProveedores { get; private set; }

        private void InicializarComandos()
        {
            ComandoBuscarProveedores = new RelayCommand<string>(BuscarProveedores);
            ComandoObtenerInformacionReporte = new RelayCommand(ObtenerInformacionReporte);
        }

        private void ObtenerInformacionReporte()
        {
            ReportBoletasPaymentPending = new RptBoletaPaymentPendingResumenDto();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetBoletaPaymentPending
            {
                ProveedorId = ProveedorSeleccionado != null ? (int)ProveedorSeleccionado.Id : 0,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.BoletasPending.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                ReportBoletasPaymentPending = new RptBoletaPaymentPendingResumenDto
                {
                    BoletasPending = res.BoletasPending
                };

                ReportBoletasPaymentPending.GetValuesForPayment();
                RaisePropertyChanged(nameof(ReportBoletasPaymentPending));

                BoletasPendingPaymentInfo.BoletaPendingPaymentInfo = ReportBoletasPaymentPending;
                BoletasPendingPaymentInfo.BoletaPendingPaymentInfo.VendorId = (int)ProveedorSeleccionado.Id;
                BoletasPendingPaymentInfo.BoletaPendingPaymentInfo.VendorName = ProveedorSeleccionado.DisplayName;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarProveedores(string valorBusqueda)
        {
            ReportBoletasPaymentPending = new RptBoletaPaymentPendingResumenDto();
            BoletasPendingPaymentInfo.BoletaPendingPaymentInfo = new RptBoletaPaymentPendingResumenDto();

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetProveedoresPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    Proveedores = (from reg in res
                                   select new AutoCompleteEntry(reg.Nombre, reg.ProveedorId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
    }
}
