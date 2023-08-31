using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos
{
    public class AjusteBoletaPagoDto : BaseDTO
    {
        public int AjusteBoletaPagoId { get; set; }
        public int AjusteBoletaDetalleId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaAbono { get; set; }

        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Planta { get; set; }

    }
}
