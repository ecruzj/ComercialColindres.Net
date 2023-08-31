using ComercialColindres.Busquedas;
using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalle;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.DTOs.RequestDTOs.FacturasCategorias;
using ComercialColindres.DTOs.RequestDTOs.NotasCredito;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Sucursales;
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
    public class FacturasViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public FacturasViewModel(IServiciosComunes serviciosComunes)
        {
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

        public int NumeroPaginaFacturas
        {
            get
            {
                return _numeroPaginaFacturas;
            }
            set
            {
                _numeroPaginaFacturas = value;
                RaisePropertyChanged("NumeroPaginaFacturas");
            }
        }
        int _numeroPaginaFacturas = 1;

        public string ValorBusquedaFacturas
        {
            get { return _valorBusquedaFacturas; }
            set
            {
                _valorBusquedaFacturas = value;
                RaisePropertyChanged("ValorBusquedaFacturas");
            }
        }
        private string _valorBusquedaFacturas;

        public BusquedaFacturasDTO BusquedaFacturasControl
        {
            get { return _busquedaFacturasControl; }
            set
            {
                _busquedaFacturasControl = value;
                RaisePropertyChanged("BusquedaFacturasControl");
            }
        }
        private BusquedaFacturasDTO _busquedaFacturasControl;

        public FacturasDTO FacturaSeleccionada
        {
            get { return _facturaSeleccionada; }
            set
            {
                _facturaSeleccionada = value;
                RaisePropertyChanged("FacturaSeleccionada");

                if (IsInDesignMode)
                {
                    return;
                }
                                
                LoadInvoiceDetailItems();
                CargarBoletasPorFactura();
                LoadInvoicePayments();
                LoadNotasCredito();
            }
        }
        private FacturasDTO _facturaSeleccionada;

        public FacturasDTO DatoFactura
        {
            get { return _datoFactura; }
            set
            {
                _datoFactura = value;
                RaisePropertyChanged("DatoFactura");
            }
        }
        private FacturasDTO _datoFactura;

        public List<FacturaDetalleBoletasDTO> ListaFacturaDetalleBoletas
        {
            get { return _listaFacturaDetalleBoletas; }
            set
            {
                _listaFacturaDetalleBoletas = value;
                RaisePropertyChanged("ListaFacturaDetalleBoletas");
            }
        }
        private List<FacturaDetalleBoletasDTO> _listaFacturaDetalleBoletas;
        
        public List<FacturaDetalleBoletasDTO> ListaValidacionBoletas
        {
            get { return _listaValidacionBoletas; }
            set
            {
                _listaValidacionBoletas = value;
                RaisePropertyChanged("ListaValidacionBoletas");
            }
        }
        private List<FacturaDetalleBoletasDTO> _listaValidacionBoletas;

        public ObservableCollection<FacturaDetalleBoletasModel> ListaFacturaDetalleBoletasModel
        {
            get { return _listaFacturaDetalleBoletasModel; }
            set
            {
                _listaFacturaDetalleBoletasModel = value;
                RaisePropertyChanged("ListaFacturaDetalleBoletasModel");
            }
        }
        private ObservableCollection<FacturaDetalleBoletasModel> _listaFacturaDetalleBoletasModel;

        public ObservableCollection<FacturaDetalleBoletasModel> ListaFacturaDetalleBoletasMasivoModel
        {
            get { return _listaFacturaDetalleBoletasMasivoModel; }
            set
            {
                _listaFacturaDetalleBoletasMasivoModel = value;
                RaisePropertyChanged("ListaFacturaDetalleBoletasMasivoModel");
            }
        }
        private ObservableCollection<FacturaDetalleBoletasModel> _listaFacturaDetalleBoletasMasivoModel;

        public FacturaDetalleBoletasModel FacturaDetalleBoletaSeleccionado
        {
            get { return _facturaDetalleBoletaSeleccionado; }
            set
            {
                _facturaDetalleBoletaSeleccionado = value;
                RaisePropertyChanged("FacturaDetalleBoletaSeleccionado");
            }
        }
        private FacturaDetalleBoletasModel _facturaDetalleBoletaSeleccionado;

        public FacturaDetalleBoletasModel DatoFacturaDetalleBoleta
        {
            get { return _datoFacturaDetalleBoleta; }
            set
            {
                _datoFacturaDetalleBoleta = value;
                RaisePropertyChanged("DatoFacturaDetalleBoleta");
            }
        }
        private FacturaDetalleBoletasModel _datoFacturaDetalleBoleta;

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

        public List<SucursalesDTO> Sucursales
        {
            get { return _sucursales; }
            set
            {
                if (_sucursales != value)
                {
                    _sucursales = value;
                    RaisePropertyChanged("Sucursales");
                }
            }
        }
        private List<SucursalesDTO> _sucursales;

        public SucursalesDTO SucursalSeleccionada
        {
            get { return _sucursalSeleccionada; }
            set
            {
                if (_sucursalSeleccionada != value)
                {
                    _sucursalSeleccionada = value;
                    RaisePropertyChanged("SucursalSeleccionada");
                }
            }
        }
        private SucursalesDTO _sucursalSeleccionada;

        public List<FacturasCategoriasDTO> ListaFacturaCategorias
        {
            get { return _listaFacturaCategorias; }
            set
            {
                _listaFacturaCategorias = value;
                RaisePropertyChanged("ListaFacturaCategorias");
            }
        }
        private List<FacturasCategoriasDTO> _listaFacturaCategorias;

        public List<BoletasDTO> ListaBoletasPorFacturar
        {
            get { return _listaBoletasPorFacturar; }
            set
            {
                _listaBoletasPorFacturar = value;
                RaisePropertyChanged("ListaBoletasPorFacturar");
            }
        }
        private List<BoletasDTO> _listaBoletasPorFacturar;

        public FacturasCategoriasDTO FacturaCategoriaSeleccionada
        {
            get { return _facturaCategoriaSeleccionada; }
            set
            {
                _facturaCategoriaSeleccionada = value;
                RaisePropertyChanged("FacturaCategoriaSeleccionada");

                if (FacturaCategoriaSeleccionada != null)
                {
                    DatoFactura.FacturaCategoriaId = FacturaCategoriaSeleccionada.FacturaCategoriaId;
                }
            }
        }
        private FacturasCategoriasDTO _facturaCategoriaSeleccionada;

        public List<AutoCompleteEntry> ListaBoletasAutoComplete
        {
            get { return _listaBoletasAutoComplete; }
            set
            {
                _listaBoletasAutoComplete = value;
                RaisePropertyChanged("ListaBoletasAutoComplete");
            }
        }
        private List<AutoCompleteEntry> _listaBoletasAutoComplete;

        public AutoCompleteEntry BoletaAutoCompleteSeleccionada
        {
            get { return _boletaAutoCompleteSeleccionada; }
            set
            {
                _boletaAutoCompleteSeleccionada = value;
                RaisePropertyChanged("BoletaAutoCompleteSeleccionada");

                if (BoletaAutoCompleteSeleccionada != null && BoletaAutoCompleteSeleccionada.Id != null)
                {
                    DatoFacturaDetalleBoleta.BoletaId = (int)BoletaAutoCompleteSeleccionada.Id;
                    ObtenerDatosBoleta();
                }
            }
        }
        private AutoCompleteEntry _boletaAutoCompleteSeleccionada;

        public bool BuscarBoletasPendientes
        {
            get { return _buscarBoletasPendientes; }
            set
            {
                _buscarBoletasPendientes = value;
                RaisePropertyChanged("BuscarBoletasPendientes");
            }
        }
        private bool _buscarBoletasPendientes;

        public decimal TotalBoletasPendienteFacturar
        {
            get { return _totalBoletasPendienteFacturar; }
            set
            {
                _totalBoletasPendienteFacturar = value;
                RaisePropertyChanged("TotalBoletasPendienteFacturar");
            }
        }
        private decimal _totalBoletasPendienteFacturar;

        public IList XlsColeccionImportacion
        {
            get { return _xlsColeccionImportacion; }
            set
            {
                if (_xlsColeccionImportacion != value)
                {
                    _xlsColeccionImportacion = value;
                    RaisePropertyChanged("XlsColeccionImportacion");
                    ProcesarImportacion();
                }

            }
        }
        private IList _xlsColeccionImportacion;

        public bool EsImportacionManual
        {
            get { return _esImportacionManual; }
            set
            {
                _esImportacionManual = value;
                RaisePropertyChanged("EsImportacionManual");
            }
        }
        private bool _esImportacionManual;

        public bool ImportacionErrores
        {
            get { return _importacionErrores; }
            set
            {
                _importacionErrores = value;
                RaisePropertyChanged("ImportacionErrores");
            }
        }
        private bool _importacionErrores;

        public List<ReporteFacturacionDTO> ReporteFacturacion
        {
            get { return _reporteFacturacion; }
            set
            {
                _reporteFacturacion = value;
                RaisePropertyChanged("ReporteFacturacion");
            }
        }
        private List<ReporteFacturacionDTO> _reporteFacturacion;
                
        public FindPos OrdenesCompraControl
        {
            get { return _ordenesCompraControl; }
            set
            {
                _ordenesCompraControl = value;
                RaisePropertyChanged(nameof(OrdenesCompraControl));
            }
        }
        private FindPos _ordenesCompraControl;

        public List<FacturaDetalleItemDto> InvoiceDetailItems
        {
            get { return _invoiceDetailItems; }
            set
            {
                _invoiceDetailItems = value;
                RaisePropertyChanged(nameof(InvoiceDetailItems));

                if (IsInDesignMode)
                {
                    return;
                }
            }
        }
        private List<FacturaDetalleItemDto> _invoiceDetailItems;

        public ObservableCollection<FacturaDetalleItemModel> InvoiceDetailItemsModel
        {
            get { return _invoiceDetailItemsModels; }
            set
            {
                _invoiceDetailItemsModels = value;
                RaisePropertyChanged(nameof(InvoiceDetailItemsModel));
            }
        }
        private ObservableCollection<FacturaDetalleItemModel> _invoiceDetailItemsModels;

        public FacturaDetalleItemModel InvoiceDetailItemSelected
        {
            get { return _invoiceDetailItemSelected; }
            set
            {
                _invoiceDetailItemSelected = value;
                RaisePropertyChanged(nameof(InvoiceDetailItemSelected));
            }
        }
        private FacturaDetalleItemModel _invoiceDetailItemSelected;

        public FacturaDetalleItemModel InvoiceDetailItemDato
        {
            get { return _invoiceDetailItemDato; }
            set
            {
                _invoiceDetailItemDato = value;
                RaisePropertyChanged(nameof(InvoiceDetailItemDato));
            }
        }
        private FacturaDetalleItemModel _invoiceDetailItemDato;

        public List<AutoCompleteEntry> ProductItems
        {
            get { return _productItems; }
            set
            {
                _productItems = value;
                RaisePropertyChanged(nameof(ProductItems));
            }
        }
        private List<AutoCompleteEntry> _productItems;

        public AutoCompleteEntry ProductItemSelected
        {
            get { return _productItemSelected; }
            set
            {
                _productItemSelected = value;
                RaisePropertyChanged(nameof(ProductItemSelected));

                if (InvoiceDetailItemDato == null) return;

                InvoiceDetailItemDato.CategoriaProductoId = ProductItemSelected != null && ProductItemSelected.Id != null ? (int)ProductItemSelected.Id : 0;
                InvoiceDetailItemDato.ProductoDescripcion = ProductItemSelected != null && ProductItemSelected.DisplayName != null ? (string)ProductItemSelected.DisplayName : string.Empty;
            }
        }
        private AutoCompleteEntry _productItemSelected;

        public List<AutoCompleteEntry> ListaPlantasAutoComplete
        {
            get { return _listaPlantasAutoComplete; }
            set
            {
                _listaPlantasAutoComplete = value;
                RaisePropertyChanged(nameof(ListaPlantasAutoComplete));
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

                if (PlantaAutoCompleteSeleccionada != null && PlantaAutoCompleteSeleccionada.Id != null)
                {
                    if (BuscarBoletasPendientes)
                    {
                        CargarBoltasPendientes();
                        return;
                    }

                    DatoFactura.PlantaId = (int)PlantaAutoCompleteSeleccionada.Id;
                    Planta = Plantas.FirstOrDefault(p => p.PlantaId == DatoFactura.PlantaId);

                    if (Planta != null)
                    {
                        DatoFactura.RequiereOrdenCompra = Planta.RequiresPurchaseOrder;
                        DatoFactura.IsExonerated = Planta.IsExempt;
                    }

                    RaisePropertyChanged("DatoFactura");
                }
            }
        }
        private AutoCompleteEntry _plantaAutoCompleteSeleccionada;

        public List<AutoCompleteEntry> SubPlantas
        {
            get { return _subPlantas; }
            set
            {
                _subPlantas = value;
                RaisePropertyChanged(nameof(SubPlantas));
            }
        }
        private List<AutoCompleteEntry> _subPlantas;

        public AutoCompleteEntry SubPlantaSelected
        {
            get { return _subPlantaSelected; }
            set
            {
                _subPlantaSelected = value;
                RaisePropertyChanged(nameof(SubPlantaSelected));

                if (SubPlantaSelected == null || SubPlantaSelected.Id == null) return;
                DatoFactura.SubPlantaId = (int)SubPlantaSelected.Id;
            }
        }
        private AutoCompleteEntry _subPlantaSelected;

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

        public List<FacturaPagoDto> InvoicePaids
        {
            get { return _inovicePaids; }
            set
            {
                _inovicePaids = value;
                RaisePropertyChanged(nameof(InvoicePaids));
            }
        }
        private List<FacturaPagoDto> _inovicePaids;

        public List<BancosDTO> Banks
        {
            get
            {
                return _banks;
            }
            set
            {
                _banks = value;
                RaisePropertyChanged(nameof(Banks));
            }
        }
        private List<BancosDTO> _banks;

        public BancosDTO BankSelected
        {
            get
            {
                return _bankSelected;
            }
            set
            {
                _bankSelected = value;
                RaisePropertyChanged(nameof(BankSelected));

                if (BankSelected == null) return;
                if (InvoicePaymentDato == null) return;

                InvoicePaymentDato.BancoId = BankSelected.BancoId;
                InvoicePaymentDato.NombreBanco = BankSelected.Descripcion;
            }
        }
        private BancosDTO _bankSelected;

        public ObservableCollection<FacturaPagoModel> InvoicePayments
        {
            get { return _invoicePayments; }
            set
            {
                _invoicePayments = value;
                RaisePropertyChanged(nameof(InvoicePayments));
            }
        }
        private ObservableCollection<FacturaPagoModel> _invoicePayments;

        public FacturaPagoModel InvoicePaymentSelected
        {
            get { return _invoicePaymentSelected; }
            set
            {
                _invoicePaymentSelected = value;
                RaisePropertyChanged(nameof(InvoicePaymentSelected));
            }
        }
        private FacturaPagoModel _invoicePaymentSelected;

        public FacturaPagoModel InvoicePaymentDato
        {
            get { return _invoicePaymentDato; }
            set
            {
                _invoicePaymentDato = value;
                RaisePropertyChanged(nameof(InvoicePaymentDato));
            }
        }
        private FacturaPagoModel _invoicePaymentDato;

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
                if (InvoicePaymentDato == null) return;

                InvoicePaymentDato.FormaDePago = FormaDePagoSeleccionada.Descripcion;
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;

        public List<NotaCreditoDto> NotasCredito
        {
            get { return _notasCredito; }
            set
            {
                _notasCredito = value;
                RaisePropertyChanged(nameof(NotasCredito));
            }
        }
        private List<NotaCreditoDto> _notasCredito;

        public ObservableCollection<NotasCreditoModel> NotasCreditoModel
        {
            get { return _notasCreditoModel; }
            set
            {
                _notasCreditoModel = value;
                RaisePropertyChanged(nameof(NotasCreditoModel));
            }
        }
        private ObservableCollection<NotasCreditoModel> _notasCreditoModel;
        
        public NotasCreditoModel NotaCreditoSelected
        {
            get { return _notaCreditoSelected; }
            set
            {
                _notaCreditoSelected = value;
                RaisePropertyChanged(nameof(NotaCreditoSelected));
            }
        }
        private NotasCreditoModel _notaCreditoSelected;

        public NotasCreditoModel NotaCreditoDato
        {
            get { return _notaCreditoDato; }
            set
            {
                _notaCreditoDato = value;
                RaisePropertyChanged(nameof(NotaCreditoDato));
            }
        }
        private NotasCreditoModel _notaCreditoDato;

        public bool IsVisibleBillingPermission
        {
            get { return _isVisibleBillingPermission; }
            set
            {
                _isVisibleBillingPermission = value;
                RaisePropertyChanged(nameof(IsVisibleBillingPermission));
            }
        }
        private bool _isVisibleBillingPermission;
               
        public RelayCommand ComandoMostrarCrearFactura { get; set; }
        public RelayCommand ComandoCrearFactura { get; set; }
        public RelayCommand CommandShowEditInvoice { get; set; }
        public RelayCommand CommandEditInvoiceInfo { get; set; }
        public RelayCommand<string> ComandoBuscarPlantas { get; set; }
        public RelayCommand CommandSearchInvoice { get; set; }
        public RelayCommand ComandoAnularFactura { get; set; }
        public RelayCommand ComandoNavegarFacturas { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }
        public RelayCommand CommandActiveInvoice { get; set; }
        public RelayCommand<string> CommandSearchSubPlantas { get; set; }

        public RelayCommand ComandoMostrarDetalleBoletas { get; set; }
        public RelayCommand<string> ComandoBuscarBoletas { get; set; }
        public RelayCommand ComandoAgregarFacturaBoletaDetalle { get; set; }
        public RelayCommand ComandoQuitarFacturaBoletaDetalle { get; set; }
        public RelayCommand ComandoGuardarDetalleBoletasManual { get; set; }
        public RelayCommand ComandoGuardarDetalleBoletasMasivo { get; set; }

        public RelayCommand ComandoMostrarBoletasPendientes { get; set; }
        public RelayCommand ComandoBuscarBoletasPendientes { get; set; }
        public RelayCommand<bool> ComandoIndicarTipoImportacion { get; set; }

        public RelayCommand CommandShowInvoiceDetailItems { get; set; }
        public RelayCommand<string> CommandSearchProductItem { get; set; }
        public RelayCommand CommandAddInvoiceDetailItem { get; set; }
        public RelayCommand CommandEditInvoiceDetailItem { get; set; }
        public RelayCommand CommandRemoveInvoiceDetailItem { get; set; }
        public RelayCommand CommandSaveInvoiceDetailItems { get; set; }

        public RelayCommand CommandShowInvoicePayment { get; set; }
        public RelayCommand CommandSaveInvoicePayment { get; set; }
        public RelayCommand CommandAddInvoicePayItem { get; set; }
        public RelayCommand CommandRemoveInvoicePayItem { get; set; }
        public RelayCommand CommandEditInvoicePayItem { get; set; }

        public RelayCommand CommandShowNotasCredito { get; set; }
        public RelayCommand CommandAddNotaCredito { get; set; }
        public RelayCommand CommandRemoveNotaCredito { get; set; }
        public RelayCommand CommandSaveNotasCredito { get; set; }

        public RelayCommand ComandoImprimirFacturacion { get; set; }
        
        private void InicializarComandos()
        {
            ComandoMostrarCrearFactura = new RelayCommand(MotrarCrearFactura);
            ComandoCrearFactura = new RelayCommand(CrearFactura);
            CommandShowEditInvoice = new RelayCommand(ShowEditInvoice);
            CommandEditInvoiceInfo = new RelayCommand(EditInvoiceInfo);
            ComandoBuscarPlantas = new RelayCommand<string>(BuscarPlantas);
            ComandoAnularFactura = new RelayCommand(AnularFactura);
            CommandSearchInvoice = new RelayCommand(SearchInvoice);
            ComandoNavegarFacturas = new RelayCommand(NavergarFacturas);
            ComandoRefrescar = new RelayCommand(NavergarFacturas);
            CommandActiveInvoice = new RelayCommand(ActiveInvoice);
            CommandSearchSubPlantas = new RelayCommand<string>(SearchSubPlantas);

            ComandoMostrarDetalleBoletas = new RelayCommand(MostrarDetalleBoletas);
            ComandoBuscarBoletas = new RelayCommand<string>(BuscarBoletas);
            ComandoAgregarFacturaBoletaDetalle = new RelayCommand(AgregarFacturaBoletaDetalle);
            ComandoQuitarFacturaBoletaDetalle = new RelayCommand(RemoverFacturaBoletaDetalle);
            ComandoGuardarDetalleBoletasManual = new RelayCommand(GuardarDetalleBoletasManual);
            ComandoGuardarDetalleBoletasMasivo = new RelayCommand(GuardarDetalleBoletasMasivo);

            ComandoMostrarBoletasPendientes = new RelayCommand(MostrarBoletasPendientes);
            ComandoBuscarBoletasPendientes = new RelayCommand(CargarBoltasPendientes);

            ComandoIndicarTipoImportacion = new RelayCommand<bool>(ActualizarTipoImportacion);
            ComandoImprimirFacturacion = new RelayCommand(ImprimirFacturacion);

            CommandShowInvoiceDetailItems = new RelayCommand(ShowInvoiceDetailItems);
            CommandSearchProductItem = new RelayCommand<string>(SearchProductItem);
            CommandAddInvoiceDetailItem = new RelayCommand(AddInvoiceDetailItem);
            CommandEditInvoiceDetailItem = new RelayCommand(EditInvoiceDetailItem);
            CommandRemoveInvoiceDetailItem = new RelayCommand(RemoveInvoiceDetailItem);
            CommandSaveInvoiceDetailItems = new RelayCommand(SaveInvoiceDetailItems);

            CommandShowInvoicePayment = new RelayCommand(ShowInvoicePayment);
            CommandSaveInvoicePayment = new RelayCommand(SaveInvoicePayment);
            CommandAddInvoicePayItem = new RelayCommand(AddInvoicePayItem);
            CommandRemoveInvoicePayItem = new RelayCommand(RemoveInvoicePayItem);
            CommandEditInvoicePayItem = new RelayCommand(EditInvoicePayItem);

            CommandShowNotasCredito = new RelayCommand(ShowNotasCredito);
            CommandAddNotaCredito = new RelayCommand(AddNotaCredito);
            CommandRemoveNotaCredito = new RelayCommand(RemoveNotaCredito);
            CommandSaveNotasCredito = new RelayCommand(SaveNotasCredito);
        }

        private void EditInvoiceInfo()
        {
            var mensajeError = ValidarFactura();

            if (!string.IsNullOrWhiteSpace(mensajeError))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeError);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new UpdateInfoInvoice
            {
                Factura = DatoFactura,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
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
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("EditarFacturaFlyout");
                CargarFacturas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ShowEditInvoice()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede editar la factura en estado NUEVO");
                return;
            }

            BuscarBoletasPendientes = false;
            DatoFactura = new FacturasDTO
            {
                FacturaId = FacturaSeleccionada.FacturaId,
                Estado = FacturaSeleccionada.Estado,
                ExonerationNo = FacturaSeleccionada.ExonerationNo,
                FacturaCategoriaId = FacturaSeleccionada.FacturaCategoriaId,
                TaxPercent = FacturaSeleccionada.TaxPercent,
                HasSubPlanta = FacturaSeleccionada.HasSubPlanta,
                Fecha = FacturaSeleccionada.Fecha,
                IsExonerated = FacturaSeleccionada.IsExonerated,
                NombrePlanta = FacturaSeleccionada.NombrePlanta,
                NombreSubPlanta = FacturaSeleccionada.NombreSubPlanta,
                NombreSucursal = FacturaSeleccionada.NombreSucursal,
                NumeroFactura = FacturaSeleccionada.NumeroFactura,
                Observaciones = FacturaSeleccionada.Observaciones,
                OrdenCompra = FacturaSeleccionada.OrdenCompra,
                PlantaId = FacturaSeleccionada.PlantaId,
                ProFormaNo = FacturaSeleccionada.ProFormaNo,
                RequiereOrdenCompra = FacturaSeleccionada.RequiereOrdenCompra,
                RequiereProForm = FacturaSeleccionada.RequiereProForm,
                Semana = FacturaSeleccionada.Semana,
                ShippingNumberRequired = FacturaSeleccionada.ShippingNumberRequired,
                SubPlantaId = FacturaSeleccionada.SubPlantaId,
                SucursalId = FacturaSeleccionada.SucursalId,
                TipoFactura = FacturaSeleccionada.TipoFactura,
                Total = FacturaSeleccionada.Total,
                LocalCurrencyAmount = FacturaSeleccionada.LocalCurrencyAmount,
                IsForeignCurrency = FacturaSeleccionada.IsForeignCurrency,
                HasUnitPriceItem = FacturaSeleccionada.HasUnitPriceItem
            };

            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(DatoFactura.NombrePlanta, DatoFactura.PlantaId);

            if (DatoFactura.SubPlantaId > 0)
            {
                SubPlantaSelected = new AutoCompleteEntry(DatoFactura.NombreSubPlanta, DatoFactura.SubPlantaId);
            }
            else
            {
                SubPlantaSelected = new AutoCompleteEntry(null, null);
            }

            CargarSlidePanel("EditarFacturaFlyout");
        }

        private void GetFacturaConteo()
        {
            FacturaSeleccionada.ConteoFacturas = $"{ListaFacturaDetalleBoletas.Where(b => b.BoletaId > 0).ToList().Count}/{ListaFacturaDetalleBoletas.Count}";
            RaisePropertyChanged(nameof(FacturaSeleccionada));
        }

        private void SaveNotasCredito()
        {
            if (FacturaSeleccionada == null) return;

            var invoicePaymentsDto = (from reg in NotasCreditoModel
                                      select new NotaCreditoDto
                                      {
                                          FacturaId = reg.FacturaId,
                                          Monto = reg.Monto,
                                          NotaCreditoId = reg.NotaCreditoId,
                                          NotaCreditoNo = reg.NotaCreditoNo,
                                          Observaciones = reg.Observaciones
                                      }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostNotasCredito
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                NotasCredito = invoicePaymentsDto,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
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
                CargarSlidePanel("InvoiceNotasCreditoFlyout");
                CargarFacturas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void RemoveNotaCredito()
        {
            if (NotaCreditoSelected == null) return;

            NotasCreditoModel.Remove(NotaCreditoSelected);
            CalculateNotasCredito();
        }

        private void AddNotaCredito()
        {
            if (NotaCreditoDato == null) return;

            if (string.IsNullOrWhiteSpace(NotaCreditoDato.Observaciones))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe ingresar una observación");
                return;
            }

            NotasCreditoModel notaCredito = NotasCreditoModel.FirstOrDefault(n => n.NotaCreditoNo == NotaCreditoDato.NotaCreditoNo);

            if (notaCredito == null)
            {
                NotasCreditoModel.Add(NotaCreditoDato);
            }

            CalculateNotasCredito();
        }

        private void ShowNotasCredito()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la factura debe ser Activo!");
                return;
            }

            IEnumerable<NotasCreditoModel> datos = from reg in NotasCredito
                        select new NotasCreditoModel
                        {
                            FacturaId = reg.FacturaId,
                            Monto = reg.Monto,
                            NotaCreditoId = reg.NotaCreditoId,
                            NotaCreditoNo = reg.NotaCreditoNo,
                            Observaciones = reg.Observaciones
                        };

            NotasCreditoModel = new ObservableCollection<NotasCreditoModel>(datos);
            CalculateNotasCredito();

            CargarSlidePanel("InvoiceNotasCreditoFlyout");
        }

        private void CalculateNotasCredito()
        {
            NotaCreditoDato = new NotasCreditoModel
            {
                FacturaId = FacturaSeleccionada.FacturaId
            };

            NotaCreditoDato.TotalNotasCredito = NotasCreditoModel.Sum(n => n.Monto);
            RaisePropertyChanged(nameof(NotaCreditoDato));
        }

        private void EditInvoicePayItem()
        {
            if (InvoicePaymentSelected == null) return;

            CalculateInvoicePayments();
            InvoicePaymentDato.BancoId = InvoicePaymentSelected.BancoId;
            InvoicePaymentDato.Monto = InvoicePaymentSelected.Monto;
            InvoicePaymentDato.ReferenciaBancaria = InvoicePaymentSelected.ReferenciaBancaria;
            InvoicePaymentDato.FacturaPagoId = InvoicePaymentSelected.FacturaPagoId;
            InvoicePaymentDato.FacturaId = InvoicePaymentSelected.FacturaId;
            InvoicePaymentDato.NombreBanco = InvoicePaymentSelected.NombreBanco;
            InvoicePaymentDato.FormaDePago = InvoicePaymentSelected.FormaDePago;
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

        private void RemoveInvoicePayItem()
        {
            if (InvoicePaymentSelected == null) return;

            InvoicePayments.Remove(InvoicePaymentSelected);
            CalculateInvoicePayments();
        }

        private void AddInvoicePayItem()
        {
            if (InvoicePaymentDato == null) return;

            FacturaPagoModel item = InvoicePayments.FirstOrDefault(p => p.FacturaPagoId == InvoicePaymentDato.FacturaPagoId && p.BancoId == InvoicePaymentDato.BancoId);

            if (item == null)
            {
                InvoicePayments.Add(InvoicePaymentDato);
            }
            else
            {
                item.Monto = InvoicePaymentDato.Monto;
                item.BancoId = InvoicePaymentDato.BancoId;
                item.NombreBanco = InvoicePaymentDato.NombreBanco;
            }

            CalculateInvoicePayments();
        }

        private void SaveInvoicePayment()
        {
            if (FacturaSeleccionada == null) return;

            var invoicePaymentsDto = (from reg in InvoicePayments
                                      select new FacturaPagoDto
                                      {
                                          FacturaId = reg.FacturaId,
                                          BancoId = reg.BancoId,
                                          FacturaPagoId = reg.FacturaPagoId,
                                          Monto = reg.Monto,
                                          ReferenciaBancaria = reg.ReferenciaBancaria,
                                          FormaDePago = reg.FormaDePago,
                                          FechaDePago = reg.FechaDePago
                                      }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostInvoicePagos
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                FacturaPagos = invoicePaymentsDto,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
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
                CargarSlidePanel("InvoicePaymentFlyout");
                CargarFacturas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });

        }

        private void ShowInvoicePayment()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado == Estados.NUEVO || FacturaSeleccionada.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Factura debe estar en estado ACTIVO o En Proceso");
                return;
            }

            if (Banks == null || !Banks.Any())
            {
                CargarBancos();
            }
            
            var datos = from reg in InvoicePaids
                    select new FacturaPagoModel
                    {
                        FacturaId = reg.FacturaId,
                        BancoId = reg.BancoId,
                        FacturaPagoId = reg.FacturaPagoId,
                        Monto = reg.Monto,
                        ReferenciaBancaria = reg.ReferenciaBancaria,
                        NombreBanco = reg.NombreBanco,
                        FormaDePago = reg.FormaDePago,
                        FechaDePago = reg.FechaDePago
                    };

            InvoicePayments = new ObservableCollection<FacturaPagoModel>(datos);
            CalculateInvoicePayments();

            CargarSlidePanel("InvoicePaymentFlyout");
        }

        private void CalculateInvoicePayments()
        {
            InvoicePaymentDato = new FacturaPagoModel
            {
                TotalPaid = InvoicePayments.Sum(m => m.Monto),
                Planta = FacturaSeleccionada.HasSubPlanta
                         ? FacturaSeleccionada.NombreSubPlanta
                         : FacturaSeleccionada.NombrePlanta,
                FacturaId = FacturaSeleccionada.FacturaId,
                FechaDePago = DateTime.Now
            };

            FormaDePagoSeleccionada = new CuentasFinancieraTiposDTO();
            BankSelected = new BancosDTO();
            RaisePropertyChanged(nameof(InvoicePaymentDato));
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
                Banks = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void SearchSubPlantas(string value)
        {
            if (Planta == null || !Planta.HasSubPlanta) return;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetSubPlantasByValue
            {
                PlantaId = Planta.PlantaId,
                ValorBusqueda = value,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    SubPlantas = (from reg in res
                                    select new AutoCompleteEntry(reg.NombreSubPlanta, reg.SubPlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarPlantas(string valorBusqueda)
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
                    ListaPlantasAutoComplete = (from reg in res
                                                select new AutoCompleteEntry(reg.NombrePlanta, reg.PlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ActiveInvoice()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la factura debe ser NUEVO!");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Activar_Factura, FacturaSeleccionada.NumeroFactura),
                Titulo = $"Facturas | {FacturaSeleccionada.NombrePlanta}"
            };

            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);

                    var request = new ActiveInvoiceById
                    {
                        InvoiceId = FacturaSeleccionada.FacturaId,
                        RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
                    };

                    _client.PutAsync(request, res =>
                    {
                        if (!string.IsNullOrWhiteSpace(res.ValidationErrorMessage))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.ValidationErrorMessage);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);

                        CargarFacturas();

                    }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
                }
            };

            DialogSettings = null;
        }

        private void SearchInvoice()
        {
            NumeroPaginaFacturas = 1;
            CargarFacturas();
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

        private void SaveInvoiceDetailItems()
        {
            var invoiceDetailItems = (from reg in InvoiceDetailItemsModel
                                select new FacturaDetalleItemDto
                                {
                                    FacturaDetalleItemId = reg.FacturaDetalleItemId,
                                    Cantidad = reg.Cantidad,
                                    CategoriaProductoId = reg.CategoriaProductoId,
                                    FacturaId = reg.FacturaId,
                                    Precio = reg.Precio
                                }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostInvoiceDetailItems
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                FacturaDetalleItems = invoiceDetailItems,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
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
                CargarSlidePanel("InvoiceDetailItemsFlyout");
                CargarFacturas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void RemoveInvoiceDetailItem()
        {
            if (InvoiceDetailItemSelected == null) return;

            InvoiceDetailItemsModel.Remove(InvoiceDetailItemSelected);
            CalculateInvoiceDetailTotal();
        }

        private void EditInvoiceDetailItem()
        {
            if (InvoiceDetailItemSelected == null) return;

            InvoiceDetailItemDato.FacturaDetalleItemId = InvoiceDetailItemSelected.FacturaDetalleItemId;
            InvoiceDetailItemDato.CategoriaProductoId = InvoiceDetailItemSelected.CategoriaProductoId;
        }

        private void SearchProductItem(string searchValue)
        {
            if (FacturaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetCategoriaProductoPorValorBusqueda
            {
                ValorBusqueda = searchValue,
                PlantaId = FacturaSeleccionada.PlantaId
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    ProductItems = (from reg in res
                                    select new AutoCompleteEntry(reg.Descripcion, reg.CategoriaProductoId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AddInvoiceDetailItem()
        {
            if (InvoiceDetailItemDato == null) return;

            string validationMessage = EvaluateInvoiceDetailItem();

            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, validationMessage);
                return;
            }

            FacturaDetalleItemModel detailItem = !FacturaSeleccionada.HasUnitPriceItem
                                                 ? InvoiceDetailItemsModel.FirstOrDefault(d => d.CategoriaProductoId == InvoiceDetailItemDato.CategoriaProductoId)
                                                 : InvoiceDetailItemsModel.FirstOrDefault(d => d.CategoriaProductoId == InvoiceDetailItemDato.CategoriaProductoId && d.Precio == InvoiceDetailItemDato.Precio);

            if (detailItem == null)
            {
                InvoiceDetailItemDato.GetSubTotalItem(FacturaSeleccionada);
                InvoiceDetailItemsModel.Add(InvoiceDetailItemDato);
            }
            else
            {
                detailItem.Cantidad = InvoiceDetailItemDato.Cantidad;
                detailItem.Precio = InvoiceDetailItemDato.Precio;
            }

            CalculateInvoiceDetailTotal();
        }

        private void CalculateInvoiceDetailTotal()
        {
            InvoiceDetailItemDato = new FacturaDetalleItemModel();

            if (!InvoiceDetailItemsModel.Any()) return;

            foreach (FacturaDetalleItemModel item in InvoiceDetailItemsModel)
            {
                item.GetSubTotalItem(FacturaSeleccionada);
            }

            InvoiceDetailItemDato.SubTotal = InvoiceDetailItemsModel.Sum(s => s.SubTotal);
            InvoiceDetailItemDato.Tax = InvoiceDetailItemsModel.Sum(s => s.Tax);
            InvoiceDetailItemDato.Total = InvoiceDetailItemsModel.Sum(s => s.SubTotal + s.Tax);

            ProductItemSelected = new AutoCompleteEntry(null, null);
        }

        private string EvaluateInvoiceDetailItem()
        {
            if (InvoiceDetailItemDato.Cantidad <= 0)
            {
                return "Debe ingresar la cantidad mayor a 0";
            }

            if (InvoiceDetailItemDato.CategoriaProductoId <= 0)
            {
                return "Debe seleccionar un producto";
            }

            if (InvoiceDetailItemDato.Precio <= 0)
            {
                return "Debe ingresar un precio mayor a 0";
            }

            return string.Empty;
        }

        private void LoadInvoiceDetailItems()
        {
            InvoiceDetailItems = new List<FacturaDetalleItemDto>();
            if (FacturaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDetailItemsByInvoiceId
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                InvoiceDetailItems = res;
                RaisePropertyChanged(nameof(InvoiceDetailItems));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadInvoicePayments()
        {
            InvoicePaids = new List<FacturaPagoDto>(); ;
            if (FacturaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPagosByInvoiceId
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                InvoicePaids = res;
                RaisePropertyChanged(nameof(InvoicePaids));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadNotasCredito()
        {
            NotasCredito = new List<NotaCreditoDto>();
            if (FacturaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetNotasCreditoByInvoiceId
            {
                InvoiceId = FacturaSeleccionada.FacturaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                NotasCredito = res;
                RaisePropertyChanged(nameof(InvoicePaids));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ShowInvoiceDetailItems()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Factura debe estar en estado NUEVO!");
                return;
            }

            IEnumerable<FacturaDetalleItemModel> items = from reg in InvoiceDetailItems
                                                         select new FacturaDetalleItemModel
                                                         {
                                                             FacturaDetalleItemId = reg.FacturaDetalleItemId,
                                                             Cantidad = reg.Cantidad,
                                                             CategoriaProductoId = reg.CategoriaProductoId,
                                                             FacturaId = reg.FacturaId,
                                                             ForeignCurrencyAmount = reg.ForeignCurrencyItemAmount,
                                                             LocalCurrencyAmount = reg.LocalCurrencyItemAmount,
                                                             Precio = reg.Precio,
                                                             ProductoDescripcion = reg.ProductoDescripcion,
                                                             Tax = reg.TaxItem
                                                         };

            InvoiceDetailItemsModel = new ObservableCollection<FacturaDetalleItemModel>(items);
            CalculateInvoiceDetailTotal();
            CargarSlidePanel("InvoiceDetailItemsFlyout");
        }

        private void ImprimirFacturacion()
        {
            ObtenerInformacionReporte();
        }

        private void ObtenerInformacionReporte()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetFacturacion
            {
                FacturacionId = FacturaSeleccionada.FacturaId
            };
            _client.GetAsync(request, res =>
            {
                ReporteFacturacion = new List<ReporteFacturacionDTO>(res);

                if (!ReporteFacturacion.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Existe Infomacion que Mostrar");
                    return;
                }

                ImprimirReporte("ReporteFacturacion.rdlc");
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void ImprimirReporte(string nombreReporte)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte de Facturación";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = nombreReporte;
                manejadorReporte.Datos = ReporteFacturacion;
                manejadorReporte.NombreDataSet = "DataSet";
                manejadorReporte.AgregarParametro("Empresa", "Comercial Colindres");
                manejadorReporte.MostarReporte();
            });
        }

        private void ActualizarTipoImportacion(bool esManual)
        {
            EsImportacionManual = esManual;
            
            if (!esManual)
            {
                ListaFacturaDetalleBoletasMasivoModel = new ObservableCollection<FacturaDetalleBoletasModel>();
            }
        }

        private void CargarBoltasPendientes()
        {
            if (PlantaAutoCompleteSeleccionada == null)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe seleccionar una Planta");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletasPendientesDeFacturar
            {
                PlantaId = (int)PlantaAutoCompleteSeleccionada.Id
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                ListaBoletasPorFacturar = res;
                TotalBoletasPendienteFacturar = ListaBoletasPorFacturar.Sum(t => t.TotalFacturaVenta);

                if (!res.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Information, string.Format("No existen Boletas pendientes para la Planta {0}", PlantaAutoCompleteSeleccionada.DisplayName));
                }
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarBoletasPendientes()
        {
            ListaBoletasPorFacturar = new List<BoletasDTO>();
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            BuscarBoletasPendientes = true;

            CargarSlidePanel("BoletasPendientesFacturarFlyout");
        }

        private void AnularFactura()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Anular Facturas Activas");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Anular_Datos, FacturaSeleccionada.NumeroFactura),
                Titulo = "Facturas"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutFacturaAnular
                    {
                        FacturaId = FacturaSeleccionada.FacturaId,
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
                        CargarFacturas();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void GuardarDetalleBoletasManual()
        {
            if (!ListaFacturaDetalleBoletasMasivoModel.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existen Boletas agregadas al Detalle");
                return;
            }

            
            var listadoBoletas = from reg in ListaFacturaDetalleBoletasMasivoModel
                                 select new FacturaDetalleBoletasDTO
                                 {
                                     CodigoBoleta = reg.CodigoBoleta,
                                     NumeroEnvio = reg.NumeroEnvio,
                                     PesoProducto = reg.PesoProducto
                                 };

            var request = new PostDetalleBoletas
            {
                DetalleBoletas = listadoBoletas.ToList(),
                FacturaId = FacturaSeleccionada.FacturaId,
                EsImportacionMasiva = false,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (res.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Existen Validaciones Pendientes");
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("AgregarFacturaBoletasFlyout");
                CargarFacturas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GuardarDetalleBoletasMasivo()
        {
            if (!ListaFacturaDetalleBoletasMasivoModel.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existen Boletas agregadas al Detalle Masivo");
                return;
            }
            
            var listadoBoletas = from reg in ListaFacturaDetalleBoletasMasivoModel
                                 select new FacturaDetalleBoletasDTO
                                 {
                                     CodigoBoleta = reg.CodigoBoleta,
                                     NumeroEnvio = reg.NumeroEnvio,
                                     PesoProducto = reg.PesoProducto,
                                     FechaIngreso = reg.FechaIngreso,
                                     UnitPrice = reg.UnitPrice
                                 };

            var request = new PostDetalleBoletas
            {
                DetalleBoletas = listadoBoletas.ToList(),
                FacturaId = FacturaSeleccionada.FacturaId,
                EsImportacionMasiva = true,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PostAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (res.Any())
                {
                    ImportacionErrores = true;
                    ListaValidacionBoletas = res;
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Existen Validaciones Pendientes");
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("AgregarFacturaBoletasFlyout");
                CargarFacturas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarFacturaBoletaDetalle()
        {
            if (DatoFacturaDetalleBoleta.BoletaId == 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe seleccionar una Boleta disponible para Facturar");
                return;
            }

            var itemBoleta = ListaFacturaDetalleBoletasModel.FirstOrDefault(b => b.BoletaId == DatoFacturaDetalleBoleta.BoletaId);

            if (itemBoleta == null)
            {
                ListaFacturaDetalleBoletasModel.Add(DatoFacturaDetalleBoleta);                
            }

            DatoFacturaDetalleBoleta = new FacturaDetalleBoletasModel();
            ObtenerTotalFactura();
            BoletaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);         
        }

        private void RemoverFacturaBoletaDetalle()
        {
            if (FacturaDetalleBoletaSeleccionado != null)
            {
                ListaFacturaDetalleBoletasModel.Remove(FacturaDetalleBoletaSeleccionado);
                ObtenerTotalFactura();
            }
        }

        private void MostrarDetalleBoletas()
        {
            if (FacturaSeleccionada == null) return;

            if (FacturaSeleccionada.Estado != Estados.ACTIVO)
            {
                if (FacturaSeleccionada.Estado == Estados.CERRADO && ListaFacturaDetalleBoletas.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "La factura ya esta cerrada y cuenta con detalle de boletas!");
                    return;
                }

                if (FacturaSeleccionada.Estado == Estados.CERRADO && !ListaFacturaDetalleBoletas.Any())
                {
                    LoadPanelAddInvoiceBoletasDetail();
                    return;
                }

                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede agregar detalle de Boletas a Facturas Activas");
                return;
            }

            LoadPanelAddInvoiceBoletasDetail();
        }

        private void LoadPanelAddInvoiceBoletasDetail()
        {
            BoletaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);

            EsImportacionManual = true;
            ImportacionErrores = false;
            XlsColeccionImportacion = null;
            DatoFacturaDetalleBoleta = new FacturaDetalleBoletasModel();
            ListaFacturaDetalleBoletasModel = new ObservableCollection<FacturaDetalleBoletasModel>();

            CargarSlidePanel("AgregarFacturaBoletasFlyout");
        }

        private void NavergarFacturas()
        {
            CargarFacturas();
        }

        private void MotrarCrearFactura()
        {
            SucursalSeleccionada = new SucursalesDTO();
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            SubPlantaSelected = new AutoCompleteEntry(null, null);
            BuscarBoletasPendientes = false;

            DatoFactura = new FacturasDTO
            {
                Fecha = DateTime.Now,
                IsExonerated = true
            };

            Planta = new ClientePlantasDTO();

            CargarSlidePanel("AgregarFacturaFlyout");
        }

        private void CrearFactura()
        {
            var mensajeError = ValidarFactura();

            if (!string.IsNullOrWhiteSpace(mensajeError))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeError);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostFactura
            {
                Factura = DatoFactura,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
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
                CargarSlidePanel("AgregarFacturaFlyout");
                CargarFacturas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
        
        private string ValidarFactura()
        {
            var mensajeError = string.Empty;

            if (DatoFactura.SucursalId == 0)
            {
                return "Debe seleccionar la Empresa Destino";
            }

            if (DatoFactura.PlantaId == 0)
            {
                return "Debe Ingresar la Planta Origen";
            }

            if (DatoFactura.FacturaCategoriaId == 0)
            {
                return "Debe ingresar el Tipo de Factura";
            }

            return mensajeError;
        }

        private void BuscarBoletas(string valorBusqueda)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetBoletasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                PlantaId = FacturaSeleccionada.PlantaId
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    ListaBoletasAutoComplete = (from reg in res
                                                select new AutoCompleteEntry(reg.CodigoBoleta, reg.BoletaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarSucursales()
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);

            var request = new GetAllSucursales
            {
            };
            client.GetAsync(request, res =>
            {
                Sucursales = new List<SucursalesDTO>(res);
                SucursalSeleccionada = Sucursales.FirstOrDefault();
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void CargarBoletasPorFactura()
        {
            ListaFacturaDetalleBoletas = new List<FacturaDetalleBoletasDTO>();
            if (FacturaSeleccionada == null) return;
            
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDetalleBoletasPorFacturaId
            {
                FacturaId = FacturaSeleccionada.FacturaId
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaFacturaDetalleBoletas = res;

                if (ListaFacturaDetalleBoletas.Any())
                {
                    GetFacturaConteo();
                }

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarFacturas()
        {
            var facturaId = FacturaSeleccionada != null ? FacturaSeleccionada.FacturaId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorFacturas
            {
                PaginaActual = NumeroPaginaFacturas,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaFacturas
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaFacturasControl = res;
                if (BusquedaFacturasControl.Items != null && BusquedaFacturasControl.Items.Any())
                {
                    if (facturaId == 0)
                    {
                        FacturaSeleccionada = BusquedaFacturasControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        FacturaSeleccionada = BusquedaFacturasControl.Items.FirstOrDefault(r => r.FacturaId == facturaId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });

            InicializarPropiedades();
        }

        private void CargarFacturasCategorias()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllFacturasCategorias
            {
            };

            _client.GetAsync(request, res =>
            {
                ListaFacturaCategorias = res;
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void CargarDatosIniciales()
        {
            CargarFacturas();
            CargarSucursales();
            GetPlantas();
            CargarFacturasCategorias();
            CargarFormarDePago();
        }

        private void ObtenerDatosBoleta()
        {
            var request = new GetBoleta
            {
                BoletaNo = BoletaAutoCompleteSeleccionada.DisplayName,
                PlantaId = FacturaSeleccionada.PlantaId
            };

            var datosBoleta = _client.Get(request);
            DatoFacturaDetalleBoleta = new FacturaDetalleBoletasModel
            {
              BoletaId = datosBoleta.BoletaId,
              CodigoBoleta = datosBoleta.CodigoBoleta,
              NombreProveedor = datosBoleta.NombreProveedor,
              DescripcionTipoProducto = datosBoleta.DescripcionTipoProducto,
              PesoProducto = datosBoleta.PesoProducto,
              PrecioVenta = datosBoleta.PrecioProductoVenta,
              PrecioProductoCompra = datosBoleta.PrecioProductoCompra,
              TotalPrecioBoleta = datosBoleta.PesoProducto * datosBoleta.PrecioProductoVenta              
            };

            ObtenerTotalFactura();
        }

        private void ObtenerTotalFactura()
        {
            DatoFacturaDetalleBoleta.TotalPago = ListaFacturaDetalleBoletasModel.Sum(t => t.TotalPrecioBoleta);
            RaisePropertyChanged("DatoFacturaDetalleBoleta");
        }

        private void ProcesarImportacion()
        {
            if (FacturaSeleccionada == null) return;

            if (XlsColeccionImportacion != null && XlsColeccionImportacion.Count >= 0)
            {
                foreach (var item in XlsColeccionImportacion)
                {
                    var type = item.GetType();
                    var elementoDetalle = new FacturaDetalleBoletasModel();

                    foreach (var propiedad in type.GetProperties())
                    {
                        AsignarDatosDeImportacion(elementoDetalle, propiedad, item);
                    }

                    var existeBoleta = FacturaSeleccionada.ShippingNumberRequired
                                       ? ListaFacturaDetalleBoletasMasivoModel.FirstOrDefault(f => f.NumeroEnvio == elementoDetalle.NumeroEnvio)
                                       : ListaFacturaDetalleBoletasMasivoModel.FirstOrDefault(f => f.CodigoBoleta == elementoDetalle.CodigoBoleta);

                    if (existeBoleta == null)
                    {
                        if (FacturaSeleccionada.ShippingNumberRequired && string.IsNullOrWhiteSpace(elementoDetalle.NumeroEnvio)) continue;
                        if (!FacturaSeleccionada.ShippingNumberRequired && string.IsNullOrWhiteSpace(elementoDetalle.CodigoBoleta)) continue;

                        ListaFacturaDetalleBoletasMasivoModel.Add(elementoDetalle);
                    }
                }

                RaisePropertyChanged(nameof(ListaFacturaDetalleBoletasMasivoModel));
            }
        }

        private static void AsignarDatosDeImportacion(FacturaDetalleBoletasModel elementoDetalle, PropertyInfo propiedad,
                                               object item)
        {
            var value = propiedad.GetValue(item, null);

            if (propiedad.Name.ToUpper().Contains("CODIGOBOLETA") || propiedad.Name.ToUpper().Contains("CODIGO_BOLETA") || propiedad.Name.ToUpper().Contains("CODIGO BOLETA"))
            {
                elementoDetalle.CodigoBoleta = value != null ? value.ToString() : string.Empty; ;
            }

            if (propiedad.Name.ToUpper().Contains("NUMEROENVIO") || propiedad.Name.ToUpper().Contains("NUMERO_ENVIO") || propiedad.Name.ToUpper().Contains("# Envío"))
            {
                elementoDetalle.NumeroEnvio = value != null ? value.ToString() : string.Empty; ;
            }

            if (propiedad.Name.ToUpper().Contains("PESOPRODUCTO") || propiedad.Name.ToUpper().Contains("PESO_PRODUCTO") || propiedad.Name.ToUpper().Contains("PESO PRODUCTO"))
            {
                elementoDetalle.PesoProducto = value != null ? Convert.ToDecimal(value) : 0;
            }

            if (propiedad.Name.ToUpper().Contains("FECHAINGRESO") || propiedad.Name.ToUpper().Contains("FECHA_INGRESO") || propiedad.Name.ToUpper().Contains("FECHA INGRESO"))
            {
                elementoDetalle.FechaIngreso = value != null ? Convert.ToDateTime(value) : DateTime.MinValue;
            }

            if (propiedad.Name.ToUpper().Contains("PRECIOUNITARIO") || propiedad.Name.ToUpper().Contains("PRECIO_UNITARIO") || propiedad.Name.ToUpper().Contains("PRECIO UNITARIO"))
            {
                elementoDetalle.UnitPrice = value != null ? Convert.ToDecimal(value) : 0;
            }
        }

        private void InicializarPropiedades()
        {
            var permiso =
                InformacionSistema.UsuarioActivo.UsuariosOpciones.FirstOrDefault(r => r.NombreOpcion == "Admin - Facturacion");

            if (permiso == null)
            {
                IsVisibleBillingPermission = false;
            }
            else
            {
                IsVisibleBillingPermission = true;                           
            }

            RaisePropertyChanged(nameof(IsVisibleBillingPermission));
        }

        private void CargarDatosPruebas()
        {
            BusquedaFacturasControl = new BusquedaFacturasDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaFacturas()
            };
            FacturaSeleccionada = BusquedaFacturasControl.Items.FirstOrDefault();

            InvoiceDetailItems = DatosDiseño.ListaFacturaDetalleItem();
        }        
    }
}
