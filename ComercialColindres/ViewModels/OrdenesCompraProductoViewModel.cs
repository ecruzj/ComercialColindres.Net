using ComercialColindres.Clases;
using ComercialColindres.DatosEjemplos;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle;
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

namespace ComercialColindres.ViewModels
{
    public class OrdenesCompraProductoViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public OrdenesCompraProductoViewModel(IServiciosComunes serviciosComunes)
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
        
        public string ValorBusquedaOrdenCompraProducto
        {
            get { return _valorBusquedaOrdenCompraProducto; }
            set
            {
                _valorBusquedaOrdenCompraProducto = value;
                RaisePropertyChanged("ValorBusquedaOrdenCompraProducto");
            }
        }
        private string _valorBusquedaOrdenCompraProducto;

        public BusquedaOrdenesCompraProductoDTO BusquedaOrdenesCompraProductoControl
        {
            get { return _busquedaOrdenesCompraProductoControl; }
            set
            {
                _busquedaOrdenesCompraProductoControl = value;
                RaisePropertyChanged("BusquedaOrdenesCompraProductoControl");
            }
        }
        private BusquedaOrdenesCompraProductoDTO _busquedaOrdenesCompraProductoControl;
        
        public OrdenesCompraProductoDTO OrdenCompraProductoSeleccionada
        {
            get { return _ordenCompraProductoSeleccionada; }
            set
            {
                _ordenCompraProductoSeleccionada = value;
                RaisePropertyChanged("OrdenCompraProductoSeleccionada");

                if (IsInDesignMode) return;

                CargarOrdenCompraProductoDetalle();
                CargarOrdenCompraDetalleBoletas();
            }
        }
        private OrdenesCompraProductoDTO _ordenCompraProductoSeleccionada;
        
        public OrdenesCompraProductoDTO DatoOrdenCompraProducto
        {
            get { return _datoOrdenCompraProducto; }
            set
            {
                _datoOrdenCompraProducto = value;
                RaisePropertyChanged("DatoOrdenCompraProducto");
            }
        }
        private OrdenesCompraProductoDTO _datoOrdenCompraProducto;
        
        public ObservableCollection<OrdenesCompraProductoDetalleModel> ListaPODetalleModel
        {
            get { return _listaPODetalleModel; }
            set
            {
                _listaPODetalleModel = value;
                RaisePropertyChanged("ListaPODetalleModel");
            }
        }
        private ObservableCollection<OrdenesCompraProductoDetalleModel> _listaPODetalleModel;
        
        public OrdenesCompraProductoDetalleModel DatoPODetalleModel
        {
            get { return _datoPODetalleModel; }
            set
            {
                _datoPODetalleModel = value;
                RaisePropertyChanged("DatoPODetalleModel");
            }
        }
        private OrdenesCompraProductoDetalleModel _datoPODetalleModel;
        
        public OrdenesCompraProductoDetalleModel PODetalleSeleccionado
        {
            get { return _pODetalleSeleccionado; }
            set
            {
                _pODetalleSeleccionado = value;
                RaisePropertyChanged("PODetalleSeleccionado");
            }
        }
        private OrdenesCompraProductoDetalleModel _pODetalleSeleccionado;

        public List<OrdenesCompraProductoDetalleDTO> ListaOrdenesCompraProductoDetalle
        {
            get { return _listaOrdenesCompraProductoDetalle; }
            set
            {
                _listaOrdenesCompraProductoDetalle = value;
                RaisePropertyChanged("ListaOrdenesCompraProductoDetalle");
            }
        }
        private List<OrdenesCompraProductoDetalleDTO> _listaOrdenesCompraProductoDetalle;
        
        public List<OrdenesCompraDetalleBoletaDTO> ListaOrdenesCompraDetalleBoletas
        {
            get { return _listaOrdenesCompraDetalleBoletas; }
            set
            {
                _listaOrdenesCompraDetalleBoletas = value;
                RaisePropertyChanged("ListaOrdenesCompraDetalleBoletas");
            }
        }
        private List<OrdenesCompraDetalleBoletaDTO> _listaOrdenesCompraDetalleBoletas;

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

