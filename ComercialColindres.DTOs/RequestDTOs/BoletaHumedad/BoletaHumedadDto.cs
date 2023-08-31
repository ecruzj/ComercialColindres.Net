using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs
{
    public class BoletaHumedadDto : BaseDTO
    {
        public int BoletaHumedadId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public int PlantaId { get; set; }
        public int ProveedorId { get; set; }
        public bool BoletaIngresada { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
        public DateTime FechaHumedad { get; set; }
        public string Estado { get; set; }

        public string NombrePlanta { get; set; }
        public string NombreProveedor { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal OutStandingPay { get; set; }
    }
}
