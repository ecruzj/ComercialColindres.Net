using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.NotasCredito
{
    public class NotaCreditoDto : BaseDTO
    {
        public int NotaCreditoId { get; set; }
        public int FacturaId { get; set; }
        public string NotaCreditoNo { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
    }
}
