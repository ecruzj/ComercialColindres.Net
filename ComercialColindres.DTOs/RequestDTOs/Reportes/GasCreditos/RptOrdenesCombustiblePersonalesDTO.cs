using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptOrdenesCombustiblePersonalesDTO : BaseDTO
    {
        public string CodigoFactura { get; set; }
        public string AutorizadoPor { get; set; }
        public string PlacaCarro { get; set; }
        public string NombreCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
