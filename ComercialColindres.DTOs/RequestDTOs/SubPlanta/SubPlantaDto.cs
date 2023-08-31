using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs
{
    public class SubPlantaDto : BaseDTO
    {
        public int SubPlantaId { get; set; }
        public int PlantaId { get; set; }
        public string NombreSubPlanta { get; set; }
        public string Rtn { get; set; }
        public string Direccion { get; set; }
        public bool IsExonerado { get; set; }
        public string RegistroExoneracion { get; set; }
    }
}
