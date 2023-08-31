using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class ConductoresModel : ObservableObject
    {
        public int ConductorId { get; set; }
        public int ProveedorId { get; set; }
        public string Telefonos
        {
            get
            {
                return _telefonos;
            }
            set
            {
                _telefonos = value;
                RaisePropertyChanged("Telefonos");
            }
        }
        string _telefonos;
        public string Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                _nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }
        string _nombre;

        public bool EsNuevo
        {
            get
            {
                return _esnuevo;
            }
            set
            {
                _esnuevo = value;
                RaisePropertyChanged("EsNuevo");
            }
        }
        private bool _esnuevo;

    }
}
