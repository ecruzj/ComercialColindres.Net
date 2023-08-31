using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual
{
    public class BoletaDeduccionManualDto : BaseDTO
    {
        public int DeduccionManualId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public string MotivoDeduccion { get; set; }
    }
}
