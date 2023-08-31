using GalaSoft.MvvmLight.Command;
using MahApps.Metro;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Recursos;
using ComercialColindres.Views;
using ComercialColindres.Windows;
using WPFCore.Funciones;
using WPFCore.UtilidadesWindows;

namespace ComercialColindres.ViewModels
{
    public class MainViewModel : BaseVM
    {
        private readonly IServiciosComunes _serviciosComunes;
        LeerConfiguracion _configuracionManager = new LeerConfiguracion(@"C:\ComercialColindres\Config.ini");

        public MainViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            if (IsInDesignMode)
            {
                return;
            }
            ObtenerVersionApp();
            InicializarComandos();
            AsignarUri_ApiService();
            CambiarAparienciaAplicacion();
            CargarSucursales();
        }

        public string VersionAplicacion
        {
            get
            {
                return _versionAplicacion;
            }
            set
            {
                _versionAplicacion = value;
                RaisePropertyChanged("VersionAplicacion");
            }
        }
        string _versionAplicacion;

        public string AnimacionActual
        {
            get { return _animacionActual; }
            set
            {
                if (_animacionActual != value)
                {
                    _animacionActual = value;
                    RaisePropertyChanged("AnimacionActual");
                }
            }
        }
        private string _animacionActual;

        public Visibility ControlEsperaEntrandoAlSistema
        {
            get { return _controlEsperaEntrandoAlSistema; }
            set
            {
                if (_controlEsperaEntrandoAlSistema != value)
                {
                    _controlEsperaEntrandoAlSistema = value;
                    RaisePropertyChanged("ControlEsperaEntrandoAlSistema");
                }
            }
        }
        private Visibility _controlEsperaEntrandoAlSistema = Visibility.Collapsed;

        public string Usuario
        {
            get { return _usuario; }
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    RaisePropertyChanged("Usuario");
                }
            }
        }
        private string _usuario;

        public string Clave
        {
            get { return _clave; }
            set
            {
                if (_clave != value)
                {
                    _clave = value;
                    RaisePropertyChanged("Clave");
                }
            }
        }
        private string _clave;

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


        public RelayCommand ComandoEntrarSistema { get; set; }
        public RelayCommand<string> ComandoEntrarOpcion { get; set; }

        void InicializarComandos()
        {
            ComandoEntrarSistema = new RelayCommand(EntrarSistema);
            ComandoEntrarOpcion = new RelayCommand<string>(EntrarOpcion);
        }

        void EntrarOpcion(string opcion)
        {
            switch (opcion)
            {
                case "Proveedores":
                    {
                        Window ventana1;
                        ventana1 = new ProveedoresWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Usuarios":
                    {
                        Window ventana1;
                        ventana1 = new UsuariosWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Boletas":
                    {
                        Window ventana1;
                        ventana1 = new BoletasWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Prestamos":
                    {
                        Window ventana1;
                        ventana1 = new PrestamosWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Descargadores":
                    {
                        Window ventana1;
                        ventana1 = new DescargadoresWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Gasolineras":
                    {
                        Window ventana1;
                        ventana1 = new GasolinerasWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Facturas":
                    {
                        Window ventana1;
                        ventana1 = new FacturasWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "LineasCredito":
                    {
                        Window ventana1;
                        ventana1 = new LineasCreditoWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "OrdenesCompraProducto":
                    {
                        Window ventana1;
                        ventana1 = new OrdenesCompraProductoWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Reportes":
                    {
                        Window ventana1;
                        ventana1 = new ReportesWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "MonitorBoletasInProcess":
                    {
                        Window ventana1;
                        ventana1 = new MonitorBoletasInProcess();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "Humedades":
                    {
                        Window ventana1;
                        ventana1 = new BoletasHumedadWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
                case "AjusteBoletas":
                    {
                        Window ventana1;
                        ventana1 = new AjusteBoletasWindow();
                        ventana1.Closed += (o, args) =>
                        {
                            ((MetroWindowExt)ventana1).ShowFlyout = null;
                            ventana1 = null;
                        };
                        if (ventana1.IsVisible)
                            ventana1.Hide();
                        else
                            ventana1.Show();
                        break;
                    }
            }
        }

        void EntrarSistema()
        {
            ControlEsperaEntrandoAlSistema = Visibility.Visible;
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);

            var request = new GetUsuario
            {
                Usuario = Usuario,
                Clave = Clave,
                SucursalId = SucursalSeleccionada.SucursalId
            };

            var respuesta = client.Get(request);
            ControlEsperaEntrandoAlSistema = Visibility.Collapsed;
            if (!string.IsNullOrWhiteSpace(respuesta.MensajeError))
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, respuesta.MensajeError);
                return;
            }
            InformacionSistema.UsuarioActivo = respuesta;
            InformacionSistema.SucursalActiva = SucursalSeleccionada;
            Animar("MenuStoryboard");
            CargarMenuPrincipal();
        }

        static void CargarMenuPrincipal()
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
            {
                var control = Application.Current.MainWindow as MainWindow;
                if (control != null)
                {
                    control.ScrollViewer.Content = new MenuPrincipalView();
                }
            }));
        }

        void AsignarUri_ApiService()
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (path == @"file:\C:\dev\ComercialColindres\ComercialColindres\bin\Debug")
            {
                InformacionSistema.Uri_ApiService = Configuracion.Uri_ApiServiceLocal;
                InformacionSistema.RutaReporte = @"C:\dev\ComercialColindres\ComercialColindres.Reportes\Reportes";
            }
            else
            {
                var uriApiServer = _configuracionManager.ObtenerValorConfiguracion("ApiServer");
                InformacionSistema.Uri_ApiService = uriApiServer + "/api/";

                var pathReports = _configuracionManager.ObtenerValorConfiguracion("ReportFilePath");
                InformacionSistema.RutaReporte = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "/ReportFiles/";
            }
        }

        void ObtenerVersionApp()
        {
            try
            {
                VersionAplicacion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch
            {
                var sumaDia = DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Month;
                var usuarioActualAuto = string.Format("{0}", sumaDia);
                var claveActualAuto = DateTime.Now.Year - DateTime.Now.Day - DateTime.Now.Month;
                VersionAplicacion = string.Format("{0} / {1}", usuarioActualAuto, claveActualAuto);
            }
        }

        void Animar(string animacion)
        {
            AnimacionActual = string.Empty;
            AnimacionActual = animacion;
        }

        void CambiarAparienciaAplicacion()
        {
            //var theme = ThemeManager.DetectAppStyle(Application.Current);
            //var appTheme = ThemeManager.GetAppTheme("BaseDark");            
            var appTheme = ThemeManager.GetAppTheme("BaseLight");
            var accent = ThemeManager.GetAccent("Green");
            //ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);            
            ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
        }

        private void CargarSucursales()
        {
            ControlEsperaEntrandoAlSistema = Visibility.Visible;

            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);

            var request = new GetAllSucursales
            {
            };
            client.GetAsync(request, res =>
            {
                ControlEsperaEntrandoAlSistema = Visibility.Collapsed;
                Sucursales = new List<SucursalesDTO>(res);
                SucursalSeleccionada = Sucursales.FirstOrDefault();
            }, (r, ex) => _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message));
        }
    }
}
