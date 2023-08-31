using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Bonificaciones
{
    public class BonificacionProductoDto : BaseDTO
    {
        public int BonifacionId { get; set; }
        public int PlantaId { get; set; }
        public int CategoriaProductoId { get; set; }
        public bool IsEnable { get; set; }
    }
}
