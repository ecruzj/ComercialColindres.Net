using ComercialColindres.Busquedas;
using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels
{
    public class PrestamosViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public PrestamosViewModel(IServiciosComunes serviciosComunes)
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

        public BusquedaPrestamosDTO BusquedaPrestamosControl
        {
            get
            {
                return _busquedaPrestamosControl;
            }
            set
            {
                _busquedaPrestamosControl = value;
                RaisePropertyChanged("BusquedaPrestamosControl");
            }
        }
        private BusquedaPrestamosDTO _busquedaPrestamosControl;
        
        public PrestamosDTO PrestamoSeleccionado
        {
            get
            {
                return _prestamoSeleccionado;
            }
            set
            {
                _prestamoSeleccionado = value;
                RaisePropertyChanged("PrestamoSeleccionado");

                if (IsInDesignMode)
                {
                    return;
                }

                CargarPrestamoTransferencias();
                CargarAbonosPrestamo();
            }
        }
        private PrestamosDTO _prestamoSeleccionado;
        
        public string ValorBusquedaPrestamo
        {
            get
            {
                return _valorBusquedaPrestamo;
            }
            set
            {
                _valorBusquedaPrestamo = value;
                RaisePropertyChanged("ValorBusquedaPrestamo");
            }
        }
        private string _valorBusquedaPrestamo;

        public PrestamosDTO DatoPrestamo
        {
            get
            {
                return _datoPrestamo;
            }
            set
            {
                _datoPrestamo = value;
                RaisePropertyChanged("DatoPrestamo");
            }
        }
        private PrestamosDTO _datoPrestamo;

        public BusquedaProveedores ControlBusquedaProveedores
        {
            get
            {
                return _controlBusquedaProveedores;
            }
            set
            {
                _controlBusquedaProveedores = value;
                RaisePropertyChanged("ControlBusquedaProveedores");
            }
        }
        private BusquedaProveedores _controlBusquedaProveedores;
        
        public List<PrestamosTransferenciasDTO> ListaPrestamosTransferencias
        {
            get
            {
                return _listaPrestamosTransferencias;
            }
            set
            {
                if (_listaPrestamosTransferencias != value)
                {
                    _listaPrestamosTransferencias = value;
                    RaisePropertyChanged("ListaPrestamosTransferencias");
                }
            }
        }
        private List<PrestamosTransferenciasDTO> _listaPrestamosTransferencias;
        
        public ObservableCollection<PrestamoTransferenciasModel> ListaPrestamosTransferenciasModel
        {
            get
            {
                return _listaPrestamosTransferenciasModel;
            }
            set
            {
                _listaPrestamosTransferenciasModel = value;
                RaisePropertyChanged("ListaPrestamosTransferenciasModel");
            }
        }
        private ObservableCollection<PrestamoTransferenciasModel> _listaPrestamosTransferenciasModel;

        public PrestamoTransferenciasModel PrestamoTransferenciaSeleccionada
        {
            get
            {
                return _prestamoTransaferenciaSeleccionada;
            }
            set
            {
                _prestamoTransaferenciaSeleccionada = value;
                RaisePropertyChanged("PrestamoTransferenciaSeleccionada");
            }
        }
        private PrestamoTransferenciasModel _prestamoTransaferenciaSeleccionada;
        
        public PrestamoTransferenciasModel DatoPrestamoTransferencia
        {
            get
            {
                return _datoPrestamoTransferencia;
            }
            set
            {
                _datoPrestamoTransferencia = value;
                RaisePropertyChanged("DatoPrestamoTransferencia");
            }
        }
        private PrestamoTransferenciasModel _datoPrestamoTransferencia;

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
                DatoPrestamoTransferencia.BancoId = BancoSeleccionado.BancoId;
                DatoPrestamoTransferencia.NombreBanco = BancoSeleccionado.Descripcion;

                if (FormaDePagoSeleccionada != null && FormaDePagoSeleccionada.RequiereBanco)
                {
                    CargarLineasDeCreditoPorBanco(BancoSeleccionado.BancoId, FormaDePagoSeleccionada.CuentaFinancieraTipoId);
                }
                
            }
        }
        private BancosDTO _bancoSeleccionado;
        
        public BancosDTO BancoDelAbono
        {
            get { return _bancoDelAbono; }
            set
            {
                _bancoDelAbono = value;
                RaisePropertyChanged("BancoDelAbono");

                if (BancoDelAbono == null) return;
                DatoOtroAbonoPrestamo.BancoId = BancoDelAbono.BancoId;
                DatoOtroAbonoPrestamo.NombreBanco = BancoDelAbono.Descripcion;
            }
        }
        private BancosDTO _bancoDelAbono;

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

                if (!DatoPrestamoTransferencia.EsEdicion)
                {
                    DatoPrestamoTransferencia = new PrestamoTransferenciasModel
                    {
                        EsEdicion = false,
                        PrestamoId = PrestamoSeleccionado.PrestamoId,
                        CantidadJustificada = ListaPrestamosTransferenciasModel.Sum(c => c.Monto)
                    };

                    LineasCredito = new List<LineasCreditoDTO>();
                    LineaCreditoSeleccionada = new LineasCreditoDTO();
                    BancoSeleccionado = new BancosDTO();
                }
            }
        }
        private CuentasFinancieraTiposDTO _formaDePagoSeleccionada;
        
        public CuentasFinancieraTiposDTO FormaDeAbonoSeleccionada
        {
            get { return _formaDeAbonoSeleccionada; }
            set
            {
                _formaDeAbonoSeleccionada = value;
                RaisePropertyChanged("FormaDeAbonoSeleccionada");
            }
        }
        private CuentasFinancieraTiposDTO _formaDeAbonoSeleccionada;

        public List<PagoPrestamosDTO> ListaPagoPrestamos
        {
            get
            {
                return _listaPagoPrestamos;
            }
            set
            {
                _listaPagoPrestamos = value;
                RaisePropertyChanged("ListaPagoPrestamos");
            }
        }
        private List<PagoPrestamosDTO> _listaPagoPrestamos;

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
                DatoPrestamoTransferencia.LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId;
                DatoPrestamoTransferencia.CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito + " - " + LineaCreditoSeleccionada.CuentaFinanciera;
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;
        
        public List<ReporteBoletaDeduccionesPrestamoDTO> BoletaDeduccionesPrestamo
        {
            get { return _boletaDeduccionesPrestamo; }
            set
            {
                _boletaDeduccionesPrestamo = value;
                RaisePropertyChanged("BoletaDeduccionesPrestamo");
            }
        }
        private List<ReporteBoletaDeduccionesPrestamoDTO> _boletaDeduccionesPrestamo;

        public ObservableCollection<PagoPrestamoModel> AbonosPrestamoPorBoletasModel
        {
            get
            {
                return _abonosPrestamoPorBoletasModel;
            }
            set
            {
                _abonosPrestamoPorBoletasModel = value;
                RaisePropertyChanged("AbonosPrestamoPorBoletasModel");
            }
        }
        private ObservableCollection<PagoPrestamoModel> _abonosPrestamoPorBoletasModel;
        
        public PagoPrestamoModel AbonoPorBoletaSeleccionado
        {
            get { return _abonoPorBoletaSeleccionado; }
            set
            {
                _abonoPorBoletaSeleccionado = value;
                RaisePropertyChanged("AbonoPorBoletaSeleccionado");
            }
        }
        private PagoPrestamoModel _abonoPorBoletaSeleccionado;

        public ObservableCollection<PagoPrestamoModel> OtrosAbonosPrestamo
        {
            get { return _otrosAbonosPrestamo; }
            set
            {
                _otrosAbonosPrestamo = value;
                RaisePropertyChanged("OtrosAbonosPrestamo");
            }
        }
        private ObservableCollection<PagoPrestamoModel> _otrosAbonosPrestamo;
        
        public PagoPrestamoModel OtroAbonoSeleccionado
        {
            get { return _otroAbonoSeleccionado; }
            set
            {
                _otroAbonoSeleccionado = value;
                RaisePropertyChanged("OtroAbonoSeleccionado");
            }
        }
        private PagoPrestamoModel _otroAbonoSeleccionado;

        public PagoPrestamoModel DatoAbonoPrestamo
        {
            get
            {
                return _datoAbonoPrestamo;
            }
            set
            {
                _datoAbonoPrestamo = value;
                RaisePropertyChanged("DatoAbonoPrestamo");
            }
        }
        private PagoPrestamoModel _datoAbonoPrestamo;
        
        public PagoPrestamoModel DatoOtroAbonoPrestamo
        {
            get { return _datoOtroAbonoPrestamo; }
            set
            {
                _datoOtroAbonoPrestamo = value;
                RaisePropertyChanged("DatoOtroAbonoPrestamo");
            }
        }
        private PagoPrestamoModel _datoOtroAbonoPrestamo;

        public bool EsAbonoPorBoleta
        {
            get { return _esAbonoPorBoleta; }
            set
            {
                _esAbonoPorBoleta = value;
                RaisePropertyChanged("EsAbonoPorBoleta");
            }
        }
        private bool _esAbonoPorBoleta;

        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoNavegar { get; set; }
        public RelayCommand ComandoMostrarAgregar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }
        public RelayCommand ComandoActivarPrestamo { get; set; }
        public RelayCommand ComandoObtenerCorrelativoPrestamo { get; set; }

        public RelayCommand ComandoMostrarPrestamosTransferencias { get; set; }
        public RelayCommand ComandoRemoverTransferencia { get; set; }
        public RelayCommand ComandoEditarTransferencia { get; set; }
        public RelayCommand ComandoAgregarTransferencia { get; set; }
        public RelayCommand ComandoGuardarTransferenciasPrestamo { get; set; }

        public RelayCommand ComandoMostrarBusquedaProveedores { get; set; }
        public RelayCommand ComandoBuscarProveedores { get; set; }
        public RelayCommand<object> ComandoSeleccionaProveedor { get; set; }

        public RelayCommand ComandoMostrarAbonosPrestamo { get; set; }
        public RelayCommand<bool> ComandoIndicarTipoAbono { get; set; }
        public RelayCommand ComandoAgregarAbonoPorBoleta { get; set; }
        public RelayCommand ComandoRemoverAbonoPorBoleta { get; set; }
        public RelayCommand ComandoActualizarAbonoPorBoleta { get; set; }
        public RelayCommand ComandoGuardarAbonosPorBoletas { get; set; }

        public RelayCommand ComandoAgregarOtroAbono { get; set; }
        public RelayCommand ComandoRemoverOtroAbono { get; set; }
        public RelayCommand ComandoActualizarOtroAbono { get; set; }
        public RelayCommand ComandoGuardarOtrosAbonos { get; set; }        

        public RelayCommand ComandoImprimirDetallePrestamo { get; set; }

        private void InicializarComandos()
        {
            ComandoBuscar = new RelayCommand(BuscarPrestamo);
            ComandoEditar = new RelayCommand(EditarPrestamo);
            ComandoMostrarAgregar = new RelayCommand(MostrarAgregarPrestamo);
            ComandoCrear = new RelayCommand(CrearPrestamo);
            ComandoMostrarEditar = new RelayCommand(MostrarEditarPrestamo);
            ComandoEliminar = new RelayCommand(EliminarPrestamo);
            ComandoRefrescar = new RelayCommand(Navegar);
            ComandoNavegar = new RelayCommand(Navegar);
            ComandoActivarPrestamo = new RelayCommand(ActivarPrestamo);
            ComandoObtenerCorrelativoPrestamo = new RelayCommand(ObtenerCorrelativoPrestamo);

            ComandoMostrarPrestamosTransferencias = new RelayCommand(MostrarPrestamosTransferencias);
            ComandoRemoverTransferencia = new RelayCommand(RemoverItemTransferencia);
            ComandoEditarTransferencia = new RelayCommand(EditarItemTransferencia);
            ComandoAgregarTransferencia = new RelayCommand(AgregarItemTransferencia);
            ComandoGuardarTransferenciasPrestamo = new RelayCommand(GuardarTransferenciasPrestamo);

            ComandoMostrarBusquedaProveedores = new RelayCommand(MostrarBusquedaProveedores);
            ComandoBuscarProveedores = new RelayCommand(BuscarProveedores);
            ComandoSeleccionaProveedor = new RelayCommand<object>(SeleccionaProveedor);

            ComandoMostrarAbonosPrestamo = new RelayCommand(MostrarAbonosPrestamo);
            ComandoIndicarTipoAbono = new RelayCommand<bool>(ActualizarFormaDeAbono);
            ComandoRemoverAbonoPorBoleta = new RelayCommand(RemoverAbonoPorBoleta);
            ComandoActualizarAbonoPorBoleta = new RelayCommand(ActualizarAbonoPorBoleta);
            ComandoAgregarAbonoPorBoleta = new RelayCommand(AgregarItemAbonoPorBoleta);
            ComandoGuardarAbonosPorBoletas = new RelayCommand(GuardarAbonosPorBoletas);

            ComandoAgregarOtroAbono = new RelayCommand(AgregarOtroAbono);
            ComandoRemoverOtroAbono = new RelayCommand(RemoverOtroAbono);
            ComandoActualizarOtroAbono = new RelayCommand(ActualizarOtroAbono);
            ComandoGuardarOtrosAbonos = new RelayCommand(GuardarOtrosAbonos);

            ComandoImprimirDetallePrestamo = new RelayCommand(ImprimirBoletaDeduccionesPrestamo);
        }

        private void GuardarOtrosAbonos()
        {
            var otrosAbonos = from reg in OtrosAbonosPrestamo
                                   select new PagoPrestamosDTO
                                   {
                                       PagoPrestamoId = reg.PagoPrestamoId,
                                       PrestamoId = reg.PrestamoId,
                                       MontoAbono = reg.MontoAbono,
                                       FormaDePago = reg.FormaDePago,
                                       BancoId = reg.BancoId,
                                       NoDocumento = reg.NoDocumento,
                                       Observaciones = reg.Observaciones
                                   };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostOtrosAbonosPrestamo
            {
                PagoPrestamos = otrosAbonos.ToList(),
                PrestamoId = PrestamoSeleccionado.PrestamoId,
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
                CargarSlidePanel("AbonosPrestamoFlyout");
                CargarPrestamos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ActualizarOtroAbono()
        {
            if (OtroAbonoSeleccionado == null) return;

            DatoOtroAbonoPrestamo = new PagoPrestamoModel
            {
                PagoPrestamoId = OtroAbonoSeleccionado.PagoPrestamoId,
                BancoId = OtroAbonoSeleccionado.BancoId,
                NoDocumento = OtroAbonoSeleccionado.NoDocumento,
                MontoAbono = OtroAbonoSeleccionado.MontoAbono,
                FormaDePago = OtroAbonoSeleccionado.FormaDePago,
                Observaciones = OtroAbonoSeleccionado.Observaciones
            };                     
        }

        private void RemoverOtroAbono()
        {
            if (OtroAbonoSeleccionado == null) return;

            OtrosAbonosPrestamo.Remove(OtroAbonoSeleccionado);
            SetOtroAbono();
        }

        private void AgregarOtroAbono()
        {
            var mensajeValidacion = ValidarOtroAbono();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemOtroAbono = OtrosAbonosPrestamo.FirstOrDefault(o => o.PagoPrestamoId == DatoOtroAbonoPrestamo.PagoPrestamoId && o.FormaDePago == DatoOtroAbonoPrestamo.FormaDePago && o.BancoId == DatoOtroAbonoPrestamo.BancoId);

            if (itemOtroAbono == null)
            {
                OtrosAbonosPrestamo.Add(DatoOtroAbonoPrestamo);
            }
            else
            {
                itemOtroAbono.MontoAbono = DatoOtroAbonoPrestamo.MontoAbono;
                itemOtroAbono.BancoId = DatoOtroAbonoPrestamo.BancoId;
                itemOtroAbono.FormaDePago = DatoOtroAbonoPrestamo.FormaDePago;
                itemOtroAbono.Observaciones = DatoOtroAbonoPrestamo.Observaciones;
            }

            SetOtroAbono();
        }

        private void SetOtroAbono()
        {
            DatoOtroAbonoPrestamo = new PagoPrestamoModel();
            DatoOtroAbonoPrestamo.TotalAbono = OtrosAbonosPrestamo.Sum(a => a.MontoAbono) + AbonosPrestamoPorBoletasModel.Sum(a => a.MontoAbono);
            DatoOtroAbonoPrestamo.SaldoPendiente = PrestamoSeleccionado.TotalACobrar - DatoOtroAbonoPrestamo.TotalAbono;
        }

        private string ValidarOtroAbono()
        {
            if (DatoOtroAbonoPrestamo.MontoAbono <= 0)
            {
                return "Debe ingresar un monto mayor a 0";
            }

            if (string.IsNullOrWhiteSpace(DatoOtroAbonoPrestamo.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }

            if (FormaDeAbonoSeleccionada.RequiereBanco)
            {
                if (DatoOtroAbonoPrestamo.BancoId == 0)
                {
                    return "Debe seleccionar el un Banco";
                }

                if (string.IsNullOrWhiteSpace(DatoOtroAbonoPrestamo.NoDocumento))
                {
                    return "Debe ingresar el Número del Abono";
                }
            }

            return string.Empty;
        }

        private void GuardarAbonosPorBoletas()
        {
            var abonosPorBoletas = from reg in AbonosPrestamoPorBoletasModel
                                   select new PagoPrestamosDTO
                                   {
                                       PagoPrestamoId = reg.PagoPrestamoId,
                                       PrestamoId = reg.PrestamoId,
                                       BoletaId = reg.BoletaId,
                                       MontoAbono = reg.MontoAbono,
                                       FormaDePago = reg.FormaDePago                                      
                                   };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutAbonosPrestamoPorBoletas
            {
                PagoPrestamos = abonosPorBoletas.ToList(),
                PrestamoId = PrestamoSeleccionado.PrestamoId,
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
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                CargarSlidePanel("AbonosPrestamoFlyout");
                CargarPrestamos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarItemAbonoPorBoleta()
        {
            var abonoPorBoleta = AbonosPrestamoPorBoletasModel.FirstOrDefault(a => a.PagoPrestamoId == DatoAbonoPrestamo.PagoPrestamoId);

            if (abonoPorBoleta == null) return;

            abonoPorBoleta.MontoAbono = DatoAbonoPrestamo.MontoAbono;
            SetDatoPrestamo();
        }

        private void ActualizarAbonoPorBoleta()
        {
            if (AbonoPorBoletaSeleccionado == null) return;

            if (!AbonoPorBoletaSeleccionado.PuedeEditarAbonoPorBoleta)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la Boleta debe ser ACTIVO");
                return;
            }

            SetDatoPrestamo();
            DatoAbonoPrestamo.PagoPrestamoId = AbonoPorBoletaSeleccionado.PagoPrestamoId;
            DatoAbonoPrestamo.BoletaId = AbonoPorBoletaSeleccionado.BoletaId;
            DatoAbonoPrestamo.CodigoBoleta = AbonoPorBoletaSeleccionado.CodigoBoleta;
            DatoAbonoPrestamo.MontoAbono = AbonoPorBoletaSeleccionado.MontoAbono;       

            RaisePropertyChanged("DatoAbonoPrestamo");
        }

        private void RemoverAbonoPorBoleta()
        {
            if (AbonoPorBoletaSeleccionado == null) return;
            
            if (!AbonoPorBoletaSeleccionado.PuedeEditarAbonoPorBoleta)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la Boleta debe ser ACTIVO");
                return;
            }

            AbonosPrestamoPorBoletasModel.Remove(AbonoPorBoletaSeleccionado);
            SetDatoPrestamo();
        }

        private void ActualizarFormaDeAbono(bool esAbonoPorBoleta)
        {
            EsAbonoPorBoleta = esAbonoPorBoleta;
        }        

        private void MostrarAbonosPrestamo()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Préstamo debe estar Activo");
                return;
            }
            
            CargarOtrosAbonos();
            CargarAbonosPorBoletas();

            EsAbonoPorBoleta = true;

            DatoAbonoPrestamo = new PagoPrestamoModel
            {
                TotalAbono = PrestamoSeleccionado.TotalAbono,
                SaldoPendiente = PrestamoSeleccionado.SaldoPendiente
            };
            RaisePropertyChanged("DatoAbonoPrestamo");

            DatoOtroAbonoPrestamo = new PagoPrestamoModel
            {
                TotalAbono = PrestamoSeleccionado.TotalAbono,
                SaldoPendiente = PrestamoSeleccionado.SaldoPendiente
            };
            RaisePropertyChanged("DatoOtroAbonoPrestamo");

            CargarSlidePanel("AbonosPrestamoFlyout");
        }

        private void SetDatoPrestamo()
        {
            DatoAbonoPrestamo = new PagoPrestamoModel();
            DatoAbonoPrestamo.TotalAbono = OtrosAbonosPrestamo.Sum(a => a.MontoAbono) + AbonosPrestamoPorBoletasModel.Sum(a => a.MontoAbono);
            DatoAbonoPrestamo.SaldoPendiente = PrestamoSeleccionado.TotalACobrar - DatoAbonoPrestamo.TotalAbono;

            RaisePropertyChanged("DatoAbonoPrestamo");
        }

        private void CargarOtrosAbonos()
        {
            OtrosAbonosPrestamo = new ObservableCollection<PagoPrestamoModel>();

            if (PrestamoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPagoPrestamos
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                FiltrarAbonosPorBoletas = false,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                var datos = from reg in res
                            select new PagoPrestamoModel
                            {
                                PagoPrestamoId = reg.PagoPrestamoId,                                
                                BancoId = reg.BancoId,
                                NoDocumento = reg.NoDocumento,
                                MontoAbono = reg.MontoAbono,
                                FormaDePago = reg.FormaDePago,
                                Observaciones = reg.Observaciones
                            };

                OtrosAbonosPrestamo = new ObservableCollection<PagoPrestamoModel>(datos);
                RaisePropertyChanged("OtrosAbonosPrestamo");
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarAbonosPorBoletas()
        {
            AbonosPrestamoPorBoletasModel = new ObservableCollection<PagoPrestamoModel>();

            if (PrestamoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPagoPrestamos
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                FiltrarAbonosPorBoletas = true,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                var abonosPorBoletas = from reg in res
                                       select new PagoPrestamoModel
                                       {
                                           BoletaId = reg.BoletaId,
                                           CodigoBoleta = reg.CodigoBoleta,
                                           PlantaDestino = reg.PlantaDestino,
                                           CodigoPrestamo = reg.CodigoPrestamo,
                                           PagoPrestamoId = reg.PagoPrestamoId,
                                           PrestamoId = reg.PrestamoId,
                                           MontoAbono = reg.MontoAbono,
                                           FormaDePago = reg.FormaDePago,
                                           PuedeEditarAbonoPorBoleta = reg.PuedeEditarAbonoPorBoleta
                                       };

                AbonosPrestamoPorBoletasModel = new ObservableCollection<PagoPrestamoModel>(abonosPorBoletas);
                RaisePropertyChanged("AbonosPrestamoPorBoletasModel");
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void ImprimirBoletaDeduccionesPrestamo()
        {
            ObtenerInformacionReporte();
        }

        private void ObtenerInformacionReporte()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado == Estados.NUEVO || PrestamoSeleccionado.Estado == Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Estado Inválido del Prestamo");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetBoletaDeduccionesPrestamo
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId
            };
            _client.GetAsync(request, res =>
            {
                BoletaDeduccionesPrestamo = new List<ReporteBoletaDeduccionesPrestamoDTO>(res);

                if (!BoletaDeduccionesPrestamo.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No Existe Infomacion que Mostrar");
                    return;
                }
                ImprimirReporteBoletaDeducciones("RptBoletasDeduccionesPrestamo.rdlc");
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));            
        }

        private void ImprimirReporteBoletaDeducciones(string nombreReporte)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte de Productos";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = nombreReporte;
                manejadorReporte.Datos = BoletaDeduccionesPrestamo;
                manejadorReporte.NombreDataSet = "ds_BoletaDeducciones";
                //manejadorReporte.AgregarParametro("Sucursal", Sucursal.Nombre);
                manejadorReporte.MostarReporte();
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

        private void ActivarPrestamo()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado != Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede activar Prestamos EnProceso");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutActivarPrestamo
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                UserId = InformacionSistema.UsuarioActivo.Usuario
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.PutAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;

                if (!string.IsNullOrWhiteSpace(res.MensajeError))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarPrestamos();

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });

        }

        private void CargarAbonosPrestamo()
        {
            ListaPagoPrestamos = new List<PagoPrestamosDTO>();

            if (PrestamoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPagoPrestamos
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                FiltrarAbonosPorBoletas = false,
                MostrarTodosLosAbonos = true,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaPagoPrestamos = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void GuardarTransferenciasPrestamo()
        {
            var  prestamoTransferencias = from reg in ListaPrestamosTransferenciasModel
                                          select new PrestamosTransferenciasDTO
                                          {
                                              LineaCreditoId = reg.LineaCreditoId,
                                              BancoId = reg.BancoId,
                                              NombreBanco = reg.NombreBanco,
                                              FormaDePago = reg.FormaDePago,
                                              Monto = reg.Monto,
                                              NoDocumento = reg.NoDocumento,
                                              PrestamoId = reg.PrestamoId,
                                              PrestamoTransferenciaId = reg.PrestamoTransferenciaId,
                                              EstaEditandoRegistro = reg.EsEdicion
                                          };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostPrestamoTransferencias
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                PrestamoTransferencias = prestamoTransferencias.ToList()
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
                CargarSlidePanel("TransferenciasPrestamoFlyout");
                CargarPrestamos();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarItemTransferencia()
        {
            var mensajeValidacion = ValidarPrestamoTransferencia();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemTransferencia = ListaPrestamosTransferenciasModel.FirstOrDefault(t => t.PrestamoTransferenciaId == DatoPrestamoTransferencia.PrestamoTransferenciaId &&
                                                                        t.LineaCreditoId == DatoPrestamoTransferencia.LineaCreditoId &&
                                                                        t.FormaDePago == DatoPrestamoTransferencia.FormaDePago &&
                                                                        t.NoDocumento == DatoPrestamoTransferencia.NoDocumento);

            var montoJustificado = DatoPrestamoTransferencia.CantidadJustificada;

            if (itemTransferencia == null)
            {
                montoJustificado += DatoPrestamoTransferencia.Monto;
            }
            else
            {
                montoJustificado = (montoJustificado - PrestamoTransferenciaSeleccionada.Monto) + DatoPrestamoTransferencia.Monto;
            }           

            if (montoJustificado > PrestamoSeleccionado.MontoPrestamo)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, string.Format("La Justificación de Transferencia L. {0} supera el total del Prestamo L. {1}", montoJustificado, PrestamoSeleccionado.MontoPrestamo));
                return;
            }

            if (itemTransferencia == null)
            {   
                ListaPrestamosTransferenciasModel.Add(DatoPrestamoTransferencia);
            }
            else
            {
                itemTransferencia.LineaCreditoId = DatoPrestamoTransferencia.LineaCreditoId;
                itemTransferencia.FormaDePago = DatoPrestamoTransferencia.FormaDePago;
                itemTransferencia.Monto = DatoPrestamoTransferencia.Monto;
                itemTransferencia.NoDocumento = DatoPrestamoTransferencia.NoDocumento;
                itemTransferencia.EsEdicion = true;
            }

            DatoPrestamoTransferencia = new PrestamoTransferenciasModel
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                CantidadJustificada = ListaPrestamosTransferenciasModel.Sum(t => t.Monto)
            };

            BancoSeleccionado = new BancosDTO();
        }

        private string ValidarPrestamoTransferencia()
        {
            if (DatoPrestamoTransferencia.Monto <= 0)
            {
                return "Debe ingresar un monto mayor a 0";
            }

            if (string.IsNullOrWhiteSpace(DatoPrestamoTransferencia.FormaDePago))
            {
                return "Debe seleccionar la forma de Pago";
            }
            
            if (FormaDePagoSeleccionada.RequiereBanco)
            {
                if (DatoPrestamoTransferencia.BancoId == 0)
                {
                    return "Debe seleccionar el un Banco";
                }

                if (string.IsNullOrWhiteSpace(DatoPrestamoTransferencia.NoDocumento))
                {
                    return "Debe ingresar el Número de la Transacción";
                }
            }

            if (DatoPrestamoTransferencia.LineaCreditoId == 0)
            {
                return "Debe seleccionar la Linea de Crédito";
            }

            return string.Empty;
        }

        private void EditarItemTransferencia()
        {
            if (PrestamoTransferenciaSeleccionada != null)
            {
                if (!PrestamoTransferenciaSeleccionada.PuedeEditarCreditoDeduccion && PrestamoTransferenciaSeleccionada.PrestamoTransferenciaId > 0)
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                    DatoPrestamoTransferencia = new PrestamoTransferenciasModel();
                    return;
                }

                DatoPrestamoTransferencia = new PrestamoTransferenciasModel
                {
                    EsEdicion = true,
                    LineaCreditoId = PrestamoTransferenciaSeleccionada.LineaCreditoId,
                    BancoId = PrestamoTransferenciaSeleccionada.BancoId,
                    NombreBanco = PrestamoTransferenciaSeleccionada.NombreBanco,
                    FormaDePago = PrestamoTransferenciaSeleccionada.FormaDePago,
                    Monto = PrestamoTransferenciaSeleccionada.Monto,
                    NoDocumento = PrestamoTransferenciaSeleccionada.NoDocumento,
                    PrestamoId = PrestamoTransferenciaSeleccionada.PrestamoId,
                    PrestamoTransferenciaId = PrestamoTransferenciaSeleccionada.PrestamoTransferenciaId,
                    CantidadJustificada = ListaPrestamosTransferenciasModel.Sum(t => t.Monto)                                  
                };
            }
        }

        private void RemoverItemTransferencia()
        {
            if (PrestamoTransferenciaSeleccionada == null) return;

            if (!PrestamoTransferenciaSeleccionada.PuedeEditarCreditoDeduccion && PrestamoTransferenciaSeleccionada.PrestamoTransferenciaId > 0)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No puede editar la Deducción porque la Linea de Crédito está Cerrada");
                DatoPrestamoTransferencia = new PrestamoTransferenciasModel();
                return;
            }

            ListaPrestamosTransferenciasModel.Remove(PrestamoTransferenciaSeleccionada);
            DatoPrestamoTransferencia.CantidadJustificada = ListaPrestamosTransferenciasModel.Sum(t => t.Monto);
        }

        private void MostrarPrestamosTransferencias()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado != Estados.NUEVO && PrestamoSeleccionado.Estado != Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede justificar transferencias de Prestamos con estado Nuevo o EnProceso");
                return;
            }

            var transferenciasPrestamo = from reg in ListaPrestamosTransferencias
                                         select new PrestamoTransferenciasModel
                                         {
                                             LineaCreditoId = reg.LineaCreditoId,
                                             FormaDePago = reg.FormaDePago,
                                             Monto = reg.Monto,
                                             NoDocumento = reg.NoDocumento,
                                             BancoId = reg.BancoId,
                                             NombreBanco = reg.NombreBanco,
                                             PrestamoId = reg.PrestamoId,
                                             PrestamoTransferenciaId = reg.PrestamoTransferenciaId,
                                             CodigoLineaCredito = reg.CodigoLineaCredito,
                                             PuedeEditarCreditoDeduccion = reg.PuedeEditarCreditoDeduccion
                                         };

            ListaPrestamosTransferenciasModel = new ObservableCollection<PrestamoTransferenciasModel>(transferenciasPrestamo);
            DatoPrestamoTransferencia = new PrestamoTransferenciasModel
            {
                CantidadJustificada = ListaPrestamosTransferenciasModel.Sum(t => t.Monto)
            };

            CargarSlidePanel("TransferenciasPrestamoFlyout");
        }

        private void InicializarPropiedades()
        {
            BusquedaPrestamosControl = new BusquedaPrestamosDTO();
            DatoPrestamoTransferencia = new PrestamoTransferenciasModel();
        }

        private void ObtenerCorrelativoPrestamo()
        {
            var request = new GetPrestamoUltimo
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                Fecha = DatoPrestamo.FechaCreacion
            };

            var ultimoCorrelativoPrestamo = _client.Get(request);
            DatoPrestamo.CodigoPrestamo = ultimoCorrelativoPrestamo.CodigoPrestamo;
            RaisePropertyChanged("DatoPrestamo");
        }

        private void MostrarBusquedaProveedores()
        {
            ControlBusquedaProveedores = new BusquedaProveedores();
            ControlBusquedaProveedores.DatosEncontrados = new ObservableCollection<DTOs.RequestDTOs.Proveedores.ProveedoresDTO>();
            CargarSlidePanel("BusquedaProveedoresFlyout");
        }

        private void BuscarProveedores()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorProveedores
            {
                PaginaActual = 1,
                CantidadRegistros = 30,
                Filtro = ControlBusquedaProveedores.ValorBusqueda
            };

            MostrarVentanaEspera = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                ControlBusquedaProveedores.DatosEncontrados = new ObservableCollection<ProveedoresDTO>(res.Items);
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void SeleccionaProveedor(object valor)
        {
            var valorRetorno = (ProveedoresDTO)valor;

            if (valorRetorno != null)
            {
                DatoPrestamo.ProveedorId = valorRetorno.ProveedorId;
                DatoPrestamo.NombreProveedor = valorRetorno.Nombre;

                RaisePropertyChanged("DatoPrestamo");
                CargarSlidePanel("BusquedaProveedoresFlyout");
            }
        }

        private void Navegar()
        {
            CargarPrestamos();
        }

        private void EliminarPrestamo()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado == Estados.CERRADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Prestamo YA está CERRADO");
                return;
            }

            if (PrestamoSeleccionado.Estado == Estados.ENPROCESO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Prestamo está en PROCESO");
                return;
            }

            if (PrestamoSeleccionado.Estado == Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El Prestamo está en ACTIVO");
                return;
            }

            if (PrestamoSeleccionado.Estado == Estados.NUEVO)
            {
                DialogSettings = new ConfiguracionDialogoModel
                {
                    Mensaje = string.Format(Mensajes.Eliminando_Datos, PrestamoSeleccionado.CodigoPrestamo),
                    Titulo = "Prestamos"
                };
                DialogSettings.Respuesta += result =>
                {
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var uri = InformacionSistema.Uri_ApiService;
                        _client = new JsonServiceClient(uri);
                        var request = new PutPrestamoAnular
                        {
                            PrestamoId = PrestamoSeleccionado.PrestamoId,
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
                            CargarPrestamos();
                        }, (r, ex) =>
                        {
                            MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                            _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                        });
                    }
                };
                DialogSettings = null;
            }
        }

        private void MostrarEditarPrestamo()
        {
            if (PrestamoSeleccionado == null) return;

            if (PrestamoSeleccionado.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Editar Prestamos con estado Nuevo");
                return;
            }

            DatoPrestamo = new PrestamosDTO
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId,
                AutorizadoPor = PrestamoSeleccionado.AutorizadoPor,
                CodigoPrestamo = PrestamoSeleccionado.CodigoPrestamo,
                FechaCreacion = PrestamoSeleccionado.FechaCreacion,
                MontoPrestamo = PrestamoSeleccionado.MontoPrestamo,
                ProveedorId = PrestamoSeleccionado.ProveedorId,
                NombreProveedor = PrestamoSeleccionado.NombreProveedor,
                PorcentajeInteres = PrestamoSeleccionado.PorcentajeInteres,
                Observaciones = PrestamoSeleccionado.Observaciones,
                SucursalId = PrestamoSeleccionado.SucursalId,
                EsInteresMensual = PrestamoSeleccionado.EsInteresMensual
            };
            
            CargarSlidePanel("EditarPrestamoFlyout");
        }

        private void EditarPrestamo()
        {
            var mensajeValidacion = ValidarPrestamo();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutPrestamo
            {
                Prestamo = DatoPrestamo,
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
                CargarSlidePanel("EditarPrestamoFlyout");
                CargarPrestamos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarAgregarPrestamo()
        {
            DatoPrestamo = new PrestamosDTO
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                FechaCreacion = DateTime.Now,
                Estado = Estados.ACTIVO
            };

            ControlBusquedaProveedores = new BusquedaProveedores();
            CargarSlidePanel("CrearPrestamoFlyout");
        }

        private void CrearPrestamo()
        {
            var mensajeValidacion = ValidarPrestamo();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostPrestamo
            {
                Prestamo = DatoPrestamo,
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
                CargarSlidePanel("CrearPrestamoFlyout");
                CargarPrestamos();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarPrestamo()
        {
            if (string.IsNullOrWhiteSpace(DatoPrestamo.AutorizadoPor))
            {
                return "Debe especificar Quien Autoriza el prestamo";
            }

            if (DatoPrestamo.MontoPrestamo <= 0)
            {
                return "Debe ingresar el monto correcto del prestamo";
            }

            if (DatoPrestamo.ProveedorId == 0)
            {
                return "Debe especificar el Cliente a quien hará el prestamo";
            }

            if (string.IsNullOrWhiteSpace(DatoPrestamo.Observaciones))
            {
                return "Debe ingresar un pequeño comentario descriptivo";
            }

            return string.Empty;
        }
        
        private void BuscarPrestamo()
        {
            NumeroPagina = 1;
            CargarPrestamos();
        }

        private void CargarPrestamos()
        {
            var prestamoId = PrestamoSeleccionado != null ? PrestamoSeleccionado.PrestamoId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorPrestamos
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 12,
                Filtro = ValorBusquedaPrestamo
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaPrestamosControl = res;
                if (BusquedaPrestamosControl.Items != null && BusquedaPrestamosControl.Items.Any())
                {
                    if (prestamoId == 0)
                    {
                        PrestamoSeleccionado = BusquedaPrestamosControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        PrestamoSeleccionado = BusquedaPrestamosControl.Items.FirstOrDefault(r => r.PrestamoId == prestamoId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarPrestamoTransferencias()
        {
            ListaPrestamosTransferencias = new List<PrestamosTransferenciasDTO>();
            if (PrestamoSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetPrestamosTransferenciasPorPrestamoId
            {
                PrestamoId = PrestamoSeleccionado.PrestamoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaPrestamosTransferencias = res;

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

        private void CargarDatosIniciales()
        {
            CargarPrestamos();
            CargarBancos();
            CargarFormarDePago();
        }
        
        private void CargarDatosPruebas()
        {
            BusquedaPrestamosControl = new BusquedaPrestamosDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaPrestamos()
            };

            PrestamoSeleccionado = BusquedaPrestamosControl.Items.FirstOrDefault();

            ListaPrestamosTransferencias = DatosDiseño.ListaPrestamosTransferencias();
        }
    }
}
