using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado
{
    public class DescargasPorAdelantadoDTO : ResponseBase
    {
        public int DescargaPorAdelantadoId { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoBoleta { get; set; }
        public int PlantaId { get; set; }
        public int PagoDescargaId { get; set; }
        public string CreadoPor { get; set; }
        public decimal PrecioDescarga { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }

        public bool EsActualizacion { get; set; }
        public bool HasShippingNumber { get; set; }
    }
}
