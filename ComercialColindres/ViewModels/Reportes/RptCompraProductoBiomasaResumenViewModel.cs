using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCore.Reportes;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptCompraProductoBiomasaResumenViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptCompraProductoBiomasaResumenViewModel(IServiciosComunes serviciosComunes)
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
                    CompraProductoBiomasaResumen = new RptCompraProductoBiomasaResumenDto();
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
                    CompraProductoBiomasaResumen = new RptCompraProductoBiomasaResumenDto();
                }
            }
        }
        private DateTime _fechaFinal = DateTime.Now;
        
        public RptCompraProductoBiomasaResumenDto CompraProductoBiomasaResumen
        {
            get { return _compraProductoBiomasaResumen; }
            set
            {
                if (_compraProductoBiomasaResumen != value)
                {
                    _compraProductoBiomasaResumen = value;
                    RaisePropertyChanged(nameof(CompraProductoBiomasaResumen));
                }
            }
        }
        private RptCompraProductoBiomasaResumenDto _compraProductoBiomasaResumen;

        public RelayCommand ComandoObtenerInformacionReporte { get; set; }
        public RelayCommand ComandoImprimir { get; set; }

        private void InicializarComandos()
        {
            ComandoObtenerInformacionReporte = new RelayCommand(ObtenerInformacionReporte);
            ComandoImprimir = new RelayCommand(Imprimir);
        }

        private void ObtenerInformacionReporte()
        {
            CompraProductoBiomasaResumen = new RptCompraProductoBiomasaResumenDto();

            if (FechaInicio > FechaFinal)
            {
                _serviciosComunes.MostrarNotificacion(EventType.Warning, "La Fecha Final debe de ser mayor a la Fecha Inicial");
                return;
            }

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            var request = new GetCompraProductoResumen
            {
                FechaInicio = FechaInicio,
                FechaFinal = FechaFinal
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.CompraBiosaResumen.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                CompraProductoBiomasaResumen = new RptCompraProductoBiomasaResumenDto
                {
                    CompraBiosaResumen = new List<CompraProductoBiomasaResumenDato>(res.CompraBiosaResumen)
                };

                CompraProductoBiomasaResumen.ObtenerCompraTotal();
                RaisePropertyChanged(nameof(CompraProductoBiomasaResumen));

            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
        
        private void Imprimir()
        {
            if (CompraProductoBiomasaResumen == null) return;

            if (!CompraProductoBiomasaResumen.CompraBiosaResumen.Any())
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
                manejadorReporte.TituloReporte = "Reporte Compra de Producto Biomasa Resumen";
                manejadorReporte.RutaReporte = InformacionSistema.RutaReporte;
                manejadorReporte.ArchivoReporte = "RptCompraProductoBiomasaResumen.rdlc";
                manejadorReporte.NombreDataSet = "CompraProductoBiomasaResumen";
                manejadorReporte.Datos = CompraProductoBiomasaResumen.CompraBiosaResumen;
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
