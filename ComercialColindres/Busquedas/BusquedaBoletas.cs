using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComercialColindres.Busquedas
{
    public class BusquedaBoletas : BaseVM, ICriterioBusqueda<BoletasDTO>
    {
        public BusquedaBoletas()
        {
            Configurar();
        }

        public string ValorBusqueda { get; set; }
        public ObservableCollection<BoletasDTO> DatosEncontrados
        {
            get
            {
                return _datosEncontrados;
            }
            set
            {
                _datosEncontrados = value;
                RaisePropertyChanged(nameof(DatosEncontrados));
            }
        }
        ObservableCollection<BoletasDTO> _datosEncontrados;
        public ObservableCollection<DataGridColumn> ListadoColumnasBusqueda { get; set; }

        public void Configurar()
        {
            ListadoColumnasBusqueda = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn
                {
                    Header = "#Boleta",
                    Binding = new Binding("CodigoBoleta")
                },
                new DataGridTextColumn
                {
                    Header = "#Envio",
                    Binding = new Binding("NumeroEnvio")
                },
                new DataGridTextColumn
                {
                    Header = "Producto",
                    Binding = new Binding("DescripcionTipoProducto")
                },
                new DataGridTextColumn
                {
                    Header = "Proveedor",
                    Binding = new Binding("NombreProveedor")
                },
                new DataGridTextColumn
                {
                    Header = "Planta",
                    Binding = new Binding("NombrePlanta")
                }
            };
        }
    }
}
