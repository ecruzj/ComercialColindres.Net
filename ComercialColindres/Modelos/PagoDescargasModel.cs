using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class PagoDescargasModel : ObservableObject
    {
        public int CuadrillaId { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoBoleta { get; set; }
        public decimal PagoDescarga { get; set; }
    }
}
