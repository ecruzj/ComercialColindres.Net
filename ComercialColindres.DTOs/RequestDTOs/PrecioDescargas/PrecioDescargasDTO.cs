using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.PrecioDescargas
{
    public class PrecioDescargasDTO : BaseDTO
    {
        public int PrecioDescargaId { get; set; }
        public int PlantaId { get; set; }
        public int EquipoCategoriaId { get; set; }
        public decimal PrecioDescarga { get; set; }
    }
}
