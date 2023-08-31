using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    public class PagoOrdenesCombustibleDTO : BaseDTO
    {
        public int PagoOrdenCombustibleId { get; set; }
        public int OrdenCombustibleId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public int? BoletaId { get; set; }
        public decimal MontoAbono { get; set; }
        public string Observaciones { get; set; }
    }
}
