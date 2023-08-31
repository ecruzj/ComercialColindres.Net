using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice;
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
    public class RptBoletasWithoutInvoiceViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;
        const string _mostrarTodas = "Mostrar Todas";
        const string _porPlanta = "Por Planta";

        public RptBoletasWithoutInvoiceViewModel(IServiciosComunes serviciosComunes)
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

        public bool FiltrarPorRangoFecha
        {
            get { return _filtrarPorRangoFecha; }
            set
            {
                if (_filtrarPorRangoFecha != value)
                {
                    _filtrarPorRangoFecha = value;
                    RaisePropertyChanged(nameof(FiltrarPorRangoFecha));
                }
            }
        }
        private bool _filtrarPorRangoFecha;

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set
            {
                if (_fechaInicio != value)
                {
                    _fechaInicio = value;
                    RaisePropertyChanged(nameof(FechaInicio));

                    CleanInformation();
                }
            }
        }
        private DateTime _fechaInicio = DateTime.Now;

        public DateTime FechaFinal
        {
            get { return _fechaFinal; }
            set
            {
                if (_fechaFinal != value)
                {
                    _fechaFinal = value;
                    RaisePropertyChanged(nameof(FechaFinal));

                    CleanInformation();
                }
            }
        }
        private DateTime _fechaFinal = DateTime.Now;

        public List<RptBoletaWithOutInvoiceDto> BoletasWithoutInvoice
        {
            get { return _boletasWithoutInvoice; }
            set
            {
                _boletasWithoutInvoice = value;
                RaisePropertyChanged(nameof(BoletasWithoutInvoice));
            }
        }
        private List<RptBoletaWithOutInvoiceDto> _boletasWithoutInvoice;

        public int BoletasPentiendes
        {
            get { return _boletasPentiendes; }
            set
            {
                _boletasPentiendes = value;
                RaisePropertyChanged(nameof(BoletasPentiendes));
            }
        }
        private int _boletasPentiendes;

        public RelayCommand CommandGetInformation { get; set; }
        public RelayCommand<string> CommandFindPlanta { get; set; }

        private void InicializarComandos()
        {
            CommandGetInformation = new RelayCommand(GetInformation);
            CommandFindPlanta = new RelayCommand<string>(FindPlanta);
        }

        private void CleanInformation()
        {
            BoletasWithoutInvoice = new List<RptBoletaWithOutInvoiceDto>();
            BoletasPentiendes = 0;
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

        private void GetInformation()
        {
            CleanInformation();

            if (FechaInicio > FechaFinal && FiltrarPorRangoFecha)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Fecha Final debe de ser mayor a la Fecha Inicial");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            int plantaId = PlantaSelected != null && PlantaSelected.Id != null
                           ? (int)PlantaSelected.Id
                           : 0;

            var request = new GetBoletasWithOutInvoice
            {
                FiltroBusqueda = FilterSelected,
                PlantaId = plantaId,
                FechaInicial = FechaInicio,
                FechaFinal = FechaFinal,
                FiltrarPorFechas = FiltrarPorRangoFecha,
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

                BoletasWithoutInvoice = res;
                RaisePropertyChanged(nameof(BoletasWithoutInvoice));

                BoletasPentiendes = res.Count;
                RaisePropertyChanged(nameof(BoletasPentiendes));

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
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
