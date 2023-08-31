using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Modelos;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptPrestamosPorProveedorViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptPrestamosPorProveedorViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }

        public bool FiltrarPorRangoFecha
        {
            get { return _filtrarPorRangoFecha; }
            set
            {
                if (_filtrarPorRangoFecha != value)
                {
                    _filtrarPorRangoFecha = value;
                    RaisePropertyChanged("FiltrarPorRangoFecha");
                }
            }
        }
        private bool _filtrarPorRangoFecha;

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set
            {
                if (_fechaInicio != value)
                {
                    _fechaInicio = value;
                    RaisePropertyChanged("FechaInicio");
                }
            }
        }
        private DateTime _fechaInicio = DateTime.Now;

        public DateTime FechaFinal
        {
            get { return _fechaFinal; }
            set
            {
                if (_fechaFinal != value)
                {
                    _fechaFinal = value;
                    RaisePropertyChanged("FechaFinal");
                }
            }
        }
        private DateTime _fechaFinal = DateTime.Now;
        
        public List<AutoCompleteEntry> Proveedores
        {
            get { return _proveedores; }
            set
            {
                _proveedores = value;
                RaisePropertyChanged("Proveedores");
            }
        }
        private List<AutoCompleteEntry> _proveedores;
        
        public AutoCompleteEntry ProveedorSeleccionado
        {
            get { return _proveedorSeleccionado; }
            set
            {
                _proveedorSeleccionado = value;
                RaisePropertyChanged("ProveedorSeleccionado");
            }
        }
        private AutoCompleteEntry _proveedorSeleccionado;
        
        public RptPrestamosPorProveedorResumenDTO ReportePrestamosPorProveedor
        {
            get { return _reportePrestamosPorProveedor; }
            set
            {
                _reportePrestamosPorProveedor = value;
                RaisePropertyChanged("ReportePrestamosPorProveedor");
            }
        }
        private RptPrestamosPorProveedorResumenDTO _reportePrestamosPorProveedor;
        
        public RelayCommand ComandoObtenerInformacionReporte { get; set; }
        public RelayCommand ComandoImprimir { get; set; }
        public RelayCommand<string> ComandoBuscarProveedores { get; private set; }

        void InicializarComandos()
        {
            ComandoBuscarProveedores = new RelayCommand<string>(BuscarProveedores);
            ComandoObtenerInformacionReporte = new RelayCommand(ObtenerInformacionReporte);
            ComandoImprimir = new RelayCommand(Imprimir);
        }
        
        private void BuscarProveedores(string valorBusqueda)
        {
            ReportePrestamosPorProveedor = new RptPrestamosPorProveedorResumenDTO();

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
                    Proveedores = (from reg in res
                                                select new AutoCompleteEntry(reg.Nombre, reg.ProveedorId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
        
        private void ObtenerInformacionReporte()
        {
            ReportePrestamosPorProveedor = new RptPrestamosPorProveedorResumenDTO();
            if (FechaInicio > FechaFinal)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Fecha Final debe de ser mayor a la Fecha Inicial");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetPrestamosPorProveedor
            {
                SucursalId = InformacionSistema.SucursalActiva.SucursalId,
                FechaInicio = FechaInicio,
                FechaFinal = FechaFinal,
                ProveedorId = ProveedorSeleccionado != null ? (int)ProveedorSeleccionado.Id : 0,
                FiltrarPorFechas = FiltrarPorRangoFecha
            };
            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.EncabezadoPrestamos.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                ReportePrestamosPorProveedor = new RptPrestamosPorProveedorResumenDTO
                {
                    EncabezadoPrestamos = res.EncabezadoPrestamos,
                    AbonosPorBoletas = res.AbonosPorBoletas
                };

                AsignarValoresPrestamos();

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void AsignarValoresPrestamos()
        {
            ReportePrestamosPorProveedor.TotalPrestamos = ReportePrestamosPorProveedor.ObtenerTotalPrestamos();
            ReportePrestamosPorProveedor.TotalAbonos = ReportePrestamosPorProveedor.ObtenerTotalAbonos();
            ReportePrestamosPorProveedor.SaldoPendiente = ReportePrestamosPorProveedor.ObtenerSaldoPendiente();

            RaisePropertyChanged("ReportePrestamosPorProveedor");
        }

        private void Imprimir()
        {
            if (ReportePrestamosPorProveedor == null) return;

            if (!ReportePrestamosPorProveedor.EncabezadoPrestamos.Any())
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                return;
            }

            ImprimirReporte();
        }

        private void ImprimirReporte()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var manejadorReporte = new ManejadorReportes(TipoReporte.RDLC);
                manejadorReporte.TituloReporte = "Reporte Prestamos por Proveedor";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "RptPrestamosPorProveedor.rdlc";
                manejadorReporte.MultipleDataSet = true;
                manejadorReporte.AgregarParametro("UserId", _serviciosComunes.GetRequestUserInfo().UserId);
                manejadorReporte.ListadoItemsDataSet = new List<ItemDataSetModel>
                {
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Encabezado",
                        Datos = ReportePrestamosPorProveedor.EncabezadoPrestamos
                    },
                    new ItemDataSetModel
                    {
                        NombreDataSet = "Detalle",
                        Datos = ReportePrestamosPorProveedor.AbonosPorBoletas
                    }
                };
                manejadorReporte.MostarReporte();
            });
        }
    }
}
