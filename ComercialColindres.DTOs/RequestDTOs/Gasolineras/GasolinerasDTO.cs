using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Gasolineras
{
    public class GasolinerasDTO : BaseDTO
    {
        public int GasolineraId { get; set; }
        public string Descripcion { get; set; }
        public string NombreContacto { get; set; }
        public string TelefonoContacto { get; set; }
    }
}
