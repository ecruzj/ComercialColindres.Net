using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.DTOs.RequestDTOs.Gasolineras;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels
{
    public class GasolinerasViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public GasolinerasViewModel(IServiciosComunes serviciosComunes)
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

        public string ValorBusquedaOrdenesCombustible
        {
            get { return _valorBusquedaOrdenesCombustible; }
            set
            {
                _valorBusquedaOrdenesCombustible = value;
                RaisePropertyChanged("ValorBusquedaOrdenesCombustible");
            }
        }
        private string _valorBusquedaOrdenesCombustible;

        public int NumeroPaginaOrdenesCombustible
        {
            get { return _numeroPaginaOrdenesCombustible; }
            set
            {
                _numeroPaginaOrdenesCombustible = value;
                RaisePropertyChanged("NumeroPaginaOrdenesCombustible");
            }
        }
        private int _numeroPaginaOrdenesCombustible = 1;

        public BusquedaOrdenesCombustibleDTO BusquedaOrdenesCombustibleControl
        {
            get { return _busquedaOrdenesCombustibleControl; }
            set
            {
                _busquedaOrdenesCombustibleControl = value;
                RaisePropertyChanged("BusquedaOrdenesCombustibleControl");
            }
        }
        private BusquedaOrdenesCombustibleDTO _busquedaOrdenesCombustibleControl;

        public OrdenesCombustibleDTO OrdenCombustibleSeleccionado
        {
            get { return _ordenCombustibleSeleccionado; }
            set
            {
                _ordenCombustibleSeleccionado = value;
                RaisePropertyChanged("OrdenCombustibleSeleccionado");

                LoadFuelOrderImg();
                LoadFuelOrderManualPayments();
            }
        }
        private OrdenesCombustibleDTO _ordenCombustibleSeleccionado;

        public OrdenesCombustibleDTO DatoOrdenCombustible
        {
            get { return _datoOrdenCombustible; }
            set
            {
                _datoOrdenCombustible = value;
                RaisePropertyChanged("DatoOrdenCombustible");
            }
        }
        private OrdenesCombustibleDTO _datoOrdenCombustible;

        public List<OrdenesCombustibleDTO> listaOrdenesCombustible
        {
            get { return _listaOrdenesCombustible; }
            set
            {
                _listaOrdenesCombustible = value;
                RaisePropertyChanged("listaOrdenesCombustible");
            }
        }
        private List<OrdenesCombustibleDTO> _listaOrdenesCombustible;

        public string ValorBusquedaGasolineraCredito
        {
            get { return _valorBusquedaGasolineraCredito; }
            set
            {
                _valorBusquedaGasolineraCredito = value;
                RaisePropertyChanged("ValorBusquedaGasolineraCredito");
            }
        }
        private string _valorBusquedaGasolineraCredito;

        public int NumeroPaginaGasolineraCreditos
        {
            get { return _numeroPaginaGasolineraCreditos; }
            set
            {
                _numeroPaginaGasolineraCreditos = value;
                RaisePropertyChanged("NumeroPaginaGasolineraCreditos");
            }
        }
        private int _numeroPaginaGasolineraCreditos = 1;

        public BusquedaGasolineraCreditosDTO BusquedaGasolineraCreditosControl
        {
            get { return _busquedaGasolineraCreditosControl; }
            set
            {
                _busquedaGasolineraCreditosControl = value;
                RaisePropertyChanged("BusquedaGasolineraCreditosControl");
            }
        }
        private BusquedaGasolineraCreditosDTO _busquedaGasolineraCreditosControl;

        public GasolineraCreditosDTO GasolineraCreditoSeleccionado
        {
            get { return _gasolineraCreditoSeleccionado; }
            set
            {
                _gasolineraCreditoSeleccionado = value;
                RaisePropertyChanged("GasolineraCreditoSeleccionado");

                if (IsInDesignMode)
                {
                    return;
                }

                CargarOrdenesCombustiblePorGasCredito();
                CargarPagosGasCreditos();
            }
        }
        private GasolineraCreditosDTO _gasolineraCreditoSeleccionado;

        public List<GasolineraCreditosDTO> ListaGasolineraCreditos
        {
            get { return _listaGasolineraCreditos; }
            set
            {
                _listaGasolineraCreditos = value;
                RaisePropertyChanged("ListaGasolineraCreditos");
            }
        }
        private List<GasolineraCreditosDTO> _listaGasolineraCreditos;

        public GasolineraCreditosDTO DatoGasCredito
        {
            get { return _datoGasCredito; }
            set
            {
                _datoGasCredito = value;
                RaisePropertyChanged("DatoGasCredito");
            }
        }
        private GasolineraCreditosDTO _datoGasCredito;

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

                if (FuelManualPaymentData != null)
                {
                    FuelManualPaymentData.BankId = BancoSeleccionado.BancoId;
                    FuelManualPaymentData.BankName = BancoSeleccionado.Descripcion;
                    return;
                }

                if (DatoGasCreditoPago != null)
                {
                    DatoGasCreditoPago.BancoId = BancoSeleccionado.BancoId;
                    DatoGasCreditoPago.NombreBanco = BancoSeleccionado.Descripcion;
                }

                if (FormaDePagoSeleccionada == null) return;

                if (!DatoGasCreditoPago.EsEdicion)
                {
                    DatoGasCreditoPago = new GasolineraCreditoPagosModel
                    {
                        EsEdicion = false,
                        FormaDePago = FormaDePagoSeleccionada.Descripcion,
                        BancoId = BancoSeleccionado.BancoId,
                        GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
                        CantidadJustificada = ListaGasCreditoPagosModel.Sum(c => c.Monto)
                    };
                }
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

                if (DatoGasCreditoPago != null && !DatoGasCreditoPago.EsEdicion)
                {
                    DatoGasCreditoPago = new GasolineraCreditoPagosModel
                    {
                        EsEdicion = false,
                        GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
                        CantidadJustificada = ListaGasCreditoPagosModel.Sum(c => c.Monto)
                    };

                    LineasCredito = new List<LineasCreditoDTO>();
                    LineaCreditoSeleccionada = new LineasCreditoDTO();
                    BancoSeleccionado = new BancosDTO();
                }
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;

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
                DatoGasCreditoPago.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                DatoGasCreditoPago.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;

        public List<AutoCompleteEntry> ListaGasolinerasAutoComplete
        {
            get
            {
                return _listaGasolinerasAutoComplete;
            }
            set
            {
                _listaGasolinerasAutoComplete = value;
                RaisePropertyChanged("ListaGasolinerasAutoComplete");
            }
        }
        private List<AutoCompleteEntry> _listaGasolinerasAutoComplete;

        public AutoCompleteEntry GasolineraAutoCompleteSeleccionada
        {
            get
            {
                return _gasolineraAutoCompleteSeleccionada;
            }
            set
            {
                _gasolineraAutoCompleteSeleccionada = value;
                RaisePropertyChanged("GasolineraAutoCompleteSeleccionada");

                if (GasolineraAutoCompleteSeleccionada != null && GasolineraAutoCompleteSeleccionada.Id != null)
                {
                    if (IsFuelOrder)
                    {
                        var gasCreditoActual = ObtenerGasCreditoActual();

                        if (gasCreditoActual == null)
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe un Crédito de Gasolina Disponible");
                            return;
                        }

                        DatoOrdenCombustible.GasCreditoId = gasCreditoActual.GasCreditoId;
                        DatoOrdenCombustible.CodigoCreditoCombustible = gasCreditoActual.CodigoGasCredito;
                        RaisePropertyChanged("DatoOrdenCombustible");
                    }
                    else
                    {
                        DatoGasCredito.GasolineraId = (int)GasolineraAutoCompleteSeleccionada.Id;
                        RaisePropertyChanged("DatoGasCredito");
                    }
                }
            }
        }
        private AutoCompleteEntry _gasolineraAutoCompleteSeleccionada;

        public bool IsFuelOrder
        {
            get { return _isFuelOrder; }
            set
            {
                _isFuelOrder = value;
                RaisePropertyChanged("IsFuelOrder");
            }
        }
        private bool _isFuelOrder;

        public List<GasolineraCreditoPagosDTO> ListaGasCreditoPagos
        {
            get { return _listaGasCreditoPagos; }
            set
            {
                _listaGasCreditoPagos = value;
                RaisePropertyChanged("ListaGasCreditoPagos");
            }
        }
        private List<GasolineraCreditoPagosDTO> _listaGasCreditoPagos;

        public ObservableCollection<GasolineraCreditoPagosModel> ListaGasCreditoPagosModel
        {
            get { return _listaGasCreditoPagosModel; }
            set
            {
                _listaGasCreditoPagosModel = value;
                RaisePropertyChanged("ListaGasCreditoPagosModel");
            }
        }
        private ObservableCollection<GasolineraCreditoPagosModel> _listaGasCreditoPagosModel;

        public GasolineraCreditoPagosModel DatoGasCreditoPago
        {
            get { return _datoGasCreditoPago; }
            set
            {
                _datoGasCreditoPago = value;
                RaisePropertyChanged("DatoGasCreditoPago");
            }
        }
        private GasolineraCreditoPagosModel _datoGasCreditoPago;

        public GasolineraCreditoPagosModel GasCreditoPagoSeleccionado
        {
            get { return _gasCreditoPagoSeleccionado; }
            set
            {
                _gasCreditoPagoSeleccionado = value;
                RaisePropertyChanged("GasCreditoPagoSeleccionado");
            }
        }
        private GasolineraCreditoPagosModel _gasCreditoPagoSeleccionado;

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
                if (DatoOrdenCombustible == null) return;

                DatoOrdenCombustible.ProveedorId = (int)ProveedorAutoCompleteSeleccionado.Id;
                DatoOrdenCombustible.NombreProveedor = (string)ProveedorAutoCompleteSeleccionado.DisplayName;

                RaisePropertyChanged(nameof(DatoOrdenCombustible));
            }
        }
        private AutoCompleteEntry _proveedorAutoCompleteSeleccionado;

        public List<FuelOrderManualPaymentDto> FuelOrderManualPayments
        {
            get { return _fuelOrderManualPayments; }
            set { _fuelOrderManualPayments = value;
                RaisePropertyChanged(nameof(FuelOrderManualPayments));
            }
        }
        private List<FuelOrderManualPaymentDto> _fuelOrderManualPayments;

        public ObservableCollection<FuelOrderManualPaymentModel> FuelManualPayments
        {
            get { return _fuelManualPayments; }
            set { _fuelManualPayments = value;
                RaisePropertyChanged(nameof(FuelManualPayments));
            }
        }
        private ObservableCollection<FuelOrderManualPaymentModel> _fuelManualPayments;

        public FuelOrderManualPaymentModel FuelManualPaymentSelected
        {
            get { return _fuelManualPaymentSelected; }
            set { _fuelManualPaymentSelected = value;
                RaisePropertyChanged(nameof(FuelManualPaymentSelected));
            }
        }
        private FuelOrderManualPaymentModel _fuelManualPaymentSelected;

        public FuelOrderManualPaymentModel FuelManualPaymentData
        {
            get { return _fuelManualPaymentData; }
            set { _fuelManualPaymentData = value;
                RaisePropertyChanged(nameof(FuelManualPaymentData));
            }
        }
        private FuelOrderManualPaymentModel _fuelManualPaymentData;


        public RelayCommand ComandoMostrarOrdenesCombustible { get; set; }
        public RelayCommand ComandoBuscarOrdenCombustible { get; set; }
        public RelayCommand ComandoMostrarAgregarOrdenCombustible { get; set; }
        public RelayCommand ComandoAgregarOrdenCombustible { get; set; }
        public RelayCommand ComandoActualizarOrdenCombustible { get; set; }
        public RelayCommand ComandoMostrarEditarOrdenCombustible { get; set; }
        public RelayCommand ComandoEliminarOrdenCombustible { get; set; }
        public RelayCommand ComandoRefrescarOrdenesCombustible { get; set; }
        public RelayCommand ComandoNavegarOrdenesCombustible { get; set; }
        public RelayCommand<string> CommandSearchVendor { get; set; }
        public RelayCommand CommandSelectImage { get; set; }
        public RelayCommand CommandSaveAndOpenImage { get; set; }

        public RelayCommand ComandoMostrarFacturaciones { get; set; }
        public RelayCommand ComandoBuscarGasolineraCreditos { get; set; }
        public RelayCommand ComandoMostrarAgregarGasolineraCredito { get; set; }
        public RelayCommand ComandoGuardarGasolineraCreditos { get; set; }
        public RelayCommand ComandoActivarGasCredito { get; set; }
        public RelayCommand ComandoMostrarEditarGasolineraCredito { get; set; }
        public RelayCommand ComandoActualizarGasolineraCredito { get; set; }
        public RelayCommand ComandoEliminarGasolineraCredito { get; set; }
        public RelayCommand ComandoRefrescarGasolineraCreditos { get; set; }
        public RelayCommand ComandoNavegarGasolineraCreditos { get; set; }

        public RelayCommand<string> ComandoBuscarGasolineras { get; set; }
        public RelayCommand ComandoMostrarGasCreditoPagos { get; set; }
        public RelayCommand ComandoAgregarGasPagoCredito { get; set; }
        public RelayCommand ComandoRemoverGasCreditoPago { get; set; }
        public RelayCommand ComandoEditarGasCreditoPago { get; set; }
        public RelayCommand ComandoGuardarGasCreditoPagos { get; set; }

        public RelayCommand ComandoImprimirDetalleGasCredito { get; set; }

        public RelayCommand CommandShowCloseFuelOrderManual { get; set; }
        public RelayCommand CommandAddManualPaymentItem { get; set; }
        public RelayCommand CommandRemoveFuelManualPaymentItem { get; set; }
        public RelayCommand CommandSaveFuelManualPayment { get; set; }

        private void InicializarComandos()
        {
            ComandoMostrarOrdenesCombustible = new RelayCommand(MostrarOrdenesCombustible);
            ComandoMostrarAgregarOrdenCombustible = new RelayCommand(MostrarAgregarOrdenCombustible);
            ComandoAgregarOrdenCombustible = new RelayCommand(SaveOrdenCombustible);
            ComandoMostrarEditarOrdenCombustible = new RelayCommand(MostrarEditarOrdenCombustible);
            ComandoActualizarOrdenCombustible = new RelayCommand(ActualizarOrdenCombustible);
            ComandoEliminarOrdenCombustible = new RelayCommand(EliminarOrdenCombustible);
            ComandoBuscarOrdenCombustible = new RelayCommand(BuscarOrdenCombustible);
            ComandoRefrescarOrdenesCombustible = new RelayCommand(NavegarOrdenesCombustible);
            ComandoNavegarOrdenesCombustible = new RelayCommand(NavegarOrdenesCombustible);
            CommandSearchVendor = new RelayCommand<string>(SearchVendor);
            CommandSelectImage = new RelayCommand(SelectImage);
            CommandSaveAndOpenImage = new RelayCommand(SaveAndOpenImage);

            ComandoMostrarFacturaciones = new RelayCommand(MostrarFacturaciones);
            ComandoMostrarAgregarGasolineraCredito = new RelayCommand(MostrarAgregarGasCredito);
            ComandoGuardarGasolineraCreditos = new RelayCommand(CrearGasCredito);
            ComandoActivarGasCredito = new RelayCommand(ActivarGasCredito);
            ComandoMostrarEditarGasolineraCredito = new RelayCommand(MostrarEditarGasCredito);
            ComandoActualizarGasolineraCredito = new RelayCommand(ActualizarGasCredito);
            ComandoEliminarGasolineraCredito = new RelayCommand(EliminarGasCredito);
            ComandoBuscarGasolineraCreditos = new RelayCommand(BuscarGasolineraCredito);
            ComandoRefrescarGasolineraCreditos = new RelayCommand(NavegarGasCreditos);
            ComandoNavegarGasolineraCreditos = new RelayCommand(NavegarGasCreditos);

            ComandoBuscarGasolineras = new RelayCommand<string>(BuscarGasolineras);
            ComandoMostrarGasCreditoPagos = new RelayCommand(MostrarGasCreditoPagos);
            ComandoAgregarGasPagoCredito = new RelayCommand(AgregarItemGasPagoCredito);
            ComandoRemoverGasCreditoPago = new RelayCommand(RemoverGasCreditoPago);
            ComandoEditarGasCreditoPago = new RelayCommand(EditarGasCreditoPago);
            ComandoGuardarGasCreditoPagos = new RelayCommand(GuardarGasCreditoPagos);

            ComandoImprimirDetalleGasCredito = new RelayCommand(ImprimirDetalleGasCredito);

            CommandShowCloseFuelOrderManual = new RelayCommand(ShowCloseFuelOrderManual);
            CommandAddManualPaymentItem = new RelayCommand(AddManualPaymentItem);
            CommandRemoveFuelManualPaymentItem = new RelayCommand(RemoveFuelManualPaymentItem);
            CommandSaveFuelManualPayment = new RelayCommand(SaveFuelManualPayment);
        }

        private void SaveFuelManualPayment()
        {
            var data = (from reg in FuelManualPayments
                       select new FuelOrderManualPaymentDto
                       {
                           Amount = reg.Amount,
                           BankId = reg.BankId,
                           BankReference = reg.BankReference,
                           FuelOrderId = reg.FuelOrderId,
                           FuelOrderManualPaymentId = reg.FuelOrderManualPaymentId,
                           Observations = reg.Observations,
                           PaymentDate = reg.PaymentDate,
                           WayToPay = reg.WayToPay
                       }).ToList();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostFuelOrderManualPayments
            {
                FuelOrderId = OrdenCombustibleSeleccionado.OrdenCombustibleId,
                FuelOrderManualPayments = data,
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
                CargarSlidePanel("CloseFuelOrderManualFlyout");
                CargarOrdenesCombustible();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void RemoveFuelManualPaymentItem()
        {
            if (FuelManualPaymentSelected == null) return;

            FuelManualPayments.Remove(FuelManualPaymentSelected);
            InstanceNewManualPayment();
        }

        private void AddManualPaymentItem()
        {
            if (FuelManualPaymentData == null) return;

            var item = FuelManualPayments.FirstOrDefault(t => t.BankId == FuelManualPaymentData.BankId &&
                                                              t.BankReference == FuelManualPaymentData.BankReference &&
                                                              t.WayToPay == FuelManualPaymentData.WayToPay);

            // new item
            if (item == null)
            {
                FuelManualPayments.Add(FuelManualPaymentData);
            }
            else
            {
                //update item
                item.Observations = FuelManualPaymentData.Observations;
                item.PaymentDate = FuelManualPaymentData.PaymentDate;
                item.Amount = FuelManualPaymentData.Amount;
            }

            InstanceNewManualPayment();
        }

        private void LoadFuelOrderManualPayments()
        {
            FuelOrderManualPayments = new List<FuelOrderManualPaymentDto>();

            if (OrdenCombustibleSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetFuelOrderManualPayments
            {
                FuelOrderId = OrdenCombustibleSeleccionado.OrdenCombustibleId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                FuelOrderManualPayments = res;
                RaisePropertyChanged(nameof(FuelOrderManualPayments));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ShowCloseFuelOrderManual()
        {
            if (OrdenCombustibleSeleccionado == null) return;

            if (OrdenCombustibleSeleccionado.Estado == Estados.CERRADO && OrdenCombustibleSeleccionado.BoletaId != null)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Information, "La orden de combustible fue cerrada con Boleta");
                return;
            }

            IEnumerable<FuelOrderManualPaymentModel> data = from reg in FuelOrderManualPayments
                                                            select new FuelOrderManualPaymentModel
                                                            {
                                                                FuelOrderId = reg.FuelOrderId,
                                                                Amount = reg.Amount,
                                                                BankId = reg.BankId,
                                                                BankName = reg.BankName,
                                                                BankReference = reg.BankReference,
                                                                FuelOrderManualPaymentId = reg.FuelOrderManualPaymentId,
                                                                Observations = reg.Observations,
                                                                PaymentDate = reg.PaymentDate,
                                                                WayToPay = reg.WayToPay
                                                            };

            FuelManualPayments = new ObservableCollection<FuelOrderManualPaymentModel>(data);
            InstanceNewManualPayment();
            CargarSlidePanel("CloseFuelOrderManualFlyout");
        }

        private void InstanceNewManualPayment()
        {
            FuelManualPaymentData = new FuelOrderManualPaymentModel
            {
                FuelOrderId = OrdenCombustibleSeleccionado.OrdenCombustibleId,
                PaymentDate = DateTime.Now,
                TotalPayments = FuelManualPayments.Sum(t => t.Amount)
            };
        }

        private void LoadFuelOrderImg()
        {
            if (OrdenCombustibleSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetOrdenCombustibleImg
            {
                OrdenCombustibleId = OrdenCombustibleSeleccionado.OrdenCombustibleId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;

                if (res == null) return;

                OrdenCombustibleSeleccionado.Imagen = res.Imagen;
                RaisePropertyChanged(nameof(OrdenCombustibleSeleccionado));
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
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
                DatoOrdenCombustible.Imagen = data;
                RaisePropertyChanged(nameof(DatoOrdenCombustible));
            }
        }

        private void SaveAndOpenImage()
        {
            if (OrdenCombustibleSeleccionado == null || OrdenCombustibleSeleccionado.Imagen == null) return;

            string name = OrdenCombustibleSeleccionado.GetOrderFuelName();

            SaveImage(OrdenCombustibleSeleccionado.Imagen, name);

        }

        private void SaveImage(byte[] byteArrayIn, string fileName)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image img = Image.FromStream(ms);

            string folderPath = _serviciosComunes.GetPathFuelOrder();
            string imagePath = folderPath + fileName;

            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            Process.Start(imagePath);
        }

        private void SearchVendor(string valorBusqueda)
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

        private void BuscarOrdenCombustible()
        {
            NumeroPaginaOrdenesCombustible = 1;
            CargarOrdenesCombustible();
        }

        private void BuscarGasolineraCredito()
        {
            NumeroPaginaGasolineraCreditos = 1;
            CargarGasolineraCreditos();
        }

        private void CargarLineasDeCreditoPorBanco()
        {
            LineasCredito = new List<LineasCreditoDTO>();

            if (DatoGasCreditoPago.BancoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetLineasCreditoPorBancoPorTipoCuenta
            {
                BancoId = DatoGasCreditoPago.BancoId,
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

        private void ImprimirDetalleGasCredito()
        {
            ObtenerInformacionReporte();
        }

        private void ObtenerInformacionReporte()
        {
            if (GasolineraCreditoSeleccionado == null) return;

            if (GasolineraCreditoSeleccionado.Estado == Estados.NUEVO || GasolineraCreditoSeleccionado.Estado == Estados.ANULADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe información para mostrar");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetGasCreditoDetalle
            {
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId
            };
            _client.GetAsync(request, res =>
            {
                if (res.GasCreditoEncabezado == null)
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Existe Infomacion que Mostrar");
                    return;
                }

                ImprimirReporte(res);
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void ImprimirReporte(RptGasCreditoDetalleResumenDTO datos)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Gasolinera Créditos";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "ReporteGasCreditoDetalle.rdlc";
                manejadorReporte.ListadoItemsDataSet = new List<ItemDataSetModel>
                {
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Encabezado",
                        Datos = datos.GasCreditoEncabezado
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "DetalleOrdenesOperativas",
                        Datos = datos.OrdenesCombustibleOperativo
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "DetalleOrdenesPersonales",
                        Datos = datos.OrdenesCombustiblePersonales
                    }
                };
                manejadorReporte.MultipleDataSet = true;
                manejadorReporte.MostarReporte();
            });
        }

        private void MostrarAgregarOrdenCombustible()
        {
            IsFuelOrder = true;
            DatoOrdenCombustible = new OrdenesCombustibleDTO
            {
                Estado = Estados.ACTIVO,
                FechaCreacion = DateTime.Now
            };

            ProveedorAutoCompleteSeleccionado = new AutoCompleteEntry(null, null);
            GasolineraAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            CargarSlidePanel("AgregarOrdenCombustibleFlyout");
        }

        private GasolineraCreditosDTO ObtenerGasCreditoActual()
        {
            var request = new GetGasolineraCreditoActual
            {
                GasolineraId = (int)GasolineraAutoCompleteSeleccionada.Id
            };

            var gasCreditoActual = _client.Get(request);

            return gasCreditoActual;
        }

        private void SaveOrdenCombustible()
        {
            var mensajeValidacion = ValidarOrdenCombustible();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostOrdenesCombustible
            {
                OrdenCombustible = DatoOrdenCombustible,
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
                CargarSlidePanel("AgregarOrdenCombustibleFlyout");
                CargarOrdenesCombustible();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarOrdenCombustible()
        {
            if (string.IsNullOrWhiteSpace(DatoOrdenCombustible.CodigoFactura))
            {
                return "Debe ingresar el Número de Factura de la Orden de Combustible";
            }

            if (string.IsNullOrWhiteSpace(DatoOrdenCombustible.AutorizadoPor))
            {
                return "Debe ingresar Quién Autoriza la Orden de Combustible";
            }

            if (!DatoOrdenCombustible.EsOrdenPersonal)
            {
                if (DatoOrdenCombustible.ProveedorId == null)
                {
                    return "Debe ingresar quien es el dueño de la orden";
                }
            }

            if (string.IsNullOrWhiteSpace(DatoOrdenCombustible.PlacaEquipo))
            {
                return "Debe ingresar la Placa del Carro de la Orden de Combustible";
            }

            if (DatoOrdenCombustible.Monto == 0)
            {
                return "Debe ingresar el Monto de la Factura de la Orden de Combustible";
            }

            if (DatoOrdenCombustible.GasCreditoId == 0)
            {
                return "Debe ingresar la Gasolinera donde se aquirió de la Orden de Combustible";
            }

            if (DatoOrdenCombustible.BoletaId != null || DatoOrdenCombustible.BoletaId == 0)
            {
                return "No pudo obtener el EquipoId";
            }

            return string.Empty;
        }

        private void NavegarGasCreditos()
        {
            CargarGasolineraCreditos();
        }

        private void NavegarOrdenesCombustible()
        {
            CargarOrdenesCombustible();
        }

        private void MostrarGasCreditoPagos()
        {
            if (GasolineraCreditoSeleccionado == null) return;

            if (GasolineraCreditoSeleccionado.Estado == Estados.ACTIVO || GasolineraCreditoSeleccionado.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Justificar Pagos para Gas Créditos en estados NUEVO o ENPROCESO");
                return;
            }

            var listaGasCreditoPagos = from reg in ListaGasCreditoPagos
                                       select new GasolineraCreditoPagosModel
                                       {
                                           LineaCreditoId = reg.LineaCreditoId,
                                           CodigoLineaCredito = reg.CodigoLineaCredito,
                                           FormaDePago = reg.FormaDePago,
                                           GasCreditoId = reg.GasCreditoId,
                                           GasCreditoPagoId = reg.GasCreditoPagoId,
                                           Monto = reg.Monto,
                                           NoDocumento = reg.NoDocumento,
                                           BancoId = reg.BancoId,
                                           NombreBanco = reg.NoDocumento,
                                           PuedeEditarGasCreditoPago = reg.PuedeEditarCreditoDeduccion
                                       };
            ListaGasCreditoPagosModel = new ObservableCollection<GasolineraCreditoPagosModel>(listaGasCreditoPagos);
            DatoGasCreditoPago = new GasolineraCreditoPagosModel
            {
                CantidadJustificada = ListaGasCreditoPagosModel.Sum(j => j.Monto)
            };

            CargarSlidePanel("GasCreditoPagosFlyout");
        }

        private void AgregarItemGasPagoCredito()
        {
            var mensajeValidacion = ValidarItemGasPagoCredito();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemGasCredito = ListaGasCreditoPagosModel.FirstOrDefault(c => c.GasCreditoPagoId == DatoGasCreditoPago.GasCreditoPagoId &&
                                                                        c.LineaCreditoId == DatoGasCreditoPago.LineaCreditoId &&
                                                                        c.FormaDePago == DatoGasCreditoPago.FormaDePago &&
                                                                        c.NoDocumento == DatoGasCreditoPago.NoDocumento);

            var montoJustificado = Math.Round(ListaGasCreditoPagosModel.Sum(j => j.Monto), 2);
            var totalAPagar = Math.Round(GasolineraCreditoSeleccionado.Credito, 2);

            if (itemGasCredito == null)
            {
                montoJustificado += DatoGasCreditoPago.Monto;
            }
            else
            {
                montoJustificado = (montoJustificado - itemGasCredito.Monto) + DatoGasCreditoPago.Monto;
            }

            if (montoJustificado > totalAPagar)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, string.Format("La Justificación de Pago L. {0} supera el total a Pagar del Crédito L. {1}", montoJustificado, totalAPagar));
                return;
            }

            if (itemGasCredito == null)
            {
                ListaGasCreditoPagosModel.Add(DatoGasCreditoPago);
            }
            else
            {
                itemGasCredito.LineaCreditoId = DatoGasCreditoPago.LineaCreditoId;
                itemGasCredito.FormaDePago = DatoGasCreditoPago.FormaDePago;
                itemGasCredito.Monto = DatoGasCreditoPago.Monto;
                itemGasCredito.NoDocumento = DatoGasCreditoPago.NoDocumento;
            }

            DatoGasCreditoPago = new GasolineraCreditoPagosModel
            {
                CantidadJustificada = ListaGasCreditoPagosModel.Sum(j => j.Monto)
            };

            BancoSeleccionado = new BancosDTO();
        }

        private string ValidarItemGasPagoCredito()
        {
            if (string.IsNullOrWhiteSpace(DatoGasCreditoPago.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }

            if (DatoGasCreditoPago.Monto <= 0)
            {
                return "Debe ingresar un monto mayor a 0";
            }

            if (FormaDePagoSeleccionada.RequiereBanco)
            {
                if (DatoGasCreditoPago.BancoId == 0)
                {
                    return "Debe seleccinar un Banco";
                }

                if (string.IsNullOrWhiteSpace(DatoGasCreditoPago.NoDocumento))
                {
                    return "Debe ingresar el Número de la Transacción";
                }
            }

            if (DatoGasCreditoPago.LineaCreditoId == 0)
            {
                return "Debe seleccionar una Linea de Crédito";
            }

            return string.Empty;
        }

        private void GuardarGasCreditoPagos()
        {
            var gasCreditoPagos = from reg in ListaGasCreditoPagosModel
                                  select new GasolineraCreditoPagosDTO
                                  {
                                      LineaCreditoId = reg.LineaCreditoId,
                                      FormaDePago = reg.FormaDePago,
                                      GasCreditoId = reg.GasCreditoId,
                                      GasCreditoPagoId = reg.GasCreditoPagoId,
                                      Monto = reg.Monto,
                                      NoDocumento = reg.NoDocumento,
                                      EstaEditandoRegistro = reg.EsEdicion
                                  };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostGasCreditoPagos
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
                GasolineraCreditoPagos = gasCreditoPagos.ToList()
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
                CargarSlidePanel("GasCreditoPagosFlyout");
                CargarGasolineraCreditos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EditarGasCreditoPago()
        {
            if (GasCreditoPagoSeleccionado != null)
            {
                if (!GasCreditoPagoSeleccionado.PuedeEditarGasCreditoPago && GasCreditoPagoSeleccionado.GasCreditoPagoId > 0)
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                    GasCreditoPagoSeleccionado = new GasolineraCreditoPagosModel();
                    return;
                }

                DatoGasCreditoPago = new GasolineraCreditoPagosModel
                {
                    EsEdicion = true,
                    LineaCreditoId = GasCreditoPagoSeleccionado.LineaCreditoId,
                    FormaDePago = GasCreditoPagoSeleccionado.FormaDePago,
                    GasCreditoId = GasCreditoPagoSeleccionado.GasCreditoId,
                    GasCreditoPagoId = GasCreditoPagoSeleccionado.GasCreditoPagoId,
                    Monto = GasCreditoPagoSeleccionado.Monto,
                    NoDocumento = GasCreditoPagoSeleccionado.NoDocumento,
                    BancoId = GasCreditoPagoSeleccionado.BancoId,
                    NombreBanco = GasCreditoPagoSeleccionado.NombreBanco,

                    CantidadJustificada = ListaGasCreditoPagosModel.Sum(j => j.Monto)
                };
            }
        }

        private void RemoverGasCreditoPago()
        {
            if (GasCreditoPagoSeleccionado == null) return;

            if (!GasCreditoPagoSeleccionado.PuedeEditarGasCreditoPago && GasCreditoPagoSeleccionado.GasCreditoPagoId > 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                GasCreditoPagoSeleccionado = new GasolineraCreditoPagosModel();
                return;
            }

            ListaGasCreditoPagosModel.Remove(GasCreditoPagoSeleccionado);
            DatoGasCreditoPago = new GasolineraCreditoPagosModel { CantidadJustificada = ListaGasCreditoPagosModel.Sum(j => j.Monto) };
        }

        private void ActivarGasCredito()
        {
            if (GasolineraCreditoSeleccionado == null) return;

            if (GasolineraCreditoSeleccionado.Estado != Estados.PENDIENTE)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Activar Créditos de Gasolina en Estado PENDIENTE");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutActivarGasolineraCreditos
            {
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
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
                CargarGasolineraCreditos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EliminarGasCredito()
        {
            if (GasolineraCreditoSeleccionado == null) return;

            if (GasolineraCreditoSeleccionado.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Créditos de Gasolina Nuevos");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, GasolineraCreditoSeleccionado.CodigoGasCredito),
                Titulo = "Gasolinera Créditos"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteGasolineraCredito
                    {
                        GasolineraCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
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
                        CargarGasolineraCreditos();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void ActualizarGasCredito()
        {
            if (DatoGasCredito.Credito <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Monto del Crédito de Combustible debe ser mayor a 0");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutGasolineraCreditos
            {
                GasolineraCredito = DatoGasCredito,
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
                CargarSlidePanel("EditarGasCreditoFlyout");
                CargarGasolineraCreditos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CrearGasCredito()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostGasolineraCreditos
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                GasolineraCredito = DatoGasCredito
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
                CargarSlidePanel("AgregarGasCreditoFlyout");
                CargarGasolineraCreditos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarAgregarGasCredito()
        {
            IsFuelOrder = false;
            GasolineraAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            DatoGasCredito = new GasolineraCreditosDTO
            {
                FechaInicio = DateTime.Now
            };
            ObtenerCorrelativoGasCredito();

            CargarSlidePanel("AgregarGasCreditoFlyout");
        }

        private void ObtenerCorrelativoGasCredito()
        {
            var request = new GetGasolineraCreditoUltimo
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                Fecha = DateTime.Now
            };

            var ultimoCorrelativoGasCreditos = _client.Get(request);
            DatoGasCredito.CodigoGasCredito = ultimoCorrelativoGasCreditos.CodigoGasCredito;
            RaisePropertyChanged("DatoGasCredito");
        }

        private void MostrarEditarGasCredito()
        {
            if (GasolineraCreditoSeleccionado == null) return;

            if (GasolineraCreditoSeleccionado.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Editar Créditos de Gasolina en Estado NUEVO");
                return;
            }

            DatoGasCredito = new GasolineraCreditosDTO
            {
                FechaInicio = DateTime.Now,
                Credito = GasolineraCreditoSeleccionado.Credito,
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId,
                GasolineraId = GasolineraCreditoSeleccionado.GasolineraId
            };

            CargarSlidePanel("EditarGasCreditoFlyout");
        }

        private void MostrarEditarOrdenCombustible()
        {
            if (OrdenCombustibleSeleccionado == null) return;

            if (OrdenCombustibleSeleccionado.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede actualizar Ordenes de Combustible Activas");
                return;
            }

            DatoOrdenCombustible = new OrdenesCombustibleDTO
            {
                OrdenCombustibleId = OrdenCombustibleSeleccionado.OrdenCombustibleId,
                AutorizadoPor = OrdenCombustibleSeleccionado.AutorizadoPor,
                BoletaId = OrdenCombustibleSeleccionado.BoletaId,
                CodigoBoleta = OrdenCombustibleSeleccionado.CodigoBoleta,
                CodigoCreditoCombustible = OrdenCombustibleSeleccionado.CodigoCreditoCombustible,
                CodigoFactura = OrdenCombustibleSeleccionado.CodigoFactura,
                PlacaEquipo = OrdenCombustibleSeleccionado.PlacaEquipo,
                Monto = OrdenCombustibleSeleccionado.Monto,
                FechaCreacion = OrdenCombustibleSeleccionado.FechaCreacion,
                Observaciones = OrdenCombustibleSeleccionado.Observaciones,
                EsOrdenPersonal = OrdenCombustibleSeleccionado.EsOrdenPersonal,
                ProveedorId = OrdenCombustibleSeleccionado.ProveedorId,
                Imagen = OrdenCombustibleSeleccionado.Imagen
            };

            CargarSlidePanel("EditarOrdenCombustibleFlyout");
        }

        private void ActualizarOrdenCombustible()
        {
            if (DatoOrdenCombustible.Monto <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Monto de la Orden de Combustible debe ser mayor a 0");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutOrdenesCombustible
            {
                OrdenCombustible = DatoOrdenCombustible,
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
                CargarSlidePanel("EditarOrdenCombustibleFlyout");
                CargarOrdenesCombustible();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void EliminarOrdenCombustible()
        {
            if (OrdenCombustibleSeleccionado == null) return;

            if (OrdenCombustibleSeleccionado.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Ordenes Activas");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, OrdenCombustibleSeleccionado.CodigoFactura),
                Titulo = "Ordenes Combustible"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteOrdenesCombustible
                    {
                        OrdenCombustibleId = OrdenCombustibleSeleccionado.OrdenCombustibleId,
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
                        CargarOrdenesCombustible();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void CargarOrdenesCombustible()
        {
            var ordenCombustibleId = OrdenCombustibleSeleccionado != null ? OrdenCombustibleSeleccionado.OrdenCombustibleId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorOrdenesCombustible
            {
                PaginaActual = NumeroPaginaOrdenesCombustible,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaOrdenesCombustible
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaOrdenesCombustibleControl = res;
                if (BusquedaOrdenesCombustibleControl.Items != null && BusquedaOrdenesCombustibleControl.Items.Any())
                {
                    if (ordenCombustibleId == 0)
                    {
                        OrdenCombustibleSeleccionado = BusquedaOrdenesCombustibleControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        OrdenCombustibleSeleccionado = BusquedaOrdenesCombustibleControl.Items.FirstOrDefault(r => r.OrdenCombustibleId == ordenCombustibleId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarOrdenesCombustiblePorGasCredito()
        {
            listaOrdenesCombustible = new List<OrdenesCombustibleDTO>();
            if (GasolineraCreditoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetOrdenesCombustiblePorGasCreditoId
            {
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                listaOrdenesCombustible = res;
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarPagosGasCreditos()
        {
            ListaGasCreditoPagos = new List<GasolineraCreditoPagosDTO>();

            if (GasolineraCreditoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetGasCreditoPagosPorGasCreditoId
            {
                GasCreditoId = GasolineraCreditoSeleccionado.GasCreditoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaGasCreditoPagos = res;

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

        private void MostrarFacturaciones()
        {
            EsFacturacion = true;
            CargarGasolineraCreditos();
        }

        private void MostrarOrdenesCombustible()
        {
            EsFacturacion = false;
            CargarOrdenesCombustible();
        }

        private void CargarGasolineraCreditos()
        {
            var gasCreditoId = GasolineraCreditoSeleccionado != null ? GasolineraCreditoSeleccionado.GasCreditoId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorGasCreditos
            {
                PaginaActual = NumeroPaginaGasolineraCreditos,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaGasolineraCredito
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaGasolineraCreditosControl = res;
                if (BusquedaGasolineraCreditosControl.Items != null && BusquedaGasolineraCreditosControl.Items.Any())
                {
                    if (gasCreditoId == 0)
                    {
                        GasolineraCreditoSeleccionado = BusquedaGasolineraCreditosControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        GasolineraCreditoSeleccionado = BusquedaGasolineraCreditosControl.Items.FirstOrDefault(r => r.GasCreditoId == gasCreditoId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void BuscarGasolineras(string valorBusqueda)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetGasolinerasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda,
                ConGasCredito = false
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    ListaGasolinerasAutoComplete = (from reg in res
                                                    select new AutoCompleteEntry(reg.Descripcion, reg.GasolineraId)).ToList();
                }
            }, (r, ex) =>
            {
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

        private void CargarDatosIniciales()
        {
            CargarOrdenesCombustible();
            CargarBancos();
            CargarFormarDePago();
        }

        private void CargarDatosPruebas()
        {
            BusquedaOrdenesCombustibleControl = new BusquedaOrdenesCombustibleDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaOrdenesCombustible()
            };
            OrdenCombustibleSeleccionado = BusquedaOrdenesCombustibleControl.Items.FirstOrDefault();

            BusquedaGasolineraCreditosControl = new BusquedaGasolineraCreditosDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaGasolineraCreditos()
            };
            GasolineraCreditoSeleccionado = BusquedaGasolineraCreditosControl.Items.FirstOrDefault();
        }
    }
}
