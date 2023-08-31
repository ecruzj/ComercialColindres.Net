using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComercialColindres.Busquedas
{
    public class BusquedaProveedores : BaseVM, ICriterioBusqueda<ProveedoresDTO>
    {
        public BusquedaProveedores()
        {
            Configurar();
        }

        public string ValorBusqueda { get; set; }
        public ObservableCollection<ProveedoresDTO> DatosEncontrados
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
        ObservableCollection<ProveedoresDTO> _datosEncontrados;
        public ObservableCollection<DataGridColumn> ListadoColumnasBusqueda { get; set; }
        
        public void Configurar()
        {
            ListadoColumnasBusqueda = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn
                {
                    Header = "Nombre",
                    Binding = new Binding("Nombre")
                },
                new DataGridTextColumn
                {
                    Header = "Cedula",
                    Binding = new Binding("CedulaNo")
                }
            };
        }
    }
}
