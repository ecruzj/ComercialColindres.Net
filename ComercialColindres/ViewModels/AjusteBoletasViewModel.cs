using ComercialColindres.Busquedas;
using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;
using ComercialColindres.DTOs.RequestDTOs.AjusteTipos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPFCore.Modelos;

namespace ComercialColindres.ViewModels
{
    public class AjusteBoletasViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public AjusteBoletasViewModel(IServiciosComunes serviciosComunes)
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
        
        public BusquedaAjusteBoletas AjusteBoletasControl
        {
            get { return _controlAjusteBoletas; }
            set
            {
                _controlAjusteBoletas = value;
                RaisePropertyChanged(nameof(AjusteBoletasControl));
            }
        }
        private BusquedaAjusteBoletas _controlAjusteBoletas;

        public AjusteBoletaDto AjusteBoletaSelected
        {
            get { return _ajusteBoletaSelected; }
            set
            {
                _ajusteBoletaSelected = value;
                RaisePropertyChanged(nameof(AjusteBoletaSelected));

                if (IsInDesignMode) return;

                LoadAjusteBoletaDetalles();
            }
        }
        private AjusteBoletaDto _ajusteBoletaSelected;

        public List<AjusteBoletaDetalleDto> AjusteBoletaDetalles
        {
            get { return _ajusteBoletaDetalles; }
            set
            {
                _ajusteBoletaDetalles = value;
                RaisePropertyChanged(nameof(AjusteBoletaDetalles));
            }
        }
        private List<AjusteBoletaDetalleDto> _ajusteBoletaDetalles;

        public AjusteBoletaDetalleDto AjusteBoletaDetalleSelected
        {
            get { return _ajusteBoletaDetalleSelected; }
            set
            {
                _ajusteBoletaDetalleSelected = value;
                RaisePropertyChanged(nameof(AjusteBoletaDetalleSelected));

                if (IsInDesignMode) return;

                if (AjusteBoletaDetalleSelected == null) return;
                LoadAjusteBoletaPagos();
            }
        }
        private AjusteBoletaDetalleDto _ajusteBoletaDetalleSelected;

        public ObservableCollection<AjusteBoletaDetallesModel> AjusteBoletaDetallesModel
        {
            get { return _ajusteBoletaDetallesModel; }
            set
            {
                _ajusteBoletaDetallesModel = value;
                RaisePropertyChanged(nameof(AjusteBoletaDetallesModel));
            }
        }
        private ObservableCollection<AjusteBoletaDetallesModel> _ajusteBoletaDetallesModel;

        public AjusteBoletaDetallesModel AjusteBoletaDetalleSelectedModel
        {
            get { return _ajusteBoletaDetalleSelectedModel; }
            set
            {
                _ajusteBoletaDetalleSelectedModel = value;
                RaisePropertyChanged(nameof(AjusteBoletaDetalleSelectedModel));
            }
        }
        private AjusteBoletaDetallesModel _ajusteBoletaDetalleSelectedModel;

        public AjusteBoletaDetallesModel AjusteBoletaDetalleDato
        {
            get { return _ajusteBoletaDetalleDato; }
            set
            {
                _ajusteBoletaDetalleDato = value;
                RaisePropertyChanged(nameof(AjusteBoletaDetalleDato));
            }
        }
        private AjusteBoletaDetallesModel _ajusteBoletaDetalleDato;

        public List<AjusteTipoDto> AjusteTipos
        {
            get { return _ajusteTipos; }
            set
            {
                _ajusteTipos = value;
                RaisePropertyChanged(nameof(AjusteTipos));
            }
        }
        private List<AjusteTipoDto> _ajusteTipos;

        public AjusteTipoDto AjusteTipoSelected
        {
            get { return _ajusteTipoSelected; }
            set
            {
                _ajusteTipoSelected = value;
                RaisePropertyChanged(nameof(AjusteTipoSelected));

                if (AjusteTipoSelected == null) return;
                if (AjusteBoletaDetalleDato == null) return;

                AjusteBoletaDetalleDato.AjusteTipoDescripcion = AjusteTipoSelected.Descripcion;
                RaisePropertyChanged(nameof(AjusteBoletaDetalleDato));
            }
        }
        private AjusteTipoDto _ajusteTipoSelected;

        public List<AjusteBoletaPagoDto> AjusteBoletaPagos
        {
            get { return _ajusteBoletaPagos; }
            set
            {
                _ajusteBoletaPagos = value;
                RaisePropertyChanged(nameof(AjusteBoletaPagos));
            }
        }
        private List<AjusteBoletaPagoDto> _ajusteBoletaPagos;

