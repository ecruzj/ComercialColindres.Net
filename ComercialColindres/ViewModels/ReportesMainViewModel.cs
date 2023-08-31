using ComercialColindres.Clases;
using ComercialColindres.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using ComercialColindres.Views.ReportViews;
using System;
using System.Collections.ObjectModel;
using ComercialColindres.Modelos;
using System.Collections.Generic;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ServiceStack.ServiceClient.Web;
using ComercialColindres.Enumeraciones;
using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.Recursos;
using ComercialColindres.ViewModels.Reportes;
using ComercialColindres.CrossViewModel;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;

namespace ComercialColindres.ViewModels
{
    public class ReportesMainViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public ReportesMainViewModel(IServiciosComunes serviciosComunes)
        {
            TituloPantalla = "Reportes Comercial Colindres";
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
            CargarBancos();
            CargarFormarDePago();
        }

        public override void CambiarTitulo()
        {
            TituloPantalla = "Reportes Comercial Colindres";
        }

        public UIElement ReporteView
        {
            get
            {
                return _reporteView;
            }
            set
            {
                _reporteView = value;
                RaisePropertyChanged("ReporteView");
            }
        }
        UIElement _reporteView;

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
                    RaisePropertyChanged(nameof(ListaBoletaCierresModel));
                }
            }
        }
        private ObservableCollection<BoletaCierresModel> _listaBoletaCierresModel;

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
                    RaisePropertyChanged(nameof(DatoBoletaCierre));
                }
            }
        }
        private BoletaCierresModel _datoBoletaCierre;

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
                    RaisePropertyChanged(nameof(BoletaCierreSeleccionado));
                }
            }
        }
        private BoletaCierresModel _boletaCierreSeleccionado;

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

                if (DatoBoletaCierre != null)
                {
                    DatoBoletaCierre.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                    DatoBoletaCierre.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
                }
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;
        
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
        
        public RptBoletaPaymentPendingResumenDto BoletaPaymentPending
        {
            get { return _boletaPaymentPending; }
            set
            {
                _boletaPaymentPending = value;
                RaisePropertyChanged(nameof(BoletaPaymentPending));
            }
        }
        private RptBoletaPaymentPendingResumenDto _boletaPaymentPending;

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
                }

                if (DatoBoletaCierre != null && !DatoBoletaCierre.EsEdicion)
                {
                    DatoBoletaCierre = new BoletaCierresModel
                    {
                        EsEdicion = false,
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
        
        public bool BoletasWithError
        {
            get { return _boletasWithError; }
            set
            {
                _boletasWithError = value;
                RaisePropertyChanged(nameof(BoletasWithError));
            }
        }
        private bool _boletasWithError;
        
        public bool IsCierreMasivo
        {
            get { return _isCierreMasivo; }
            set
            {
                _isCierreMasivo = value;
                RaisePropertyChanged(nameof(IsCierreMasivo));
            }
        }
        private bool _isCierreMasivo;

        public List<BoletaCierresDTO> BoletaPendingsWithError
        {
            get { return _boletaPendingsWithError; }
            set
            {
                _boletaPendingsWithError = value;
                RaisePropertyChanged(nameof(BoletaPendingsWithError));
            }
        }
        private List<BoletaCierresDTO> _boletaPendingsWithError;

        public RelayCommand ComandoMostrarPrestamosPoProveedor { get; set; }
        public RelayCommand ComandoMostrarBoletasPoProveedor { get; set; }
        public RelayCommand ComandoMostrarPrestamosPendientes { get; set; }
        public RelayCommand ComandoMostrarCompraBiomasaResumen { get; set; }
        public RelayCommand ComandoMostrarCompraBiomasaDetalle { get; set; }

        public RelayCommand CommandShowBoletasPaymentPending { get; set; }
        public RelayCommand CommandShowBoletaMasiveClose { get; set; }
        public RelayCommand ComandoAgregarPagoBoleta { get; set; }
        public RelayCommand CommandHoleTotalToPayment { get; set; }
        public RelayCommand ComandoRemoverPagoBoleta { get; set; }
        public RelayCommand ComandoGuardarPagosBoleta { get; set; }

        public RelayCommand CommandShowHumidityPendingPayment { get; set; }
        public RelayCommand CommandShowPendingInvoice { get; set; }
        public RelayCommand CommandShowBoletasWithoutInvoice { get; set; }
        public RelayCommand CommandShowBillsWithWeightsError { get; set; }
        public RelayCommand CommandShowHistoricalOfInvoicesBalances { get; set; }

        private void InicializarComandos()
        {
            ComandoMostrarPrestamosPoProveedor = new RelayCommand(MostrarPrestamosPorProveedor);
            ComandoMostrarBoletasPoProveedor = new RelayCommand(MostrarBoletasPorProveedor);
            ComandoMostrarPrestamosPendientes = new RelayCommand(MostrarPrestamosPendientes);
            ComandoMostrarCompraBiomasaResumen = new RelayCommand(MostrarCompraBiomasaResumen);
            ComandoMostrarCompraBiomasaDetalle = new RelayCommand(MostrarCompraBiomasaDetalle);

            CommandShowBoletasPaymentPending = new RelayCommand(ShowBoletasPaymentPending);
            CommandShowBoletaMasiveClose = new RelayCommand(ShowBoletaMasiveClose);
            ComandoAgregarPagoBoleta = new RelayCommand(AgregarItemPagoBoleta);
            CommandHoleTotalToPayment = new RelayCommand(HoleTotalToPayment);
            ComandoRemoverPagoBoleta = new RelayCommand(RemoverItemPagoBoleta);
            ComandoGuardarPagosBoleta = new RelayCommand(GuardarPagosBoleta);

            CommandShowHumidityPendingPayment = new RelayCommand(ShowHumidityPendingPayment);
            CommandShowPendingInvoice = new RelayCommand(ShowPendingInvoice);
            CommandShowBoletasWithoutInvoice = new RelayCommand(ShowBoletasWithoutInvoice);
            CommandShowBillsWithWeightsError = new RelayCommand(ShowBillsWithWeightsError);
            CommandShowHistoricalOfInvoicesBalances = new RelayCommand(ShowHistoricalOfInvoicesBalances);
        }

        private void ShowHistoricalOfInvoicesBalances()
        {
            ReporteView = new RptHistoryOfInvoiceBalancesView();
        }

        private void HoleTotalToPayment()
        {
            string mensajeValidacion = ValidarCierreBoleta(true);

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            decimal totalAmount = BoletaPaymentPending.SaldoPendiente;
            decimal payments = ListaBoletaCierresModel.Sum(t => t.Monto);

            if (payments > 0)
            {
                DatoBoletaCierre.Monto = totalAmount - payments;
                return;
            }

            DatoBoletaCierre.Monto = totalAmount;
        }

        private void ShowBillsWithWeightsError()
        {
            ReporteView = new RptBillsWithWeightsErrorView();
        }

        private void ShowBoletasWithoutInvoice()
        {
            ReporteView = new RptBoletasWithoutInvoiceView();
        }

        private void ShowPendingInvoice()
        {
            ReporteView = new RptPendingInvoiceView();
        }

        private void ShowHumidityPendingPayment()
        {
            ReporteView = new RptHumidityPendingPaymentView();
        }

        private void RemoverItemPagoBoleta()
        {
            if (BoletaCierreSeleccionado == null) return;
            
            ListaBoletaCierresModel.Remove(BoletaCierreSeleccionado);
            DatoBoletaCierre = new BoletaCierresModel { CantidadJustificada = ListaBoletaCierresModel.Sum(j => j.Monto) };
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

        private void GuardarPagosBoleta()
        {
            BoletaPendingsWithError = new List<BoletaCierresDTO>();
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
            var request = new CloseBoletaMasive
            {
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo(),
                VendorId = BoletaPaymentPending.VendorId,
                IsPartialPayment = BoletaPaymentPending.IsPartialPayment(),
                BoletasToPay = BoletaPaymentPending.GetBoletasToPay(),
                BoletaCierres = cierresBoleta.ToList()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.PutAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                if ((res.Any()))
                {
                    BoletaPendingsWithError = new List<BoletaCierresDTO>(res);
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "Existen Boletas con errores");
                    BoletasWithError = true;
                    return;
                }

                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("CierresBoletaFlyout");
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarItemPagoBoleta()
        {
            string mensajeValidacion = ValidarCierreBoleta();

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
            var totalAPagar = Math.Round(BoletaPaymentPending.SaldoPendiente, 2);

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
                itemPagoBoleta.NoDocumento = DatoBoletaCierre.NoDocumento;
                itemPagoBoleta.EsEdicion = true;
            }

            DatoBoletaCierre = new BoletaCierresModel
            {
                CantidadJustificada = ListaBoletaCierresModel.Sum(c => c.Monto),
                FechaPago = DateTime.Now
            };

            BancoSeleccionado = new BancosDTO();
            LineaCreditoSeleccionada = new LineasCreditoDTO();
        }

        private string ValidarCierreBoleta(bool isHolePayment = false)
        {
            if (string.IsNullOrWhiteSpace(DatoBoletaCierre.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }

            if (!isHolePayment && DatoBoletaCierre.Monto <= 0)
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

            if (DatoBoletaCierre.FechaPago == DateTime.MinValue)
            {
                return "Debe seleccionar la fecha de pago";
            }

            if (DatoBoletaCierre.LineaCreditoId == 0)
            {
                return "Debe Seleccionar una Linea de Crédito";
            }

            return string.Empty;
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

        private void ShowBoletaMasiveClose()
        {
            BoletaPaymentPending = BoletasPendingPaymentInfo.BoletaPendingPaymentInfo;

            if (BoletaPaymentPending == null || BoletaPaymentPending.VendorId == 0) return;

            ListaBoletaCierresModel = new ObservableCollection<BoletaCierresModel>();
            DatoBoletaCierre = new BoletaCierresModel { FechaPago = DateTime.Now };
            BoletasWithError = false;
            BoletaPendingsWithError = new List<BoletaCierresDTO>();

            if (BoletaPaymentPending.SaldoPendiente <= 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe saldo pendiente!");
                return;
            }

            BoletaPaymentPending.GetValuesForPayment();
            RaisePropertyChanged(nameof(BoletaPaymentPending));
            CargarSlidePanel("CierresBoletaFlyout");
        }

        private void ShowBoletasPaymentPending()
        {
            ShowCierreMasivo(true);
            ReporteView = new RptBoletasPaymentPendingView();            
        }

        private void ShowCierreMasivo(bool value = false)
        {
            IsCierreMasivo = value;
        }

        private void MostrarCompraBiomasaDetalle()
        {
            ReporteView = new RptCompraBiomasaDetalleView();
        }

        private void MostrarCompraBiomasaResumen()
        {
            ReporteView = new RptCompraProductoBiomasaResumenView();
        }

        private void MostrarPrestamosPendientes()
        {
            ReporteView = new RptPrestamosPendientesView();
        }

        private void MostrarPrestamosPorProveedor()
        {
            ReporteView = new RptPrestamosPorProveedorView();
        }

        private void MostrarBoletasPorProveedor()
        {
            ReporteView = new RptBoletasPorProveedorView();
        }
    }
}
