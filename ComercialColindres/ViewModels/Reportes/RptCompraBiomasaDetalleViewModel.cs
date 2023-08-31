using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using ServiceStack.ServiceClient.Web;
using System;
using System.Linq;
using System.Windows;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptCompraBiomasaDetalleViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptCompraBiomasaDetalleViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }
        
        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set
            {
                if (_fechaInicio != value)
                {
                    _fechaInicio = value;
                    RaisePropertyChanged(nameof(FechaInicio));
                    CompraBiomasaDetalle = new RptCompraProductoBiomasaDetalleDto();
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
                    RaisePropertyChanged(nameof(FechaFinal));
                    CompraBiomasaDetalle = new RptCompraProductoBiomasaDetalleDto();
                }
            }
        }
        private DateTime _fechaFinal = DateTime.Now;
        
        public RptCompraProductoBiomasaDetalleDto CompraBiomasaDetalle
        {
            get { return _compraBiomasaDetalle; }
            set
            {
                _compraBiomasaDetalle = value;
                RaisePropertyChanged(nameof(CompraBiomasaDetalle));
            }
        }
        private RptCompraProductoBiomasaDetalleDto _compraBiomasaDetalle;

        public RelayCommand ComandoObtenerInformacionReporte { get; set; }
        public RelayCommand ComandoImprimir { get; set; }

        private void InicializarComandos()
        {
            ComandoObtenerInformacionReporte = new RelayCommand(ObtenerInformacionReporte);
            ComandoImprimir = new RelayCommand(Imprimir);
        }

        private void ObtenerInformacionReporte()
        {
            CompraBiomasaDetalle = new RptCompraProductoBiomasaDetalleDto();

            if (FechaInicio > FechaFinal)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Fecha Final debe de ser mayor a la Fecha Inicial");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);
            
            var request = new GetCompraProductoDetalle
            {
                FechaInicio = FechaInicio,
                FechaFinal = FechaFinal
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.CompraBiomasaDetalle.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                CompraBiomasaDetalle = new RptCompraProductoBiomasaDetalleDto
                {
                    CompraBiomasaDetalle = res.CompraBiomasaDetalle
                };

                CompraBiomasaDetalle.ObtenerCompraTotal();
                CompraBiomasaDetalle.ObtenerTotalBoletas();
                CompraBiomasaDetalle.ObtenerTotalToneladas();
                RaisePropertyChanged(nameof(CompraBiomasaDetalle));

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void Imprimir()
        {
            if (CompraBiomasaDetalle == null) return;

            if (!CompraBiomasaDetalle.CompraBiomasaDetalle.Any())
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
                manejadorReporte.TituloReporte = "Reporte Compra de Producto Biomasa Detalle";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "RptCompraProductoBiomasaDetalle.rdlc";
                manejadorReporte.NombreDataSet = "CompraBiomasaDetalle";
                manejadorReporte.Datos = CompraBiomasaDetalle.CompraBiomasaDetalle;
                manejadorReporte.AgregarParametro("FechaInicio", GetDateInLetters(FechaInicio));
                manejadorReporte.AgregarParametro("FechaFinal", GetDateInLetters(FechaFinal));
                manejadorReporte.MostarReporte();
            });
        }

        public static string GetDateInLetters(DateTime date)
        {
            string format = "MMM d yyyy"; // Use this format
            return date.ToString(format);
        }
    }
}
