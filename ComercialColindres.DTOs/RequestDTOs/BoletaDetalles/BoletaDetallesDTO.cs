using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaDetalles
{
    public class BoletaDetallesDTO : BaseDTO
    {
        public int PagoBoletaId { get; set; }
        public int CodigoBoleta { get; set; }
        public int DeduccionId { get; set; }
        public decimal MontoDeduccion { get; set; }
        public string NoDocumento { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; }

        public string DescripcionDeduccion { get; set; }
    }
}