                if (PlantaAutoCompleteSeleccionada != null && PlantaAutoCompleteSeleccionada.Id != null)
                {
                    DatoOrdenCompraProducto.PlantaId = (int)PlantaAutoCompleteSeleccionada.Id;
                    RaisePropertyChanged("DatoOrdenCompraProducto");
                }

            }
        }
        private AutoCompleteEntry _plantaAutoCompleteSeleccionada;
        
        public List<MaestroBiomasaDTO> ListaMaestroBiomasa
        {
            get { return _listaMaestroBiomasa; }
            set
            {
                _listaMaestroBiomasa = value;
                RaisePropertyChanged("ListaMaestroBiomasa");
            }
        }
        private List<MaestroBiomasaDTO> _listaMaestroBiomasa;
        
        public MaestroBiomasaDTO MaestroBiomasaSeleccionado
        {
            get { return _maestroBiomasaSeleccionado; }
            set
            {
                _maestroBiomasaSeleccionado = value;
                RaisePropertyChanged("MaestroBiomasaSeleccionado");

                if (MaestroBiomasaSeleccionado == null) return;

                if (DatoPODetalleModel != null)
                {
                    DatoPODetalleModel.BiomasaId = MaestroBiomasaSeleccionado.BiomasaId;
                    DatoPODetalleModel.MaestroBiomasaDescripcion = MaestroBiomasaSeleccionado.Descripcion;
                }
            }
        }
        private MaestroBiomasaDTO _maestroBiomasaSeleccionado;

        public RelayCommand<string> ComandoBuscarPlantas { get; set; }
        public RelayCommand ComandoBuscar { get; set; }
        public RelayCommand ComandoNavegar { get; set; }
        public RelayCommand ComandoRefrescar { get; set; }

        public RelayCommand ComandoMostrarAgregar { get; set; }
        public RelayCommand ComandoCrear { get; set; }
        public RelayCommand ComandoMostrarEditar { get; set; }
        public RelayCommand ComandoEditar { get; set; }
        public RelayCommand ComandoEliminar { get; set; }
        public RelayCommand ComandoActivarPO { get; set; }
        public RelayCommand ComandoCerrarPO { get; set; }
        public RelayCommand ComandoImprimirDetallePO { get; set; }
        public RelayCommand ComandoCalcularConversionDollarPO { get; set; }

        public RelayCommand ComandoDetalleBiomasa { get; set; }
        public RelayCommand ComandoCalcularTotalLpsDetallePO { get; set; }
        public RelayCommand ComandoAgregarDetallePO { get; set; }
        public RelayCommand ComandoEditarDetallePO { get; set; }
        public RelayCommand ComandoRemoverDetallePO { get; set; }
        public RelayCommand ComandoGuardarPODetalleBiomasa { get; set; }

        private void InicializarComandos()
        {
            ComandoBuscar = new RelayCommand(BuscarOrdenesCompraProducto);
            ComandoNavegar = new RelayCommand(Navegar);
            ComandoRefrescar = new RelayCommand(Navegar);

            ComandoMostrarAgregar = new RelayCommand(MostrarCrearOrdenCompraProducto);
            ComandoCrear = new RelayCommand(CrearPO);
            ComandoMostrarEditar = new RelayCommand(MostrarEditarOrdenCompraProducto);
            ComandoEditar = new RelayCommand(EditarPO);
            ComandoCerrarPO = new RelayCommand(CerrarPO);
            ComandoActivarPO = new RelayCommand(ActivarPO);
            ComandoEliminar = new RelayCommand(EliminarPO);
            ComandoCalcularConversionDollarPO = new RelayCommand(CalcularConversionDollarPO);

            ComandoDetalleBiomasa = new RelayCommand(MostrarDetalleBiomasa);
            ComandoCalcularTotalLpsDetallePO = new RelayCommand(CalcularMontoLpsPODetalle);
            ComandoAgregarDetallePO = new RelayCommand(AgregarDetallePO);
            ComandoEditarDetallePO = new RelayCommand(EditarDetallePO);
            ComandoRemoverDetallePO = new RelayCommand(RemoverDetallePO);
            ComandoGuardarPODetalleBiomasa = new RelayCommand(GuardarPODetalleBiomasa);

            ComandoBuscarPlantas = new RelayCommand<string>(BuscarPlantas);
        }

        private void GuardarPODetalleBiomasa()
        {
            var detallePOBiomsa = from reg in ListaPODetalleModel
                                  select new OrdenesCompraProductoDetalleDTO
                                  {
                                      OrdenCompraProductoDetalleId = reg.OrdenCompraProductoDetalleId,
                                      OrdenCompraProductoId = reg.OrdenCompraProductoId,
                                      BiomasaId = reg.BiomasaId,
                                      Toneladas = reg.Toneladas,
                                      PrecioDollar = reg.PrecioDollar,
                                      PrecioLPS = reg.PrecioLPS
                                  };

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostOrdenesCompraProductoDetalle
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId,
                OrdenesCompraProductoDetalle = detallePOBiomsa.ToList()
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
                CargarSlidePanel("PODetalleBiomasaFlyout");
                CargarOrdenesCompraProducto();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AgregarDetallePO()
        {
            var mensajeValidacion = ValidarPODetalle();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var itemDetallePO = ListaPODetalleModel.FirstOrDefault(d => d.OrdenCompraProductoDetalleId == DatoPODetalleModel.OrdenCompraProductoDetalleId
                                                                   && d.BiomasaId == DatoPODetalleModel.BiomasaId);

            if (itemDetallePO == null)
            {
                ListaPODetalleModel.Add(DatoPODetalleModel);
            }
            else
            {
                itemDetallePO.OrdenCompraProductoDetalleId = DatoPODetalleModel.OrdenCompraProductoDetalleId;
                itemDetallePO.OrdenCompraProductoId = DatoPODetalleModel.OrdenCompraProductoId;
                itemDetallePO.BiomasaId = DatoPODetalleModel.BiomasaId;
                itemDetallePO.Toneladas = DatoPODetalleModel.Toneladas;
                itemDetallePO.PrecioDollar = DatoPODetalleModel.PrecioDollar;
            }

            CalcularTotalesDetallePO();
            MaestroBiomasaSeleccionado = new MaestroBiomasaDTO();
        }

        private string ValidarPODetalle()
        {
            if (DatoPODetalleModel.BiomasaId == 0)
            {
                return "Debe seleccionar una Categoría de Biomasa";
            }

            if (DatoPODetalleModel.PrecioDollar <= 0)
            {
                return "Debe ingresar el Precio por Tonelada en $";
            }

            if (DatoPODetalleModel.PrecioLPS <= 0)
            {
                return "No se pudo calcular el Precio por Tonelada en Lps";
            }

            if (DatoPODetalleModel.PrecioDollar >= DatoPODetalleModel.PrecioLPS)
            {
                return "El Precio del Producto por Tonelada en $ debe ser menor al precio Lps";
            }

            if (DatoPODetalleModel.Toneladas <= 0)
            {
                return string.Format("Debe ingresar la Cantidad de las Toneladas para la Categoría Biomasa {0}", DatoPODetalleModel.MaestroBiomasaDescripcion);
            }

            return string.Empty;
        }

        private void EditarDetallePO()
        {
            if (PODetalleSeleccionado == null) return;

            DatoPODetalleModel = new OrdenesCompraProductoDetalleModel
            {
                OrdenCompraProductoDetalleId = PODetalleSeleccionado.OrdenCompraProductoDetalleId,
                OrdenCompraProductoId = PODetalleSeleccionado.OrdenCompraProductoId,
                BiomasaId = PODetalleSeleccionado.BiomasaId,
                Toneladas = PODetalleSeleccionado.Toneladas,
                PrecioDollar = PODetalleSeleccionado.PrecioDollar,
                ConversionDollarToLps = PODetalleSeleccionado.ConversionDollarToLps,
            };
        }

        private void RemoverDetallePO()
        {
            if (PODetalleSeleccionado == null) return;

            ListaPODetalleModel.Remove(PODetalleSeleccionado);
            CalcularTotalesDetallePO();
        }

        private void MostrarDetalleBiomasa()
        {
            if (OrdenCompraProductoSeleccionada == null) return;

            if (OrdenCompraProductoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Editar el Requerimiento de una PO en Estado NUEVO");
                return;
            }

            var detalleBiomasa = from reg in ListaOrdenesCompraProductoDetalle
                                 select new OrdenesCompraProductoDetalleModel
                                 {
                                     OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId,
                                     OrdenCompraProductoDetalleId = reg.OrdenCompraProductoDetalleId,
                                     BiomasaId = reg.BiomasaId,
                                     MaestroBiomasaDescripcion = reg.MaestroBiomasaDescripcion,
                                     ConversionDollarToLps = OrdenCompraProductoSeleccionada.ConversionDollarToLps,
                                     PrecioDollar = reg.PrecioDollar,
                                     PrecioLPS = reg.PrecioLPS,
                                     Toneladas = reg.Toneladas                                                                          
                                 };

            ListaPODetalleModel = new ObservableCollection<OrdenesCompraProductoDetalleModel>(detalleBiomasa);
            CalcularTotalesDetallePO();

            CargarSlidePanel("PODetalleBiomasaFlyout");
        }

        private void CalcularTotalesDetallePO()
        {
            DatoPODetalleModel = new OrdenesCompraProductoDetalleModel
            {
                OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId,
                ConversionDollarToLps = OrdenCompraProductoSeleccionada.ConversionDollarToLps,
                TotalDollares = ListaPODetalleModel.Sum(d => d.PrecioDollar * d.Toneladas),
                TotalLps = ListaPODetalleModel.Sum(d => d.Toneladas * d.PrecioLPS)
            };            
        }

        private void EliminarPO()
        {
            if (OrdenCompraProductoSeleccionada == null) return;

            if (OrdenCompraProductoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Eliminar PO en estado NUEVO");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Eliminando_PO, OrdenCompraProductoSeleccionada.OrdenCompraNo, OrdenCompraProductoSeleccionada.NombrePlanta),
                Titulo = "Eliminar PO de BIOMASA"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new DeleteOrdenCompraProducto
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId
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
                        CargarOrdenesCompraProducto();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void ActivarPO()
        {
            if (OrdenCompraProductoSeleccionada == null) return;

            if (OrdenCompraProductoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Activar PO en estado NUEVOS");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Activando_PO, OrdenCompraProductoSeleccionada.OrdenCompraNo, OrdenCompraProductoSeleccionada.NombrePlanta),
                Titulo = "Activar PO de BIOMASA"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutActivarOrdenCompraProducto
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId,
                        PlantaId = OrdenCompraProductoSeleccionada.PlantaId
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
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                        CargarOrdenesCompraProducto();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void CerrarPO()
        {
            if (OrdenCompraProductoSeleccionada == null) return;

            if (OrdenCompraProductoSeleccionada.Estado != Estados.ACTIVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "Solo puede Cerrar PO en estado ACTIVOS");
                return;
            }

            DialogSettings = new ConfiguracionDialogoModel
            {
                Mensaje = string.Format(Mensajes.Cerrando_PO, OrdenCompraProductoSeleccionada.OrdenCompraNo, OrdenCompraProductoSeleccionada.NombrePlanta),
                Titulo = "Cerrar PO de BIOMASA"
            };
            DialogSettings.Respuesta += result =>
            {
                if (result == MessageDialogResult.Affirmative)
                {
                    var uri = InformacionSistema.Uri_ApiService;
                    _client = new JsonServiceClient(uri);
                    var request = new PutCerrarOrdenCompraProducto
                    {
                        UserId = InformacionSistema.UsuarioActivo.Usuario,
                        OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId
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
                        _serviciosComunes.MostrarNotificacion(EventType.Successful, Mensajes.Datos_Actualizados);
                        CargarOrdenesCompraProducto();
                    }, (r, ex) =>
                    {
                        MostrarVentanaEsperaFlyOut = Visibility.Collapsed;
                        _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
                    });
                }
            };
            DialogSettings = null;
        }

        private void EditarPO()
        {
            var mensajeValidacion = ValidarOrdenCompraProducto();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PutOrdenCompraProducto
            {
                OrdenCompraProducto = DatoOrdenCompraProducto,
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
                CargarSlidePanel("EditarPOBIomasaFlyout");
                CargarOrdenesCompraProducto();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarEditarOrdenCompraProducto()
        {
            if (OrdenCompraProductoSeleccionada == null) return;

            if (OrdenCompraProductoSeleccionada.Estado != Estados.NUEVO)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Information, "Solo puede editar Ordenes de Compra de Productos en estado NUEVOS");
                return;
            }

            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(OrdenCompraProductoSeleccionada.NombrePlanta, OrdenCompraProductoSeleccionada.PlantaId, null);

            DatoOrdenCompraProducto = new OrdenesCompraProductoDTO
            {
                OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId,
                OrdenCompraNo = OrdenCompraProductoSeleccionada.OrdenCompraNo,
                NoExoneracionDEI = OrdenCompraProductoSeleccionada.NoExoneracionDEI,
                FechaCreacion = OrdenCompraProductoSeleccionada.FechaCreacion,
                PlantaId = OrdenCompraProductoSeleccionada.PlantaId,
                NombrePlanta = OrdenCompraProductoSeleccionada.NombrePlanta,
                EsOrdenCompraActual = OrdenCompraProductoSeleccionada.EsOrdenCompraActual,
                FechaActivacion = OrdenCompraProductoSeleccionada.FechaActivacion,
                FechaCierre = OrdenCompraProductoSeleccionada.FechaCierre,
                CreadoPor = OrdenCompraProductoSeleccionada.CreadoPor,
                Estado = OrdenCompraProductoSeleccionada.Estado,
                MontoDollar = OrdenCompraProductoSeleccionada.MontoDollar,
                ConversionDollarToLps = OrdenCompraProductoSeleccionada.ConversionDollarToLps,
                MontoLPS = OrdenCompraProductoSeleccionada.MontoDollar * OrdenCompraProductoSeleccionada.ConversionDollarToLps
            };
            RaisePropertyChanged("DatoOrdenCompraProducto");

            CargarSlidePanel("EditarPOBIomasaFlyout");
        }

        private void CrearPO()
        {
            var mensajeValidacion = ValidarOrdenCompraProducto();

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, mensajeValidacion);
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new PostOrdenCompraProducto
            {
                OrdenCompraProducto = DatoOrdenCompraProducto,
                UsuarioId = InformacionSistema.UsuarioActivo.Usuario
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
                CargarSlidePanel("CrearPOBIomasaFlyout");
                CargarOrdenesCompraProducto();
            }, (r, ex) =>
            {
                MostrarVentanaEspera = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void MostrarCrearOrdenCompraProducto()
        {
            PlantaAutoCompleteSeleccionada = new AutoCompleteEntry(null, null);
            DatoOrdenCompraProducto = new OrdenesCompraProductoDTO
            {
                FechaCreacion = DateTime.Now
            };

            CargarSlidePanel("CrearPOBIomasaFlyout");
        }

        private string ValidarOrdenCompraProducto()
        {
            if (DatoOrdenCompraProducto.PlantaId == 0)
            {
                return "Debe indicar la Planta a la que se hará la Orden de Compra de Productos BIOMASA";
            }

            if (string.IsNullOrWhiteSpace(DatoOrdenCompraProducto.OrdenCompraNo))
            {
                return "Debe ingresar el Número de la PO";
            }

            if (string.IsNullOrWhiteSpace(DatoOrdenCompraProducto.NoExoneracionDEI))
            {
                return "Debe ingresar el Número de la Exoneración de la DEI para la PO";
            }

            if (string.IsNullOrWhiteSpace(DatoOrdenCompraProducto.CreadoPor))
            {
                return "Debe especificar quién está creadno la PO";
            }

            if (DatoOrdenCompraProducto.MontoDollar <= 0)
            {
                return "Debe ingresar Total en Dollares para la PO";
            }

            if (DatoOrdenCompraProducto.ConversionDollarToLps <= 0)
            {
                return "Debe ingresar La tasa de cambio del $ a Lps para la PO";
            }

            if (DatoOrdenCompraProducto.MontoDollar >= DatoOrdenCompraProducto.MontoLPS)
            {
                return "El Total en $ no puede ser mayor o igual al Total en Lps";
            }

            return string.Empty;
        }

        private void CalcularConversionDollarPO()
        {
            if (DatoOrdenCompraProducto.MontoDollar == 0) return;

            DatoOrdenCompraProducto.ConversionDollarToLps = DatoOrdenCompraProducto.MontoLPS / DatoOrdenCompraProducto.MontoDollar;
            RaisePropertyChanged("DatoOrdenCompraProducto");
        }

        private void CalcularMontoLpsPODetalle()
        {
            DatoPODetalleModel.PrecioLPS = Math.Round(DatoPODetalleModel.PrecioDollar * OrdenCompraProductoSeleccionada.ConversionDollarToLps, 4);
        }

        private void Navegar()
        {
            CargarOrdenesCompraProducto();
        }

        private void BuscarOrdenesCompraProducto()
        {
            NumeroPagina = 1;
            CargarOrdenesCompraProducto();
        }

        private void CargarOrdenesCompraProducto()
        {
            var ordenCompraProductoId = OrdenCompraProductoSeleccionada != null ? OrdenCompraProductoSeleccionada.OrdenCompraProductoId : 0;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetByValorOrdenesComprasProducto
            {
                PaginaActual = NumeroPagina,
                CantidadRegistros = 18,
                Filtro = ValorBusquedaOrdenCompraProducto                
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                BusquedaOrdenesCompraProductoControl = res;
                if (BusquedaOrdenesCompraProductoControl.Items != null && BusquedaOrdenesCompraProductoControl.Items.Any())
                {
                    if (ordenCompraProductoId == 0)
                    {
                        OrdenCompraProductoSeleccionada = BusquedaOrdenesCompraProductoControl.Items.FirstOrDefault();
                    }
                    else
                    {
                        OrdenCompraProductoSeleccionada = BusquedaOrdenesCompraProductoControl.Items.FirstOrDefault(r => r.OrdenCompraProductoId == ordenCompraProductoId);
                    }
                }
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarOrdenCompraProductoDetalle()
        {
            ListaOrdenesCompraProductoDetalle = new List<OrdenesCompraProductoDetalleDTO>();

            if (OrdenCompraProductoSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetOrdenesCompraProductoDetallePorPO
            {
                OrdenCompraProductoId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaOrdenesCompraProductoDetalle = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CargarOrdenCompraDetalleBoletas()
        {
            ListaOrdenesCompraDetalleBoletas = new List<OrdenesCompraDetalleBoletaDTO>();

            if (OrdenCompraProductoSeleccionada == null) return;

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetDetalleBoletasPorOrdenCompraId
            {
                OrdenCompraId = OrdenCompraProductoSeleccionada.OrdenCompraProductoId
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                ListaOrdenesCompraDetalleBoletas = res;

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
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

        private void CargarMaestroBiomasa()
        {
            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            var request = new GetAllMaestroBiomasa();

            _client.GetAsync(request, res =>
            {
                ListaMaestroBiomasa = res;

            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }

        private void InicializarPropiedades()
        {
            BusquedaOrdenesCompraProductoControl = new BusquedaOrdenesCompraProductoDTO();
            DatoOrdenCompraProducto = new OrdenesCompraProductoDTO();
        }

        private void CargarDatosIniciales()
        {
            CargarOrdenesCompraProducto();
            CargarMaestroBiomasa();
        }
        
        private void CargarDatosPruebas()
        {
            BusquedaOrdenesCompraProductoControl = new BusquedaOrdenesCompraProductoDTO
            {
                TotalRegistros = 23,
                Items = DatosDiseño.ListaOrdenesCompraProducto()
            };

            OrdenCompraProductoSeleccionada = BusquedaOrdenesCompraProductoControl.Items.FirstOrDefault();
        }        
    }
}
