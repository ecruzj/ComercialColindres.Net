using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
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
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels
{
    public class DescargadoresViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public DescargadoresViewModel(IServiciosComunes serviciosComunes)
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
            CargarDatosIniciales();
            InicializarPropiedades();
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
                    RaisePropertyChanged("MostrarVentanaEspera");
                }
            }
        }
        private Visibility _mostrarVentanaEspera = Visibility.Collapsed;

        public int NumeroPaginaDescargas
        {
            get
            {
                return _numeroPaginaDescargas;
            }
            set
            {
                _numeroPaginaDescargas = value;
                RaisePropertyChanged("NumeroPaginaDescargas");
            }
        }
        int _numeroPaginaDescargas = 1;

        public int NumeroPaginaPagoDescargas
        {
            get { return _numeroPaginaPagoDescargas; }
            set
            {
                _numeroPaginaPagoDescargas = value;
                RaisePropertyChanged("NumeroPaginaPagoDescargas");
            }
        }
        private int _numeroPaginaPagoDescargas = 1;

        public BusquedaDescargadoresDTO BusquedaDescargasControl
        {
            get
            {
                return _busquedaDescargasControl;
            }
            set
            {
                _busquedaDescargasControl = value;
                RaisePropertyChanged("BusquedaDescargasControl");
            }
        }
        private BusquedaDescargadoresDTO _busquedaDescargasControl;

        public DescargadoresDTO DescargaSeleccionada
        {
            get
            {
                return _descargaSeleccionada;
            }
            set
            {
                _descargaSeleccionada = value;
                RaisePropertyChanged("DescargaSeleccionada");
            }
        }
        private DescargadoresDTO _descargaSeleccionada;

        public BusquedaPagoDescargadoresDTO BusquedaPagoDescargadoresControl
        {
            get
            {
                return _busquedaPagoDescargadoresControl;
            }
            set
            {
                _busquedaPagoDescargadoresControl = value;
                RaisePropertyChanged("BusquedaPagoDescargadoresControl");
            }
        }
        private BusquedaPagoDescargadoresDTO _busquedaPagoDescargadoresControl;

        public PagoDescargadoresDTO PagoDescargadoresSeleccionado
        {
            get
            {
                return _pagoDescargadoresSeleccionado;
            }
            set
            {
                _pagoDescargadoresSeleccionado = value;
                RaisePropertyChanged("PagoDescargadoresSeleccionado");

                if (IsInDesignMode)
                {
                    return;
                }

                CargarDescargasPorPago();
                CargarPagosDescargasDetalle();
                CargarDescargasPorAdelanto();
            }
        }
        private PagoDescargadoresDTO _pagoDescargadoresSeleccionado;

        public List<DescargadoresDTO> ListaDescargasPorPago
        {
            get { return _listaDescargasPorPago; }
            set
            {
                _listaDescargasPorPago = value;
                RaisePropertyChanged(nameof(ListaDescargasPorPago));
            }
        }
        private List<DescargadoresDTO> _listaDescargasPorPago;

        public CuadrillasDTO CuadrillaSeleccionada
        {
            get
            {
                return _cuadrillaSeleccionada;
            }
            set
            {
                _cuadrillaSeleccionada = value;
                RaisePropertyChanged("CuadrillaSeleccionada");

                CargarDescargas();
            }
        }
        private CuadrillasDTO _cuadrillaSeleccionada;

        public string ValorBusquedaDescarga
        {
            get
            {
                return _valorBusquedaDescarga;
            }
            set
            {
                _valorBusquedaDescarga = value;
                RaisePropertyChanged("ValorBusquedaDescarga");
            }
        }
        private string _valorBusquedaDescarga;

        public string ValorBusquedaPagoDescarga
        {
            get { return _valorBusquedaPagoDescarga; }
            set
            {
                _valorBusquedaPagoDescarga = value;
                RaisePropertyChanged("ValorBusquedaPagoDescarga");
            }
        }
        private string _valorBusquedaPagoDescarga;

        public List<BancosDTO> ListadoBancos
        {
            get
            {
                return _listadoBancos;
            }
            set
            {
                _listadoBancos = value;
                RaisePropertyChanged("ListadoBancos");
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
                RaisePropertyChanged("BancoSeleccionado");

                if (BancoSeleccionado == null) return;
                DatoPagoDescargaDetalle.BancoId = BancoSeleccionado.BancoId;
                DatoPagoDescargaDetalle.NombreBanco = BancoSeleccionado.Descripcion;

                if (FormaDePagoSeleccionada == null) return;
                CargarLineasDeCreditoPorBanco();
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
                RaisePropertyChanged("ListaFormaPagos");
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
                RaisePropertyChanged("FormaDePagoSeleccionada");

                if (FormaDePagoSeleccionada == null) return;

                if (!FormaDePagoSeleccionada.RequiereBanco)
                {
                    CargarLineasDeCreditoCajaChica();
                }

                if (!DatoPagoDescargaDetalle.EsEdicion)
                {
                    DatoPagoDescargaDetalle = new PagoDescargaDetallesModel
                    {
                        EsEdicion = false,
                        PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                        CantidadJustificada = ListaPagoDescargaDetalleModel.Sum(c => c.Monto)
                    };

                    LineasCredito = new List<LineasCreditoDTO>();
                    LineaCreditoSeleccionada = new LineasCreditoDTO();
                    BancoSeleccionado = new BancosDTO();
                }
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;

        public PagoDescargadoresDTO DatoPagoDescargadores
        {
            get
            {
                return _datoPagoDescargadores;
            }
            set
            {
                _datoPagoDescargadores = value;
                RaisePropertyChanged("DatoPagoDescargadores");
            }
        }
        private PagoDescargadoresDTO _datoPagoDescargadores;

        public PagoDescargaDetallesModel DatoPagoDescargaDetalle
        {
            get
            {
                return _datoPagoDescargaDetalle;
            }
            set
            {
                _datoPagoDescargaDetalle = value;
                RaisePropertyChanged("DatoPagoDescargaDetalle");
            }
        }
        private PagoDescargaDetallesModel _datoPagoDescargaDetalle;

        public List<PagoDescargaDetallesDTO> ListaPagoDescargasDetalle
        {
            get
            {
                return _listaPagoDescargasDetalle;
            }
            set
            {
                _listaPagoDescargasDetalle = value;
                RaisePropertyChanged("ListaPagoDescargasDetalle");
            }
        }
        private List<PagoDescargaDetallesDTO> _listaPagoDescargasDetalle;

        public ObservableCollection<PagoDescargaDetallesModel> ListaPagoDescargaDetalleModel
        {
            get
            {
                return _listaPagoDescargaDetalleModel;
            }
            set
            {
                _listaPagoDescargaDetalleModel = value;
                RaisePropertyChanged("ListaPagoDescargaDetalleModel");
            }
        }
        private ObservableCollection<PagoDescargaDetallesModel> _listaPagoDescargaDetalleModel;

        public PagoDescargaDetallesModel PagoDescargaDetalleSeleccionado
        {
            get
            {
                return _pagoDescargaDetalleSeleccionado;
            }
            set
            {
                _pagoDescargaDetalleSeleccionado = value;
                RaisePropertyChanged("PagoDescargaDetalleSeleccionado");
            }
        }
        private PagoDescargaDetallesModel _pagoDescargaDetalleSeleccionado;

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

        public AutoCompleteEntry PlantaAutoCompleteSeleccionada
        {
            get
            {
                return _plantaAutoCompleteSeleccionada;
            }
            set
            {
                _plantaAutoCompleteSeleccionada = value;
                RaisePropertyChanged("PlantaAutoCompleteSeleccionada");

                if (_plantaAutoCompleteSeleccionada != null && _plantaAutoCompleteSeleccionada.Id != null)
                {
                    Planta = Plantas.FirstOrDefault(p => p.PlantaId == (int)PlantaAutoCompleteSeleccionada.Id);
                    IsPlantaSelected = Planta != null;
                }

                if (DatoPagoDescargadores == null || Planta == null) return;
                DatoPagoDescargadores.PlantaId = Planta.PlantaId;
                RaisePropertyChanged("DatoPagoDescargadores");
            }
        }
        private AutoCompleteEntry _plantaAutoCompleteSeleccionada;

        public List<ClientePlantasDTO> Plantas
        {
            get { return _plantas; }
            set
            {
                _plantas = value;
                RaisePropertyChanged(nameof(Plantas));
            }
        }
        private List<ClientePlantasDTO> _plantas;

        public ClientePlantasDTO Planta
        {
            get { return _planta; }
            set
            {
                _planta = value;
                RaisePropertyChanged(nameof(Planta));
            }
        }
        private ClientePlantasDTO _planta;

        public bool IsPlantaSelected
        {
            get { return _isPlantaSelected; }
            set
            {
                _isPlantaSelected = value;
                RaisePropertyChanged(nameof(IsPlantaSelected));
            }
        }
        private bool _isPlantaSelected;

        public List<AutoCompleteEntry> ListaCuadrillasAutoComplete
        {
            get
            {
                return _listaCuadrillasAutoComplete;
            }
            set
            {
                _listaCuadrillasAutoComplete = value;
                RaisePropertyChanged("ListaCuadrillasAutoComplete");
            }
        }
        private List<AutoCompleteEntry> _listaCuadrillasAutoComplete;

        public AutoCompleteEntry CuadrillaAutoCompleteSeleccionado
        {
            get
            {
                return _cuadrillaAutoCompleteSeleccionado;
            }
            set
            {
                _cuadrillaAutoCompleteSeleccionado = value;
                RaisePropertyChanged(nameof(CuadrillaAutoCompleteSeleccionado));

                if (DatoPagoDescargadores == null) return;
                DatoPagoDescargadores.CuadrillaId = CuadrillaAutoCompleteSeleccionado != null &&
                                                 CuadrillaAutoCompleteSeleccionado.Id != null ?
                                                 (int)CuadrillaAutoCompleteSeleccionado.Id : 0;
                RaisePropertyChanged(nameof(DatoPagoDescargadores));
            }
        }
        private AutoCompleteEntry _cuadrillaAutoCompleteSeleccionado;
        
        public DescargadoresDTO DatoDescargadores
        {
            get
            {
                return _datoDescargadores;
            }
            set
            {
                _datoDescargadores = value;
                RaisePropertyChanged("DatoDescargadores");
            }
        }
        private DescargadoresDTO _datoDescargadores;

        public bool EsFacturacion
        {
            get
            {
                return _esFacturacion;
            }
            set
            {
                _esFacturacion = value;
                RaisePropertyChanged("EsFacturacion");
            }
        }
        private bool _esFacturacion;

        public ReportePagoDescargadoresResumen ReportePagoDescargadores
        {
            get { return _reportePagoDescargadores; }
            set
            {
                _reportePagoDescargadores = value;
                RaisePropertyChanged("ReportePagoDescargadores");
            }
        }
        private ReportePagoDescargadoresResumen _reportePagoDescargadores;

        public List<ReporteDescargadoresDTO> ReporteDescargas
        {
            get { return _reporteDescargas; }
            set
            {
                _reporteDescargas = value;
                RaisePropertyChanged("ReporteDescargas");
            }
        }
        private List<ReporteDescargadoresDTO> _reporteDescargas;


        public List<LineasCreditoDTO> LineasCredito
        {
            get { return _lineasCredito; }
            set
            {
                _lineasCredito = value;
                RaisePropertyChanged("LineasCredito");
            }
        }
        private List<LineasCreditoDTO> _lineasCredito;

        public LineasCreditoDTO LineaCreditoSeleccionada
        {
            get { return _lineaCreditoSeleccionada; }
            set
            {
                _lineaCreditoSeleccionada = value;
                RaisePropertyChanged("LineaCreditoSeleccionada");

                if (LineaCreditoSeleccionada == null) return;
                DatoPagoDescargaDetalle.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                DatoPagoDescargaDetalle.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;

        public List<DescargasPorAdelantadoDTO> DescargasPorAdelantado
        {
            get { return _descargasPorAdelantado; }
            set
            {
                _descargasPorAdelantado = value;
                RaisePropertyChanged("DescargasPorAdelantado");
            }
        }
        private List<DescargasPorAdelantadoDTO> _descargasPorAdelantado;

        public ObservableCollection<DescargasPorAdelantadoModel> DescargasPorAdelantoModel
        {
            get { return _descargasPorAdelantoModel; }
            set
            {
                _descargasPorAdelantoModel = value;
                RaisePropertyChanged("DescargasPorAdelantoModel");
            }
        }
        private ObservableCollection<DescargasPorAdelantadoModel> _descargasPorAdelantoModel;

        public DescargasPorAdelantadoModel DatoDescargaPorAdelantado
        {
            get { return _datoDescargaPorAdelantado; }
            set
            {
                _datoDescargaPorAdelantado = value;
                RaisePropertyChanged("DatoDescargaPorAdelantado");
            }
        }
        private DescargasPorAdelantadoModel _datoDescargaPorAdelantado;

        public DescargasPorAdelantadoModel DescargaPorAdelantadoSeleccionado
        {
            get { return _descargaPorAdelantadoSeleccionado; }
            set
            {
                _descargaPorAdelantadoSeleccionado = value;
                RaisePropertyChanged("DescargaPorAdelantadoSeleccionado");
            }
        }
        private DescargasPorAdelantadoModel _descargaPorAdelantadoSeleccionado;

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

        public ObservableCollection<PagoDescargasModel> PagoDescargasModel
        {
            get { return _pagoDescargasModel; }
            set
            {
                _pagoDescargasModel = value;
                RaisePropertyChanged(nameof(PagoDescargasModel));
            }
        }
        private ObservableCollection<PagoDescargasModel> _pagoDescargasModel;

        public List<PagoDescargadoresDTO> PagoDescargasErrors
        {
            get { return _pagoDescargasErrors; }
            set
            {
                _pagoDescargasErrors = value;
                RaisePropertyChanged(nameof(PagoDescargasErrors));
            }
        }
        private List<PagoDescargadoresDTO> _pagoDescargasErrors;

        public RelayCommand ComandoMostrarDescargas { get; set; }
        public RelayCommand ComandoBuscarDescarga { get; set; }
        public RelayCommand ComandoActualizarDescarga { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }
        public RelayCommand ComandoNavegarDescargas { get; set; }

        public RelayCommand ComandoMostrarFacturaciones { get; set; }
        public RelayCommand ComandoBuscarPagoDescargas { get; set; }
        public RelayCommand ComandoMostrarPagoDescargas { get; set; }
        public RelayCommand CommandShowEditPagoDescargas { get; set; }
        public RelayCommand ComandoGuardarPagoDescargas { get; set; }
        public RelayCommand ComandoEliminarPagoDescargas { get; set; }
        public RelayCommand ComandoActualizarDescargas { get; set; }
        public RelayCommand ComandoRefrescarPagos { get; set; }
        public RelayCommand ComandoNavegarPagoDescargas { get; set; }

        public RelayCommand<string> ComandoBuscarPlantas { get; set; }
        public RelayCommand<string> ComandoBuscarCuadrillas { get; set; }
        public RelayCommand ComandoMostrarPagoDescargasDetalle { get; set; }
        public RelayCommand ComandoRemoverPagoDescargaDetalle { get; set; }
        public RelayCommand ComandoEditarPagoDescargaDetalle { get; set; }
        public RelayCommand ComandoAgregarPagoDescargaDetalle { get; set; }
        public RelayCommand ComandoGuardarPagoDescargasDetalle { get; set; }

        public RelayCommand ComandoImprimirPagoDescargas { get; set; }

        public RelayCommand ComandoMostrarAdelantoDescargas { get; set; }
        public RelayCommand ComandoRemoverAdelantoDescarga { get; set; }
        public RelayCommand ComandoEditarAdelantoDescarga { get; set; }
        public RelayCommand ComandoAgregarAdelantoDescarga { get; set; }
        public RelayCommand ComandoGuardarAdelantoDescargas { get; set; }

        private void InicializarComandos()
        {
            ComandoMostrarDescargas = new RelayCommand(MostrarDescargas);
            ComandoBuscarDescarga = new RelayCommand(BuscarDescarga);
            ComandoMostrarEditar = new RelayCommand(MostrarActualizarDescarga);
            ComandoActualizarDescarga = new RelayCommand(ActualizarDescarga);
            ComandoEliminar = new RelayCommand(EliminarDescarga);
            ComandoRefrescar = new RelayCommand(NavegarDescargas);
            ComandoNavegarDescargas = new RelayCommand(NavegarDescargas);

            ComandoMostrarFacturaciones = new RelayCommand(MostrarFacturaciones);
            ComandoBuscarPlantas = new RelayCommand<string>(BuscarPlantas);
            ComandoBuscarCuadrillas = new RelayCommand<string>(BuscarCuadrillas);
            ComandoGuardarPagoDescargas = new RelayCommand(GuardarPagoDescargas);
            ComandoBuscarPagoDescargas = new RelayCommand(BuscarPagoDescargas);
            ComandoMostrarPagoDescargas = new RelayCommand(MostrarCrearPagoDescargas);
            CommandShowEditPagoDescargas = new RelayCommand(ShowEditPagoDescargas);
            ComandoMostrarPagoDescargasDetalle = new RelayCommand(MostrarPagoDescargasDetalle);
            ComandoRemoverPagoDescargaDetalle = new RelayCommand(RemoverPagoDescargaDetalle);
            ComandoEditarPagoDescargaDetalle = new RelayCommand(EditarPagoDescargaDetalle);
            ComandoAgregarPagoDescargaDetalle = new RelayCommand(AgregarPagoDescargaDetalle);
            ComandoGuardarPagoDescargasDetalle = new RelayCommand(GuardarPagoDescargasDetalle);
            ComandoRefrescarPagos = new RelayCommand(NavegarPagoDescargas);
            ComandoNavegarPagoDescargas = new RelayCommand(NavegarPagoDescargas);
            ComandoEliminarPagoDescargas = new RelayCommand(EliminarPagoDescargas);
            ComandoActualizarDescargas = new RelayCommand(ActualizarDescargas);

            ComandoImprimirPagoDescargas = new RelayCommand(ImprimirPagoDescargas);

            ComandoMostrarAdelantoDescargas = new RelayCommand(MostrarAdelantoDescargas);
            ComandoRemoverAdelantoDescarga = new RelayCommand(RemoverAdelantoDescarga);
            ComandoEditarAdelantoDescarga = new RelayCommand(EditarAdelantoDescarga);
            ComandoAgregarAdelantoDescarga = new RelayCommand(AgregarAdelantoDescarga);
            ComandoGuardarAdelantoDescargas = new RelayCommand(GuardarAdelantoDescargas);
        }

        private void GetPlantas()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllPlantas { };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                Plantas = res;
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ProcessImportation()
        {
            if (XlsColeccionImportacion == null || XlsColeccionImportacion.Count == 0) return;
            
            foreach (var item in XlsColeccionImportacion)
            {
                var type = item.GetType();
                var elementoDetalle = new PagoDescargasModel();

                foreach (var propiedad in type.GetProperties())
                {
                    AssignImportationData(elementoDetalle, propiedad, item);
                }

                PagoDescargasModel existeBoleta = Planta.IsShippingNumberRequired ?
                                                  PagoDescargasModel.FirstOrDefault(f => f.NumeroEnvio == elementoDetalle.NumeroEnvio) :
                                                  PagoDescargasModel.FirstOrDefault(f => f.CodigoBoleta == elementoDetalle.CodigoBoleta);

                if (existeBoleta == null)
                {
                    PagoDescargasModel.Add(elementoDetalle);
                }
            }

            DatoPagoDescargadores.TotalBoletaDescargas = PagoDescargasModel.Count;
            DatoPagoDescargadores.TotalPago = PagoDescargasModel.Sum(d => d.PagoDescarga);

            RaisePropertyChanged(nameof(DatoPagoDescargadores));
        }

        private void GetDescargasConteo()
        {
            PagoDescargadoresSeleccionado.ConteoDescargas = $"{ListaDescargasPorPago.Where(d => d.BoletaId > 0).ToList().Count}/{PagoDescargadoresSeleccionado.TotalBoletaDescargas}";
            RaisePropertyChanged(nameof(PagoDescargadoresSeleccionado));
        }

        private static void AssignImportationData(PagoDescargasModel elementoDetalle, PropertyInfo propiedad, object item)
        {
            var value = propiedad.GetValue(item, null);

            if (propiedad.Name.ToUpper().Contains("NUMEROENVIO") || propiedad.Name.ToUpper().Contains("NOENVIO"))
            {
                elementoDetalle.NumeroEnvio = value != null ? value.ToString() : string.Empty;
            }

            if (propiedad.Name.ToUpper().Contains("CODIGOBOLETA") || propiedad.Name.ToUpper().Contains("NOBOLETA"))
            {
                elementoDetalle.CodigoBoleta = value != null ? value.ToString() : string.Empty;
            }

            if (propiedad.Name.ToUpper().Contains("PRECIODESCARGA") || propiedad.Name.ToUpper().Contains("DESCARGA"))
            {
                elementoDetalle.PagoDescarga = value != null ? Convert.ToDecimal(value) : 0;
            }
        }

        private void GuardarAdelantoDescargas()
        {
            var datos = from reg in DescargasPorAdelantoModel
                        select new DescargasPorAdelantadoDTO
                        {
                            DescargaPorAdelantadoId = reg.DescargaPorAdelantadoId,
                            CodigoBoleta = reg.CodigoBoleta,
                            NumeroEnvio = reg.NumeroEnvio,
                            PlantaId = reg.PlantaId,
                            PagoDescargaId = reg.PagoDescargaId,
                            CreadoPor = reg.CreadoPor,
                            PrecioDescarga = reg.PrecioDescarga,
                            FechaCreacion = reg.FechaCreacion
                        };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostDescargasPorAdelantado
            {
                PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                DescargasPorAdelantado = datos.ToList(),
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
                CargarSlidePanel("DescargasManualFlyout");
                CargarPagosDescargas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });

        }

        private void AgregarAdelantoDescarga()
        {
            string codigo = DatoDescargaPorAdelantado.HasShippingNumber
                            ? DatoDescargaPorAdelantado.NumeroEnvio
                            : DatoDescargaPorAdelantado.CodigoBoleta;

            if (DatoDescargaPorAdelantado.HasShippingNumber)
            {
                if (string.IsNullOrWhiteSpace(DatoDescargaPorAdelantado.NumeroEnvio))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe Ingresar el Número de envío de la Boleta");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(DatoDescargaPorAdelantado.CodigoBoleta))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe Ingresar el Código de la Boleta");
                    return;
                }
            }
            
            if (DatoDescargaPorAdelantado.PrecioDescarga <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Precio de la Descarga debe ser mayor a 0");
                return;
            }

            var itemDescarga = DatoDescargaPorAdelantado.HasShippingNumber
                               ? DescargasPorAdelantoModel.FirstOrDefault(d => d.NumeroEnvio == DatoDescargaPorAdelantado.NumeroEnvio)
                               : DescargasPorAdelantoModel.FirstOrDefault(d => d.CodigoBoleta == DatoDescargaPorAdelantado.CodigoBoleta);

            if (itemDescarga == null)
            {
                DescargasPorAdelantoModel.Add(DatoDescargaPorAdelantado);
            }
            else
            {
                itemDescarga.PrecioDescarga = DatoDescargaPorAdelantado.PrecioDescarga;
                itemDescarga.EsActualizacion = true;
            }

            InicializarAdelantoPorDescargas();
        }

        private void EditarAdelantoDescarga()
        {
            if (DescargaPorAdelantadoSeleccionado == null) return;

            DatoDescargaPorAdelantado = new DescargasPorAdelantadoModel
            {
                DescargaPorAdelantadoId = DescargaPorAdelantadoSeleccionado.DescargaPorAdelantadoId,
                NumeroEnvio = DescargaPorAdelantadoSeleccionado.NumeroEnvio,
                PlantaId = DescargaPorAdelantadoSeleccionado.PlantaId,
                PagoDescargaId = DescargaPorAdelantadoSeleccionado.PagoDescargaId,
                CreadoPor = DescargaPorAdelantadoSeleccionado.CreadoPor,
                PrecioDescarga = DescargaPorAdelantadoSeleccionado.PrecioDescarga,
                FechaCreacion = DescargaPorAdelantadoSeleccionado.FechaCreacion
            };
        }

        private void RemoverAdelantoDescarga()
        {
            if (DescargaPorAdelantadoSeleccionado == null) return;

            DescargasPorAdelantoModel.Remove(DescargaPorAdelantadoSeleccionado);
            InicializarAdelantoPorDescargas();
        }

        private void InicializarAdelantoPorDescargas()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            DatoDescargaPorAdelantado = new DescargasPorAdelantadoModel
            {
                CreadoPor = _serviciosComunes.GetRequestUserInfo().UserId,
                PlantaId = PagoDescargadoresSeleccionado.PlantaId,
                FechaCreacion = DateTime.Now,
                PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                TotalDescargasPorAdelanto = DescargasPorAdelantoModel.Sum(d => d.PrecioDescarga),
                TotalDescargas = PagoDescargadoresSeleccionado.TotalBoletaDescargas + DescargasPorAdelantoModel.Count(),
                TotalPagoDescargas = DescargasPorAdelantoModel.Sum(d => d.PrecioDescarga) + PagoDescargadoresSeleccionado.TotalPago,
                HasShippingNumber = PagoDescargadoresSeleccionado.HasShippingNumber
            };

            RaisePropertyChanged(nameof(DatoDescargaPorAdelantado));
        }

        private void MostrarAdelantoDescargas()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            if (PagoDescargadoresSeleccionado.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado del Pago de Descargas debe ser Activo");
                return;
            }

            var datos = from reg in DescargasPorAdelantado
                        select new DescargasPorAdelantadoModel
                        {
                            DescargaPorAdelantadoId = reg.DescargaPorAdelantadoId,
                            CodigoBoleta = reg.CodigoBoleta,
                            NumeroEnvio = reg.NumeroEnvio,
                            PlantaId = PagoDescargadoresSeleccionado.PlantaId,
                            PagoDescargaId = reg.PagoDescargaId,
                            CreadoPor = reg.CreadoPor,
                            PrecioDescarga = reg.PrecioDescarga,
                            FechaCreacion = reg.FechaCreacion,
                            Estado = reg.Estado
                        };

            DescargasPorAdelantoModel = new ObservableCollection<DescargasPorAdelantadoModel>(datos);
            InicializarAdelantoPorDescargas();

            CargarSlidePanel("DescargasManualFlyout");
        }

        private void CargarDescargasPorAdelanto()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDescargasAdelantadasPorPagoDescargadaId
            {
                PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                DescargasPorAdelantado = new List<DescargasPorAdelantadoDTO>(res);

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarLineasDeCreditoPorBanco()
        {
            LineasCredito = new List<LineasCreditoDTO>();
            
            if (DatoPagoDescargaDetalle.BancoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetLineasCreditoPorBancoPorTipoCuenta
            {
                BancoId = DatoPagoDescargaDetalle.BancoId,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId,
                CuentaFinancieraTipoId = FormaDePagoSeleccionada.CuentaFinancieraTipoId
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

        private void ImprimirPagoDescargas()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            if (PagoDescargadoresSeleccionado.Estado == Estados.ACTIVO)
            {
                ObtenerInformacionReportePreview();
            }
            else
            {
                ObtenerInformacionReporteFinal();
            }
        }

        private void ObtenerInformacionReporteFinal()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetFacturacionPagoDescargadores
            {
                PagoDescargadoresId = PagoDescargadoresSeleccionado.PagoDescargaId
            };
            _client.GetAsync(request, res =>
            {
                if (!res.Descargas.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Existe Infomacion que Mostrar");
                    return;
                }

                ImprimirReporteFinal(res);
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void ObtenerInformacionReportePreview()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetPreviewPagoDescargadores
            {
                PagoDescargadoresId = PagoDescargadoresSeleccionado.PagoDescargaId
            };
            _client.GetAsync(request, res =>
            {
                ReporteDescargas = new List<ReporteDescargadoresDTO>(res);

                if (!ReporteDescargas.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Existe Infomacion que Mostrar");
                    return;
                }

                ImprimirPreviewPagoDescargas("rptPreviewPagoDescargadores.rdlc");
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));            
        }

        private void ImprimirPreviewPagoDescargas(string nombreReporte)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte Pago de Descargas";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = nombreReporte;
                manejadorReporte.Datos = ReporteDescargas;
                manejadorReporte.NombreDataSet = "DataSet";
                manejadorReporte.MostarReporte();
            });
        }

        private void ImprimirReporteFinal(ReportePagoDescargadoresResumen datos)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte Pago de Descargas";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "ReportePagoDescargadores.rdlc";
                manejadorReporte.MultipleDataSet = true;
                manejadorReporte.ListadoItemsDataSet = new List<ItemDataSetModel>
                {
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Detalle",
                        Datos = datos.PagoDescargadores
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "DataSet",
                        Datos = datos.Descargas
                    }
                };
                manejadorReporte.MultipleDataSet = true;
                manejadorReporte.AgregarParametro("Empresa", "Comercial Colindres");
                manejadorReporte.MostarReporte();
            });
        }

        private void MostrarFacturaciones()
        {
            EsFacturacion = true;
            CargarPagosDescargas();
        }

        private void MostrarDescargas()
        {
            EsFacturacion = false;
            CargarDescargas();
        }

        private void EliminarDescarga()
        {
            if (DescargaSeleccionada == null) return;

            if (DescargaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Descargas Activas");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, DescargaSeleccionada.CodigoBoleta),
                Titulo = "Descargas"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutDescargaAnular
                    {
                        DescargadaId = DescargaSeleccionada.DescargadaId,
                        UserId = InformacionSistema.UsuarioActivo.Usuario
                    };
                    MostrarVentanaEsperaFlyOut = Visibility.Visible;
                    _client.PutAsync(request, res =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        if (!string.IsNullOrWhiteSpace(res.MensajeError))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Eliminados);
                        CargarDescargas();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void MostrarActualizarDescarga()
        {
            if (DescargaSeleccionada == null) return;

            if (DescargaSeleccionada.EstadoBoleta != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, string.Format("El estado de la Boleta {0} al que esta asociada la Descarga debe estar Activa", DescargaSeleccionada.CodigoBoleta));
                return;
            }

            if (DescargaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede actualizar descargas Activas");
                return;
            }

            DatoDescargadores = new DescargadoresDTO
            {
                BoletaId = DescargaSeleccionada.BoletaId,
                CodigoBoleta = DescargaSeleccionada.CodigoBoleta,
                CuadrillaId = DescargaSeleccionada.CuadrillaId,
                DescargadaId = DescargaSeleccionada.DescargadaId,
                PrecioDescarga = DescargaSeleccionada.PrecioDescarga,
                NombreProveedor = DescargaSeleccionada.NombreProveedor,
                PlacaEquipo = DescargaSeleccionada.PlacaEquipo,
                DescripcionTipoEquipo = DescargaSeleccionada.DescripcionTipoEquipo,
                FechaDescarga = DescargaSeleccionada.FechaDescarga
            };

            CargarSlidePanel("EditarDescargaFlyout");
        }

        private void ActualizarDescarga()
        {
            if (DatoDescargadores.PrecioDescarga <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Precio de la Descarga debe ser mayor a 0");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutDescarga
            {
                Descarga = DatoDescargadores,
                UserId = InformacionSistema.UsuarioActivo.Usuario
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PutAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(res.MensajeError))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                CargarSlidePanel("EditarDescargaFlyout");
                CargarDescargas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarDescarga()
        {
            NumeroPaginaDescargas = 1;
            CargarDescargas();
        }

        private void BuscarPagoDescargas()
        {
            NumeroPaginaPagoDescargas = 1;
            CargarPagosDescargas();
        }

        private void NavegarPagoDescargas()
        {
            CargarPagosDescargas();
        }
        
        private void NavegarDescargas()
        {
            CargarDescargas();
        }

        private void GuardarPagoDescargas()
        {
            if (CuadrillaAutoCompleteSeleccionado == null || CuadrillaAutoCompleteSeleccionado.Id == null)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Antes debe ingresar una Cuadrilla");
                return;
            }

            if (PagoDescargasModel == null || !PagoDescargasModel.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Information, string.Format("No existen Descargas Pendientes para la Cuadrilla de {0}", CuadrillaAutoCompleteSeleccionado.DisplayName));
                return;
            }

            List<DescargadoresDTO> descargas = (from reg in PagoDescargasModel
                                                select new DescargadoresDTO
                                                {
                                                    NumeroEnvio = reg.NumeroEnvio,
                                                    CodigoBoleta = reg.CodigoBoleta,
                                                    PagoDescarga = reg.PagoDescarga
                                                }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostPagosDescargas
            {
                PagoDescargas = DatoPagoDescargadores,
                Descargas = descargas,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                PagoDescargasErrors = res;
                if (PagoDescargasErrors.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Existen errores que validar");
                    ImportationErrors = true;
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("AgregarPagoDescargasFlyout");
                CargarPagosDescargas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarPagoDescargaDetalle()
        {
            if (DatoPagoDescargaDetalle.Monto <= 0)
            {
                return "Debe ingresar un monto mayor a 0";
            }

            if (string.IsNullOrWhiteSpace(DatoPagoDescargaDetalle.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }

            if (FormaDePagoSeleccionada.RequiereBanco)
            {
                if (DatoPagoDescargaDetalle.BancoId == 0)
                {
                    return "Debe seleccionar el un Banco";
                }

                if (string.IsNullOrWhiteSpace(DatoPagoDescargaDetalle.NoDocumento))
                {
                    return "Debe ingresar el Número de la Transacción";
                }
            }

            if (DatoPagoDescargaDetalle.LineaCreditoId == 0)
            {
                return "Debe seleccionar la Linea de Crédito";
            }

            return string.Empty;
        }

        private void AgregarPagoDescargaDetalle()
        {
            var mensajeValidacion = ValidarPagoDescargaDetalle();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemPagoDescarga = ListaPagoDescargaDetalleModel.FirstOrDefault(c => c.PagoDescargaDetalleId == DatoPagoDescargaDetalle.PagoDescargaDetalleId &&
                                                                        c.LineaCreditoId == DatoPagoDescargaDetalle.LineaCreditoId &&
                                                                        c.FormaDePago == DatoPagoDescargaDetalle.FormaDePago &&
                                                                        c.NoDocumento == DatoPagoDescargaDetalle.NoDocumento);

            var montoJustificado = Math.Round(ListaPagoDescargaDetalleModel.Sum(j => j.Monto), 2);
            var totalAPagar = Math.Round(PagoDescargadoresSeleccionado.TotalPago, 2);

            if (itemPagoDescarga == null)
            {
                montoJustificado += DatoPagoDescargaDetalle.Monto;
            }
            else
            {
                montoJustificado = (montoJustificado - itemPagoDescarga.Monto) + DatoPagoDescargaDetalle.Monto;
            }

            if (montoJustificado > totalAPagar)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, string.Format("La Justificación de Pago L. {0} supera el total a Pagar de las Descargas L. {1}", montoJustificado, totalAPagar));
                return;
            }

            if (itemPagoDescarga == null)
            {
                ListaPagoDescargaDetalleModel.Add(DatoPagoDescargaDetalle);
            }
            else
            {
                itemPagoDescarga.LineaCreditoId = DatoPagoDescargaDetalle.LineaCreditoId;
                itemPagoDescarga.FormaDePago = DatoPagoDescargaDetalle.FormaDePago;
                itemPagoDescarga.Monto = DatoPagoDescargaDetalle.Monto;
                itemPagoDescarga.NoDocumento = DatoPagoDescargaDetalle.NoDocumento;
            }

            DatoPagoDescargaDetalle = new PagoDescargaDetallesModel
            {
                CantidadJustificada = ListaPagoDescargaDetalleModel.Sum(c => c.Monto)
            };

            BancoSeleccionado = new BancosDTO();
        }

        private void EditarPagoDescargaDetalle()
        {
            if (PagoDescargaDetalleSeleccionado != null)
            {
                if (!PagoDescargaDetalleSeleccionado.PuedeEditarPagoDescargaDetalle && PagoDescargaDetalleSeleccionado.PagoDescargaDetalleId > 0)
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                    PagoDescargaDetalleSeleccionado = new PagoDescargaDetallesModel();
                    return;
                }
                
                DatoPagoDescargaDetalle = new PagoDescargaDetallesModel
                {
                    EsEdicion = true,
                    LineaCreditoId = PagoDescargaDetalleSeleccionado.LineaCreditoId,
                    BancoId = PagoDescargaDetalleSeleccionado.BancoId,
                    CantidadJustificada = ListaPagoDescargaDetalleModel.Sum(j => j.Monto),
                    FormaDePago = PagoDescargaDetalleSeleccionado.FormaDePago,
                    Monto = PagoDescargaDetalleSeleccionado.Monto,
                    NoDocumento = PagoDescargaDetalleSeleccionado.NoDocumento,
                    NombreBanco = PagoDescargaDetalleSeleccionado.NombreBanco,
                    PagoDescargaDetalleId = PagoDescargaDetalleSeleccionado.PagoDescargaDetalleId,
                    PagoDescargaId = PagoDescargaDetalleSeleccionado.PagoDescargaId
                };
            }
        }

        private void RemoverPagoDescargaDetalle()
        {
            if (PagoDescargaDetalleSeleccionado == null) return;

            if (!PagoDescargaDetalleSeleccionado.PuedeEditarPagoDescargaDetalle && PagoDescargaDetalleSeleccionado.PagoDescargaDetalleId > 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                PagoDescargaDetalleSeleccionado = new PagoDescargaDetallesModel();
                return;
            }

            ListaPagoDescargaDetalleModel.Remove(PagoDescargaDetalleSeleccionado);
            DatoPagoDescargaDetalle = new PagoDescargaDetallesModel
            {
                CantidadJustificada = ListaPagoDescargaDetalleModel.Sum(j => j.Monto)
            };
        }
        
        private void BuscarCuadrillas(string valorBusqueda)
        {
            if (DatoPagoDescargadores == null || DatoPagoDescargadores.PlantaId == 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Antes debe ingresar la Planta Orgien");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetCuadrillasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                PlantaId = DatoPagoDescargadores.PlantaId
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    ListaCuadrillasAutoComplete = (from reg in res
                                                   select new AutoCompleteEntry(reg.NombreEncargado, reg.CuadrillaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarPlantas(string valorBusqueda)
        {
            IsPlantaSelected = false;

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
                    ListaPlantasAutoComplete = (from reg in res
                                                select new AutoCompleteEntry(reg.NombrePlanta, reg.PlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ObtenerCorrelativoPagoDescarga()
        {
            var request = new GetPagoDescargadoresUltimo
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                Fecha = DatoPagoDescargadores.FechaPago
            };

            var ultimoCorrelativoPagoDescargas = _client.Get(request);
            DatoPagoDescargadores.CodigoPagoDescarga = ultimoCorrelativoPagoDescargas.CodigoPagoDescarga;
            RaisePropertyChanged("DatoPagoDescargadores");
        }

        private void CargarDescargas()
        {
            var boletaId = DescargaSeleccionada != null ? DescargaSeleccionada.BoletaId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorDescargadores
            {
                PaginaActual = NumeroPaginaDescargas,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaDescarga
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaDescargasControl = res;
                if (BusquedaDescargasControl.Items != null && BusquedaDescargasControl.Items.Any())
                {
                    if (boletaId == 0)
                    {
                        DescargaSeleccionada = BusquedaDescargasControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        DescargaSeleccionada = BusquedaDescargasControl.Items.FirstOrDefault(r => r.BoletaId == boletaId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarDescargasPorPago()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDescargasPorPagoDescargaId
            {
                PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaDescargasPorPago = res;
                RaisePropertyChanged(nameof(ListaDescargasPorPago));

                if (ListaDescargasPorPago.Any())
                {
                    GetDescargasConteo();
                }

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarPagosDescargas()
        {
            var pagoDescargaId = PagoDescargadoresSeleccionado != null ? PagoDescargadoresSeleccionado.PagoDescargaId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorPagosDescargas
            {
                PaginaActual = NumeroPaginaPagoDescargas,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaPagoDescarga
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaPagoDescargadoresControl = res;
                if (BusquedaPagoDescargadoresControl.Items != null && BusquedaPagoDescargadoresControl.Items.Any())
                {
                    if (pagoDescargaId == 0)
                    {
                        PagoDescargadoresSeleccionado = BusquedaPagoDescargadoresControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        PagoDescargadoresSeleccionado = BusquedaPagoDescargadoresControl.Items.FirstOrDefault(r => r.PagoDescargaId == pagoDescargaId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void InicializarPropiedades()
        {            
            EsFacturacion = false;
        }

        private void CargarFormarDePago()
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

        private void ShowEditPagoDescargas()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            if (PagoDescargadoresSeleccionado.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede modficar pagos en estado ACTIVO");
                return;
            }

            ImportationErrors = false;
            PagoDescargasModel = new ObservableCollection<PagoDescargasModel>();
            CuadrillaAutoCompleteSeleccionado = new AutoCompleteEntry(PagoDescargadoresSeleccionado.EncargadoCuadrilla, (int)PagoDescargadoresSeleccionado.CuadrillaId);
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(PagoDescargadoresSeleccionado.NombrePlanta, (int)PagoDescargadoresSeleccionado.PlantaId);
            XlsColeccionImportacion = null;

            DatoPagoDescargadores.FechaPago = PagoDescargadoresSeleccionado.FechaPago;
        }

        private void MostrarCrearPagoDescargas()
        {
            ImportationErrors = false;
            IsPlantaSelected = false;
            PagoDescargasModel = new ObservableCollection<PagoDescargasModel>();
            CuadrillaAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            
            DatoPagoDescargadores = new PagoDescargadoresDTO { FechaPago = DateTime.Now };
            ObtenerCorrelativoPagoDescarga();
            XlsColeccionImportacion = null;
            
            CargarSlidePanel("AgregarPagoDescargasFlyout");
        }

        private void MostrarPagoDescargasDetalle()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            if (PagoDescargadoresSeleccionado.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Estado del Pago de Descargadores esta Cerrado");
                return;
            }

            var pagoDescargasDetalle = from reg in ListaPagoDescargasDetalle
                                       select new PagoDescargaDetallesModel
                                       {
                                           LineaCreditoId = reg.LineaCreditoId,
                                           FormaDePago = reg.FormaDePago,
                                           Monto = reg.Monto,
                                           NoDocumento = reg.NoDocumento,
                                           BancoId = reg.BancoId,
                                           NombreBanco = reg.NombreBanco,
                                           PagoDescargaId = reg.PagoDescargaId,
                                           PagoDescargaDetalleId = reg.PagoDescargaDetalleId,
                                           CodigoLineaCredito = reg.CodigoLineaCredito,
                                           PuedeEditarPagoDescargaDetalle = reg.PuedeEditarCreditoDeduccion
                                       };

            ListaPagoDescargaDetalleModel = new ObservableCollection<PagoDescargaDetallesModel>(pagoDescargasDetalle);            
            DatoPagoDescargaDetalle = new PagoDescargaDetallesModel();

            DatoPagoDescargaDetalle.CantidadJustificada = ListaPagoDescargaDetalleModel.Sum(j => j.Monto);
            CargarSlidePanel("PagoDescargasDetalleFlyout");
        }

        private void CargarPagosDescargasDetalle()
        {
            ListaPagoDescargasDetalle = new List<PagoDescargaDetallesDTO>();

            if (PagoDescargadoresSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPagoDescargasDetallePorPagoDescargaId
            {
                PagoDescargasId = PagoDescargadoresSeleccionado.PagoDescargaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaPagoDescargasDetalle = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GuardarPagoDescargasDetalle()
        {
            var pagoDescargasDetalle = from reg in ListaPagoDescargaDetalleModel
                                       select new PagoDescargaDetallesDTO
                                       {
                                           LineaCreditoId = reg.LineaCreditoId,
                                           BancoId = reg.BancoId,
                                           NombreBanco = reg.NombreBanco,
                                           PagoDescargaDetalleId = reg.PagoDescargaDetalleId,
                                           PagoDescargaId = reg.PagoDescargaId,
                                           FormaDePago = reg.FormaDePago,
                                           Monto = reg.Monto,
                                           NoDocumento = reg.NoDocumento,
                                           EstaEditandoRegistro = reg.EsEdicion
                                       };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostPagoDescargasDetalle
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                PagoDescargaDetalle = pagoDescargasDetalle.ToList()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(res.MensajeError))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("PagoDescargasDetalleFlyout");
                CargarPagosDescargas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EliminarPagoDescargas()
        {
            if (PagoDescargadoresSeleccionado == null) return;

            if (PagoDescargadoresSeleccionado.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Pago de Descargas YA está CERRADO");
                return;
            }

            if (PagoDescargadoresSeleccionado.Estado == Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Pago de Descargas está en PROCESO");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, PagoDescargadoresSeleccionado.CodigoPagoDescarga),
                Titulo = "Pago de Descargadores"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeletePagoDescargas
                    {
                        PagoDescargaId = PagoDescargadoresSeleccionado.PagoDescargaId,
                        UserId = InformacionSistema.UsuarioActivo.Usuario
                    };
                    MostrarVentanaEsperaFlyOut = Visibility.Visible;
                    _client.DeleteAsync(request, res =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        if (!string.IsNullOrWhiteSpace(res.MensajeError))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Eliminados);
                        CargarPagosDescargas();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void ActualizarDescargas()
        {
            //if (PagoDescargadoresSeleccionado == null) return;

            //if (PagoDescargadoresSeleccionado.Estado != Estados.ACTIVO)
            //{
            //    _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Pago de Descargas debe estar Activo");
            //    return;
            //}            

            //DialogSettings = new ConfiguracionDialogoModel
            //{
            //    Mensaje = string.Format(Mensajes.Actualizando_Datos, PagoDescargadoresSeleccionado.CodigoPagoDescarga),
            //    Titulo = "Pago de Descargadores"
            //};
            //DialogSettings.Respuesta += result =>
            //{
            //    if (result == MessageDialogResult.Affirmative)
            //    {
            //        var uri = InformacionSistema.Uri_ApiService;
            //        _client = new JsonServiceClient(uri);
            //        var request = new PutPagoDescargas
            //        {
                        
            //        };
            //        MostrarVentanaEsperaFlyOut = Visibility.Visible;
            //        _client.PutAsync(request, res =>
            //        {
            //            MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
            //            if (!string.IsNullOrWhiteSpace(res.MensajeError))
            //            {
            //                _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
            //                return;
            //            }
            //            _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
            //            CargarPagosDescargas();
            //        }, (r, ex) =>
            //        {
            //            MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
            //            _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            //        });
            //    }
            //};
            //DialogSettings = null;
        }

        private void CargarBancos()
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

        private void CargarDatosIniciales()
        {
            CargarDescargas();
            CargarBancos();
            CargarFormarDePago();
            GetPlantas();
        }        

        private void CargarDatosPruebas()
        {
            BusquedaDescargasControl = new BusquedaDescargadoresDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaDescargas()
            };
            DescargaSeleccionada = BusquedaDescargasControl.Items.FirstOrDefault();
            
            BusquedaPagoDescargadoresControl = new BusquedaPagoDescargadoresDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaPagoDescargas()
            };
            PagoDescargadoresSeleccionado = BusquedaPagoDescargadoresControl.Items.FirstOrDefault();

            var x = DatosDiseño.ListaDescargasPorPago();
            ListaDescargasPorPago = new List<DescargadoresDTO>(x);
        }
    }
}
