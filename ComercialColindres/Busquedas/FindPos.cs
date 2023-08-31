using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComercialColindres.Busquedas
{
    public class FindPos : BaseVM, ICriterioBusqueda<OrdenesCompraProductoDTO>
    {
        public FindPos()
        {
            Configurar();
        }

        public string ValorBusqueda { get; set; }
        public ObservableCollection<OrdenesCompraProductoDTO> DatosEncontrados
        {
            get
            {
                return _datosEncontrados;
            }
            set
            {
                _datosEncontrados = value;
                RaisePropertyChanged("DatosEncontrados");
            }
        }
        ObservableCollection<OrdenesCompraProductoDTO> _datosEncontrados;
        public ObservableCollection<DataGridColumn> ListadoColumnasBusqueda { get; set; }

        public void Configurar()
        {
            ListadoColumnasBusqueda = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn
                {
                    Header = "OrdenCompra",
                    Binding = new Binding("OrdenCompraNo")
                },
                new DataGridTextColumn
                {
                    Header = "#Exoneracion",
                    Binding = new Binding("NoExoneracionDEI")
                }
            };
        }
    }
}
