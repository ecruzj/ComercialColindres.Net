using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class EquiposModel : ObservableObject
    {
        public int EquipoId { get; set; }
        public int ProveedorId { get; set; }
        public int EquipoCategoriaId
        {
            get
            {
                return _equipoCategoriaId;
            }
            set
            {
                _equipoCategoriaId = value;
                RaisePropertyChanged("EquipoCategoriaId");
            }
        }
        int _equipoCategoriaId;
        public string PlacaCabezal
        {
            get
            {
                return _placaCabezal;
            }
            set
            {
                _placaCabezal = value;
                RaisePropertyChanged("PlacaCabezal");
            }
        }
        string _placaCabezal;
        public string Estado { get; set; }

        public string DescripcionCategoria
        {
            get
            {
                return _descripconCategoria;
            }
            set
            {
                _descripconCategoria = value;
                RaisePropertyChanged("DescripcionCategoria");
            }
        }
        string _descripconCategoria;
    }
}
