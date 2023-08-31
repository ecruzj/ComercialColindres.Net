using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class OpcionesUsuarioModel : ObservableObject
    {
        public int OpcionId { get; set; }
        public string Nombre { get; set; }
        public bool Seleccionada { get; set; }
        public int SucursalId { get; set; }
        public string TipoAcceso { get; set; }
    }
}