        public BusquedaBoletas BusquedaBoletasControl
        {
            get
            {
                return _busquedaBoletasControl;
            }
            set
            {
                _busquedaBoletasControl = value;
                RaisePropertyChanged(nameof(BusquedaBoletasControl));
            }
        }
        private BusquedaBoletas _busquedaBoletasControl;

        public BoletasDTO DatoBoleta
        {
            get { return _datoBoleta; }
            set
            {
                _datoBoleta = value;
                RaisePropertyChanged(nameof(DatoBoleta));
            }
        }
        private BoletasDTO _datoBoleta;

        public List<BancosDTO> ListadoBancos
        {
            get
            {
                return _listadoBancos;
            }
            set
            {
                _listadoBancos = value;
                RaisePropertyChanged(nameof(ListadoBancos));
            }
        }
        private List<BancosDTO> _listadoBancos;

        public BancosDTO BancoSeleccionado
        {
            get
            {
                return _bancoSeleccionado;
            }
            set
            {
                _bancoSeleccionado = value;
                RaisePropertyChanged(nameof(BancoSeleccionado));

                if (BancoSeleccionado == null) return;

                AjusteBoletaDetalleDato.BancoId = BancoSeleccionado.BancoId;
                AjusteBoletaDetalleDato.NombreBanco = BancoSeleccionado.Descripcion;

                if (FormaDePagoSeleccionada == null) return;
                CargarLineasDeCreditoPorBanco(AjusteBoletaDetalleDato.BancoId, FormaDePagoSeleccionada.CuentaFinancieraTipoId);
            }
        }
        private BancosDTO _bancoSeleccionado;

        public List<CuentasFinancieraTiposDTO> ListaFormaPagos
        {
            get
            {
                return _listaFormaPagos;
            }
            set
            {
                _listaFormaPagos = value;
                RaisePropertyChanged(nameof(ListaFormaPagos));
            }
        }
        private List<CuentasFinancieraTiposDTO> _listaFormaPagos;

