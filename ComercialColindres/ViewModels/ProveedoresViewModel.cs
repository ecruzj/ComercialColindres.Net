using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.DTOs.RequestDTOs.EquiposCategorias;
using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPFCore.Modelos;

namespace ComercialColindres.ViewModels
{
    public class ProveedoresViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public ProveedoresViewModel(IServiciosComunes serviciosComunes)
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
        
        public ProveedoresDTO ProveedorSeleccionado
        {
            get
            {
                return _proveedorSeleccionado;
            }
            set
            {
                _proveedorSeleccionado = value;
                RaisePropertyChanged("ProveedorSeleccionado");

                if (IsInDesignMode)
                {
                    return;
                }
                CargarCuentasBancarias();
                CargarEquipos();
                CargarConductores();
            }
        }
        ProveedoresDTO _proveedorSeleccionado;

        public ProveedoresDTO DatoProveedor
        {
            get
            {
                return _datoProveedor;
            }
            set
            {
                _datoProveedor = value;
                RaisePropertyChanged("DatoProveedor");
            }
        }
        ProveedoresDTO _datoProveedor;

        public string ValorBusquedaProveedor
        {
            get
            {
                return _valorBusquedaProveedor;
            }
            set
            {
                _valorBusquedaProveedor = value;
                RaisePropertyChanged("ValorBusquedaProveedor");
            }
        }
        string _valorBusquedaProveedor;

        public BusquedaProveedoresDTO BusquedaProveedorControl
        {
            get
            {
                return _busquedaProveedorControl;
            }
            set
            {
                _busquedaProveedorControl = value;
                RaisePropertyChanged("BusquedaProveedorControl");
            }
        }
        BusquedaProveedoresDTO _busquedaProveedorControl;
        
        public List<FacturasDTO> ListaFacturas
        {
            get
            {
                return _listaFacturas;
            }
            set
            {
                _listaFacturas = value;
                RaisePropertyChanged("ListaFacturas");
            }
        }
        private List<FacturasDTO> _listaFacturas;
        
        public List<int> ListadoTiposPrecios
        {
            get { return _listadoTiposPrecios; }
            set
            {
                if (_listadoTiposPrecios != value)
                {
                    _listadoTiposPrecios = value;
                    RaisePropertyChanged("ListadoTiposPrecios");
                }
            }
        }
        private List<int> _listadoTiposPrecios;
        
        public List<CuentasBancariasDTO> ListaCuentasBancarias
        {
            get
            {
                return _listaCuentasBancarias;
            }
            set
            {
                _listaCuentasBancarias = value;
                RaisePropertyChanged("ListaCuentasBancarias");
            }
        }
        private List<CuentasBancariasDTO> _listaCuentasBancarias;
        
        public List<EquiposDTO> ListaEquipos
        {
            get
            {
                return _listaEquipos;
            }
            set
            {
                _listaEquipos = value;
                RaisePropertyChanged("ListaEquipos");
            }
        }
        private List<EquiposDTO> _listaEquipos;
        
        public List<ConductoresDTO> ListaCondunctores
        {
            get
            {
                return _listaConductores;
            }
            set
            {
                _listaConductores = value;
                RaisePropertyChanged("ListaCondunctores");
            }
        }
        private List<ConductoresDTO> _listaConductores;
        
        public List<BancosDTO> ListaBancos
        {
            get
            {
                return _listaBancos;
            }
            set
            {
                _listaBancos = value;
                RaisePropertyChanged("ListaBancos");
            }
        }
        private List<BancosDTO> _listaBancos;
        
        public ObservableCollection<CuentasBancariasModel> ListaCuentasBancariasModel
        {
            get
            {
                return _listaCuentasBancariasModel;
            }
            set
            {
                if (_listaCuentasBancariasModel != value)
                {
                    _listaCuentasBancariasModel = value;
                    RaisePropertyChanged("ListaCuentasBancariasModel");
                }
            }
        }
        private ObservableCollection<CuentasBancariasModel> _listaCuentasBancariasModel;
        
        public CuentasBancariasModel CuentaBancariaSeleccionada
        {
            get
            {
                return _cuentaBancariaSeleccionado;
            }
            set
            {
                _cuentaBancariaSeleccionado = value;
                RaisePropertyChanged("CuentaBancariaSeleccionada");
            }
        }
        private CuentasBancariasModel _cuentaBancariaSeleccionado;
        
