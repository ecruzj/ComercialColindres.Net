using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad
{
    public class BoletaHumedadPagoDto : BaseDTO
    {
        public int BoletaHumedadPagoId { get; set; }
        public int BoletaId { get; set; }
        public int BoletaHumedadId { get; set; }

        public decimal TotalHumidityPayment { get; set; }
        public string NumeroEnvio { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
        public decimal PrecioProducto { get; set; }
        public string Motorista { get; set; }
        public string ProductoBiomasa { get; set; }
        public string PlacaCabezal { get; set; }
        public decimal Toneladas { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
