using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Cuadrillas
{
    public class CuadrillasDTO : BaseDTO
    {
        public int CuadrillaId { get; set; }
        public string NombreEncargado { get; set; }
        public int PlantaId { get; set; }
        public bool AplicaPago { get; set; }
        public string Estado { get; set; }

        public string NombrePlanta { get; set; }
    }
}
