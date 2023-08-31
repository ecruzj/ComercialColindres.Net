using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptPendingInvoiceViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptPendingInvoiceViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;            
            InicializarComandos();
            LoadFilterTypes();
        }

        const string _mostrarTodas = "Mostrar Todas";
        const string _porPlanta = "Por Planta";
        const string _lps = "Lps";
        const string _dollar = "$";

        public List<string> FilterTypes
        {
            get { return _filterTypes; }
            set
            {
                _filterTypes = value;
                RaisePropertyChanged(nameof(FilterTypes));
            }
        }
        private List<string> _filterTypes;

        public string FilterSelected
        {
            get { return _filterSelected; }
            set
            {
                _filterSelected = value;
                RaisePropertyChanged(nameof(FilterSelected));

                if (string.IsNullOrWhiteSpace(FilterSelected)) return;

                if (FilterSelected == _porPlanta)
                {
                    ShowPlanta = true;
                }
                else
                {
                    ShowPlanta = false;
                }

                CleanInformation();
            }
        }
        private string _filterSelected;

        public bool ShowPlanta
        {
            get { return _showPlanta; }
            set
            {
                _showPlanta = value;
                RaisePropertyChanged(nameof(ShowPlanta));
            }
        }
        private bool _showPlanta;

        public decimal TotalLps
        {
            get { return _totalLps; }
            set
            {
                _totalLps = value;
                RaisePropertyChanged(nameof(TotalLps));
            }
        }
        private decimal _totalLps;

        public bool HasLps
        {
            get { return _hasLps; }
            set
            {
                _hasLps = value;
                RaisePropertyChanged(nameof(HasLps));
            }
        }
        private bool _hasLps;

        public decimal TotalDollar
        {
            get { return _totalDollar; }
            set
            {
                _totalDollar = value;
                RaisePropertyChanged(nameof(TotalDollar));
            }
        }
        private decimal _totalDollar;

        public bool HasDollar
        {
            get { return _hasDollar; }
            set
            {
                _hasDollar = value;
                RaisePropertyChanged(nameof(HasDollar));
            }
        }
        private bool _hasDollar;


        public List<AutoCompleteEntry> Plantas
        {
            get
            {
                return _plantas;
            }
            set
            {
                _plantas = value;
                RaisePropertyChanged(nameof(Plantas));
            }
        }
        private List<AutoCompleteEntry> _plantas;

        public AutoCompleteEntry PlantaSelected
        {
            get
            {
                return _plantaSelected;
            }
            set
            {
                _plantaSelected = value;
                RaisePropertyChanged(nameof(PlantaSelected));

                CleanInformation();
            }
        }
        private AutoCompleteEntry _plantaSelected;

        public List<RptPendingInvoiceDto> PendingInvoice
        {
            get { return _pendingInvoice; }
            set
            {
                _pendingInvoice = value;
                RaisePropertyChanged(nameof(PendingInvoice));
            }
        }
        private List<RptPendingInvoiceDto> _pendingInvoice;

        public RelayCommand CommandGetInformation { get; set; }
        public RelayCommand<string> CommandFindPlanta { get; set; }

        private void InicializarComandos()
        {
            CommandGetInformation = new RelayCommand(GetInformation);
            CommandFindPlanta = new RelayCommand<string>(FindPlanta);
        }

        private void GetInformation()
        {
            CleanInformation();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            int plantaId = PlantaSelected != null && PlantaSelected.Id != null
                           ? (int)PlantaSelected.Id
                           : 0;
            
            var request = new GetPendingInvoice
            {
                FiltroBusqueda = FilterSelected,
                PlantaId = plantaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                PendingInvoice = res;
                RaisePropertyChanged(nameof(PendingInvoice));

                TotalLps = PendingInvoice.Where(m => m.Moneda == _lps).Sum(t => t.SaldoPendiente);
                RaisePropertyChanged(nameof(TotalLps));                

                TotalDollar = PendingInvoice.Where(m => m.Moneda == _dollar).Sum(t => t.SaldoPendiente);
                RaisePropertyChanged(nameof(TotalDollar));

                if (TotalDollar > 0)
                {
                    HasDollar = true;
                    RaisePropertyChanged(nameof(HasDollar));
                }

                if (TotalLps > 0)
                {
                    HasLps = true;
                    RaisePropertyChanged(nameof(HasLps));
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CleanInformation()
        {
            PendingInvoice = new List<RptPendingInvoiceDto>();
            HasDollar = false;
            HasLps = false;
        }

        private void FindPlanta(string valorBusqueda)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPlantasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda                
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    Plantas = (from reg in res
                                    select new AutoCompleteEntry(reg.NombrePlanta, reg.PlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadFilterTypes()
        {
            FilterTypes = new List<string>
            {
                _mostrarTodas,
                _porPlanta
            };
        }
    }
}
