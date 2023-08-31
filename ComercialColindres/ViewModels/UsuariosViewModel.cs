using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.Opciones;
using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Modelos;
using ComercialColindres.Recursos;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using WPFCore.Funciones;
using WPFCore.Modelos;

namespace ComercialColindres.ViewModels
{
    public class UsuariosViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public UsuariosViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            if (IsInDesignMode)
            {
                CargarDatosDePrueba();
                return;
            }
            InicializarComandos();
            CargarDatos();
        }

        public string ValorBusqueda
        {
            get { return _valorBusqueda; }
            set
            {
                if (_valorBusqueda != value)
                {
                    _valorBusqueda = value;
                    RaisePropertyChanged("ValorBusqueda");
                }
            }
        }
        private string _valorBusqueda;

        public UsuariosDTO UsuarioSeleccionado
        {
            get { return _usuarioSeleccionado; }
            set
            {
                _usuarioSeleccionado = value;
                RaisePropertyChanged("UsuarioSeleccionado");

                if (value != null && value.UsuariosOpciones != null)
                {
                    UsuarioSucursalAsignadaSeleccionada = value.UsuariosSucursalesAsignadas.FirstOrDefault();
                }
            }
        }
        private UsuariosDTO _usuarioSeleccionado;

        public UsuariosDTO DatoUsuario
        {
            get { return _datoUsuario; }
            set
            {
                if (_datoUsuario != value)
                {
                    _datoUsuario = value;
                    RaisePropertyChanged("DatoUsuario");
                }
            }
        }
        private UsuariosDTO _datoUsuario;

        public List<UsuariosDTO> ListadoUsuarios
        {
            get { return _listadoUsuarios; }
            set
            {
                _listadoUsuarios = value;
                RaisePropertyChanged("ListadoUsuarios");
            }
        }
        private List<UsuariosDTO> _listadoUsuarios;

        public List<EstadosModel> ListadoEstados
        {
            get { return _listadoEstados; }
            set
            {
                if (_listadoEstados != value)
                {
                    _listadoEstados = value;
                    RaisePropertyChanged("ListadoEstados");
                }
            }
        }
        private List<EstadosModel> _listadoEstados;

        public ObservableCollection<OpcionesUsuarioModel> DatoOpcionesUsuarioModel
        {
            get { return _datoOpcionesUsuarioModel; }
            set
            {
                if (_datoOpcionesUsuarioModel != value)
                {
                    _datoOpcionesUsuarioModel = value;
                    RaisePropertyChanged("DatoOpcionesUsuarioModel");
                }
            }
        }
        private ObservableCollection<OpcionesUsuarioModel> _datoOpcionesUsuarioModel;

        public ObservableCollection<OpcionesUsuarioModel> DatoOpcionesUsuarioOriginalModel
        {
            get
            {
                return _datoOpcionesUsuarioOriginalModel;
            }
            set
            {
                _datoOpcionesUsuarioOriginalModel = value;
                RaisePropertyChanged("DatoOpcionesUsuarioOriginalModel");
            }
        }
        ObservableCollection<OpcionesUsuarioModel> _datoOpcionesUsuarioOriginalModel;

        public ListCollectionView DatoOpcionesUsuarioAgrupadosModel
        {
            get
            {
                return _datoOpcionesUsuarioAgrupadosModel;
            }
            set
            {
                _datoOpcionesUsuarioAgrupadosModel = value;
                RaisePropertyChanged("DatoOpcionesUsuarioAgrupadosModel");
            }
        }
        ListCollectionView _datoOpcionesUsuarioAgrupadosModel;

        public List<SucursalesDTO> ListadoSucursales
        {
            get
            {
                return _listadoSucursales;
            }
            set
            {
                _listadoSucursales = value;
                RaisePropertyChanged("ListadoSucursales");
            }
        }
        List<SucursalesDTO> _listadoSucursales;

        public SucursalesDTO SucursalSeleccionada
        {
            get
            {
                return _sucursalSeleccionada;
            }
            set
            {
                _sucursalSeleccionada = value;
                RaisePropertyChanged("SucursalSeleccionada");
            }
        }
        SucursalesDTO _sucursalSeleccionada;

        public SucursalesDTO DatoSucursalSeleccionada
        {
            get
            {
                return _datoSucursalSeleccionada;
            }
            set
            {
                _datoSucursalSeleccionada = value;
                RaisePropertyChanged("DatoSucursalSeleccionada");
                if (value != null)
                {
                    FiltrarDatoOpcionesPorSucursal(value.SucursalId);
                }
            }
        }
        SucursalesDTO _datoSucursalSeleccionada;

        public List<SucursalesDTO> DatoListadoSucursales
        {
            get
            {
                return _datoListadoSucursales;
            }
            set
            {
                _datoListadoSucursales = value;
                RaisePropertyChanged("DatoListadoSucursales");
            }
        }
        List<SucursalesDTO> _datoListadoSucursales;

        public UsuariosSucursalesAsignadasDTO UsuarioSucursalAsignadaSeleccionada
        {
            get
            {
                return _usuarioSucursalAsignadaSeleccionada;
            }
            set
            {
                _usuarioSucursalAsignadaSeleccionada = value;
                RaisePropertyChanged("UsuarioSucursalAsignadaSeleccionada");

                FiltrarOpcionesSucursal();
            }
        }
        UsuariosSucursalesAsignadasDTO _usuarioSucursalAsignadaSeleccionada;

        public List<UsuariosOpcionesDTO> ListadoUsuarioOpciones
        {
            get
            {
                return _listadoUsuarioOpciones;
            }
            set
            {
                _listadoUsuarioOpciones = value;
                RaisePropertyChanged("ListadoUsuarioOpciones");
            }
        }
        List<UsuariosOpcionesDTO> _listadoUsuarioOpciones;

        public ListCollectionView ListadoUsuarioOpcionesAgrupadas
        {
            get { return _listadoUsuarioOpcionesAgrupadas; }
            set
            {
                if (_listadoUsuarioOpcionesAgrupadas != value)
                {
                    _listadoUsuarioOpcionesAgrupadas = value;
                    RaisePropertyChanged("ListadoUsuarioOpcionesAgrupadas");
                }
            }
        }
        private ListCollectionView _listadoUsuarioOpcionesAgrupadas;


        public RelayCommand ComandoAgregar { get; set; }
        public RelayCommand ComandoActualizar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }

        public RelayCommand ComandoRefrescar { get; set; }

        void InicializarComandos()
        {
            ComandoAgregar = new RelayCommand(Agregar);
            ComandoActualizar = new RelayCommand(Actualizar);
            ComandoBuscar = new RelayCommand(Buscar);
            ComandoEditar = new RelayCommand(Editar);
            ComandoEliminar = new RelayCommand(Eliminar);
            ComandoCrear = new RelayCommand(Crear);
            ComandoRefrescar = new RelayCommand(Refrescar);
        }

        void Actualizar()
        {
            MaterializarOpcionesUsuariosOpciones();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new UpdateUsuario
            {
                Usuario = DatoUsuario
            };
            _client.PutAsync(request, res =>
            {
                if (!string.IsNullOrWhiteSpace(res.MensajeError))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);
                CargarSlidePanel("1");
                CargarUsuarios();
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        void Buscar()
        {
            CargarUsuarios();
        }

        void Eliminar()
        {
            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_Datos, UsuarioSeleccionado.Usuario),
                Titulo = "Usuarios"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {

                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteUsuario
                    {
                        UsuarioId = UsuarioSeleccionado.UsuarioId
                    };

                    _client.DeleteAsync(request, res =>
                    {
                        if (!string.IsNullOrWhiteSpace(res.MensajeError))
                        {
                            _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                            return;
                        }

                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Eliminados);
                        CargarUsuarios();

                    }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
                }
            };
            DialogSettings = null;
        }

        void Editar()
        {
            if (UsuarioSeleccionado != null)
            {
                CargarEmpresasDisponibles();

                CargarOpcionesSistema();

                DatoUsuario = UsuarioSeleccionado.DeepCopy();
                MarcarOpcionesRegistradasUsuario();

                RaisePropertyChanged("DatoUsuario");
                RaisePropertyChanged("DatoOpcionesUsuarioOriginalModel");

                DatoSucursalSeleccionada = DatoListadoSucursales.FirstOrDefault();

                CargarSlidePanel("1");
            }
        }

        void Crear()
        {
            MaterializarOpcionesUsuariosOpciones();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new CreateUsuario
            {
                Usuario = DatoUsuario
            };
            _client.PostAsync(request, res =>
            {
                if (!string.IsNullOrWhiteSpace(res.MensajeError))
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, res.MensajeError);
                    return;
                }
                _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Guardados);

                CargarDatosUsuarioNuevo();
                CargarUsuarios();
                CargarSlidePanel("0");
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));

        }

        void Agregar()
        {
            CargarDatosUsuarioNuevo();
            CargarSlidePanel("0");
        }

        void CargarDatosUsuarioNuevo()
        {
            CargarEmpresasDisponibles();

            CargarOpcionesSistema();

            DatoSucursalSeleccionada = DatoListadoSucursales.FirstOrDefault();

            DatoUsuario = new UsuariosDTO
            {
                UsuariosOpciones = new List<UsuariosOpcionesDTO>(),
                Estado = Estados.ACTIVO
            };
        }

        void Refrescar()
        {
            CargarDatos();
        }

        void FiltrarOpcionesSucursal()
        {
            ListadoUsuarioOpciones = new List<UsuariosOpcionesDTO>();
            if (UsuarioSeleccionado != null && UsuarioSucursalAsignadaSeleccionada != null)
            {
                var opciones = UsuarioSeleccionado.UsuariosOpciones.Where(r => r.SucursalId == UsuarioSucursalAsignadaSeleccionada.SucursalId);
                if (opciones.Any())
                {
                    ListadoUsuarioOpciones = new List<UsuariosOpcionesDTO>(opciones);
                    var datos = new ObservableCollection<UsuariosOpcionesDTO>(opciones);
                    ListadoUsuarioOpcionesAgrupadas = new ListCollectionView(datos);
                    ListadoUsuarioOpcionesAgrupadas.GroupDescriptions.Add(new PropertyGroupDescription("TipoAcceso"));
                }
            }
        }

        void CargarEmpresasDisponibles()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllSucursales();
            var datos = _client.Get(request);

            DatoListadoSucursales = datos;
        }

        void MaterializarOpcionesUsuariosOpciones()
        {
            if (DatoUsuario.UsuariosOpciones == null)
            {
                DatoUsuario.UsuariosOpciones = new List<UsuariosOpcionesDTO>();
            }

            var listaOpcionesRegistradas = DatoUsuario.UsuariosOpciones.ToList();
            foreach (var usuarioOpcionDTO in listaOpcionesRegistradas)
            {
                var existe = DatoOpcionesUsuarioOriginalModel.Any(r => r.OpcionId == usuarioOpcionDTO.OpcionId && r.SucursalId == usuarioOpcionDTO.SucursalId && r.Seleccionada);
                if (existe == false)
                {
                    DatoUsuario.UsuariosOpciones.Remove(usuarioOpcionDTO);
                }
            }

            foreach (var opcionModel in DatoOpcionesUsuarioOriginalModel.Where(r => r.Seleccionada))
            {
                var existe = DatoUsuario.UsuariosOpciones.Any(r => r.OpcionId == opcionModel.OpcionId && r.SucursalId == opcionModel.SucursalId);
                if (existe == false)
                {
                    DatoUsuario.UsuariosOpciones.Add(new UsuariosOpcionesDTO
                    {
                        OpcionId = opcionModel.OpcionId,
                        UsuarioId = DatoUsuario.UsuarioId,
                        SucursalId = opcionModel.SucursalId
                    });
                }
            }
        }

        void MarcarOpcionesRegistradasUsuario()
        {
            foreach (var usuarioOpcionDTO in DatoUsuario.UsuariosOpciones)
            {
                var existe = DatoOpcionesUsuarioOriginalModel.FirstOrDefault(r => r.OpcionId == usuarioOpcionDTO.OpcionId && r.SucursalId == usuarioOpcionDTO.SucursalId);
                if (existe != null)
                {
                    existe.Seleccionada = true;
                }
            }
        }

        void FiltrarDatoOpcionesPorSucursal(int sucursalId)
        {
            var opciones = DatoOpcionesUsuarioOriginalModel.Where(r => r.SucursalId == sucursalId);
            DatoOpcionesUsuarioModel = new ObservableCollection<OpcionesUsuarioModel>(opciones);
            DatoOpcionesUsuarioAgrupadosModel = new ListCollectionView(DatoOpcionesUsuarioModel);
            DatoOpcionesUsuarioAgrupadosModel.GroupDescriptions.Add(new PropertyGroupDescription("TipoAcceso"));
        }

        void CargarOpcionesSistema()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new FindOpciones();
            var opciones = _client.Get(request);
            DatoOpcionesUsuarioOriginalModel = new ObservableCollection<OpcionesUsuarioModel>();
            foreach (var itemEmpresa in DatoListadoSucursales)
            {
                var opcionesUsuarioModel = MaterializarOpcionesUsuarioModel(opciones, itemEmpresa.SucursalId);
                foreach (var itemOpcionModel in opcionesUsuarioModel)
                {
                    DatoOpcionesUsuarioOriginalModel.Add(itemOpcionModel);
                }
            }
        }

        ObservableCollection<OpcionesUsuarioModel> MaterializarOpcionesUsuarioModel(IEnumerable<OpcionesDTO> opciones, int sucursalId)
        {
            var datos = new ObservableCollection<OpcionesUsuarioModel>();
            foreach (var opcionDTO in opciones)
            {
                datos.Add(new OpcionesUsuarioModel
                {
                    OpcionId = opcionDTO.OpcionId,
                    Nombre = opcionDTO.Nombre,
                    SucursalId = sucursalId,
                    TipoAcceso = opcionDTO.TipoAcceso
                });
            }
            return datos;
        }

        void CargarUsuarios()
        {
            var usuarioIdSeleccionado = 0;
            if (UsuarioSeleccionado != null)
            {
                usuarioIdSeleccionado = UsuarioSeleccionado.UsuarioId;
            }
            ListadoUsuarios = new List<UsuariosDTO>();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new FindUsuarios
            {
                Filtro = ValorBusqueda
            };
            _client.GetAsync(request, res =>
            {
                ListadoUsuarios = res;
                if (usuarioIdSeleccionado == 0)
                {
                    UsuarioSeleccionado = ListadoUsuarios.FirstOrDefault();
                }
                else
                {
                    UsuarioSeleccionado = ListadoUsuarios.FirstOrDefault(r => r.UsuarioId == usuarioIdSeleccionado);
                }
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        void CargarDatos()
        {
            CagarEstados();
            CargarUsuarios();
        }

        void CagarEstados()
        {
            ListadoEstados = new List<EstadosModel>
            {
                new EstadosModel
                {
                    Estado = Estados.ACTIVO
                },
                new EstadosModel
                {
                    Estado = Estados.INACTIVO
                }
            };
        }

        void CargarDatosDePrueba()
        {
            ListadoUsuarios = DatosDiseño.ListaUsuarios();
            UsuarioSeleccionado = ListadoUsuarios.FirstOrDefault();
            var datos = new ObservableCollection<UsuariosOpcionesDTO>
            {
                new UsuariosOpcionesDTO { NombreOpcion = "Opcion #1", TipoAcceso = "Opcion" },
                new UsuariosOpcionesDTO { NombreOpcion = "Opcion #2", TipoAcceso = "Opcion" },
                new UsuariosOpcionesDTO { NombreOpcion = "Opcion #3", TipoAcceso = "Acceso Autorizado" },
                new UsuariosOpcionesDTO { NombreOpcion = "Opcion #4", TipoAcceso = "Acceso Autorizado" },
                new UsuariosOpcionesDTO { NombreOpcion = "Opcion #4", TipoAcceso = "Acceso Autorizado" }
            };

            ListadoUsuarioOpcionesAgrupadas = new ListCollectionView(datos);
            ListadoUsuarioOpcionesAgrupadas.GroupDescriptions.Add(new PropertyGroupDescription("TipoAcceso"));
        }
    }
}
