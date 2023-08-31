using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias
{
    public class PrestamosTransferenciasDTO : BaseDTO
    {
        public int PrestamoTransferenciaId { get; set; }
        public int PrestamoId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public string DescripcionTransaccion { get; set; }
        public string ModificadoPor { get; set; }
        public System.DateTime FechaTransaccion { get; set; }

        public int BancoId { get; set; }
        public string NombreBanco { get; set; }
        public string CodigoLineaCredito { get; set; }
        public bool PuedeEditarCreditoDeduccion { get; set; }
    }
}