        public CuentasBancariasModel DatoCuentaBancaria
        {
            get
            {
                return _datoCuentaBancaria;
            }
            set
            {
                _datoCuentaBancaria = value;
                RaisePropertyChanged("DatoCuentaBancaria");
            }
        }
        private CuentasBancariasModel _datoCuentaBancaria;
        
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
            }
        }
        private BancosDTO _bancoSeleccionado;

        public ObservableCollection<EquiposModel> ListaEquiposModel
        {
            get
            {
                return _listaEquiposModel;
            }
            set
            {
                _listaEquiposModel = value;
                RaisePropertyChanged("ListaEquiposModel");
            }
        }
        private ObservableCollection<EquiposModel> _listaEquiposModel;

        public EquiposModel EquipoSeleccionado
        {
            get
            {
                return _equipoSeleccionado;
            }
            set
            {
                _equipoSeleccionado = value;
                RaisePropertyChanged("EquipoSeleccionado");
            }
        }
        private EquiposModel _equipoSeleccionado;

        public EquiposModel DatoEquipo
        {
            get
            {
                return _datoEquipo;
            }
            set
            {
                _datoEquipo = value;
                RaisePropertyChanged("DatoEquipo");
            }
        }
        private EquiposModel _datoEquipo;
        
        public List<EquiposCategoriasDTO> ListaCategoriaEquipos
        {
            get
            {
                return _listaCategoriaEquipos;
            }
            set
            {
                _listaCategoriaEquipos = value;
                RaisePropertyChanged("ListaCategoriaEquipos");
            }
        }
        private List<EquiposCategoriasDTO> _listaCategoriaEquipos;
        
        public EquiposCategoriasDTO EquipoCategoriaSeleccionado
        {
            get
            {
                return _equipoCategoriaSeleccionado;
            }
            set
            {
                _equipoCategoriaSeleccionado = value;
                RaisePropertyChanged("EquipoCategoriaSeleccionado");
            }
        }
        private EquiposCategoriasDTO _equipoCategoriaSeleccionado;
        
        public ObservableCollection<ConductoresModel> ListaConductoresModel
        {
            get
            {
                return _listaConductoresModel;
            }
            set
            {
                _listaConductoresModel = value;
                RaisePropertyChanged("ListaConductoresModel");
            }
        }
        private ObservableCollection<ConductoresModel> _listaConductoresModel;
        
        public ConductoresModel ConductorSeleccionado
        {
            get
            {
                return _conductorSeleccionado;
            }
            set
            {
                _conductorSeleccionado = value;
                RaisePropertyChanged("ConductorSeleccionado");
            }
        }
        private ConductoresModel _conductorSeleccionado;
        
        public ConductoresModel DatoConductor
        {
            get
            {
                return _datoConductor;
            }
            set
            {
                _datoConductor = value;
                RaisePropertyChanged("DatoConductor");
            }
        }
        private ConductoresModel _datoConductor;

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


        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoNavegar { get; set; }
        public RelayCommand ComandoMostrarAgregar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }

        public RelayCommand ComandoMostrarCuentasBancarias { get; set; }
        public RelayCommand ComandoRemoverCuentaBancaria { get; set; }
        public RelayCommand ComandoEditarCuentaBancaria { get; set; }
        public RelayCommand ComandoAgregarCuentaBancaria { get; set; }
        public RelayCommand ComandoGuardarCuentasBancarias { get; set; }
        public RelayCommand ComandoSeleccionaBanco { get; set; }

        public RelayCommand ComandoMostrarEquipos { get; set; }
        public RelayCommand ComandoRemoverEquipo { get; set; }
        public RelayCommand ComandoEditarEquipo { get; set; }
        public RelayCommand ComandoAgregarEquipo { get; set; }
        public RelayCommand ComandoGuardarEquipos { get; set; }

        public RelayCommand ComandoMostrarConductores { get; set; }
        public RelayCommand ComandoRemoverConductor { get; set; }
        public RelayCommand ComandoEditarConductor { get; set; }
        public RelayCommand ComandoAgregarConductor { get; set; }
        public RelayCommand ComandoGuardarConductores { get; set; }

        void InicializarComandos()
        {
            ComandoBuscar = new RelayCommand(Buscar);
            ComandoEditar = new RelayCommand(Editar);
            ComandoMostrarAgregar = new RelayCommand(MostrarAgregar);
            ComandoCrear = new RelayCommand(Crear);
            ComandoMostrarEditar = new RelayCommand(MostrarEditar);
            ComandoEliminar = new RelayCommand(Eliminar);
            ComandoNavegar = new RelayCommand(Navegar);

            ComandoMostrarCuentasBancarias = new RelayCommand(MostrarCuentasBancarias);
            ComandoAgregarCuentaBancaria = new RelayCommand(AgregarItemCuentaBancaria);
            ComandoRemoverCuentaBancaria = new RelayCommand(RemoverItemCuentaBancaria);
            ComandoEditarCuentaBancaria = new RelayCommand(EditarCuentaBancaria);
            ComandoGuardarCuentasBancarias = new RelayCommand(GuardarCuentasBancarias);

            ComandoMostrarEquipos = new RelayCommand(MostrarEquipos);
            ComandoAgregarEquipo = new RelayCommand(AgregarItemEquipo);
            ComandoRemoverEquipo = new RelayCommand(RemoverItemEquipo);
            ComandoEditarEquipo = new RelayCommand(EditarEquipo);
            ComandoGuardarEquipos = new RelayCommand(GuardarEquipos);

            ComandoMostrarConductores = new RelayCommand(MostrarConductores);
            ComandoAgregarConductor = new RelayCommand(AgregarItemConductor);
            ComandoRemoverConductor = new RelayCommand(RemoverItemConductor);
            ComandoEditarConductor = new RelayCommand(EditarConductor);
            ComandoGuardarConductores = new RelayCommand(GuardarConductores);
        }
        
        void Navegar()
        {
            CargarProveedores();
        }
        
        void Eliminar()
        {
            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, ProveedorSeleccionado.ProveedorId),
                Titulo = "Proveedores"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteProveedor
                    {
                        ProveedorId = ProveedorSeleccionado.ProveedorId
                    };
                    MostrarVentanaEsperaPrincipal = Visibility.Visible;
                    _client.DeleteAsync(request, res =>
                    {
                        MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                        if (!string.IsNullOrWhiteSpace(res.MensajeError))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                            return;
                        }
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Eliminados);
                        CargarProveedores();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;


        }

        void Editar()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutProveedor
            {
                Proveedor = DatoProveedor
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
                CargarSlidePanel("EditarProveedorFlyout");
                CargarProveedores();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        void MostrarEditar()
        {
            DatoProveedor = new ProveedoresDTO
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId,
                Nombre = ProveedorSeleccionado.Nombre,
                RTN = ProveedorSeleccionado.RTN,
                CedulaNo = ProveedorSeleccionado.CedulaNo,
                Direccion = ProveedorSeleccionado.Direccion,
                Telefonos = ProveedorSeleccionado.Telefonos,
                CorreoElectronico = ProveedorSeleccionado.CorreoElectronico,
                Estado = ProveedorSeleccionado.Estado,
            };

            CargarSlidePanel("1");
        }

        void Crear()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostProveedor
            {
                Proveedor = DatoProveedor
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
                CargarSlidePanel("AgregarProveedorFlyout");
                CargarProveedores();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
        
        void MostrarAgregar()
        {
            DatoProveedor = new ProveedoresDTO
            {
                Estado = Estados.ACTIVO
            };
            CargarSlidePanel("0");
        }
        
        void CargarFacturasPorProveedor()
        {
            if (ProveedorSeleccionado == null)
            {
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetFacturasPorProveedorId
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };

            _client.GetAsync(request, res =>
            {
                ListaFacturas = res;
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        void Buscar()
        {
            NumeroPagina = 1;
            CargarProveedores();
        }

        void CargarDatosIniciales()
        {
            CargarProveedores();
            CargarTiposPrecios();
            CargarBancos();
            CargarEquiposCategorias();
        }

        private void InicializarPropiedades()
        {
            DatoCuentaBancaria = new CuentasBancariasModel();
            ListaCuentasBancariasModel = new ObservableCollection<CuentasBancariasModel>();

            DatoEquipo = new EquiposModel();
            ListaEquiposModel = new ObservableCollection<EquiposModel>();

            DatoConductor = new ConductoresModel();
            ListaConductoresModel = new ObservableCollection<ConductoresModel>();
        }

        void CargarTiposPrecios()
        {
            ListadoTiposPrecios = new List<int> { 1, 2, 3 };
        }

        void CargarProveedores()
        {
            var proveedorId = ProveedorSeleccionado != null ? ProveedorSeleccionado.ProveedorId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorProveedores
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 30,
                Filtro = ValorBusquedaProveedor
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaProveedorControl = res;
                if (BusquedaProveedorControl.Items != null && BusquedaProveedorControl.Items.Any())
                {
                    if (proveedorId == 0)
                    {
                        ProveedorSeleccionado = BusquedaProveedorControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        ProveedorSeleccionado = BusquedaProveedorControl.Items.FirstOrDefault(r => r.ProveedorId == proveedorId);
                    }
                }
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

        private void MostrarCuentasBancarias()
        {
            if (ProveedorSeleccionado == null) return;

            var cuentasBancarias = from reg in ListaCuentasBancarias
                                   select new CuentasBancariasModel
                                   {
                                       BancoId = reg.BancoId,
                                       CedulaNo = reg.CedulaNo,
                                       ProveedorId = reg.ProveedorId,
                                       CuentaId = reg.CuentaId,
                                       CuentaNo = reg.CuentaNo,
                                       Estado = reg.Estado,
                                       NombreAbonado = reg.NombreAbonado,
                                       NombreBanco = reg.NombreBanco
                                   };

            ListaCuentasBancariasModel = new ObservableCollection<CuentasBancariasModel>(cuentasBancarias);
            CargarSlidePanel("CuentasBancariasFlyout");
        }

        private void CargarCuentasBancarias()
        {
            if (ProveedorSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetCuentasBancariasPorProveedorId
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaCuentasBancarias = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarItemCuentaBancaria()
        {
            var mensajeValidacion = ValidarCuentasBancarias();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemCuenta = ListaCuentasBancariasModel.FirstOrDefault(c => c.CuentaId == DatoCuentaBancaria.CuentaId &&
                                                                       c.BancoId == BancoSeleccionado.BancoId &&
                                                                       c.NombreAbonado == DatoCuentaBancaria.NombreAbonado &&
                                                                       c.CuentaNo == DatoCuentaBancaria.CuentaNo);

            if (itemCuenta == null)
            {
                DatoCuentaBancaria.NombreBanco = BancoSeleccionado.Descripcion;
                DatoCuentaBancaria.Estado = Estados.ACTIVO;
                ListaCuentasBancariasModel.Add(DatoCuentaBancaria);
            }
            else
            {
                itemCuenta.BancoId = DatoCuentaBancaria.BancoId;
                itemCuenta.NombreAbonado = DatoCuentaBancaria.NombreAbonado;
                itemCuenta.NombreBanco = BancoSeleccionado.Descripcion;
                itemCuenta.CuentaNo = DatoCuentaBancaria.CuentaNo;
                itemCuenta.Estado = DatoCuentaBancaria.Estado;
                itemCuenta.CedulaNo = DatoCuentaBancaria.CedulaNo;
            }

            DatoCuentaBancaria = new CuentasBancariasModel
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };
            
            BancoSeleccionado = new BancosDTO();
            RaisePropertyChanged("DatoCuentaBancaria");
        }

        private string ValidarCuentasBancarias()
        {
            if (DatoCuentaBancaria.BancoId == 0)
            {
                return "Debe seleccionar un Banco";
            }

            if (string.IsNullOrWhiteSpace(DatoCuentaBancaria.NombreAbonado))
            {
                return "Debe ingresar el nombre del abonado";
            }
            
            if (string.IsNullOrWhiteSpace(DatoCuentaBancaria.CuentaNo))
            {
                return "Debe ingresar el número de cuenta";
            }

            return string.Empty;
        }

        private void RemoverItemCuentaBancaria()
        {
            ListaCuentasBancariasModel.Remove(CuentaBancariaSeleccionada);
        }

        private void EditarCuentaBancaria()
        {
            if (CuentaBancariaSeleccionada != null)
            {
                DatoCuentaBancaria = new CuentasBancariasModel
                {
                    BancoId = CuentaBancariaSeleccionada.BancoId,
                    NombreAbonado = CuentaBancariaSeleccionada.NombreAbonado,
                    NombreBanco = CuentaBancariaSeleccionada.NombreBanco,
                    CuentaNo = CuentaBancariaSeleccionada.CuentaNo,
                    Estado = CuentaBancariaSeleccionada.Estado,
                    CedulaNo = CuentaBancariaSeleccionada.CedulaNo,
                    ProveedorId = CuentaBancariaSeleccionada.ProveedorId,
                    CuentaId = CuentaBancariaSeleccionada.CuentaId
                };
                RaisePropertyChanged("DatoCuentaBancaria");
            }
        }

        private void GuardarCuentasBancarias()
        {
            var cuentasBancarias = from reg in ListaCuentasBancariasModel
                                  select new CuentasBancariasDTO
                                  {
                                      BancoId = reg.BancoId,
                                      CedulaNo = reg.CedulaNo,
                                      CuentaId = reg.CuentaId,
                                      CuentaNo = reg.CuentaNo,
                                      Estado = reg.Estado,
                                      NombreAbonado = reg.NombreAbonado,
                                      ProveedorId = reg.ProveedorId,
                                      NombreBanco = reg.NombreBanco
                                  };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostCuentasBancarias
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId,
                CuentasBancarias = cuentasBancarias.ToList()                
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
                CargarSlidePanel("CuentasBancariasFlyout");
                CargarCuentasBancarias();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarEquiposCategorias()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllEquiposCategorias
            {
            };

            _client.GetAsync(request, res =>
            {
                ListaCategoriaEquipos = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void CargarEquipos()
        {
            if (ProveedorSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetEquiposPorProveedorId
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaEquipos = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarEquipos()
        {
            if (ProveedorSeleccionado == null) return;

            var equipos = from reg in ListaEquipos
                          select new EquiposModel
                                   {
                                        ProveedorId = reg.ProveedorId,
                                        DescripcionCategoria = reg.DescripcionCategoria,
                                        EquipoCategoriaId = reg.EquipoCategoriaId,
                                        EquipoId = reg.EquipoId,
                                        Estado = reg.Estado,
                                        PlacaCabezal = reg.PlacaCabezal                              
                                   };

            ListaEquiposModel = new ObservableCollection<EquiposModel>(equipos);
            CargarSlidePanel("EquiposFlyout");
        }

        private void AgregarItemEquipo()
        {
            var mensajeValidacion = ValidarEquipo();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemEquipo = new EquiposModel();

            if (DatoEquipo.EquipoId > 0)
            {
                itemEquipo = ListaEquiposModel.FirstOrDefault(c => c.EquipoId == DatoEquipo.EquipoId);
            }
            else
            {
                itemEquipo = ListaEquiposModel.FirstOrDefault(c => c.EquipoId == DatoEquipo.EquipoId &&
                                                                  c.PlacaCabezal == DatoEquipo.PlacaCabezal);
            }

            if (itemEquipo == null)
            {
                DatoEquipo.DescripcionCategoria = EquipoCategoriaSeleccionado.Descripcion;
                DatoEquipo.Estado = Estados.ACTIVO;
                ListaEquiposModel.Add(DatoEquipo);
            }
            else
            {
                itemEquipo.DescripcionCategoria = EquipoCategoriaSeleccionado.Descripcion;
                itemEquipo.EquipoCategoriaId = EquipoCategoriaSeleccionado.EquipoCategoriaId;
                itemEquipo.PlacaCabezal = DatoEquipo.PlacaCabezal;
            }

            DatoEquipo = new EquiposModel
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };

            EquipoCategoriaSeleccionado = new EquiposCategoriasDTO();
            RaisePropertyChanged("DatoEquipo");
        }

        private string ValidarEquipo()
        {
            if (DatoEquipo.EquipoCategoriaId == 0)
            {
                return "Debe seleccionar una Categoria de Equipo";
            }

            if (string.IsNullOrWhiteSpace(DatoEquipo.PlacaCabezal))
            {
                return "Debe ingresar la placa del cabezal";
            }

            return string.Empty;
        }

        private void RemoverItemEquipo()
        {
            ListaEquiposModel.Remove(EquipoSeleccionado);
        }

        private void EditarEquipo()
        {
            if (EquipoSeleccionado != null)
            {
                DatoEquipo = new EquiposModel
                {
                    DescripcionCategoria = EquipoSeleccionado.DescripcionCategoria,
                    ProveedorId = EquipoSeleccionado.ProveedorId,
                    EquipoCategoriaId = EquipoSeleccionado.EquipoCategoriaId,
                    EquipoId = EquipoSeleccionado.EquipoId,
                    Estado = EquipoSeleccionado.Estado,
                    PlacaCabezal = EquipoSeleccionado.PlacaCabezal
                };
                RaisePropertyChanged("DatoEquipo");
            }
        }

        private void GuardarEquipos()
        {
            var equipos = from reg in ListaEquiposModel
                          select new EquiposDTO
                                   {
                                       DescripcionCategoria = reg.DescripcionCategoria,
                                       ProveedorId = reg.ProveedorId,
                                       EquipoCategoriaId = reg.EquipoCategoriaId,
                                       EquipoId = reg.EquipoId,
                                       Estado = reg.Estado,
                                       PlacaCabezal = reg.PlacaCabezal
                                   };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostEquipos
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId,
                Equipos = equipos.ToList()
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
                CargarSlidePanel("EquiposFlyout");
                CargarEquipos();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarConductores()
        {
            if (ProveedorSeleccionado == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetConductoresPorProveedorId
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaCondunctores = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarConductores()
        {
            if (ProveedorSeleccionado == null) return;

            var conductores = from reg in ListaCondunctores
                          select new ConductoresModel
                          {
                              ProveedorId = reg.ProveedorId,
                              ConductorId = reg.ConductorId,
                              Nombre = reg.Nombre,
                              Telefonos = reg.Telefonos                            
                          };

            ListaConductoresModel = new ObservableCollection<ConductoresModel>(conductores);
            CargarSlidePanel("ConductoresFlyout");
        }

        private void AgregarItemConductor()
        {
            var mensajeValidacion = ValidarConductor();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemConductor = new ConductoresModel();

            if (DatoConductor.ConductorId > 0)
            {
                itemConductor = ListaConductoresModel.FirstOrDefault(c => c.ConductorId == DatoConductor.ConductorId);
            }
            else
            {
                itemConductor = ListaConductoresModel.FirstOrDefault(c => c.ConductorId == DatoConductor.ConductorId &&
                                                                     (c.Nombre == DatoConductor.Nombre || c.Telefonos == DatoConductor.Telefonos));
            }


            if (itemConductor == null)
            {
                ListaConductoresModel.Add(DatoConductor);
            }
            else
            {
                itemConductor.Nombre = DatoConductor.Nombre;
                itemConductor.Telefonos = DatoConductor.Telefonos;
            }

            DatoConductor = new ConductoresModel
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId
            };
            
            RaisePropertyChanged("DatoConductor");
        }

        private string ValidarConductor()
        {
            if (string.IsNullOrWhiteSpace(DatoConductor.Nombre))
            {
                return "Debe ingresar el nombre del conductor";
            }

            return string.Empty;
        }

        private void RemoverItemConductor()
        {
            ListaConductoresModel.Remove(ConductorSeleccionado);
        }

        private void EditarConductor()
        {
            if (ConductorSeleccionado != null)
            {
                DatoConductor = new ConductoresModel
                {
                    Nombre = ConductorSeleccionado.Nombre,
                    Telefonos = ConductorSeleccionado.Telefonos,
                    ProveedorId = ConductorSeleccionado.ProveedorId,
                    ConductorId = ConductorSeleccionado.ConductorId,
                    EsNuevo = true
                };
                RaisePropertyChanged("DatoConductor");
            }
        }

        private void GuardarConductores()
        {
            var conductores = from reg in ListaConductoresModel
                          select new ConductoresDTO
                          {
                              ProveedorId = ProveedorSeleccionado.ProveedorId,
                              ConductorId = reg.ConductorId,
                              Nombre = reg.Nombre,
                              Telefonos = reg.Telefonos
                          };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostConductores
            {
                ProveedorId = ProveedorSeleccionado.ProveedorId,
                Conductores = conductores.ToList()
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
                CargarSlidePanel("ConductoresFlyout");
                CargarConductores();

            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        void CargarDatosPruebas()
        {
            BusquedaProveedorControl = new BusquedaProveedoresDTO
            {
                TotalPagina = 23,
                Items = DatosDiseño.ListaProveedores()
            };
            ProveedorSeleccionado = BusquedaProveedorControl.Items.FirstOrDefault();

            ListaCuentasBancarias = DatosDiseño.ListaCuentasBancarias();
            ListaCondunctores = DatosDiseño.ListaConductores();
            ListaEquipos = DatosDiseño.ListaEquipos();
        }
    }
}
