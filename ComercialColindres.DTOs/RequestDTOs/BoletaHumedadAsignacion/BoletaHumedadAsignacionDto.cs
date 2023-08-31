using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion
{
    public class BoletaHumedadAsignacionDto : BaseDTO
    {
        public int BoletaHumedadAsignacionId { get; set; }
        public int BoletaHumedadId { get; set; }
        public int BoletaId { get; set; }

        public string NumeroEnvio { get; set; }
        public string Estado { get; set; }
        public decimal OutStandingPay { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
        public string NombrePlanta { get; set; }
    }
}