        public CuentasFinancieraTiposDTO FormaDePagoSeleccionada
        {
            get
            {
                return _formaDePagoSeleccionada;
            }
            set
            {
                _formaDePagoSeleccionada = value;
                RaisePropertyChanged(nameof(FormaDePagoSeleccionada));

                if (FormaDePagoSeleccionada == null) return;

                if (!FormaDePagoSeleccionada.RequiereBanco)
                {
                    CargarLineasDeCreditoCajaChica();
                    return;
                }

                LineasCredito = new List<LineasCreditoDTO>();
                LineaCreditoSeleccionada = new LineasCreditoDTO();
                BancoSeleccionado = new BancosDTO();

            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;

        public List<LineasCreditoDTO> LineasCredito
        {
            get { return _lineasCredito; }
            set
            {
                _lineasCredito = value;
                RaisePropertyChanged(nameof(LineasCredito));
            }
        }
        private List<LineasCreditoDTO> _lineasCredito;

        public LineasCreditoDTO LineaCreditoSeleccionada
        {
            get { return _lineaCreditoSeleccionada; }
            set
            {
                _lineaCreditoSeleccionada = value;
                RaisePropertyChanged(nameof(LineaCreditoSeleccionada));

                if (LineaCreditoSeleccionada == null) return;

                AjusteBoletaDetalleDato.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                AjusteBoletaDetalleDato.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;

        public RelayCommand CommandSearchAjusteBoleta { get; set; }
        public RelayCommand CommandSurf { get; set; }
        public RelayCommand CommandRefresh { get; set; }

        public RelayCommand CommandShowCreateAjuste { get; set; }
        public RelayCommand CommandShowSearchBoleta { get; set; }
        public RelayCommand CommandSearchBoleta { get; set; }
        public RelayCommand CommandCreateAjuste { get; set; }
        public RelayCommand<object> CommandSelectBoleta { get; set; }

        public RelayCommand CommandShowAjusteDetalles { get; set; }
        public RelayCommand CommandAddAjusteDetalle { get; set; }
        public RelayCommand CommandEditarAjusteDetalle { get; set; }
        public RelayCommand CommandDeleteAjusteDetalle { get; set; }
        public RelayCommand CommandSaveAjusteDetalles { get; set; }

        public RelayCommand CommandActivateAjusteBoleta { get; set; }


        private void InicializarComandos()
        {
            CommandSearchAjusteBoleta = new RelayCommand(SearchAjusteBoleta);
            CommandSurf = new RelayCommand(Surf);
            CommandRefresh = new RelayCommand(Surf);

            CommandShowCreateAjuste = new RelayCommand(ShowCreateAjuste);
            CommandSearchBoleta = new RelayCommand(SearchBoleta);
            CommandCreateAjuste = new RelayCommand(CreateAjuste);
            CommandSelectBoleta = new RelayCommand<object>(SelectBoleta);
            CommandShowSearchBoleta = new RelayCommand(ShowSearchBoleta);

            CommandShowAjusteDetalles = new RelayCommand(ShowAjusteDetalles);
            CommandAddAjusteDetalle = new RelayCommand(AddAjusteDetalle);
            CommandEditarAjusteDetalle = new RelayCommand(EditarAjusteDetalle);
            CommandDeleteAjusteDetalle = new RelayCommand(DeleteAjusteDetalle);
            CommandSaveAjusteDetalles = new RelayCommand(SaveAjusteDetalles);

            CommandActivateAjusteBoleta = new RelayCommand(ActivateAjusteBoleta);
        }

        private void CargarLineasDeCreditoCajaChica()
        {
            LineasCredito = new List<LineasCreditoDTO>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetLineasCreditoCajaChica
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                LineasCredito = res;

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarLineasDeCreditoPorBanco(int bancoId, int cuentaFinancieraTipoId)
        {
            LineasCredito = new List<LineasCreditoDTO>();

            if (bancoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetLineasCreditoPorBancoPorTipoCuenta
            {
                BancoId = bancoId,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId,
                CuentaFinancieraTipoId = cuentaFinancieraTipoId
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                LineasCredito = res;

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadBanks()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllBancos
            {
            };

            _client.GetAsync(request, res =>
            {
                ListadoBancos = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void LoadWayPayments()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllTiposCuentasFinancieras
            {
            };

            _client.GetAsync(request, res =>
            {
                ListaFormaPagos = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void ActivateAjusteBoleta()
        {
            if (AjusteBoletaSelected == null) return;

            if (AjusteBoletaSelected.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado del ajuste debe ser Nuevo");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Activar_AjusteBoleta, AjusteBoletaSelected.CodigoBoleta),
                Titulo = "Boletas"
            };

            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PostActiveAjusteBoleta
                    {
                        AjusteBoletaId = AjusteBoletaSelected.AjusteBoletaId,
                        RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
                    };
                    MostrarVentanaEspera = Visibility.Visible;
                    _client.PostAsync(request, res =>
                    {
                        MostrarVentanaEspera = Visibility.Collapsed;
                        if (!string.IsNullOrWhiteSpace(res.ValidationErrorMessage))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.ValidationErrorMessage);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                        LoadAjusteBoleta();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEspera = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };

            DialogSettings = null;            
        }

        private void SaveAjusteDetalles()
        {
            if (AjusteBoletaSelected == null) return;

            List<AjusteBoletaDetalleDto> datos = (from reg in AjusteBoletaDetallesModel
                        select new AjusteBoletaDetalleDto
                        {
                            AjusteBoletaDetalleId = reg.AjusteBoletaDetalleId,
                            AjusteBoletaId = reg.AjusteBoletaId,
                            AjusteTipoDescripcion = reg.AjusteTipoDescripcion,
                            AjusteTipoId = reg.AjusteTipoId,
                            FechaCreacion = reg.FechaCreacion,
                            ModificadoPor = reg.ModificadoPor,
                            Monto = reg.Monto,
                            Observaciones = reg.Observaciones,
                            LineaCreditoId = reg.LineaCreditoId,
                            NoDocumento = reg.NoDocumento
                        }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostAjusteBoletaDetalle
            {
                AjusteBoletaDetalles = datos,
                AjusteBoletaId = AjusteBoletaSelected.AjusteBoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(res.ValidationErrorMessage))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.ValidationErrorMessage);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("AjusteBoletaDetallesFlyout");
                LoadAjusteBoleta();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void DeleteAjusteDetalle()
        {
            if (AjusteBoletaDetalleSelectedModel == null) return;

            AjusteBoletaDetallesModel.Remove(AjusteBoletaDetalleSelectedModel);

            CleanAjusteDetalleDato();
        }

        private void EditarAjusteDetalle()
        {
            if (AjusteBoletaDetalleSelectedModel == null) return;

            AjusteBoletaDetalleDato = new AjusteBoletaDetallesModel
            {
                AjusteBoletaDetalleId = AjusteBoletaDetalleSelectedModel.AjusteBoletaDetalleId,
                AjusteBoletaId = AjusteBoletaDetalleSelectedModel.AjusteBoletaId,
                AjusteTipoId = AjusteBoletaDetalleSelectedModel.AjusteTipoId,
                AjusteTipoDescripcion = AjusteBoletaDetalleSelectedModel.AjusteTipoDescripcion,
                FechaCreacion = AjusteBoletaDetalleSelectedModel.FechaCreacion,
                Monto = AjusteBoletaDetalleSelectedModel.Monto,
                Observaciones = AjusteBoletaDetalleSelectedModel.Observaciones
            };
        }

        private void AddAjusteDetalle()
        {
            var ajusteDetail = AjusteBoletaDetallesModel.FirstOrDefault(d => d.AjusteTipoId == AjusteBoletaDetalleDato.AjusteTipoId);

            if (ajusteDetail == null)
            {
                AjusteBoletaDetallesModel.Add(AjusteBoletaDetalleDato);
            }
            else
            {
                ajusteDetail.Monto = AjusteBoletaDetalleDato.Monto;
                ajusteDetail.Observaciones = AjusteBoletaDetalleDato.Observaciones;
                ajusteDetail.BancoId = AjusteBoletaDetalleDato.BancoId;
                ajusteDetail.NombreBanco = AjusteBoletaDetalleDato.NombreBanco;
                ajusteDetail.LineaCreditoId = AjusteBoletaDetalleDato.LineaCreditoId;
                ajusteDetail.CodigoLineaCredito = AjusteBoletaDetalleDato.CodigoLineaCredito;
                ajusteDetail.NoDocumento = AjusteBoletaDetalleDato.NoDocumento;
                ajusteDetail.AjusteTipoDescripcion = AjusteBoletaDetalleDato.AjusteTipoDescripcion;
            }

            CleanAjusteDetalleDato();
        }

        private void CleanAjusteDetalleDato()
        {
            AjusteTipoSelected = new AjusteTipoDto();
            AjusteBoletaDetalleDato = new AjusteBoletaDetallesModel();
            BancoSeleccionado = new BancosDTO();
            LineaCreditoSeleccionada = new LineasCreditoDTO();
            FormaDePagoSeleccionada = new CuentasFinancieraTiposDTO();
        }

        private void ShowAjusteDetalles()
        {
            if (AjusteBoletaSelected == null) return;

            if (AjusteBoletaSelected.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado del ajuste debe ser nuevo");
                return;
            }

            IEnumerable<AjusteBoletaDetallesModel> details = from reg in AjusteBoletaDetalles
                                                       select new AjusteBoletaDetallesModel
                                                       {
                                                           AjusteBoletaDetalleId = reg.AjusteBoletaDetalleId,
                                                           AjusteBoletaId = AjusteBoletaSelected.AjusteBoletaId,
                                                           AjusteTipoDescripcion = reg.AjusteTipoDescripcion,
                                                           AjusteTipoId = reg.AjusteTipoId,
                                                           FechaCreacion = reg.FechaCreacion,
                                                           ModificadoPor = reg.ModificadoPor,
                                                           Monto = reg.Monto,
                                                           Observaciones = reg.Observaciones,
                                                           BancoId = reg.BancoId,
                                                           NombreBanco = reg.NombreBanco,
                                                           LineaCreditoId = reg.LineaCreditoId,
                                                           CodigoLineaCredito = reg.CodigoLineaCredito,
                                                           NoDocumento = reg.NoDocumento
                                                       };

            AjusteBoletaDetallesModel = new ObservableCollection<AjusteBoletaDetallesModel>(details);
            CleanAjusteDetalleDato();
            AjusteBoletaDetalleDato = new AjusteBoletaDetallesModel
            {
                AjusteBoletaId = AjusteBoletaSelected.AjusteBoletaId,
                FechaCreacion = DateTime.Now
            };

            CargarSlidePanel("AjusteBoletaDetallesFlyout");
        }

        private void ShowSearchBoleta()
        {
            BusquedaBoletasControl = new BusquedaBoletas();
            BusquedaBoletasControl.DatosEncontrados = new ObservableCollection<BoletasDTO>();

            RaisePropertyChanged(nameof(BusquedaBoletasControl));
            CargarSlidePanel("BusquedaBoletasFlyout");
        }

        private void ShowCreateAjuste()
        {
            DatoBoleta = new BoletasDTO();
            CargarSlidePanel("CreateBoletaAjustesFlyout");
        }

        private void SelectBoleta(object value)
        {
            BoletasDTO boletaRetorno = (BoletasDTO)value;

            if (boletaRetorno == null) return;

            DatoBoleta.BoletaId = boletaRetorno.BoletaId;
            DatoBoleta.CodigoBoleta = boletaRetorno.CodigoBoleta;
            DatoBoleta.NumeroEnvio = !string.IsNullOrWhiteSpace(boletaRetorno.NumeroEnvio) ? boletaRetorno.NumeroEnvio : "N/A";
            DatoBoleta.NombrePlanta = boletaRetorno.NombrePlanta;
            DatoBoleta.Motorista = boletaRetorno.Motorista;
            DatoBoleta.PlacaEquipo = boletaRetorno.PlacaEquipo;
            DatoBoleta.NombreProveedor = boletaRetorno.NombreProveedor;
            DatoBoleta.DescripcionTipoProducto = boletaRetorno.DescripcionTipoProducto;
            DatoBoleta.FechaSalida = boletaRetorno.FechaSalida;

            RaisePropertyChanged(nameof(DatoBoleta));
            CargarSlidePanel("BusquedaBoletasFlyout");
        }

        private void SearchBoleta()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorBoletas
            {
                PaginaActual = 1,
                CantidadRegistros = 18,
                Filtro = BusquedaBoletasControl.ValorBusqueda
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                BusquedaBoletasControl.DatosEncontrados = new ObservableCollection<BoletasDTO>(res.Items);
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CreateAjuste()
        {
            if (DatoBoleta == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostAjusteBoleta
            {
                BoletaId = DatoBoleta.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(res.ValidationErrorMessage))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.ValidationErrorMessage);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("CreateBoletaAjustesFlyout");
                LoadAjusteBoleta();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
                
        private void SearchAjusteBoleta()
        {
            NumeroPagina = 1;
            LoadAjusteBoleta();
        }

        private void Surf()
        {
            LoadAjusteBoleta();
        }

        private void LoadAjusteBoleta()
        {
            var ajusteBoletaId = AjusteBoletaSelected != null ? AjusteBoletaSelected.AjusteBoletaId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorAjusteBoletas
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 18,
                Filtro = SearchValue
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                AjusteBoletasControl = res;

                if (AjusteBoletasControl.Items != null && AjusteBoletasControl.Items.Any())
                {
                    if (ajusteBoletaId == 0)
                    {
                        AjusteBoletaSelected = AjusteBoletasControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        AjusteBoletaSelected = AjusteBoletasControl.Items.FirstOrDefault(r => r.AjusteBoletaId == ajusteBoletaId);
                    }

                    RaisePropertyChanged(nameof(AjusteBoletaSelected));
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadAjusteBoletaDetalles()
        {
            if (AjusteBoletaSelected == null) return;

            AjusteBoletaDetalles = new List<AjusteBoletaDetalleDto>();
            AjusteBoletaPagos = new List<AjusteBoletaPagoDto>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAjusteBoletaDetalleByAjusteBoletaId
            {
                AjusteBoletaId = AjusteBoletaSelected.AjusteBoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                AjusteBoletaDetalles = res;
                RaisePropertyChanged(nameof(AjusteBoletaDetalles));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadAjusteBoletaPagos()
        {
            if (AjusteBoletaDetalleSelected == null) return;

            AjusteBoletaPagos = new List<AjusteBoletaPagoDto>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAjusteBoletaPagoByDetailId
            {
                AjusteBoletaDetalleId = AjusteBoletaDetalleSelected.AjusteBoletaDetalleId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                AjusteBoletaPagos = res;
                RaisePropertyChanged(nameof(AjusteBoletaPagos));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadAjusteTipos()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllAjusteTipos
            {
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                AjusteTipos = res;
                RaisePropertyChanged(nameof(AjusteTipos));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void InicializarPropiedades()
        {
            AjusteBoletasControl = new BusquedaAjusteBoletas();
        }

        private void CargarDatosIniciales()
        {
            LoadAjusteBoleta();
            LoadAjusteTipos();
            LoadBanks();
            LoadWayPayments();
        }

        private void CargarDatosPruebas()
        {
            AjusteBoletasControl = new BusquedaAjusteBoletas
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListadoAjusteBoletas()
            };

            AjusteBoletaSelected = AjusteBoletasControl.Items.FirstOrDefault();
        }
    }
}
