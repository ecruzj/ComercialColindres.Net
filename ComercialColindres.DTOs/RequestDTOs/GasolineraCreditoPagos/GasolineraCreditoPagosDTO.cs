using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos
{
    public class GasolineraCreditoPagosDTO : BaseDTO
    {
        public int GasCreditoPagoId { get; set; }
        public int GasCreditoId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public int BancoId { get; set; }
        public string NombreBanco { get; set; }        
        public string CodigoLineaCredito { get; set; }
        public bool PuedeEditarCreditoDeduccion { get; set; }
    }
}
