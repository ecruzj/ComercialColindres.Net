using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels
{
    public class LineasCreditoViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public LineasCreditoViewModel(IServiciosComunes serviciosComunes)
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

        public BusquedaLineasCreditoDTO BusquedaLineasCreditoControl
        {
            get { return _busquedaLineasCreditoControl; }
            set
            {
                _busquedaLineasCreditoControl = value;
                RaisePropertyChanged("BusquedaLineasCreditoControl");
            }
        }
        private BusquedaLineasCreditoDTO _busquedaLineasCreditoControl;
        
        public LineasCreditoDTO LineaCreditoSeleccionada
        {
            get { return _lineaCreditoSeleccionada; }
            set
            {
                _lineaCreditoSeleccionada = value;
                RaisePropertyChanged("LineaCreditoSeleccionada");

                if (IsInDesignMode)
                {
                    return;
                }

                if (LineaCreditoSeleccionada == null) return;

                CargarDeduccionesOperativas();
                CargarDeduccionesVarias();
            }
        }
        private LineasCreditoDTO _lineaCreditoSeleccionada;
        
        public LineasCreditoDTO DatoLineaCredito
        {
            get { return _datoLineaCredito; }
            set
            {
                _datoLineaCredito = value;
                RaisePropertyChanged("DatoLineaCredito");
            }
        }
        private LineasCreditoDTO _datoLineaCredito;

        public List<LineasCreditoDeduccionesDTO> ListaDeduccionesOperativas
        {
            get { return _listaDeduccionesOperativas; }
            set
            {
                _listaDeduccionesOperativas = value;
                RaisePropertyChanged("ListaDeduccionesOperativas");
            }
        }
        private List<LineasCreditoDeduccionesDTO> _listaDeduccionesOperativas;
        
        public List<LineasCreditoDeduccionesDTO> ListaDeduccionesVarias
        {
            get { return _listaDeduccionesVarias; }
            set
            {
                _listaDeduccionesVarias = value;
                RaisePropertyChanged("ListaDeduccionesVarias");
            }
        }
        private List<LineasCreditoDeduccionesDTO> _listaDeduccionesVarias;
        
        public LineasCreditoDeduccionesDTO LineaCreditoDeduccionSeleccionado
        {
            get { return _lineaCreditoDeduccionSeleccionado; }
            set
            {
                _lineaCreditoDeduccionSeleccionado = value;
                RaisePropertyChanged("LineaCreditoDeduccionSeleccionado");
            }
        }
        private LineasCreditoDeduccionesDTO _lineaCreditoDeduccionSeleccionado;

        public LineasCreditoDeduccionesDTO DatoLineaCreditoDeduccion
        {
            get { return _datoLineaCreditoDeduccion; }
            set
            {
                _datoLineaCreditoDeduccion = value;
                RaisePropertyChanged("DatoLineaCreditoDeduccion");
            }
        }
        private LineasCreditoDeduccionesDTO _datoLineaCreditoDeduccion;

        public string ValorBusquedaLineaCredito
        {
            get
            {
                return _valorBusquedaLineaCredito;
            }
            set
            {
                _valorBusquedaLineaCredito = value;
                RaisePropertyChanged("ValorBusquedaLineaCredito");
            }
        }
        private string _valorBusquedaLineaCredito;

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
            get { return _bancoSeleccionado; }
            set
            {
                _bancoSeleccionado = value;
                RaisePropertyChanged("BancoSeleccionado");

                if (BancoSeleccionado == null) return;
                DatoLineaCredito.BancoId = BancoSeleccionado.BancoId;
                DatoLineaCredito.NombreBanco = BancoSeleccionado.Descripcion;
                CargarCuentasFinancierasPorBanco();            
            }
        }
        private BancosDTO _bancoSeleccionado;

        public List<CuentasFinancieraTiposDTO> ListaCuentasFinancieraTipos
        {
            get { return _listaCuentasFinancieraTipos; }
            set
            {
                _listaCuentasFinancieraTipos = value;
                RaisePropertyChanged("ListaCuentasFinancieraTipos");
            }
        }
        private List<CuentasFinancieraTiposDTO> _listaCuentasFinancieraTipos;
        
        public CuentasFinancieraTiposDTO CuentaFinancieraTipoSeleccionado
        {
            get { return _cuentaFinancieraTipoSeleccionado; }
            set
            {
                _cuentaFinancieraTipoSeleccionado = value;
                RaisePropertyChanged("CuentaFinancieraTipoSeleccionado");

                if (CuentaFinancieraTipoSeleccionado == null) return;

                if (!CuentaFinancieraTipoSeleccionado.RequiereBanco)
                {
                    CargarCuentasFinancierasCajaChica();
                }

                if (DatoLineaCredito == null) return;
                DatoLineaCredito.RequiereBanco = CuentaFinancieraTipoSeleccionado.RequiereBanco;
                DatoLineaCredito.CuentaFinancieraTipoId = CuentaFinancieraTipoSeleccionado.CuentaFinancieraTipoId;             
                RaisePropertyChanged("DatoLineaCredito");
                BancoSeleccionado = new BancosDTO();
                CuentaFinancieraSeleccionada = new CuentasFinancierasDTO();    
            }
        }
        private CuentasFinancieraTiposDTO _cuentaFinancieraTipoSeleccionado;

        public List<CuentasFinancierasDTO> ListaCuentasFinancieras
        {
            get { return _listaCuentasFinancieras; }
            set
            {
                _listaCuentasFinancieras = value;
                RaisePropertyChanged("ListaCuentasFinancieras");
            }
        }
        private List<CuentasFinancierasDTO> _listaCuentasFinancieras;
        
        public CuentasFinancierasDTO CuentaFinancieraSeleccionada
        {
            get { return _cuentaFinancieraSeleccionada; }
            set
            {
                _cuentaFinancieraSeleccionada = value;
                RaisePropertyChanged("CuentaFinancieraSeleccionada");

                if (CuentaFinancieraSeleccionada == null) return;
                if (DatoLineaCredito == null) return;

                DatoLineaCredito.CuentaFinancieraId = CuentaFinancieraSeleccionada.CuentaFinancieraId;
                RaisePropertyChanged("DatoLineaCredito");
            }
        }
        private CuentasFinancierasDTO _cuentaFinancieraSeleccionada;


        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoNavegar { get; set; }
        public RelayCommand ComandoMostrarAgregar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoActivarLineaCredito { get; set; }
        public RelayCommand ComandoEliminar { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }
        public RelayCommand ComandoObtenerCorrelativoLineaCredito { get; set; }

        public RelayCommand ComandoMostrarAgregarGastoVarios { get; set; }
        public RelayCommand ComandoGuardarDeduccion { get; set; }
        public RelayCommand ComandoRemoverDeduccionCredito { get; set; }
        public RelayCommand ComandoMostrarEditarDeduccionCredito { get; set; }
        public RelayCommand ComandoEditarDeduccionCredito { get; set; }
        public RelayCommand ComandoImprimirDetalleCredito { get; set; }

        private void InicializarPropiedades()
        {
            ComandoBuscar = new RelayCommand(BuscarLineaCredito);
            ComandoNavegar = new RelayCommand(Navegar);
            ComandoRefrescar = new RelayCommand(Navegar);
            ComandoMostrarAgregar = new RelayCommand(MostrarAgregarLineaCredito);
            ComandoObtenerCorrelativoLineaCredito = new RelayCommand(ObtenerCorrelativoLineaCredito);
            ComandoCrear = new RelayCommand(CrearLineaCredito);
            ComandoMostrarEditar = new RelayCommand(MostrarEditarLineacredito);
            ComandoEditar = new RelayCommand(EditarLineaCredito);
            ComandoEliminar = new RelayCommand(EliminarLineaCredito);
            ComandoActivarLineaCredito = new RelayCommand(ActivarLineaCredito);

            ComandoMostrarAgregarGastoVarios = new RelayCommand(MostrarAgregarGastoVarios);
            ComandoGuardarDeduccion = new RelayCommand(GuardarDeduccion);
            ComandoRemoverDeduccionCredito = new RelayCommand(EliminarDeduccionCredito);
            ComandoMostrarEditarDeduccionCredito = new RelayCommand(MostrarEditarDeduccionCredito);
            ComandoEditarDeduccionCredito = new RelayCommand(EditarDeduccionCredito);
            ComandoImprimirDetalleCredito = new RelayCommand(ImprimirDetalleLineaCredito);
        }

        private void ImprimirDetalleLineaCredito()
        {
            ObtenerInformacionReporte();
        }

        private void ObtenerInformacionReporte()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado == Estados.ANULADO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No existe información para mostrar");
                return;
            }

            ImprimirReporte("ReporteLineasCredito.rdlc");
        }

        private void ImprimirReporte(string nombreReporte)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var list = new List<LineasCreditoDTO>();
                list.Add(LineaCreditoSeleccionada);
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Lineas de Crédito";
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
                        NombreDataSet = "DeduccionesOperativas",
                        Datos = ListaDeduccionesOperativas
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "DeduccionesVarias",
                        Datos = ListaDeduccionesVarias
                    }
                };
                manejadorReporte.AgregarParametro("Empresa", "Comercial Colindres");
                manejadorReporte.MostarReporte();
            });
        }

        private void EditarDeduccionCredito()
        {
            var mensajeValidacion = ValidarLineaCreditoDeduccion();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutDeduccionVarios
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                LineaCreditoDeduccion = DatoLineaCreditoDeduccion
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
                CargarSlidePanel("EditarGastoVariosFlyout");
                CargarLineasDeCredito();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarEditarDeduccionCredito()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Deducciones de Lineas de Crédito Activos");
                return;
            }

            if (LineaCreditoDeduccionSeleccionado == null) return;

            DatoLineaCreditoDeduccion = new LineasCreditoDeduccionesDTO
            {
                LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId,
                CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito,
                LineaCreditoDeduccionId = LineaCreditoDeduccionSeleccionado.LineaCreditoDeduccionId,
                Descripcion = LineaCreditoDeduccionSeleccionado.Descripcion,
                Monto = LineaCreditoDeduccionSeleccionado.Monto,
                NoDocumento = LineaCreditoDeduccionSeleccionado.NoDocumento,
                FechaCreacion = LineaCreditoDeduccionSeleccionado.FechaCreacion,
                EsGastoOperativo = false                
            };
            RaisePropertyChanged("DatoLineaCreditoDeduccion");

            CargarSlidePanel("EditarGastoVariosFlyout");
        }

        private void EliminarDeduccionCredito()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Deducciones de Lineas de Crédito Activos");
                return;
            }

            if (LineaCreditoDeduccionSeleccionado == null) return;

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, LineaCreditoDeduccionSeleccionado.Descripcion),
                Titulo = "Eliminar Deducción de Linea de Crédito"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteDeduccionVarios
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        LineaCreditoDeuccionId = LineaCreditoDeduccionSeleccionado.LineaCreditoDeduccionId
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
                        CargarLineasDeCredito();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;

        }

        private void GuardarDeduccion()
        {
            var mensajeValidacion = ValidarLineaCreditoDeduccion();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostDeduccionVarios
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                LineaCreditoDeduccion = DatoLineaCreditoDeduccion
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
                CargarSlidePanel("CrearGastoVariosFlyout");
                CargarLineasDeCredito();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarLineaCreditoDeduccion()
        {
            if (LineaCreditoSeleccionada.RequiereBanco)
            {
                if (string.IsNullOrWhiteSpace(DatoLineaCreditoDeduccion.NoDocumento))
                {
                    return "Debe ingresar el No Documento de la Deducción";
                }
            }

            if (DatoLineaCreditoDeduccion.Monto <= 0)
            {
                return "El Monto de la Deducción debe ser mayor a 0";
            }

            if (string.IsNullOrWhiteSpace(DatoLineaCreditoDeduccion.Descripcion))
            {
                return "Debe ingresar una Descripción breve sobre la Deducción";
            }

            return string.Empty;
        }

        private void MostrarAgregarGastoVarios()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "El estado de la Linea de Crédito debe ser Activo");
                return;
            }

            DatoLineaCreditoDeduccion = new LineasCreditoDeduccionesDTO
            {
                LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId,
                CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito,
                FechaCreacion = DateTime.Now
            };

            CargarSlidePanel("CrearGastoVariosFlyout");
        }

        private void ActivarLineaCredito()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede activar Lineas de Crédito Nuevas");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Activando_LineaCredito, LineaCreditoSeleccionada.CodigoLineaCredito),
                Titulo = "Activar Lineas de Crédito"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutActivarLineaCredito
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId
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
                        CargarLineasDeCredito();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void EliminarLineaCredito()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede eliminar Lineas de Crédito Nuevas");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, LineaCreditoSeleccionada.CodigoLineaCredito),
                Titulo = "Anular Linea de Crédito"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutLineaCreditoAnular
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId
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
                        CargarLineasDeCredito();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void EditarLineaCredito()
        {
            var mensajeValidacion = ValidarLineaCredito();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutLineaCredito
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                LineaCredito = DatoLineaCredito
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
                CargarSlidePanel("EditarLineaCreditoFlyout");
                CargarLineasDeCredito();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarEditarLineacredito()
        {
            if (LineaCreditoSeleccionada == null) return;

            if (LineaCreditoSeleccionada.Estado == Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Linea de Crédito ya está Activada");
                return;
            }

            if (LineaCreditoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Linea de Crédito debe ser NUEVO");
                return;
            }

            CuentaFinancieraTipoSeleccionado = new CuentasFinancieraTiposDTO();

            DatoLineaCredito = new LineasCreditoDTO
            {
                CodigoLineaCredito = LineaCreditoSeleccionada.CodigoLineaCredito,
                LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId,
                CuentaFinancieraId = LineaCreditoSeleccionada.CuentaFinancieraId,
                BancoId = LineaCreditoSeleccionada.BancoId,
                RequiereBanco = LineaCreditoSeleccionada.BancoId > 0,
                NombreBanco = LineaCreditoSeleccionada.NombreBanco,
                NoDocumento = LineaCreditoSeleccionada.NoDocumento,
                Observaciones = LineaCreditoSeleccionada.Observaciones,
                FechaCreacion = LineaCreditoSeleccionada.FechaCreacion,
                CreadoPor = LineaCreditoSeleccionada.CreadoPor,
                MontoInicial = LineaCreditoSeleccionada.MontoInicial,
                TipoCredito = LineaCreditoSeleccionada.TipoCredito,
                SucursalId = LineaCreditoSeleccionada.SucursalId   ,
                EstaEditandoRegistro = true
            };
            RaisePropertyChanged(nameof(DatoLineaCredito));
                        
            CargarSlidePanel("EditarLineaCreditoFlyout");
        }

        private void CrearLineaCredito()
        {
            var mensajeValidacion = ValidarLineaCredito();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostLineaCredito
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                LineaCredito = DatoLineaCredito
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
                CargarSlidePanel("CrearLineaCreditoFlyout");
                CargarLineasDeCredito();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private string ValidarLineaCredito()
        {
            if (!DatoLineaCredito.EstaEditandoRegistro)
            {
                if (CuentaFinancieraTipoSeleccionado == null)
                {
                    return "Debe Seleccionar un Tipo de Cuenta Financiera";
                }

                if (CuentaFinancieraTipoSeleccionado.RequiereBanco)
                {
                    if (DatoLineaCredito.BancoId == 0)
                    {
                        return "Debe seleccionar un Banco";
                    }

                    if (DatoLineaCredito.CuentaFinancieraId == 0)
                    {
                        return "Debe seleccionar una Cuenta Financiera";
                    }
                }

                if (DatoLineaCredito.CuentaFinancieraId == 0)
                {
                    return "Debe seleccionar una Cuenta Financiera";
                }
            }

            if (string.IsNullOrWhiteSpace(DatoLineaCredito.NoDocumento))
            {
                return "Debe Ingresar el No Documento";
            }

            if (string.IsNullOrWhiteSpace(DatoLineaCredito.Observaciones))
            {
                return "Debe Ingresar una observación";
            }

            if (DatoLineaCredito.MontoInicial <= 0)
            {
                return "El Monto Inicial debe ser mayor a 0";
            }
            return string.Empty;
        }

        private void MostrarAgregarLineaCredito()
        {
            DatoLineaCredito = new LineasCreditoDTO
            {
                FechaCreacion = DateTime.Now,
                CreadoPor = InformacionSistema.UsuarioActivo.Usuario,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId
            };

            CargarSlidePanel("CrearLineaCreditoFlyout");
        }

        private void ObtenerCorrelativoLineaCredito()
        {
            var request = new GetLineaCreditoUltimo
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                Fecha = DatoLineaCredito.FechaCreacion
            };

            BancoSeleccionado = new BancosDTO();

            var ultimoCorrelativoLineaCredito = _client.Get(request);
            DatoLineaCredito.CodigoLineaCredito = ultimoCorrelativoLineaCredito.CodigoLineaCredito;
            RaisePropertyChanged("DatoLineaCredito");
        }

        private void BuscarLineaCredito()
        {
            NumeroPagina = 1;
            CargarLineasDeCredito();
        }

        private void Navegar()
        {
            CargarLineasDeCredito();
        }

        private void CargarDeduccionesOperativas()
        {
            ListaDeduccionesOperativas = new List<LineasCreditoDeduccionesDTO>();

            if (LineaCreditoSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDeduccionesOperativosPorLineaCreditoId
            {
                LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaDeduccionesOperativas = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarDeduccionesVarias()
        {
            ListaDeduccionesVarias = new List<LineasCreditoDeduccionesDTO>();

            if (LineaCreditoSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDeduccionesVariasPorLineaCreditoId
            {
                LineaCreditoId = LineaCreditoSeleccionada.LineaCreditoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaDeduccionesVarias = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarLineasDeCredito()
        {
            var lineaCreditoId = LineaCreditoSeleccionada != null ? LineaCreditoSeleccionada.LineaCreditoId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorLineasCredito
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaLineaCredito,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaLineasCreditoControl = res;
                if (BusquedaLineasCreditoControl.Items != null && BusquedaLineasCreditoControl.Items.Any())
                {
                    if (lineaCreditoId == 0)
                    {
                        LineaCreditoSeleccionada = BusquedaLineasCreditoControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        LineaCreditoSeleccionada = BusquedaLineasCreditoControl.Items.FirstOrDefault(r => r.LineaCreditoId == lineaCreditoId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarDatosIniciales()
        {
            CargarLineasDeCredito();
            CargarBancos();
            CargarCuentasFinancieraTipos();
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

        private void CargarCuentasFinancieraTipos()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllTiposCuentasFinancieras
            {
            };

            _client.GetAsync(request, res =>
            {
                ListaCuentasFinancieraTipos = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void CargarCuentasFinancierasCajaChica()
        {
            ListaCuentasFinancieras = new List<CuentasFinancierasDTO>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetCuentasFinancierasCajaChica { UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId};

            _client.GetAsync(request, res =>
            {
                ListaCuentasFinancieras = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void CargarCuentasFinancierasPorBanco()
        {
            ListaCuentasFinancieras = new List<CuentasFinancierasDTO>();

            if (DatoLineaCredito.BancoId == 0) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetCuentasFinancierasPorBancoPorTipoCuenta
            {
                BancoId = DatoLineaCredito.BancoId,
                UsuarioId = InformacionSistema.UsuarioActivo.UsuarioId,
                CuentaFinancieraTipoId = CuentaFinancieraTipoSeleccionado.CuentaFinancieraTipoId
            };

            _client.GetAsync(request, res =>
            {
                ListaCuentasFinancieras = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void InicializarComandos()
        {
            BusquedaLineasCreditoControl = new BusquedaLineasCreditoDTO();
        }

        private void CargarDatosPruebas()
        {
            BusquedaLineasCreditoControl = new BusquedaLineasCreditoDTO
            {
                TotalRegistros = 23,
                Items = DatosDiseño.ListaLineasCredito()
            };

            LineaCreditoSeleccionada = BusquedaLineasCreditoControl.Items.FirstOrDefault();
        }
    }
}
