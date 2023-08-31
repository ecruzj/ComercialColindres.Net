using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones
{
    public class BoletaOtrasDeduccionesDTO : BaseDTO
    {
        public int BoletaOtraDeduccionId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public string MotivoDeduccion { get; set; }
        public string FormaDePago { get; set; }
        public int? LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public bool EsDeduccionManual { get; set; }

        public string CodigoBoleta { get; set; }
        public string CodigoLineaCredito { get; set; }
        public int BancoId { get; set; }
        public string NombreBanco { get; set; }
        public bool PuedeEliminarBoletaOtraDeduccion { get; set; }
        public decimal TotalBoletaOtrasDeducciones { get; set; }
    }
}
