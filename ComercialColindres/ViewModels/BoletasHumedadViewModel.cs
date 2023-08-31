using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Modelos;

namespace ComercialColindres.ViewModels
{
    public class BoletasHumedadViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public BoletasHumedadViewModel(IServiciosComunes serviciosComunes)
        {
            //Para capturar excepciones no manejadas
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            if (IsInDesignMode)
            {
                CargarDatosPruebas();
                return;
            }
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
            InicializarPropiedades();
            CargarDatosIniciales();            
        }

        private void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                _serviciosComunes.LogError("Unhandled Exception", (Exception)e.ExceptionObject);
            }
            catch { }

            Environment.Exit(1);
        }

        public Visibility MostrarVentanaEspera
        {
            get { return _mostrarVentanaEspera; }
            set
            {
                if (_mostrarVentanaEspera != value)
                {
                    _mostrarVentanaEspera = value;
                    RaisePropertyChanged(nameof(MostrarVentanaEspera));
                }
            }
        }
        private Visibility _mostrarVentanaEspera = Visibility.Collapsed;

        public int NumeroPagina
        {
            get
            {
                return _numeroPagina;
            }
            set
            {
                _numeroPagina = value;
                RaisePropertyChanged(nameof(NumeroPagina));
            }
        }
        int _numeroPagina = 1;
        
        public BusquedaBoletasHumedadDto BoletasHumidityControl
        {
            get { return _boletasHumidityControl; }
            set
            {
                _boletasHumidityControl = value;
                RaisePropertyChanged(nameof(BoletasHumidityControl));
            }
        }
        private BusquedaBoletasHumedadDto _boletasHumidityControl;
        
        public BoletaHumedadDto BoletaHumiditeSelected
        {
            get { return _boletaHumiditeSelected; }
            set
            {
                _boletaHumiditeSelected = value;
                RaisePropertyChanged(nameof(BoletaHumiditeSelected));

                if (IsInDesignMode)
                {
                    return;
                }

                if (BoletaHumiditeSelected == null || BoletaHumiditeSelected.Estado != Estados.CERRADO)
                {
                    IsPaid = false;
                    return;
                }

                GetBoletaHumidityPayment();
            }
        }
        private BoletaHumedadDto _boletaHumiditeSelected;
        
        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                RaisePropertyChanged(nameof(SearchValue));
            }
        }
        private string _searchValue;

        public IList XlsColeccionImportacion
        {
            get { return _xlsColeccionImportacion; }
            set
            {
                if (_xlsColeccionImportacion != value)
                {
                    _xlsColeccionImportacion = value;
                    RaisePropertyChanged(nameof(XlsColeccionImportacion));
                    ProcessImportation();
                }

            }
        }
        private IList _xlsColeccionImportacion;

        public List<AutoCompleteEntry> ListaPlantasAutoComplete
        {
            get
            {
                return _listaPlantasAutoComplete;
            }
            set
            {
                _listaPlantasAutoComplete = value;
                RaisePropertyChanged("ListaPlantasAutoComplete");
            }
        }
        private List<AutoCompleteEntry> _listaPlantasAutoComplete;

        public AutoCompleteEntry FacilitySelected
        {
            get
            {
                return _facilitySelected;
            }
            set
            {
                _facilitySelected = value;
                RaisePropertyChanged(nameof(FacilitySelected));
            }
        }
        private AutoCompleteEntry _facilitySelected;
        
        public BoletaHumedadModel DatoBoletasHumidity
        {
            get { return _datoBoletasHumidity; }
            set
            {
                _datoBoletasHumidity = value;
                RaisePropertyChanged(nameof(DatoBoletasHumidity));
            }
        }
        private BoletaHumedadModel _datoBoletasHumidity;

        public ObservableCollection<BoletaHumedadModel> BoletasHumedadModel
        {
            get { return _boletasHumedadModel; }
            set
            {
                _boletasHumedadModel = value;
                RaisePropertyChanged(nameof(BoletasHumedadModel));
            }
        }
        private ObservableCollection<BoletaHumedadModel> _boletasHumedadModel;
        
        public List<BoletaHumedadDto> BoletaHumidityValidations
        {
            get { return _boletaHumidityValidations; }
            set
            {
                _boletaHumidityValidations = value;
                RaisePropertyChanged(nameof(BoletaHumidityValidations));
            }
        }
        private List<BoletaHumedadDto> _boletaHumidityValidations;
        
        public bool ImportationErrors
        {
            get { return _importationErrors; }
            set
            {
                _importationErrors = value;
                RaisePropertyChanged(nameof(ImportationErrors));
            }
        }
        private bool _importationErrors;

        public bool EsImportacionManual
        {
            get { return _esImportacionManual; }
            set
            {
                _esImportacionManual = value;
                RaisePropertyChanged(nameof(EsImportacionManual));
            }
        }
        private bool _esImportacionManual;
        
        public BoletaHumedadPagoDto BoletaHumidityPayment
        {
            get { return _boletaHumidityPayment; }
            set
            {
                _boletaHumidityPayment = value;
                RaisePropertyChanged(nameof(BoletaHumidityPayment));
            }
        }
        private BoletaHumedadPagoDto _boletaHumidityPayment;
        
        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                _isPaid = value;
                RaisePropertyChanged(nameof(IsPaid));
            }
        }
        private bool _isPaid;

        public RelayCommand CommandSearchBoletaHumidity { get; set; }
        public RelayCommand CommandSurf { get; set; }
        public RelayCommand CommandRefresh { get; set; }
        public RelayCommand<string> CommandSearchFacility { get; set; }

        public RelayCommand CommandShowBoletasHumidityPanel { get; set; }
        public RelayCommand CommandSaveBoletasHumidityMasive { get; set; }
        public RelayCommand CommandDeleteHumidity { get; set; }
        public RelayCommand<bool> ComandoIndicarTipoImportacion { get; set; }


        private void InicializarComandos()
        {
            CommandSearchBoletaHumidity = new RelayCommand(SearchBoletaHumidity);
            CommandSurf = new RelayCommand(Surf);
            CommandRefresh = new RelayCommand(Surf);
            CommandSearchFacility = new RelayCommand<string>(SearchFacility);
            
            ComandoIndicarTipoImportacion = new RelayCommand<bool>(ActualizarTipoImportacion);

            CommandShowBoletasHumidityPanel = new RelayCommand(ShowBoletasHumidityPanel);
            CommandSaveBoletasHumidityMasive = new RelayCommand(SaveBoletasHumidityMasive);
            CommandDeleteHumidity = new RelayCommand(DeleteHumidity);
        }

        private void DeleteHumidity()
        {
            if (BoletaHumiditeSelected == null) return;

            if (BoletaHumiditeSelected.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta con Humedad debe estar ACTIVA!");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_BoletaHumedad, BoletaHumiditeSelected.NumeroEnvio, BoletaHumiditeSelected.NombrePlanta),
                Titulo = "Eliminar Boleta con Humedad"
            };

            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);

                    var request = new DeleteBoletasHumedad
                    {
                        BoletaHumedadId = BoletaHumiditeSelected.BoletaHumedadId,
                        RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
                    };

                    _client.DeleteAsync(request, res =>
                    {
                        if (!string.IsNullOrWhiteSpace(res.MensajeError))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);

                        LoadBoletasHumidity();

                    }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
                }
            };

            DialogSettings = null;
        }

        private void ShowBoletasHumidityPanel()
        {
            ImportationErrors = false;
            DatoBoletasHumidity = new BoletaHumedadModel
            {
                FechaHumedad = DateTime.Now
            };
            FacilitySelected = new AutoCompleteEntry(null, null);
            XlsColeccionImportacion = null;
            BoletasHumedadModel = new ObservableCollection<BoletaHumedadModel>();

            CargarSlidePanel("AgregarBoletasHumedadFlyout");
        }

        private void ActualizarTipoImportacion(bool esManual)
        {
            EsImportacionManual = esManual;

            if (!esManual)
            {
                BoletasHumedadModel = new ObservableCollection<BoletaHumedadModel>();
            }
        }

        private void SaveBoletasHumidityMasive()
        {
            if (!BoletasHumedadModel.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe información para crear Boletas con Humedad");
                return;
            }

            if (FacilitySelected == null || FacilitySelected.Id == null)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe seleccionar la Planta Destino");
                return;
            }
            
            List<BoletaHumedadDto> boletasHumidity = (from reg in BoletasHumedadModel
                                                     select new BoletaHumedadDto
                                                     {
                                                         NumeroEnvio = reg.NumeroEnvio,
                                                         HumedadPromedio = reg.HumedadPromedio,
                                                         FechaHumedad = reg.FechaHumedad
                                                     }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutBoletasHumedad
            {
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo(),
                FacilityDestination = (int)FacilitySelected.Id,
                BoletasHumedad = boletasHumidity
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PutAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                BoletaHumidityValidations = res;

                if (BoletaHumidityValidations.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Existen Validaciones Pendientes");
                    ImportationErrors = true;
                    return;
                }

                _serviciosComunes.MostrarNotificacion(EventType.Successful, "Datos Importados Satisfactoriamente");
                CargarSlidePanel("AgregarBoletasHumedadFlyout");
                LoadBoletasHumidity();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ProcessImportation()
        {
            if (XlsColeccionImportacion != null && XlsColeccionImportacion.Count >= 0)
            {
                foreach (var item in XlsColeccionImportacion)
                {
                    var type = item.GetType();
                    var elementoDetalle = new BoletaHumedadModel();

                    foreach (var propiedad in type.GetProperties())
                    {
                        AssignImportationData(elementoDetalle, propiedad, item);
                    }

                    var existeBoleta =
                        BoletasHumedadModel.FirstOrDefault(f => f.NumeroEnvio == elementoDetalle.NumeroEnvio);

                    if (existeBoleta == null)
                    {
                        BoletasHumedadModel.Add(elementoDetalle);
                    }
                }

                DatoBoletasHumidity.TotalBoletas = BoletasHumedadModel.Count;

                RaisePropertyChanged(nameof(BoletasHumedadModel));
            }
        }

        private static void AssignImportationData(BoletaHumedadModel elementoDetalle, PropertyInfo propiedad, object item)
        {
            var value = propiedad.GetValue(item, null);

            if (propiedad.Name.ToUpper().Contains("NUMEROENVIO") || propiedad.Name.ToUpper().Contains("NOENVIO"))
            {
                elementoDetalle.NumeroEnvio = value != null ? value.ToString() : string.Empty;
            }

            if (propiedad.Name.ToUpper().Contains("HUMEDADPROMEDIO"))
            {
                elementoDetalle.HumedadPromedio = value != null ? Convert.ToDecimal(value) : 0;
            }

            if (propiedad.Name.ToUpper().Contains("FECHAINGRESO"))
            {
                if (value == null) return;

                elementoDetalle.FechaHumedad = Convert.ToDateTime(value);
            }
        }

        private void SearchFacility(string searchValue)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPlantasPorValorBusqueda
            {
                ValorBusqueda = searchValue
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    ListaPlantasAutoComplete = (from reg in res
                                                select new AutoCompleteEntry(reg.NombrePlanta, reg.PlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void Surf()
        {
            LoadBoletasHumidity();
        }

        private void SearchBoletaHumidity()
        {
            NumeroPagina = 1;
            LoadBoletasHumidity();
        }

        private void LoadBoletasHumidity()
        {
            var boletaHumedadId = BoletaHumiditeSelected != null ? BoletaHumiditeSelected.BoletaHumedadId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorBoletasHumedad
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 18,
                Filtro = SearchValue
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BoletasHumidityControl = res;

                if (BoletasHumidityControl.Items != null && BoletasHumidityControl.Items.Any())
                {
                    if (boletaHumedadId == 0)
                    {
                        BoletaHumiditeSelected = BoletasHumidityControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        BoletaHumiditeSelected = BoletasHumidityControl.Items.FirstOrDefault(r => r.BoletaHumedadId == boletaHumedadId);
                    }

                    RaisePropertyChanged("BoletaSeleccionada");
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GetBoletaHumidityPayment()
        {
            if (BoletaHumiditeSelected == null) return;

            IsPaid = false;
            BoletaHumidityPayment = new BoletaHumedadPagoDto();
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetHumidityPayment
            {
                BoletaHumedadId = BoletaHumiditeSelected.BoletaHumedadId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BoletaHumidityPayment = res;

                if (BoletaHumidityPayment != null && BoletaHumidityPayment.BoletaId > 0) IsPaid = true;
                RaisePropertyChanged(nameof(BoletaHumidityPayment));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void InicializarPropiedades()
        {
            BoletasHumidityControl = new BusquedaBoletasHumedadDto();
        }

        private void CargarDatosIniciales()
        {
            LoadBoletasHumidity();
        }
        
        private void CargarDatosPruebas()
        {
            BoletasHumidityControl = new BusquedaBoletasHumedadDto
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListadoBoletasHumedad()
            };

            BoletaHumiditeSelected = BoletasHumidityControl.Items.FirstOrDefault();
        }
    }
}
