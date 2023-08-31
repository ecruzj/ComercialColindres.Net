using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class CuentasBancariasModel : ObservableObject
    {
        public int CuentaId { get; set; }
        public int ProveedorId { get; set; }
        public int BancoId
        {
            get
            {
                return _bancoId;
            }
            set
            {
                {
                    _bancoId = value;
                    RaisePropertyChanged("BancoId");
                }
            }
        }
        int _bancoId;
        public string CuentaNo
        {
            get
            {
                return _cuentaNo;
            }
            set
            {
                _cuentaNo = value;
                RaisePropertyChanged("CuentaNo");
            }
        }
        string _cuentaNo;
        public string NombreAbonado
        {
            get
            {
                return _nombreAbonado;
            }
            set
            {
                _nombreAbonado = value;
                RaisePropertyChanged("NombreAbonado");
            }
        }
        string _nombreAbonado;
        public string CedulaNo
        {
            get
            {
                return _cedulaNo;
            }
            set
            {
                _cedulaNo = value;
                RaisePropertyChanged("CedulaNo");
            }
        }
        string _cedulaNo;
        public string Estado { get; set; }

        public string NombreBanco
        {
            get
            {
                return _nombreBanco;
            }
            set
            {
                _nombreBanco = value;
                RaisePropertyChanged("NombreBanco");
            }
        }
        string _nombreBanco;
    }
}
