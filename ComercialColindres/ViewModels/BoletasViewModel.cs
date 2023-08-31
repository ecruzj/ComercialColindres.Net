using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago;
using ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.RequestDTOs.Bonificaciones;
using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using ComercialColindres.DTOs.RequestDTOs.PrecioDescargas;
using ComercialColindres.DTOs.RequestDTOs.PrecioProductos;
using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace ComercialColindres.ViewModels
{
    public class BoletasViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public BoletasViewModel(IServiciosComunes serviciosComunes)
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

        public int NumeroPagina
        {
            get
            {
                return _numeroPagina;
            }
            set
            {
                _numeroPagina = value;
                RaisePropertyChanged("NumeroPagina");
            }
        }
        int _numeroPagina = 1;

        public BusquedaBoletasDTO BusquedaBoletasControl
        {
            get
            {
                return _busquedaBoletasControl;
            }
            set
            {
                _busquedaBoletasControl = value;
                RaisePropertyChanged("BusquedaBoletasControl");
            }
        }
        private BusquedaBoletasDTO _busquedaBoletasControl;
        
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

                LoadDataBoleta();
            }
        }
        private BoletasDTO _boletaSeleccionada;

        public string ValorBusquedaBoleta
        {
            get
            {
                return _valorBusquedaBoleta;
            }
            set
            {
                _valorBusquedaBoleta = value;
                RaisePropertyChanged("ValorBusquedaBoleta");
            }
        }
        private string _valorBusquedaBoleta;
        
        public BoletasDTO DatoBoleta
        {
            get
            {
                return _datoBoleta;
            }
            set
            {
                _datoBoleta = value;
                RaisePropertyChanged("DatoBoleta");
            }
        }
        private BoletasDTO _datoBoleta;
                        
        public List<AutoCompleteEntry> ListaProveedoresAutoComplete
        {
            get { return _listaProveedoresAutoComplete; }
            set
            {
                _listaProveedoresAutoComplete = value;
                RaisePropertyChanged("ListaProveedoresAutoComplete");
            }
        }
        private List<AutoCompleteEntry> _listaProveedoresAutoComplete;
        
        public AutoCompleteEntry ProveedorAutoCompleteSeleccionado
        {
            get { return _proveedorAutoCompleteSeleccionado; }
            set
            {
                _proveedorAutoCompleteSeleccionado = value;
                RaisePropertyChanged("ProveedorAutoCompleteSeleccionado");

                if (ProveedorAutoCompleteSeleccionado == null || ProveedorAutoCompleteSeleccionado.Id == null) return;
                if (DatoBoleta == null) return;

                DatoBoleta.ProveedorId = (int)ProveedorAutoCompleteSeleccionado.Id;
                DatoBoleta.NombreProveedor = (string)ProveedorAutoCompleteSeleccionado.DisplayName;

                RaisePropertyChanged("DatoBoleta");
            }
        }
        private AutoCompleteEntry _proveedorAutoCompleteSeleccionado;

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
                RaisePropertyChanged(nameof(PlantaAutoCompleteSeleccionada));

                if (DatoBoleta == null) return;

                DatoBoleta.PlantaId = PlantaAutoCompleteSeleccionada != null && PlantaAutoCompleteSeleccionada.Id != null ? (int)PlantaAutoCompleteSeleccionada.Id : 0;
                RaisePropertyChanged("DatoBoleta");

                //ObtenerPrecioProducto();
                GetBonificacion();
                GetPlanta();
            }
        }
        private AutoCompleteEntry _plantaAutoCompleteSeleccionada;
        
        public List<AutoCompleteEntry> ListaCategoriaProductos
        {
            get { return _listaCategoriaProductos; }
            set
            {
                _listaCategoriaProductos = value;
                RaisePropertyChanged("ListaCategoriaProductos");
            }
        }
        private List<AutoCompleteEntry> _listaCategoriaProductos;


        public AutoCompleteEntry CategoriaProductoAutoCompleteSeleccionado
        {
            get
            {
                return _categoriaProductoAutoCompleteSeleccionado;
            }
            set
            {
                _categoriaProductoAutoCompleteSeleccionado = value;
                RaisePropertyChanged("CategoriaProductoAutoCompleteSeleccionado");

                if (CategoriaProductoAutoCompleteSeleccionado == null || CategoriaProductoAutoCompleteSeleccionado.Id == null) return;
                if (DatoBoleta == null) return;

                DatoBoleta.CategoriaProductoId = (int)CategoriaProductoAutoCompleteSeleccionado.Id;
                DatoBoleta.DescripcionTipoProducto = (string)CategoriaProductoAutoCompleteSeleccionado.DisplayName;
                RaisePropertyChanged("DatoBoleta");

                ObtenerPrecioProducto();
                GetBonificacion();
            }
        }        
        private AutoCompleteEntry _categoriaProductoAutoCompleteSeleccionado;
        
        public DescargadoresDTO DatoDescarga
        {
            get
            {
                return _datoDescarga;
            }
            set
            {
                _datoDescarga = value;
                RaisePropertyChanged("DatoDescarga");
            }
        }
        private DescargadoresDTO _datoDescarga;

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
                RaisePropertyChanged("CuadrillaAutoCompleteSeleccionado");

                if (DatoBoleta != null)
                {
                    DatoBoleta.CuadrillaId = CuadrillaAutoCompleteSeleccionado != null && CuadrillaAutoCompleteSeleccionado.Id != null ? (int)CuadrillaAutoCompleteSeleccionado.Id : 0;
                    DatoBoleta.NombreDescargador = CuadrillaAutoCompleteSeleccionado != null && CuadrillaAutoCompleteSeleccionado.DisplayName != null ? CuadrillaAutoCompleteSeleccionado.DisplayName : string.Empty;
                    RaisePropertyChanged("DatoBoleta");
                }

                if (CuadrillaAutoCompleteSeleccionado != null && CuadrillaAutoCompleteSeleccionado.Id != null)
                {
                    DatoDescarga.CuadrillaId = (int)CuadrillaAutoCompleteSeleccionado.Id;
                    RaisePropertyChanged("DatoDescarga");
                }
            }
        }
        private AutoCompleteEntry _cuadrillaAutoCompleteSeleccionado;
        
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
        
        public List<BoletaCierresDTO> ListaBoletaCierres
        {
            get
            {
                return _listaBoletaCierres;
            }
            set
            {
                if (_listaBoletaCierres != value)
                {
                    _listaBoletaCierres = value;
                    RaisePropertyChanged("ListaBoletaCierres");
                }
            }
        }
        private List<BoletaCierresDTO> _listaBoletaCierres;
                
        public ObservableCollection<BoletaCierresModel> ListaBoletaCierresModel
        {
            get
            {
                return _listaBoletaCierresModel;
            }
            set
            {
                if (_listaBoletaCierresModel != value)
                {
                    _listaBoletaCierresModel = value;
                    RaisePropertyChanged("ListaBoletaCierresModel");
                }
            }
        }
        private ObservableCollection<BoletaCierresModel> _listaBoletaCierresModel;
        
        public BoletaCierresModel BoletaCierreSeleccionado
        {
            get
            {
                return _boletaCierreSeleccionado;
            }
            set
            {
                if (_boletaCierreSeleccionado != value)
                {
                    _boletaCierreSeleccionado = value;
                    RaisePropertyChanged("BoletaCierreSeleccionado");
                }
            }
        }
        private BoletaCierresModel _boletaCierreSeleccionado;

        public BoletaCierresModel DatoBoletaCierre
        {
            get
            {
                return _datoBoletaCierre;
            }
            set
            {
                if (DatoBoletaCierre != value)
                {
                    _datoBoletaCierre = value;
                    RaisePropertyChanged("DatoBoletaCierre");
                }
            }
        }
        private BoletaCierresModel _datoBoletaCierre;

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

                if (DatoBoletaCierre != null)
                {
                    DatoBoletaCierre.BancoId = BancoSeleccionado.BancoId;
                    DatoBoletaCierre.NombreBanco = BancoSeleccionado.Descripcion;

                    if (FormaDePagoSeleccionada == null) return;
                    CargarLineasDeCreditoPorBanco(DatoBoletaCierre.BancoId, FormaDePagoSeleccionada.CuentaFinancieraTipoId);
                }                                          
            }
        }
        private BancosDTO _bancoSeleccionado;
        
        public BancosDTO BancoSeleccionadoBOD
        {
            get { return _bancoSeleccionadoBOD; }
            set
            {
                _bancoSeleccionadoBOD = value;
                RaisePropertyChanged("BancoSeleccionadoBOD");

                if (BancoSeleccionadoBOD == null) return;

                if (DatoBoletaOtraDeduccion != null)
                {
                    DatoBoletaOtraDeduccion.BancoId = BancoSeleccionadoBOD.BancoId;
                    DatoBoletaOtraDeduccion.NombreBanco = BancoSeleccionadoBOD.Descripcion;

                    if (FormaDePagoSeleccionadaBOD == null) return;
                    CargarLineasDeCreditoPorBanco(DatoBoletaOtraDeduccion.BancoId, FormaDePagoSeleccionadaBOD.CuentaFinancieraTipoId);
                }
            }
        }
        private BancosDTO _bancoSeleccionadoBOD;

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
                
                if (DatoBoletaCierre != null && !DatoBoletaCierre.EsEdicion)
                {
                    DatoBoletaCierre = new BoletaCierresModel
                    {
                        EsEdicion = false,
                        BoletaId = BoletaSeleccionada.BoletaId,
                        CantidadJustificada = ListaBoletaCierresModel.Sum(c => c.Monto),
                        FechaPago = DateTime.Now
                    };

                    LineasCredito = new List<LineasCreditoDTO>();
                    LineaCreditoSeleccionada = new LineasCreditoDTO();
                    BancoSeleccionado = new BancosDTO();
                }
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;
        
        public CuentasFinancieraTiposDTO FormaDePagoSeleccionadaBOD
        {
            get { return _formaDePagoSeleccionadaBOD; }
            set
            {
                _formaDePagoSeleccionadaBOD = value;
                RaisePropertyChanged("FormaDePagoSeleccionadaBOD");

                if (FormaDePagoSeleccionadaBOD == null) return;

                if (!FormaDePagoSeleccionadaBOD.RequiereBanco)
                {
                    CargarLineasDeCreditoCajaChica();
                }
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionadaBOD;

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

                if (DatoBoletaCierre != null)
                {
                    DatoBoletaCierre.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                    DatoBoletaCierre.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
                }

                if (DatoBoletaOtraDeduccion != null)
                {
                    DatoBoletaOtraDeduccion.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                    DatoBoletaOtraDeduccion.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito;
                }
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;

        public ObservableCollection<PrestamoModel> PendingLoans
        {
            get { return _pendingLoans; }
            set
            {
                _pendingLoans = value;
                RaisePropertyChanged(nameof(PendingLoans));
            }
        }
        private ObservableCollection<PrestamoModel> _pendingLoans;

        public PrestamoModel PendingLoanSelected
        {
            get
            {
                return _pendingLoanSelected;
            }
            set
            {
                _pendingLoanSelected = value;
                RaisePropertyChanged(nameof(PendingLoanSelected));

                if (IsInDesignMode)
                {
                    return;
                }

                if (PendingLoanSelected != null)
                {
                    DatoAbonoPrestamo.CodigoPrestamo = PendingLoanSelected.CodigoPrestamo;
                    DatoAbonoPrestamo.PrestamoId = PendingLoanSelected.PrestamoId;

                    RaisePropertyChanged("DatoAbonoPrestamo");
                }
            }
        }
        private PrestamoModel _pendingLoanSelected;

        public List<PagoPrestamosDTO> LoansPaymentsBoleta
        {
            get { return _loansPaymentsBoleta; }
            set
            {
                _loansPaymentsBoleta = value;
                RaisePropertyChanged(nameof(LoansPaymentsBoleta));
            }
        }
        private List<PagoPrestamosDTO> _loansPaymentsBoleta;

        public ObservableCollection<PagoPrestamoModel> AssignedLoansBoleta
        {
            get
            {
                return _assignedLoansBoleta;
            }
            set
            {
                _assignedLoansBoleta = value;
                RaisePropertyChanged(nameof(AssignedLoansBoleta));
            }
        }
        private ObservableCollection<PagoPrestamoModel> _assignedLoansBoleta;

        public PagoPrestamoModel AssignedLoanBoletaSelected 
        {
            get
            {
                return _assignedLoanBoletaSelected;
            }
            set
            {
                _assignedLoanBoletaSelected = value;
                RaisePropertyChanged(nameof(AssignedLoanBoletaSelected));                
            }
        }
        private PagoPrestamoModel _assignedLoanBoletaSelected;

        public PagoPrestamoModel DatoAbonoPrestamo
        {
            get
            {
                return _datoAbonoPrestamo;
            }
            set
            {
                _datoAbonoPrestamo = value;
                RaisePropertyChanged(nameof(DatoAbonoPrestamo));
            }
        }
        private PagoPrestamoModel _datoAbonoPrestamo;
        
        public List<BoletaOtrasDeduccionesDTO> ListaBoletaOtrasDeducciones
        {
            get { return _listaBoletaOtrasDeducciones; }
            set
            {
                _listaBoletaOtrasDeducciones = value;
                RaisePropertyChanged("ListaBoletaOtrasDeducciones");
            }
        }
        private List<BoletaOtrasDeduccionesDTO> _listaBoletaOtrasDeducciones;

        public ObservableCollection<BoletaOtrasDeduccionesModel> ListaBoletaOtrasDeduccionesModel
        {
            get { return _listaBoletaOtrasDeduccionesModel; }
            set
            {
                _listaBoletaOtrasDeduccionesModel = value;
                RaisePropertyChanged("ListaBoletaOtrasDeduccionesModel");
            }
        }
        private ObservableCollection<BoletaOtrasDeduccionesModel> _listaBoletaOtrasDeduccionesModel;
        
        public BoletaOtrasDeduccionesModel DatoBoletaOtraDeduccion
        {
            get { return _datoBoletaOtraDeduccion; }
            set
            {
                _datoBoletaOtraDeduccion = value;
                RaisePropertyChanged("DatoBoletaOtraDeduccion");
            }
        }
        private BoletaOtrasDeduccionesModel _datoBoletaOtraDeduccion;

        public BoletaOtrasDeduccionesModel BoletaOtraDeduccionSeleccionado
        {
            get { return _boletaOtraDeduccionSeleccionado; }
            set
            {
                _boletaOtraDeduccionSeleccionado = value;
                RaisePropertyChanged("BoletaOtraDeduccionSeleccionado");
            }
        }
        private BoletaOtrasDeduccionesModel _boletaOtraDeduccionSeleccionado;
        
        public List<BoletaHumedadPagoDto> BoletaHumidityPayments
        {
            get { return _boletaHumidityPayments; }
            set
            {
                _boletaHumidityPayments = value;
                RaisePropertyChanged(nameof(BoletaHumidityPayments));
            }
        }
        private List<BoletaHumedadPagoDto> _boletaHumidityPayments;
        
        public ObservableCollection<BoletaHumedadPagoModel> BoletaHumedadPagoModel
        {
            get { return _boletaHumedadPagoModel; }
            set
            {
                _boletaHumedadPagoModel = value;
                RaisePropertyChanged(nameof(BoletaHumedadPagoModel));

                GetTotalHumidityPayments();
            }
        }
        private ObservableCollection<BoletaHumedadPagoModel> _boletaHumedadPagoModel;

        public BoletaHumedadPagoDto DatoBoletaHumidityPayment
        {
            get { return _datoBoletaHumidityPayment; }
            set
            {
                _datoBoletaHumidityPayment = value;
                RaisePropertyChanged(nameof(DatoBoletaHumidityPayment));
            }
        }
        private BoletaHumedadPagoDto _datoBoletaHumidityPayment;

        public BoletaHumedadPagoModel BoletaHumidityPaymentSelected
        {
            get { return _boletaHumidityPaymentSelected; }
            set
            {
                _boletaHumidityPaymentSelected = value;
                RaisePropertyChanged(nameof(BoletaHumidityPaymentSelected));
            }
        }
        private BoletaHumedadPagoModel _boletaHumidityPaymentSelected;
        
        public List<BoletaHumedadAsignacionDto> BoletasHumidityByVendor
        {
            get { return _boletasHumidityByVendor; }
            set
            {
                _boletasHumidityByVendor = value;
                RaisePropertyChanged(nameof(BoletasHumidityByVendor));
            }
        }
        private List<BoletaHumedadAsignacionDto> _boletasHumidityByVendor;

        public BoletaHumedadAsignacionDto BoletaHumidityByVendorSelected
        {
            get { return _boletaHumidityByVendorSelected; }
            set
            {
                _boletaHumidityByVendorSelected = value;
                RaisePropertyChanged(nameof(BoletaHumidityByVendorSelected));
            }
        }
        private BoletaHumedadAsignacionDto _boletaHumidityByVendorSelected;

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

                if (AjusteBoletaDetalleSelected == null) return;

                if (AjusteBoletaPaymentDato == null) return;

                AjusteBoletaPaymentDato.AjusteBoletaDetalleId = AjusteBoletaDetalleSelected.AjusteBoletaDetalleId;
                AjusteBoletaPaymentDato.CodigoBoleta = AjusteBoletaDetalleSelected.CodigoBoleta;
                AjusteBoletaPaymentDato.NumeroEnvio = AjusteBoletaDetalleSelected.NumeroEnvio;
            }
        }
        private AjusteBoletaDetalleDto _ajusteBoletaDetalleSelected;

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

        public ObservableCollection<AjusteBoletaPagoModel> AjusteBoletaPayments
        {
            get { return _ajusteBoletaPayments; }
            set
            {
                _ajusteBoletaPayments = value;
                RaisePropertyChanged(nameof(AjusteBoletaPayments));
            }
        }
        private ObservableCollection<AjusteBoletaPagoModel> _ajusteBoletaPayments;

        public AjusteBoletaPagoModel AjusteBoletaPaymentSelected
        {
            get { return ajusteBoletaPagoModel; }
            set
            {
                ajusteBoletaPagoModel = value;
                RaisePropertyChanged(nameof(AjusteBoletaPaymentSelected));
            }
        }
        private AjusteBoletaPagoModel ajusteBoletaPagoModel;

        public AjusteBoletaPagoModel AjusteBoletaPaymentDato
        {
            get { return _ajusteBoletaPaymentDato; }
            set
            {
                _ajusteBoletaPaymentDato = value;
                RaisePropertyChanged(nameof(AjusteBoletaPaymentDato));
            }
        }
        private AjusteBoletaPagoModel _ajusteBoletaPaymentDato;

        public ObservableCollection<OrdenCombustibleModel> FuelOrdersPending
        {
            get { return _fuelOrdersPending; }
            set
            {
                _fuelOrdersPending = value;
                RaisePropertyChanged(nameof(FuelOrdersPending));
            }
        }
        private ObservableCollection<OrdenCombustibleModel> _fuelOrdersPending;

        public OrdenCombustibleModel FuelOrderPendingSelected
        {
            get { return _fuelOrderPendingSelected; }
            set
            {
                _fuelOrderPendingSelected = value;
                RaisePropertyChanged(nameof(FuelOrderPendingSelected));
            }
        }
        private OrdenCombustibleModel _fuelOrderPendingSelected;

        public ObservableCollection<OrdenCombustibleModel> FuelOrdersAssigned
        {
            get { return _fuelOrdersAssigned; }
            set
            {
                _fuelOrdersAssigned = value;
                RaisePropertyChanged(nameof(FuelOrdersAssigned));
            }
        }
        private ObservableCollection<OrdenCombustibleModel> _fuelOrdersAssigned;

        public OrdenCombustibleModel FuelOrderAssignedSelected
        {
            get { return _fuelOrderAssignedSelected; }
            set
            {
                _fuelOrderAssignedSelected = value;
                RaisePropertyChanged(nameof(FuelOrderAssignedSelected));
            }
        }
        private OrdenCombustibleModel _fuelOrderAssignedSelected;

        public OrdenCombustibleModel FuelOrderDato
        {
            get { return _fuelOrderDato; }
            set
            {
                _fuelOrderDato = value;
                RaisePropertyChanged(nameof(FuelOrderDato));
            }
        }
        private OrdenCombustibleModel _fuelOrderDato;


        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoNavegar { get; set; }
        public RelayCommand ComandoMostrarAgregar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }
        public RelayCommand CommandBoletaProperties { get; set; }
        public RelayCommand ComandoCierreAutorizadoBoleta { get; set; }
        public RelayCommand ComandoCalcularPesoNeto { get; set; }
        public RelayCommand CommandOpenBoleta { get; set; }
        public RelayCommand CommandSelectImage { get; set; }
        public RelayCommand CommandSaveAndOpenImage { get; set; }

        public RelayCommand<string> ComandoBuscarProveedores { get; set; }
        public RelayCommand<string> ComandoBuscarPlantas { get; set; }
        public RelayCommand<string> ComandoBuscarEquipos { get; set; }
        public RelayCommand<string> ComandoBuscarMotoristas { get; set; }
        public RelayCommand<string> ComandoBuscarCuadrillas { get; set; }
        public RelayCommand<string> ComandoBuscarCuadrillasPorBoleta { get; set; }
        public RelayCommand<string> ComandoBuscarCategoriaProductos { get; set; }
        
        public RelayCommand ComandoMostrarAgregarOrdenCombustible { get; set; }
        public RelayCommand CommandAddFuelOrder { get; set; }
        public RelayCommand CommandRemoveFuelOrder { get; set; }
        public RelayCommand CommandSaveFuelOrders { get; set; }

        public RelayCommand ComandoMostrarAgregarDescarga { get; set; }
        public RelayCommand ComandoAgregarDescarga { get; set; }

        public RelayCommand ComandoMostrarPagosBoleta { get; set; }
        public RelayCommand ComandoRemoverPagoBoleta { get; set; }
        public RelayCommand ComandoEditarPagoBoleta { get; set; }
        public RelayCommand ComandoAgregarPagoBoleta { get; set; }
        public RelayCommand CommandHoleTotalToPayment { get; set; }
        public RelayCommand ComandoGuardarPagosBoleta { get; set; }

        public RelayCommand ComandoMostrarAbonosPrestamo { get; set; }
        public RelayCommand ComandoAgregarAbonoPrestamo { get; set; }
        public RelayCommand CommandFullPaymentForLoan { get; set; }
        public RelayCommand ComandoRemoverAbonoPrestamo { get; set; }
        public RelayCommand ComandoEditarAbonoPrestamo { get; set; }
        public RelayCommand ComandoGuardarAbonosPrestamos { get; set; }

        public RelayCommand ComandoMostrarOtrasDeducciones { get; set; }
        public RelayCommand ComandoActualizarRequerimientoLineaCredito { get; set; }
        public RelayCommand ComandoAgregarBoletaOtraDeduccion { get; set; }
        public RelayCommand ComandoRemoverOtraDeduccion { get; set; }
        public RelayCommand ComandoGuardarOtrasDeducciones { get; set; }

        public RelayCommand ComandoImprimirDetalleBoleta { get; set; }

        public RelayCommand CommandShowBoletasHumidity { get; set; }
        public RelayCommand CommandAddHumidityPayment { get; set; }
        public RelayCommand CommandRemoveHumidityPayment { get; set; }
        public RelayCommand CommandSaveHimidityPayments { get; set; }

        public RelayCommand CommandShowBoletaAdjustments { get; set; }
        public RelayCommand CommandAddAjustePayment { get; set; }
        public RelayCommand CommandEditAjustePayment { get; set; }
        public RelayCommand CommandDeleteAjustePayment { get; set; }
        public RelayCommand CommandSaveAjustePayments { get; set; }

        public RelayCommand CommandSavePropertiesBoleta { get; set; }

        private void InicializarComandos()
        {
            ComandoBuscar = new RelayCommand(BuscarBoleta);
            ComandoEditar = new RelayCommand(EditarBoleta);
            ComandoMostrarAgregar = new RelayCommand(MostrarAgregarBoleta);
            ComandoCrear = new RelayCommand(CrearBoleta);
            ComandoMostrarEditar = new RelayCommand(MostrarEditarBoleta);
            ComandoEliminar = new RelayCommand(EliminarBoleta);
            ComandoNavegar = new RelayCommand(Navegar);
            ComandoRefrescar = new RelayCommand(Navegar);
            ComandoCierreAutorizadoBoleta = new RelayCommand(CierreForzadoBoleta);
            ComandoCalcularPesoNeto = new RelayCommand(CalcularPesoNeto);
            CommandSelectImage = new RelayCommand(SelectImage);
            CommandSaveAndOpenImage = new RelayCommand(SaveAndOpenImage);


            ComandoBuscarProveedores = new RelayCommand<string>(BuscarProveedores);

            ComandoBuscarPlantas = new RelayCommand<string>(BuscarPlantas);
            ComandoBuscarCuadrillas = new RelayCommand<string>(BuscarCuadrillas);
            ComandoBuscarCuadrillasPorBoleta = new RelayCommand<string>(BuscarCuadrillasPorBoleta);
            ComandoBuscarCategoriaProductos = new RelayCommand<string>(BuscarCategoriaProductos);

            ComandoMostrarAgregarOrdenCombustible = new RelayCommand(MostrarAgregarOrdenCombustible);
            CommandAddFuelOrder = new RelayCommand(AddFuelOrder);
            CommandRemoveFuelOrder = new RelayCommand(RemoveFuelOrder);
            CommandSaveFuelOrders = new RelayCommand(AssignFuelOrdersToBoleta);

            ComandoMostrarAgregarDescarga = new RelayCommand(MostrarAgregarDescarga);
            ComandoAgregarDescarga = new RelayCommand(CrearDescarga);

            ComandoMostrarPagosBoleta = new RelayCommand(MostrarPagosBoleta);
            ComandoAgregarPagoBoleta = new RelayCommand(AgregarItemPagoBoleta);
            ComandoRemoverPagoBoleta = new RelayCommand(RemoverItemPagoBoleta);
            CommandHoleTotalToPayment = new RelayCommand(HoleTotalToPayment);
            ComandoEditarPagoBoleta = new RelayCommand(EditarPagoBoleta);
            ComandoGuardarPagosBoleta = new RelayCommand(GuardarPagosBoleta);

            ComandoMostrarAbonosPrestamo = new RelayCommand(MostrarAbonosPrestamo);
            ComandoAgregarAbonoPrestamo = new RelayCommand(AgregarItemAbonoPresatmo);
            CommandFullPaymentForLoan = new RelayCommand(FullPaymentForLoan);
            ComandoRemoverAbonoPrestamo = new RelayCommand(RemoverItemAbonoPrestamo);
            ComandoEditarAbonoPrestamo = new RelayCommand(EditarItemAbonoPrestamo);
            ComandoGuardarAbonosPrestamos = new RelayCommand(GuardarAbonosPrestamos);

            ComandoMostrarOtrasDeducciones = new RelayCommand(MostrarOtrasDeducciones);
            ComandoActualizarRequerimientoLineaCredito = new RelayCommand(ActualizarRequerimientoLineaCredito);
            ComandoAgregarBoletaOtraDeduccion = new RelayCommand(AgregarBoletaOtraDeduccion);
            ComandoRemoverOtraDeduccion = new RelayCommand(RemoverOtraDeduccion);
            ComandoGuardarOtrasDeducciones = new RelayCommand(GuardarBoletaOtrasDeducciones);

            ComandoImprimirDetalleBoleta = new RelayCommand(ImprimirDetalleBoleta);

            CommandShowBoletasHumidity = new RelayCommand(ShowBoletasHumidity);
            CommandAddHumidityPayment = new RelayCommand(AddHumidityPayment);
            CommandRemoveHumidityPayment = new RelayCommand(RemoveHumidityPayment);
            CommandSaveHimidityPayments = new RelayCommand(SaveHumidityPayments);

            CommandOpenBoleta = new RelayCommand(OpenBoleta);

            CommandShowBoletaAdjustments = new RelayCommand(ShowBoletaAjustments);
            CommandAddAjustePayment = new RelayCommand(AddAjustePayment);
            CommandDeleteAjustePayment = new RelayCommand(DeleteAjustePayment);
            CommandSaveAjustePayments = new RelayCommand(SaveAjustePayments);
            CommandEditAjustePayment = new RelayCommand(EditAjustePayment);

            CommandBoletaProperties = new RelayCommand(ShowBoletaProperties);
            CommandSavePropertiesBoleta = new RelayCommand(SavePropertiesBoleta);
        }

        private void SavePropertiesBoleta()
        {
            var mensajeValidacion = ValidarBoleta();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new UpdateBoletaProperties
            {
                Boleta = DatoBoleta,
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
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                CargarSlidePanel("UpdateBoletaPropertiesFlyout");
                CargarBoletas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ShowBoletaProperties()
        {
            if (BoletaSeleccionada == null) return;
                        
            DatoBoleta = new BoletasDTO
            {
                PlantaId = BoletaSeleccionada.PlantaId,
                NombrePlanta = BoletaSeleccionada.NombrePlanta,
                BoletaId = BoletaSeleccionada.BoletaId,
                CodigoBoleta = BoletaSeleccionada.CodigoBoleta,
                NumeroEnvio = BoletaSeleccionada.NumeroEnvio,
                EquipoId = BoletaSeleccionada.EquipoId,
                DescripcionTipoEquipo = BoletaSeleccionada.DescripcionTipoEquipo,
                PlacaEquipo = BoletaSeleccionada.PlacaEquipo,
                ProveedorId = BoletaSeleccionada.ProveedorId,
                NombreProveedor = BoletaSeleccionada.NombreProveedor,
                CategoriaProductoId = BoletaSeleccionada.CategoriaProductoId,
                DescripcionTipoProducto = BoletaSeleccionada.DescripcionTipoProducto,
                PesoEntrada = BoletaSeleccionada.PesoEntrada,
                PesoSalida = BoletaSeleccionada.PesoSalida,
                PesoProducto = BoletaSeleccionada.PesoProducto,
                CantidadPenalizada = BoletaSeleccionada.CantidadPenalizada,
                PrecioProductoCompra = BoletaSeleccionada.PrecioProductoCompra,
                FechaSalida = BoletaSeleccionada.FechaSalida,
                Motorista = BoletaSeleccionada.Motorista,
                Bonus = BoletaSeleccionada.Bonus,
                HasBonus = BoletaSeleccionada.HasBonus,
                IsShippingNumberRequired = BoletaSeleccionada.IsShippingNumberRequired,
                Imagen = BoletaSeleccionada.Imagen
            };

            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(DatoBoleta.NombrePlanta, DatoBoleta.PlantaId);            
            ProveedorAutoCompleteSeleccionado = new AutoCompleteEntry(DatoBoleta.NombreProveedor, DatoBoleta.ProveedorId);
            CategoriaProductoAutoCompleteSeleccionado = new AutoCompleteEntry(DatoBoleta.DescripcionTipoProducto, null);

            CargarSlidePanel("UpdateBoletaPropertiesFlyout");
        }

        private void SaveAndOpenImage()
        {
            if (BoletaSeleccionada == null || BoletaSeleccionada.Imagen == null) return;

            string name = BoletaSeleccionada.GetBoletaName();

            SaveImage(BoletaSeleccionada.Imagen, name);

        }

        private void SelectImage()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
                var data = new byte[fs.Length];
                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                DatoBoleta.Imagen = data;
                RaisePropertyChanged(nameof(DatoBoleta));
            }
        }
        
        private void SaveImage(byte[] byteArrayIn, string fileName)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image img = Image.FromStream(ms);

            string folderPath = _serviciosComunes.GetPathBoletasImg();
            string imagePath = folderPath + fileName;

            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            Process.Start(imagePath);
        }

        private void SaveImage(Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image"; // Default file name
            s.DefaultExt = ".Jpg"; // Default file extension
            s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            // Save Image
            string filename = s.FileName;
            FileStream fstream = new FileStream(filename, FileMode.Create);
            image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            fstream.Close();
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            ms.Position = 0; // this is important
            Image image = Image.FromStream(ms);
            return image;
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public BitmapImage ConvertBinaryToImage(byte[] binary)
        {
            byte[] buffer = binary.ToArray();
            MemoryStream stream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private void EditAjustePayment()
        {
            if (AjusteBoletaPaymentSelected == null) return;

            AjusteBoletaPaymentDato.AjusteBoletaDetalleId = AjusteBoletaPaymentSelected.AjusteBoletaDetalleId;
            AjusteBoletaPaymentDato.AjusteBoletaPagoId = AjusteBoletaPaymentDato.AjusteBoletaPagoId;
            AjusteBoletaPaymentDato.BoletaId = AjusteBoletaPaymentDato.BoletaId;
            AjusteBoletaPaymentDato.CodigoBoleta = AjusteBoletaPaymentDato.CodigoBoleta;
            AjusteBoletaPaymentDato.Monto = AjusteBoletaPaymentDato.Monto;
            AjusteBoletaPaymentDato.NumeroEnvio = AjusteBoletaPaymentDato.NumeroEnvio;
        }

        private void SaveAjustePayments()
        {
            var datos = from reg in AjusteBoletaPayments
                        select new AjusteBoletaPagoDto
                        {
                            Monto = reg.Monto,
                            AjusteBoletaDetalleId = reg.AjusteBoletaDetalleId,
                            AjusteBoletaPagoId = reg.AjusteBoletaPagoId,
                            BoletaId = reg.BoletaId
                        };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostAjusteBoletaPagoByBoleta
            {
                AjusteBoletaPagos = datos.ToList(),
                BoletaId = BoletaSeleccionada.BoletaId,
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
                CargarSlidePanel("AjusteBoletaPaymentslyout");
                CargarBoletas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void DeleteAjustePayment()
        {
            if (AjusteBoletaPaymentSelected == null) return;

            AjusteBoletaPayments.Remove(AjusteBoletaPaymentSelected);
            CalculateTotalAjustePayments();
        }

        private void AddAjustePayment()
        {
            AjusteBoletaPagoModel ajustePayment = AjusteBoletaPayments.FirstOrDefault(p => p.AjusteBoletaDetalleId == AjusteBoletaPaymentDato.AjusteBoletaDetalleId);

            if (!CanApplyBoletaPaymen(ajustePayment, out string errorMessage))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, errorMessage);
                return;
            }

            if (AjusteBoletaPaymentDato.Monto <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Debe ingresar un valor mayor a 0");
                return;
            }

            if (ajustePayment == null)
            {
                AjusteBoletaPayments.Add(AjusteBoletaPaymentDato);
            }
            else
            {
                ajustePayment.Monto = AjusteBoletaPaymentDato.Monto;
            }

            CalculateTotalAjustePayments();
        }

        private bool CanApplyBoletaPaymen(AjusteBoletaPagoModel ajustePayment, out string errorMessage)
        {
            if (ajustePayment == null)
            {
                if (AjusteBoletaPaymentDato.Monto > AjusteBoletaDetalleSelected.SaldoPendiente)
                {
                    errorMessage = "El Abono del ajuste supera el valor pendiente del mismo";
                    return false;
                }
            }
            else
            {
                if (((ajustePayment.Monto + AjusteBoletaPaymentDato.Monto) - ajustePayment.Monto) > AjusteBoletaDetalleSelected.SaldoPendiente)
                {
                    errorMessage = "El Abono del ajuste supera el valor pendiente del mismo";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        private void CalculateTotalAjustePayments()
        {
            AjusteBoletaPaymentDato = new AjusteBoletaPagoModel
            {
                TotalPayments = AjusteBoletaPayments.Sum(p => p.Monto),
                SaldoBoleta = BoletaSeleccionada.TotalAPagar - AjusteBoletaPayments.Sum(p => p.Monto)
            };
        }

        private void ShowBoletaAjustments()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la Boleta debe ser Activo!");
                return;
            }

            AjusteBoletaPayments = new ObservableCollection<AjusteBoletaPagoModel>();
            LoadAjusteBoletaDetalle();
            LoadAjusteBoletaPayments();
            CalculateTotalAjustePayments();
            CargarSlidePanel("AjusteBoletaPaymentslyout");
        }

        private void LoadAjusteBoletaDetalle()
        {
            if (BoletaSeleccionada == null) return;

            AjusteBoletaDetalles = new List<AjusteBoletaDetalleDto>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAjusteBoletaDetalleByVendorId
            {
                VendorId = BoletaSeleccionada.ProveedorId,
                BoletaId = BoletaSeleccionada.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                AjusteBoletaDetalles = res;
                RaisePropertyChanged(nameof(AjusteBoletaDetalles));
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadFuelOrderPendingByVendor()
        {
            if (BoletaSeleccionada == null) return;

            FuelOrdersPending = new ObservableCollection<OrdenCombustibleModel>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetOrderFuelByVendorId
            {
                ProveedorId = BoletaSeleccionada.ProveedorId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                IEnumerable<OrdenCombustibleModel> x = from reg in res
                                                       select new OrdenCombustibleModel
                                                       {
                                                           BoletaId = reg.BoletaId,
                                                           OrdenCombustibleId = reg.OrdenCombustibleId,
                                                           AutorizadoPor = reg.AutorizadoPor,
                                                           CodigoFactura = reg.CodigoFactura,
                                                           Estado = reg.Estado,
                                                           Monto = reg.Monto,
                                                           FechaCreacion = reg.FechaCreacion,
                                                           Observaciones = reg.Observaciones,
                                                           PlacaEquipo = reg.PlacaEquipo,
                                                           FuelOrderSpecification = reg.FuelOrderSpecification
                                                       };

                FuelOrdersPending = new ObservableCollection<OrdenCombustibleModel>(x);
                RaisePropertyChanged(nameof(FuelOrdersPending));
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadFuelOrderAssigned()
        {
            if (BoletaSeleccionada == null) return;

            FuelOrdersAssigned = new ObservableCollection<OrdenCombustibleModel>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetOrdenesCombustibleByBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                IEnumerable<OrdenCombustibleModel> x = from reg in res
                                     select new OrdenCombustibleModel
                                     {
                                         BoletaId = reg.BoletaId,
                                         OrdenCombustibleId = reg.OrdenCombustibleId,
                                         AutorizadoPor = reg.AutorizadoPor,
                                         CodigoFactura = reg.CodigoFactura,
                                         Estado = reg.Estado,
                                         Monto = reg.Monto,
                                         FechaCreacion = reg.FechaCreacion,
                                         Observaciones = reg.Observaciones,
                                         PlacaEquipo = reg.PlacaEquipo,
                                         FuelOrderSpecification = reg.FuelOrderSpecification
                                     };
                FuelOrdersAssigned = new ObservableCollection<OrdenCombustibleModel>(x);
                RaisePropertyChanged(nameof(FuelOrdersAssigned));
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadAjusteBoletaPayments()
        {
            if (BoletaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAjusteBoletaPagoByBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                AjusteBoletaPagos = res;
                RaisePropertyChanged(nameof(AjusteBoletaPagos));

                var datos = from reg in AjusteBoletaPagos
                            select new AjusteBoletaPagoModel
                            {
                                AjusteBoletaDetalleId = reg.AjusteBoletaDetalleId,
                                AjusteBoletaPagoId = reg.AjusteBoletaPagoId,
                                BoletaId = reg.BoletaId,
                                Monto = reg.Monto,
                                FechaAbono = reg.FechaAbono,
                                CodigoBoleta = reg.CodigoBoleta,
                                NumeroEnvio = reg.NumeroEnvio
                            };

                AjusteBoletaPayments = new ObservableCollection<AjusteBoletaPagoModel>(datos);
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void OpenBoleta()
        {
            if (BoletaSeleccionada == null) return;

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Abrir_Boleta, BoletaSeleccionada.CodigoBoleta),
                Titulo = "Activar Boleta"
            };

            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new OpenBoletaById
                    {
                        BoletaId = BoletaSeleccionada.BoletaId,
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
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                        CargarBoletas();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEspera = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };

            DialogSettings = null;            
        }

        private void SaveHumidityPayments()
        {
            var datos = from reg in BoletaHumedadPagoModel
                        select new BoletaHumedadPagoDto
                        {
                            BoletaHumedadId = reg.BoletaHumedadId,
                            BoletaHumedadPagoId = reg.BoletaHumedadPagoId,
                            BoletaId = reg.BoletaId,
                            HumedadPromedio = reg.HumedadPromedio,
                            NumeroEnvio = reg.NumeroEnvio,
                            PorcentajeTolerancia = reg.PorcentajeTolerancia,
                            TotalHumidityPayment = reg.TotalHumidityPayment
                        };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostBoletaHumedadPago
            {
                BoletasHumedadPago = datos.ToList(),
                BoletaId = BoletaSeleccionada.BoletaId,
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
                CargarSlidePanel("HumidityPaymentsFlyout");
                CargarBoletas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
        
        private void RemoveHumidityPayment()
        {
            if (BoletaHumidityPaymentSelected == null) return;

            BoletaHumedadPagoModel.Remove(BoletaHumidityPaymentSelected);
            GetTotalHumidityPayments();
        }

        private void AddHumidityPayment()
        {
            if (BoletaHumidityByVendorSelected == null) return;

            var exists = BoletaHumedadPagoModel.Any(p => p.BoletaHumedadId == BoletaHumidityByVendorSelected.BoletaHumedadId);

            if (exists) return;

            BoletaHumedadPagoModel payment = new BoletaHumedadPagoModel
            {
                BoletaHumedadId = BoletaHumidityByVendorSelected.BoletaHumedadId,
                BoletaId = BoletaSeleccionada.BoletaId,
                NumeroEnvio = BoletaHumidityByVendorSelected.NumeroEnvio,
                TotalHumidityPayment = BoletaHumidityByVendorSelected.OutStandingPay,
                PorcentajeTolerancia = BoletaHumidityByVendorSelected.PorcentajeTolerancia,
                HumedadPromedio = BoletaHumidityByVendorSelected.HumedadPromedio
            };

            BoletaHumedadPagoModel.Add(payment);
            RaisePropertyChanged(nameof(BoletaHumedadPagoModel));
            GetTotalHumidityPayments();
        }

        private void ShowBoletasHumidity()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta debe estar ACTIVA!");
                return;
            }

            if (BoletaSeleccionada.ClientHasOutStandingHumidity)
            {
                LoadBoletasHumidityByVendor();
            }

            BoletaHumedadPagoModel = new ObservableCollection<BoletaHumedadPagoModel>();                        
            LoadHumidityPaymentsByBoleta();
            CargarSlidePanel("HumidityPaymentsFlyout");
        }

        private void GetTotalHumidityPayments()
        {
            DatoBoletaHumidityPayment = new BoletaHumedadPagoDto();

            DatoBoletaHumidityPayment.TotalHumidityPayment = BoletaHumedadPagoModel.Sum(t => t.TotalHumidityPayment);
            RaisePropertyChanged(nameof(DatoBoletaHumidityPayment));

        }

        private void LoadHumidityPaymentsByBoleta()
        {
            BoletaHumidityPayments = new List<BoletaHumedadPagoDto>();

            if (BoletaSeleccionada == null) return;
               
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetHumidityPaymentByBoleta
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                BoletaHumidityPayments = res;
                RaisePropertyChanged(nameof(BoletaHumidityPayments));

                var datos = from reg in BoletaHumidityPayments
                            select new BoletaHumedadPagoModel
                            {
                                BoletaHumedadId = reg.BoletaHumedadId,
                                BoletaHumedadPagoId = reg.BoletaHumedadPagoId,
                                BoletaId = reg.BoletaId,
                                HumedadPromedio = reg.HumedadPromedio,
                                NumeroEnvio = reg.NumeroEnvio,
                                PorcentajeTolerancia = reg.PorcentajeTolerancia,
                                TotalHumidityPayment = reg.TotalHumidityPayment
                            };

                BoletaHumedadPagoModel = new ObservableCollection<BoletaHumedadPagoModel>(datos);
                
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void LoadBoletasHumidityByVendor()
        {
            BoletasHumidityByVendor = new List<BoletaHumedadAsignacionDto>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletasHumedadByVendor
            {
                VendorId = BoletaSeleccionada.ProveedorId,
                BoletaId = BoletaSeleccionada.BoletaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                BoletasHumidityByVendor = res;
                RaisePropertyChanged(nameof(BoletasHumidityByVendor));
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });

            RaisePropertyChanged(nameof(BoletasHumidityByVendor));
        }

        private void CalcularPesoNeto()
        {
            if (DatoBoleta == null) return;

            DatoBoleta.PesoProducto = ((DatoBoleta.PesoEntrada - DatoBoleta.PesoSalida) - DatoBoleta.CantidadPenalizada) + DatoBoleta.Bonus;
            RaisePropertyChanged(nameof(DatoBoleta));
        }

        private void GuardarBoletaOtrasDeducciones()
        {
            var otrasDeducciones = from reg in ListaBoletaOtrasDeduccionesModel
                                   select new BoletaOtrasDeduccionesDTO
                                   {
                                       BoletaId = reg.BoletaId,
                                       BoletaOtraDeduccionId = reg.BoletaOtraDeduccionId,
                                       Monto = reg.Monto,
                                       MotivoDeduccion = reg.MotivoDeduccion,
                                       LineaCreditoId = reg.LineaCreditoId,
                                       FormaDePago = reg.FormaDePago,
                                       NoDocumento = reg.NoDocumento,
                                       EsDeduccionManual = reg.EsDeduccionManual
                                   };
            
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostBoletaOtrasDeducciones
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                BoletaOtrasDeducciones = otrasDeducciones.ToList(),
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId
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
                CargarSlidePanel("BoletaOtrasDeduccionesFlyout");
                CargarBoletas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void RemoverOtraDeduccion()
        {
            if (BoletaOtraDeduccionSeleccionado == null) return;

            if (BoletaOtraDeduccionSeleccionado.LineaCreditoId != null && !BoletaOtraDeduccionSeleccionado.PuedeEliminarBoletaOtraDeduccion
                && BoletaOtraDeduccionSeleccionado.BoletaOtraDeduccionId > 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Puede eliminar la deducción porque la Linea de Crédito ya está Cerrada");
                return;
            }

            ListaBoletaOtrasDeduccionesModel.Remove(BoletaOtraDeduccionSeleccionado);
            ActualizarValoresBoletaOtrasDeducciones();
        }

        private void AgregarBoletaOtraDeduccion()
        {
            var mensajeValidacion = ValidarBoletaOtraDeduccion();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            if (DatoBoletaOtraDeduccion.Monto > 0 || DatoBoletaOtraDeduccion.EsDeduccionManual)
            {
                DatoBoletaOtraDeduccion.LineaCreditoId = null;
                DatoBoletaOtraDeduccion.NombreBanco = "N/A";
                DatoBoletaOtraDeduccion.FormaDePago = "N/A";
                DatoBoletaOtraDeduccion.NombreBanco = "N/A";
                DatoBoletaOtraDeduccion.NoDocumento = "N/A";
                DatoBoletaOtraDeduccion.CodigoLineaCredito = "N/A";
            }

            ListaBoletaOtrasDeduccionesModel.Add(DatoBoletaOtraDeduccion);
            ActualizarValoresBoletaOtrasDeducciones();

            BancoSeleccionado = new BancosDTO();
            LineaCreditoSeleccionada = new LineasCreditoDTO();
        }

        private void ActualizarValoresBoletaOtrasDeducciones()
        {
            var totalDebito = Math.Abs(ListaBoletaOtrasDeduccionesModel.Where(m => m.Monto < 0).Sum(d => d.Monto));
            var totalIngreso = ListaBoletaOtrasDeduccionesModel.Where(m => m.Monto > 0).Sum(d => d.Monto);
            var totalPagoBoleta = ((BoletaSeleccionada.TotalAPagar - BoletaSeleccionada.TotalOtrosIngresosBoleta) + BoletaSeleccionada.TotalBoletaOtrasDeducciones)
                                  + (totalIngreso - totalDebito);
            DatoBoletaOtraDeduccion = new BoletaOtrasDeduccionesModel
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                DeduccionTotal = totalDebito,
                TotalIngreso = totalIngreso,
                TotalPagarBoleta = totalPagoBoleta,
                LineaCreditoId = null,
                RequiereLineaCredito = false,
                EsDeduccionManual = false
            };
        }

        private string ValidarBoletaOtraDeduccion()
        {
            if (DatoBoletaOtraDeduccion.Monto == 0)
            {
                return "El Monto de la deducción no puede ser 0";
            }

            if (DatoBoletaOtraDeduccion.Monto < 0)
            {
                if (string.IsNullOrWhiteSpace(DatoBoletaOtraDeduccion.MotivoDeduccion))
                {
                    return "Debe específicar el Motivo de la Deducción";
                }
            }

            if (DatoBoletaOtraDeduccion.Monto < 0 && !DatoBoletaOtraDeduccion.EsDeduccionManual)
            {
                if (string.IsNullOrWhiteSpace(DatoBoletaOtraDeduccion.FormaDePago))
                {
                    return "Debe especificar la Forma de Pago";
                }

                if (DatoBoletaOtraDeduccion.BancoId == 0)
                {
                    return "Debe indicar el Banco";
                }

                if (DatoBoletaOtraDeduccion.LineaCreditoId == 0)
                {
                    return "Debe seleccionar una Linea de Crédito";
                }

                if (string.IsNullOrWhiteSpace(DatoBoletaOtraDeduccion.NoDocumento))
                {
                    return "Debe ingresar el Documento Ref de la Transacción";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(DatoBoletaOtraDeduccion.MotivoDeduccion))
                {
                    return "Debe específicar el Motivo del por qué está agregando Dinero";
                }
            }

            return string.Empty;
        }

        private void ActualizarRequerimientoLineaCredito()
        {
            if (DatoBoletaOtraDeduccion.Monto < 0)
            {
                DatoBoletaOtraDeduccion.IsNegativeDeduction = true;
            }
            else
            {
                DatoBoletaOtraDeduccion.IsNegativeDeduction = false;
            }

            if (DatoBoletaOtraDeduccion.Monto >= 0 || (DatoBoletaOtraDeduccion.Monto < 0 && DatoBoletaOtraDeduccion.EsDeduccionManual))
            {
                DatoBoletaOtraDeduccion.RequiereLineaCredito = false;
            }

            if (DatoBoletaOtraDeduccion.Monto < 0 && !DatoBoletaOtraDeduccion.EsDeduccionManual)
            {
                DatoBoletaOtraDeduccion.RequiereLineaCredito = true;
            }
        }

        private void MostrarOtrasDeducciones()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede realizar Otras Deducciones a las Boletas ACTIVAS");
                return;
            }
            
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletaOtrasDeduccionesPorBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaBoletaOtrasDeducciones = res;

                var otrasDeducciones = from reg in ListaBoletaOtrasDeducciones
                                       select new BoletaOtrasDeduccionesModel
                                       {
                                           BoletaId = reg.BoletaId,
                                           BoletaOtraDeduccionId = reg.BoletaOtraDeduccionId,
                                           Monto = reg.Monto,
                                           MotivoDeduccion = reg.MotivoDeduccion,
                                           LineaCreditoId = reg.LineaCreditoId,                                                                                     
                                           BancoId = reg.BancoId,
                                           EsDeduccionManual = reg.EsDeduccionManual,
                                           CodigoLineaCredito = reg.CodigoLineaCredito ?? "N/A",
                                           FormaDePago = !string.IsNullOrWhiteSpace(reg.FormaDePago) ? reg.FormaDePago : "N/A",
                                           NombreBanco = !string.IsNullOrWhiteSpace(reg.NombreBanco) ? reg.NombreBanco : "N/A",
                                           NoDocumento = !string.IsNullOrWhiteSpace(reg.NoDocumento) ? reg.NoDocumento : "N/A",
                                           PuedeEliminarBoletaOtraDeduccion = reg.PuedeEliminarBoletaOtraDeduccion
                                       };

                ListaBoletaOtrasDeduccionesModel = new ObservableCollection<BoletaOtrasDeduccionesModel>(otrasDeducciones);

                ActualizarValoresBoletaOtrasDeducciones();
                CargarSlidePanel("BoletaOtrasDeduccionesFlyout");

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
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

        private void ImprimirDetalleBoleta()
        {
            ObtenerInformacionReporte();
        }

        private void ObtenerInformacionReporte()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado == Estados.ANULADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe información para mostrar");
                return;
            }
            
            ImprimirReporte("ReportePagoBoleta.rdlc");
        }

        private void ImprimirReporte(string nombreReporte)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var list = new List<BoletasDTO>();
                list.Add(BoletaSeleccionada);
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Pago de Boleta";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = nombreReporte;
                manejadorReporte.MultipleDataSet = true;
                manejadorReporte.ListadoItemsDataSet = new List<ItemDataSetModel>
                {
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Encabezado",
                        Datos = list
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Detalle",
                        Datos = ListaBoletaDetalles
                    }
                };
                //manejadorReporte.NombreDataSet = "DataSet";
                manejadorReporte.AgregarParametro("Empresa", "Comercial Colindres");
                manejadorReporte.MostarReporte();
            });
        }

        private void GuardarAbonosPrestamos()
        {
            var abonosPrestamos = from reg in AssignedLoansBoleta
                                  select new PagoPrestamosDTO
                                  {
                                      BoletaId = reg.BoletaId,
                                      PagoPrestamoId = reg.PagoPrestamoId,
                                      PrestamoId = reg.PrestamoId,
                                      MontoAbono = reg.MontoAbono,
                                      FormaDePago = reg.FormaDePago
                                  };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostPagosPrestamoPorBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                PagoPrestamos = abonosPrestamos.ToList(),
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
                CargarSlidePanel("AbonosPrestamoFlyout");
                CargarBoletas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EditarItemAbonoPrestamo()
        {
            if (AssignedLoanBoletaSelected == null) return;

            DatoAbonoPrestamo = new PagoPrestamoModel
            {
                CodigoBoleta = AssignedLoanBoletaSelected.CodigoBoleta,
                CodigoPrestamo = AssignedLoanBoletaSelected.CodigoPrestamo,
                BoletaId = AssignedLoanBoletaSelected.BoletaId,
                MontoAbono = AssignedLoanBoletaSelected.MontoAbono,
                PrestamoId = AssignedLoanBoletaSelected.PrestamoId,
                PagoPrestamoId = AssignedLoanBoletaSelected.PagoPrestamoId
            };
            
        }

        private void RemoverItemAbonoPrestamo()
        {
            if (AssignedLoanBoletaSelected == null) return;
            
            AssignedLoansBoleta.Remove(AssignedLoanBoletaSelected);
            ActualizarDatosAbonosPrestamo();                       
        }

        private void FullPaymentForLoan()
        {
            if (PendingLoanSelected == null) return;

            decimal saldoBoletaSinAbono = GetSaldoBoletaSinAbono();
            decimal abonoEnlistado = AssignedLoansBoleta.Sum(x => x.MontoAbono);
            decimal saldoPendientePrestamo = PendingLoanSelected.SaldoPendiente;

            //se calculo el saldo real de la boleta
            decimal boletaSaldoPendiente = (saldoBoletaSinAbono - abonoEnlistado);

            if (saldoPendientePrestamo >= boletaSaldoPendiente)
            {
                DatoAbonoPrestamo.MontoAbono = boletaSaldoPendiente;
            }
            else
            {
                DatoAbonoPrestamo.MontoAbono = saldoPendientePrestamo;
            }
        }

        private void HoleTotalToPayment()
        {
            if (BoletaSeleccionada == null) return;

            decimal totalAmount = BoletaSeleccionada.TotalAPagar;
            decimal payments = ListaBoletaCierresModel.Sum(t => t.Monto);

            if (payments > 0)
            {
                DatoBoletaCierre.Monto = totalAmount - payments;
                return;
            }

            DatoBoletaCierre.Monto = totalAmount;
        }

        private void AgregarItemAbonoPresatmo()
        {
            decimal saldoBoletaSinAbono = GetSaldoBoletaSinAbono();
            decimal abonoEnlistado = GetAbonoPrestamoEnlistado();
            decimal pagoPendienteBoleta = BoletaSeleccionada.TotalAPagar;
            PagoPrestamoModel itemAbonoPrestamo = AssignedLoansBoleta.FirstOrDefault(a => a.PrestamoId == DatoAbonoPrestamo.PrestamoId);
            bool existeAbono = itemAbonoPrestamo != null && itemAbonoPrestamo.PagoPrestamoId > 0;
            decimal montoAbonoEnCurso = 0;
            decimal saldoPendientePrestamo = PendingLoanSelected.SaldoPendiente;

            //se verifica si existe un abono commit
            //con el fin de obtener cuanto es el total del abono en curso
            if (existeAbono)
            {
                //evaluar si ya esta en 0 (por abonos) el saldo pendiente del prestamo
                //para regresarlo a su valor original
                if (saldoPendientePrestamo == 0)
                {
                    saldoPendientePrestamo = itemAbonoPrestamo.MontoAbono;
                    itemAbonoPrestamo.MontoAbono = 0;
                }

                //verificar si el pago pendiente de la boleta es 0
                //indicar que el update del abono a evaluar será en valor que el usuario ha ingresado
                if (pagoPendienteBoleta == 0)
                {
                    montoAbonoEnCurso = DatoAbonoPrestamo.MontoAbono;
                }
                else
                {
                    montoAbonoEnCurso = itemAbonoPrestamo.MontoAbono + DatoAbonoPrestamo.MontoAbono;
                }

                //sumar al pago pendiente de la boleta el monto del abono previo del prestamo
                //////pagoPendienteBoleta += itemAbonoPrestamo.MontoAbono;               
                //el prestamo se vuvle a dejar en el mismo saldo pendiente de inicio
                saldoPendientePrestamo += itemAbonoPrestamo.MontoAbono;
            }
            else
            {
                montoAbonoEnCurso = DatoAbonoPrestamo.MontoAbono;
            }

            //se calculo el saldo real de la boleta
            decimal saldoPendienteReal = (saldoBoletaSinAbono - abonoEnlistado);
            if (montoAbonoEnCurso > saldoPendienteReal)
            {
                montoAbonoEnCurso = DatoAbonoPrestamo.MontoAbono;
            }
            
            //Evaluar saldo pendiente del prestamo
            if (montoAbonoEnCurso > saldoPendientePrestamo)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La cantidad del Abono supera el total del Saldo Pendiente");
                return;
            }

            //Evaluar disponibilidad del saldo de la boleta
            if (montoAbonoEnCurso > saldoPendienteReal)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La cantidad total del Abono es mayor al total a pagar de la boleta");
                return;
            }

            if (itemAbonoPrestamo == null)
            {
                AssignedLoansBoleta.Add(DatoAbonoPrestamo);
            }
            else
            {
                itemAbonoPrestamo.BoletaId = DatoAbonoPrestamo.BoletaId;
                itemAbonoPrestamo.MontoAbono = montoAbonoEnCurso;
                itemAbonoPrestamo.PrestamoId = DatoAbonoPrestamo.PrestamoId;
                itemAbonoPrestamo.PagoPrestamoId = DatoAbonoPrestamo.PagoPrestamoId;
                itemAbonoPrestamo.CodigoPrestamo = DatoAbonoPrestamo.CodigoPrestamo;
            }

            ActualizarDatosAbonosPrestamo();
        }

        private decimal GetSaldoBoletaSinAbono()
        {
            return BoletaSeleccionada.TotalAPagar + LoansPaymentsBoleta.Sum(x => x.MontoAbono);
        }

        private decimal GetAbonoPrestamoEnlistado()
        {
            return AssignedLoansBoleta.Where(x => x.PrestamoId != PendingLoanSelected.PrestamoId).Select(x => x.MontoAbono).Sum();
        }

        private decimal GetSaldoBoletaConAbono()
        {
            return GetSaldoBoletaSinAbono() - GetAbonoPrestamoEnlistado();
        }

        private void ActualizarDatosAbonosPrestamo()
        {
            DatoAbonoPrestamo = new PagoPrestamoModel
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                CodigoBoleta = BoletaSeleccionada.CodigoBoleta,
                TotalAbono = AssignedLoansBoleta.Sum(a => a.MontoAbono),
                TotalAPagar = GetSaldoBoletaConAbono(),
                FormaDePago = "Abono por Boleta"
            };

            if (PendingLoanSelected != null)
            {
                DatoAbonoPrestamo.CodigoPrestamo = PendingLoanSelected.CodigoPrestamo;
                DatoAbonoPrestamo.PrestamoId = PendingLoanSelected.PrestamoId;
            }

            RaisePropertyChanged(nameof(DatoAbonoPrestamo));
        }

        private string ValidarAbonoPrestamo()
        {
            if (PendingLoanSelected == null)
            {
                return "Debe seleccionar un prestamo";
            }

            if (DatoAbonoPrestamo.MontoAbono <= 0)
            {
                return "La cantidad del Abono debe ser mayor a 0";
            }
            
            var totalAbonado = AssignedLoansBoleta.Where(a => a.PrestamoId != DatoAbonoPrestamo.PrestamoId).Sum(m => m.MontoAbono) + DatoAbonoPrestamo.MontoAbono;

            if (BoletaSeleccionada.TotalAPagarSinAbono < totalAbonado)
            {
                return "La cantidad del abono excede el pago de la boleta";
            }

            return string.Empty;
        }

        private void MostrarAbonosPrestamo()
        {
            if (BoletaSeleccionada == null) return;
            
            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta debe estar Activa");
                return;
            }

            CargarPrestamos();
            CargarPagoPrestamos();
            ActualizarDatosAbonosPrestamo();
            //DatoAbonoPrestamo = new PagoPrestamoModel
            //{
            //    BoletaId = BoletaSeleccionada.BoletaId,
            //    CodigoBoleta = BoletaSeleccionada.CodigoBoleta,
            //    TotalAbono = AssignedLoansBoleta.Sum(a => a.MontoAbono),
            //    TotalAPagarSinAbono = (BoletaSeleccionada.TotalAPagar + AssignedLoansBoleta.Sum(a => a.MontoAbono)),
            //    TotalAPagar = (BoletaSeleccionada.TotalAPagar + AssignedLoansBoleta.Sum(a => a.MontoAbono)) - AssignedLoansBoleta.Sum(a => a.MontoAbono),
            //    TotalDeuda = PendingLoans.Sum(p => p.SaldoPendiente),
            //    FormaDePago = "Abono por Boleta"
            //};

            PendingLoanSelected = new PrestamoModel();
            

            CargarSlidePanel("AbonosPrestamoFlyout");
        }

        private void CargarPagoPrestamos()
        {
            LoansPaymentsBoleta = new List<PagoPrestamosDTO>();
            AssignedLoansBoleta = new ObservableCollection<PagoPrestamoModel>();
            if (BoletaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;

            _client = new JsonServiceClient(uri);
            var request = new GetPagoPrestamosPorBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                LoansPaymentsBoleta = res;
                IEnumerable<PagoPrestamoModel> pagoPrestamos = from reg in res
                                    select new PagoPrestamoModel
                                    {
                                        BoletaId = reg.BoletaId,
                                        CodigoBoleta = reg.CodigoBoleta,
                                        CodigoPrestamo = reg.CodigoPrestamo,
                                        PagoPrestamoId = reg.PagoPrestamoId,
                                        PrestamoId = reg.PrestamoId,
                                        MontoAbono = reg.MontoAbono,
                                        FormaDePago = reg.FormaDePago
                                    };

                AssignedLoansBoleta = new ObservableCollection<PagoPrestamoModel>(pagoPrestamos);
                RaisePropertyChanged(nameof(PagoPrestamoModel));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarPrestamos()
        {
            PendingLoans = new ObservableCollection<PrestamoModel>();
            if (BoletaSeleccionada == null) return;
            
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPrestamoPorProveedorId
            {
                ProveedorId = BoletaSeleccionada.ProveedorId
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                IEnumerable<PrestamoModel> x = from reg in res
                                              select new PrestamoModel
                                              {
                                                  PrestamoId = reg.PrestamoId,
                                                  AutorizadoPor = reg.AutorizadoPor,
                                                  CantidadAbono = reg.CantidadAbono,
                                                  CodigoPrestamo = reg.CodigoPrestamo,                                                  
                                                  MontoPrestamo = reg.MontoPrestamo,
                                                  Observaciones = reg.Observaciones,
                                                  SaldoPendiente = reg.SaldoPendiente,
                                                  TotalAbono = reg.TotalAbono,
                                                  TotalACobrar = reg.TotalACobrar,
                                                  Estado = reg.Estado
                                              };

                PendingLoans = new ObservableCollection<PrestamoModel>(x);
                RaisePropertyChanged(nameof(PendingLoans));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GuardarPagosBoleta()
        {
            var cierresBoleta = from reg in ListaBoletaCierresModel
                                select new BoletaCierresDTO
                                {
                                    LineaCreditoId = reg.LineaCreditoId,
                                    BoletaCierreId = reg.BoletaCierreId,
                                    BoletaId = reg.BoletaId,
                                    FormaDePago = reg.FormaDePago,
                                    Monto = reg.Monto,
                                    FechaPago = reg.FechaPago,
                                    NoDocumento = reg.NoDocumento,
                                    EstaEditandoRegistro = reg.EsEdicion                                  
                                };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostBoletasCierre
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                BoletaId = BoletaSeleccionada.BoletaId,
                BoletaCierres = cierresBoleta.ToList(),
                BoletaDetalles = ListaBoletaDetalles
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
                CargarSlidePanel("CierresBoletaFlyout");
                CargarBoletas();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarCierresBoleta()
        {
            ListaBoletaCierres = new List<BoletaCierresDTO>();
            if (BoletaSeleccionada == null || BoletaSeleccionada.Estado == Estados.ACTIVO) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletasCierrePorBoletaId
            {
                BoletaId = BoletaSeleccionada.BoletaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaBoletaCierres = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EditarPagoBoleta()
        {
            if (BoletaCierreSeleccionado != null)
            {
                if (!BoletaCierreSeleccionado.PuedeEditarCreditoDeduccion && BoletaCierreSeleccionado.BoletaCierreId > 0)
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                    DatoBoletaCierre = new BoletaCierresModel();
                    return;
                }
                
                DatoBoletaCierre = new BoletaCierresModel
                {
                    EsEdicion = true,
                    LineaCreditoId = BoletaCierreSeleccionado.LineaCreditoId,
                    BoletaCierreId = BoletaCierreSeleccionado.BoletaCierreId,
                    FormaDePago = BoletaCierreSeleccionado.FormaDePago,
                    Monto = BoletaCierreSeleccionado.Monto,
                    FechaPago = BoletaCierreSeleccionado.FechaPago,
                    NoDocumento = BoletaCierreSeleccionado.NoDocumento,
                    BancoId = BoletaCierreSeleccionado.BancoId,
                    NombreBanco = BoletaCierreSeleccionado.NombreBanco,
                    
                    CantidadJustificada = ListaBoletaCierresModel.Sum(m => m.Monto)
                };
            }
        }

        private void RemoverItemPagoBoleta()
        {
            if (BoletaCierreSeleccionado == null) return;

            if (!BoletaCierreSeleccionado.PuedeEditarCreditoDeduccion && BoletaCierreSeleccionado.BoletaCierreId > 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                DatoBoletaCierre = new BoletaCierresModel();
                return;
            }

            ListaBoletaCierresModel.Remove(BoletaCierreSeleccionado);
            DatoBoletaCierre = new BoletaCierresModel { CantidadJustificada = ListaBoletaCierresModel.Sum(j => j.Monto) };
        }

        private void AgregarItemPagoBoleta()
        {
            var mensajeValidacion = ValidarCierreBoleta();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemPagoBoleta = ListaBoletaCierresModel.FirstOrDefault(c => c.BoletaCierreId == DatoBoletaCierre.BoletaCierreId &&
                                                                        c.LineaCreditoId == DatoBoletaCierre.LineaCreditoId &&
                                                                        c.FormaDePago == DatoBoletaCierre.FormaDePago &&
                                                                        c.NoDocumento == DatoBoletaCierre.NoDocumento);

            var cantidadJustificada = Math.Round((ListaBoletaCierresModel.Sum(m => m.Monto)), 2);
            var totalAPagar = Math.Round(BoletaSeleccionada.TotalAPagar, 2);
            
            if (itemPagoBoleta == null)
            {
                cantidadJustificada += DatoBoletaCierre.Monto;
            }
            else
            {
                cantidadJustificada = (cantidadJustificada - itemPagoBoleta.Monto) + DatoBoletaCierre.Monto;
            }

            if (cantidadJustificada > totalAPagar)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, string.Format("La Cantidad Justificada L {0:0,0.00} supera el Total a Pagar {1:0,0.00}", cantidadJustificada, totalAPagar));
                return;
            }

            if (itemPagoBoleta == null)
            {
                ListaBoletaCierresModel.Add(DatoBoletaCierre);
            }
            else
            {
                itemPagoBoleta.LineaCreditoId = DatoBoletaCierre.LineaCreditoId;
                itemPagoBoleta.FormaDePago = DatoBoletaCierre.FormaDePago;
                itemPagoBoleta.Monto = DatoBoletaCierre.Monto;
                itemPagoBoleta.FechaPago = DatoBoletaCierre.FechaPago;
                itemPagoBoleta.NoDocumento = DatoBoletaCierre.NoDocumento;
                itemPagoBoleta.EsEdicion = true;
            }

            DatoBoletaCierre = new BoletaCierresModel
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                CantidadJustificada = ListaBoletaCierresModel.Sum(c => c.Monto),
                FechaPago = DateTime.Now
            };

            BancoSeleccionado = new BancosDTO();
            LineaCreditoSeleccionada = new LineasCreditoDTO();
        }

        private string ValidarCierreBoleta()
        {
            if (string.IsNullOrWhiteSpace(DatoBoletaCierre.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }

            if (DatoBoletaCierre.Monto <= 0)
            {
                return "Debe ingresar un monto mayor a 0";
            }
            
            if (FormaDePagoSeleccionada.RequiereBanco)
            {
                if (DatoBoletaCierre.BancoId == 0)
                {
                    return "Debe seleccionar un Banco";
                }

                if (string.IsNullOrWhiteSpace(DatoBoletaCierre.NoDocumento))
                {
                    return "Debe ingresar el Número de la Transacción";
                }
            }

            if (DatoBoletaCierre.LineaCreditoId == 0)
            {
                return "Debe Seleccionar una Linea de Crédito";
            }

            return string.Empty;
        }

        private void MostrarPagosBoleta()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta ya está Cerrada");
                return;
            }

            var boletaCierres = from reg in ListaBoletaCierres
                                select new BoletaCierresModel
                                {
                                    LineaCreditoId = reg.LineaCreditoId,
                                    BancoId = reg.BancoId,
                                    NombreBanco = reg.NombreBanco,
                                    BoletaCierreId = reg.BoletaCierreId,
                                    BoletaId = reg.BoletaId,
                                    FormaDePago = reg.FormaDePago,
                                    Monto = reg.Monto,
                                    NoDocumento = reg.NoDocumento,
                                    CodigoLineaCredito = reg.CodigoLineaCredito,
                                    PuedeEditarCreditoDeduccion = reg.PuedeEditarCreditoDeduccion
                                };

            ListaBoletaCierresModel = new ObservableCollection<BoletaCierresModel>(boletaCierres);
            DatoBoletaCierre = new BoletaCierresModel { FechaPago = DateTime.Now };

            if (ListaBoletaCierresModel.Any())
            {
                DatoBoletaCierre.CantidadJustificada = ListaBoletaCierresModel.Sum(c => c.Monto);
            }

            CargarSlidePanel("CierresBoletaFlyout");
        }

        private void CierreForzadoBoleta()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta Ya está Cerrada!");
                return;
            }
                DialogSettings = new ConfiguracionDialogoModel
                {
                    Mensaje = string.Format(Mensajes.Cerrar_Boleta, BoletaSeleccionada.CodigoBoleta),
                    Titulo = "Boletas"
                };

                DialogSettings.Respuesta += result =>
                {
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var uri = InformacionSistema.Uri_ApiService;
                        _client = new JsonServiceClient(uri);

                        var request = new PutCierreForzadoBoleta
                        {
                            BoletaId = BoletaSeleccionada.BoletaId,
                            UserId = InformacionSistema.UsuarioActivo.Usuario
                        };

                        _client.PutAsync(request, res =>
                        {
                            if (!string.IsNullOrWhiteSpace(res.MensajeError))
                            {
                                _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                                return;
                            }
                            _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);

                            CargarBoletas();

                        }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
                    }
                };
            
            DialogSettings = null;
        }

        private void MostrarAgregarDescarga()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta debe estar Activa");
                return;
            }

            if (BoletaSeleccionada.DescargaId != 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta ya tiene una descarga asignada");
                return;
            }

            DatoDescarga = new DescargadoresDTO
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                Estado = Estados.ACTIVO,
                FechaDescarga = DateTime.Now
            };

            CuadrillaAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);

            ObtenerPrecioDescarga(BoletaSeleccionada.PlantaId, BoletaSeleccionada.EquipoCategoriaId);
            CargarSlidePanel("AgregarDescargaFlyout");
        }

        private void CrearDescarga()
        {
            var mensajeValidacion = ValidarDescarga();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostDescargadores
            {
                Descarga = DatoDescarga,
                UserId = InformacionSistema.UsuarioActivo.Usuario
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
                CargarSlidePanel("AgregarDescargaFlyout");
                CargarBoletas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarDescarga()
        {
            if (DatoDescarga.BoletaId == 0)
            {
                return "No se pudo obtener la BoletaId";
            }

            if (DatoDescarga.CuadrillaId == 0)
            {
                return "Debe seleccionar la Cuadrilla que realizo la descarga";
            }

            if (DatoDescarga.PrecioDescarga == 0)
            {
                return "No se pudo obtener el precio de la Descarga";
            }

            return string.Empty;
        }

        private void MostrarAgregarOrdenCombustible()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta debe estar Activa");
                return;
            }

            CalculateFuelOrderAmount();
            LoadFuelOrderAssigned();
            LoadFuelOrderPendingByVendor();            
            CargarSlidePanel("FuelOrderFlyout");
        }

        private void CalculateFuelOrderAmount()
        {
            if (FuelOrdersAssigned == null)
            {
                FuelOrderDato = new OrdenCombustibleModel
                {
                    BoletaSaldo = BoletaSeleccionada.TotalAPagar
                };
            }
            else
            {
                decimal fuelAmount = FuelOrdersAssigned.Sum(f => f.Monto);
                FuelOrderDato = new OrdenCombustibleModel
                {
                    TotalAsignaciones = fuelAmount,
                    BoletaSaldo = Math.Round(BoletaSeleccionada.TotalAPagar - fuelAmount, 2)
                };
            }

            RaisePropertyChanged(nameof(FuelOrderDato));
        }

        private void AddFuelOrder()
        {
            OrdenCombustibleModel order = FuelOrdersAssigned.FirstOrDefault(o => o.OrdenCombustibleId == FuelOrderPendingSelected.OrdenCombustibleId);

            //new order
            if (order == null)
            {
                FuelOrdersAssigned.Add(FuelOrderPendingSelected);
            }

            CalculateFuelOrderAmount();
        }

        private void RemoveFuelOrder()
        {
            if (FuelOrderAssignedSelected == null) return;

            //remove order
            FuelOrdersAssigned.Remove(FuelOrderAssignedSelected);
            CalculateFuelOrderAmount();
        }

        private void AssignFuelOrdersToBoleta()
        {
            IEnumerable<OrdenesCombustibleDTO> x = from reg in FuelOrdersAssigned
                    select new OrdenesCombustibleDTO
                    {
                        BoletaId = reg.BoletaId,
                        OrdenCombustibleId = reg.OrdenCombustibleId,
                        AutorizadoPor = reg.AutorizadoPor,
                        CodigoFactura = reg.CodigoFactura,
                        Estado = reg.Estado,
                        Monto = reg.Monto,
                        FechaCreacion = reg.FechaCreacion,
                        Observaciones = reg.Observaciones,
                        PlacaEquipo = reg.PlacaEquipo,
                        FuelOrderSpecification = reg.FuelOrderSpecification
                    };
            List<OrdenesCombustibleDTO> fuelOrders = new List<OrdenesCombustibleDTO>(x);

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutOrdenesCombustibleABoleta
            {
                BoletaId = BoletaSeleccionada.BoletaId,
                OrdenesCombustible = fuelOrders,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };
            MostrarVentanaEspera = Visibility.Visible;
            _client.PutAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(res.ValidationErrorMessage))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.ValidationErrorMessage);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("FuelOrderFlyout");
                CargarBoletas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ObtenerPrecioProducto()
        {
            if (DatoBoleta == null || DatoBoleta.PlantaId == 0 || DatoBoleta.CategoriaProductoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPrecioProductoPorCategoriaProductoId
            {
                PlantaId = DatoBoleta.PlantaId,
                CategoriaProductoId = DatoBoleta.CategoriaProductoId
            };
            client.GetAsync(request, res =>
            {
                if (res != null)
                {
                    DatoBoleta.PrecioProductoCompra = res.PrecioCompra;
                    RaisePropertyChanged("DatoBoleta");
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GetBonificacion()
        {
            if (DatoBoleta == null || DatoBoleta.PlantaId == 0 || DatoBoleta.CategoriaProductoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetBonificacionProducto
            {
                PlantaId = DatoBoleta.PlantaId,
                CategoriaProductoId = DatoBoleta.CategoriaProductoId
            };
            client.GetAsync(request, res =>
            {
                if (res != null)
                {
                    DatoBoleta.IsBonusEnable = res.IsEnable;
                    RaisePropertyChanged(nameof(DatoBoleta));
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GetPlanta()
        {
            if (DatoBoleta == null || DatoBoleta.PlantaId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPlanta
            {
                PlantaId = DatoBoleta.PlantaId
            };
            client.GetAsync(request, res =>
            {
                if (res != null)
                {
                    DatoBoleta.IsShippingNumberRequired = res.IsShippingNumberRequired;
                    DatoBoleta.IsHorizontalImg = res.ImgHorizontalFormat;
                    RaisePropertyChanged(nameof(DatoBoleta));
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ObtenerPrecioDescarga(int plantaId, int equipoCategoriaId)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPrecioDescargaPorCategoriaEquipoId
            {
                PlantaId = plantaId,
                EquipoCategoriaId = equipoCategoriaId
            };
            client.GetAsync(request, res =>
            {
                if (res != null)
                {
                    DatoDescarga.PrecioDescarga = res.PrecioDescarga;
                    RaisePropertyChanged("DatoDescarga");
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });            
        }
        
        private void BuscarCuadrillas(string valorBusqueda)
        {
            if (DatoBoleta == null || DatoBoleta.PlantaId == 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Antes debe ingresar la Planta Destino");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetCuadrillasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                PlantaId = DatoBoleta.PlantaId
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

        private void BuscarCuadrillasPorBoleta(string valorBusqueda)
        {
            if (BoletaSeleccionada == null || BoletaSeleccionada.PlantaId == 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No se pudo obtener la Planta Destino");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetCuadrillasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                PlantaId = BoletaSeleccionada.PlantaId
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

        private void BuscarProveedores(string valorBusqueda)
        {
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
                    ListaProveedoresAutoComplete = (from reg in res
                                                select new AutoCompleteEntry(reg.Nombre, reg.ProveedorId)).ToList();
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
        
        private void Navegar()
        {
            CargarBoletas();
        }

        private void CargarBoletas()
        {
            var boletaId = BoletaSeleccionada != null ? BoletaSeleccionada.BoletaId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorBoletas
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaBoleta
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaBoletasControl = res;
                
                if (BusquedaBoletasControl.Items != null && BusquedaBoletasControl.Items.Any())
                {
                    if (boletaId == 0)
                    {
                        BoletaSeleccionada = BusquedaBoletasControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        BoletaSeleccionada = BusquedaBoletasControl.Items.FirstOrDefault(r => r.BoletaId == boletaId);
                    }

                    RaisePropertyChanged("BoletaSeleccionada");
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EliminarBoleta()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta YA está CERRADA");
                return;
            }

            if (BoletaSeleccionada.Estado == Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta está en Proceso de Pago");
                return;
            }

            if (BoletaSeleccionada.Estado == Estados.ACTIVO)
            {
                DialogSettings = new ConfiguracionDialogoModel
                {
                    Mensaje = string.Format(Mensajes.Eliminando_Boletas, BoletaSeleccionada.CodigoBoleta),
                    Titulo = "Eliminar Boleta"
                };

                DialogSettings.Respuesta += result =>
                {
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var uri = InformacionSistema.Uri_ApiService;
                        _client = new JsonServiceClient(uri);

                        var request = new DeleteBoleta
                        {
                            BoletaId = BoletaSeleccionada.BoletaId,
                            UserId = InformacionSistema.UsuarioActivo.Usuario                         
                        };

                        _client.DeleteAsync(request, res =>
                        {
                            if (!string.IsNullOrWhiteSpace(res.MensajeError))
                            {
                                _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                                return;
                            }
                            _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);

                            CargarBoletas();

                        }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
                    }
                };

                DialogSettings = null;
            }
        }

        private void MostrarEditarBoleta()
        {
            if (BoletaSeleccionada == null) return;

            if (BoletaSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Boleta debe estar Activa");
                return;
            }
            
            DatoBoleta = new BoletasDTO
            {
                PlantaId = BoletaSeleccionada.PlantaId,
                NombrePlanta = BoletaSeleccionada.NombrePlanta,
                BoletaId = BoletaSeleccionada.BoletaId,
                CodigoBoleta = BoletaSeleccionada.CodigoBoleta,
                NumeroEnvio = BoletaSeleccionada.NumeroEnvio,
                EquipoId = BoletaSeleccionada.EquipoId,
                DescripcionTipoEquipo = BoletaSeleccionada.DescripcionTipoEquipo,
                PlacaEquipo = BoletaSeleccionada.PlacaEquipo,
                ProveedorId = BoletaSeleccionada.ProveedorId,
                NombreProveedor = BoletaSeleccionada.NombreProveedor,
                CategoriaProductoId = BoletaSeleccionada.CategoriaProductoId,
                DescripcionTipoProducto = BoletaSeleccionada.DescripcionTipoProducto,
                PesoEntrada = BoletaSeleccionada.PesoEntrada,
                PesoSalida = BoletaSeleccionada.PesoSalida,
                PesoProducto = BoletaSeleccionada.PesoProducto,
                CantidadPenalizada = BoletaSeleccionada.CantidadPenalizada,
                PrecioProductoCompra = BoletaSeleccionada.PrecioProductoCompra,
                FechaSalida = BoletaSeleccionada.FechaSalida,
                Motorista = BoletaSeleccionada.Motorista,
                Bonus = BoletaSeleccionada.Bonus,
                HasBonus = BoletaSeleccionada.HasBonus,
                IsShippingNumberRequired = BoletaSeleccionada.IsShippingNumberRequired,
                Imagen = BoletaSeleccionada.Imagen
            };

            CargarSlidePanel("EditarBoletaFlyout");
        }
        
        private void CrearBoleta()
        {
            var mensajeValidacion = ValidarBoleta();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            BoletaSeleccionada = new BoletasDTO();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostBoleta
            {
                Boleta = DatoBoleta,
                UserId = InformacionSistema.UsuarioActivo.Usuario
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
                CargarSlidePanel("AgregarBoletaFlyout");
                CargarBoletas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarAgregarBoleta()
        {
            DatoBoleta = new BoletasDTO
            {
                Estado = Estados.ACTIVO,
                FechaSalida = DateTime.Now
            };

            LimpiarSeleccionAutoComplete();
            CargarSlidePanel("AgregarBoletaFlyout");            
        }

        private void BuscarCategoriaProductos(string valorBusqueda)
        {
            if (DatoBoleta == null) return;

            if (DatoBoleta.PlantaId == 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Primero debe seleccionar la Planta Destino");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetCategoriaProductoPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                PlantaId = DatoBoleta.PlantaId
            };
            client.GetAsync(request, res =>
            {
               if (res.Any())
                {
                    ListaCategoriaProductos = (from reg in res
                                               select new AutoCompleteEntry(reg.Descripcion, reg.CategoriaProductoId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EditarBoleta()
        {
            var mensajeValidacion = ValidarBoleta();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutBoleta
            {
                Boleta = DatoBoleta,
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
                CargarSlidePanel("EditarBoletaFlyout");
                CargarBoletas();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarBoleta()
        {
            NumeroPagina = 1;
            CargarBoletas();
        }
        
        public string ValidarBoleta()
        {
            var mensajeError = string.Empty;

            if (DatoBoleta.PlantaId == 0)
            {
                return "Debe seleccionar la Planta Destino de la Boleta";                
            }

            if (string.IsNullOrWhiteSpace(DatoBoleta.CodigoBoleta))
            {
                return "Debe ingresar el Código de la Boleta";
            }

            if (DatoBoleta.ProveedorId == 0)
            {
                return "Debe seleccionar el Cliente Proveedor";
            }

            if (DatoBoleta.CategoriaProductoId == 0)
            {
                return "Debe seleccionar el Tipo de Producto";
            }

            if (DatoBoleta.PesoProducto == 0)
            {
                return "Debe ingresar el Peso del Producto";
            }

            if (DatoBoleta.CantidadPenalizada < 0)
            {
                return "La cantidad penalizada no debe ser un valor negativo";
            }

            if (string.IsNullOrWhiteSpace(DatoBoleta.Motorista))
            {
                return "Debe ingresar el nombre del Motorista";
            }

            if (string.IsNullOrWhiteSpace(DatoBoleta.PlacaEquipo))
            {
                return "Debe ingresar la placa del Equipo";
            }
            
            return mensajeError;
        }

        private void LimpiarSeleccionAutoComplete()
        {
            ProveedorAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            CategoriaProductoAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);
            CuadrillaAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);
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

        private void LoadDataBoleta()
        {
            if (BoletaSeleccionada == null) return;

            LoadBoletaImg();
            CargarBoletaDetalle();
            CargarCierresBoleta();
        }

        private void LoadBoletaImg()
        {
            if (BoletaSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetBoletaImg
            {
                BoletaId = BoletaSeleccionada.BoletaId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;

                if (res == null) return;

                BoletaSeleccionada.Imagen = res.Imagen;
                RaisePropertyChanged(nameof(BoletaSeleccionada));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarDatosIniciales()
        {
            CargarBoletas();
            CargarBancos();
            CargarFormarDePago();
        }
        
        private void InicializarPropiedades()
        {
            BusquedaBoletasControl = new BusquedaBoletasDTO();
            ListaBoletaCierresModel = new ObservableCollection<BoletaCierresModel>();
            AssignedLoansBoleta = new ObservableCollection<PagoPrestamoModel>();
            DatoBoletaCierre = new BoletaCierresModel();
            DatoBoletaOtraDeduccion = new BoletaOtrasDeduccionesModel();                    
        }

        private void CargarDatosPruebas()
        {
            BusquedaBoletasControl = new BusquedaBoletasDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaBoletas()
            };

            BoletaSeleccionada = BusquedaBoletasControl.Items.FirstOrDefault();
        }
    }
}
