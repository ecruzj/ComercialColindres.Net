using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs
{
    public class FacturaPagoDto : BaseDTO
    {
        public int FacturaPagoId { get; set; }
        public int FacturaId { get; set; }
        public string FormaDePago { get; set; }
        public int BancoId { get; set; }
        public string ReferenciaBancaria { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaDePago { get; set; }

        public string NombreBanco { get; set; }
    }
}
