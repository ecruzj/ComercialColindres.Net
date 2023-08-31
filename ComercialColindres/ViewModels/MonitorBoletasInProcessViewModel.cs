using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComercialColindres.ViewModels
{
    public class MonitorBoletasInProcessViewModel : BaseVM
    {
        private readonly IServiciosComunes _serviciosComunes;
        private JsonServiceClient _client;

        public MonitorBoletasInProcessViewModel(IServiciosComunes serviciosComunes)
        {
            TituloPantalla = "Monitor de Boletas Pendientes";

            if (IsInDesignMode)
            {
                CargarDatosPruebas();
                return;
            }

            _serviciosComunes = serviciosComunes;
            InicializarComandos();
            CargarBoletasPendientes();
        }
        
        public List<BoletasDTO> BoletasInProcess
        {
            get { return _boletasInProcess; }
            set
            {
                _boletasInProcess = value;
                RaisePropertyChanged("BoletasInProcess");
            }
        }
        private List<BoletasDTO> _boletasInProcess;

        public BoletasDTO BoletaSeleccionada
        {
            get
            {
                return _boletaSeleccionada;
            }
            set
            {
                _boletaSeleccionada = value;
                RaisePropertyChanged("BoletaSeleccionada");

                if (IsInDesignMode)
                {
                    return;
                }

                CargarBoletaDetalle();
            }
        }
        private BoletasDTO _boletaSeleccionada;

        public List<BoletaDetallesDTO> ListaBoletaDetalles
        {
            get
            {
                return _listaBoletaDetalles;
            }
            set
            {
                if (_listaBoletaDetalles != value)
                {
                    _listaBoletaDetalles = value;
                    RaisePropertyChanged("ListaBoletaDetalles");
                }
            }
        }
        private List<BoletaDetallesDTO> _listaBoletaDetalles;

        public RelayCommand ComandoRefrescar { get; set; }

        private void InicializarComandos()
        {
            ComandoRefrescar = new RelayCommand(Refrescar);
        }

        private void CargarBoletasPendientes()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletasInProcess
            {
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BoletasInProcess = res;
                if (BoletasInProcess != null && BoletasInProcess.Any())
                {
                    BoletaSeleccionada = BoletasInProcess.FirstOrDefault();
                    RaisePropertyChanged("BoletaSeleccionada");
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
            });
        }

        private void CargarBoletaDetalle()
        {
            ListaBoletaDetalles = new List<BoletaDetallesDTO>();
            if (BoletaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletasDetallePorBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaBoletaDetalles = res;
                RaisePropertyChanged("ListaBoletaDetalles");
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void Refrescar()
        {
            CargarBoletasPendientes();
        }              

        private void CargarDatosPruebas()
        {
        }
    }
}
