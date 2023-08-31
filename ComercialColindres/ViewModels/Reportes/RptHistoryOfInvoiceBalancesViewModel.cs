using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptHistoryOfInvoiceBalancesViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;
        const string _mostrarTodas = "Mostrar Todas";
        const string _porPlanta = "Por Planta";

        public RptHistoryOfInvoiceBalancesViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
            LoadFilterTypes();
        }

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

        public List<RptHistoryOfInvoiceBalancesDto> HistoryOfInvoicesBalances
        {
            get { return _historyOfInvoicesBalances; }
            set
            {
                _historyOfInvoicesBalances = value;
                RaisePropertyChanged(nameof(HistoryOfInvoicesBalances));
            }
        }
        private List<RptHistoryOfInvoiceBalancesDto> _historyOfInvoicesBalances;

        public decimal HistoricalOutstandingBalance
        {
            get { return _historicalOutstandingBalance; }
            set
            {
                _historicalOutstandingBalance = value;
                RaisePropertyChanged(nameof(HistoricalOutstandingBalance));
            }
        }
        private decimal _historicalOutstandingBalance;

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }
        private DateTime _endDate = DateTime.Now;


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

            int plantaId = PlantaSelected == null || PlantaSelected.Id == null
                           ? 0
                           : (int)PlantaSelected.Id;

            var request = new GetHistoryOfInvoiceBalances
            {
                FactoryId = plantaId,
                EndDate = EndDate,
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

                HistoryOfInvoicesBalances = res;
                RaisePropertyChanged(nameof(HistoryOfInvoicesBalances));

                HistoricalOutstandingBalance = res.Sum(s => s.SaldoPendiente);
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CleanInformation()
        {
            HistoryOfInvoicesBalances = new List<RptHistoryOfInvoiceBalancesDto>();

            HistoricalOutstandingBalance = 0.0m;
            RaisePropertyChanged(nameof(HistoricalOutstandingBalance));
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
