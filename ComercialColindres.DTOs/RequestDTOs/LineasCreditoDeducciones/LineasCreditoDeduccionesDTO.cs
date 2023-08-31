using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones
{
    public class LineasCreditoDeduccionesDTO : BaseDTO
    {
        public int LineaCreditoDeduccionId { get; set; }
        public int LineaCreditoId { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public bool EsGastoOperativo { get; set; }
        public string NoDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public string CodigoLineaCredito { get; set; }
        public string FormaDePago { get; set; }
    }
}
