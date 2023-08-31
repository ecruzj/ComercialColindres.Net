using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaCierres
{
    public class BoletaCierresDTO : BaseDTO
    {
        public int BoletaCierreId { get; set; }
        public int BoletaId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string TipoTransaccion { get; set; }

        public string CodigoBoleta { get; set; }
        public int BancoId { get; set; }
        public string NombreBanco { get; set; }
        public string CodigoLineaCredito { get; set; }
        public bool PuedeEditarCreditoDeduccion { get; set; }
    }
}
