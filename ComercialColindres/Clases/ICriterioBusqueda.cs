using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ComercialColindres.Clases
{
    public interface ICriterioBusqueda<T>
    {
        string ValorBusqueda { get; set; }
        ObservableCollection<T> DatosEncontrados { get; set; }
        ObservableCollection<DataGridColumn> ListadoColumnasBusqueda { get; set; }

        void Configurar();
    }
}
