using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice
{
    public class RptBoletaWithOutInvoiceDto : BaseDTO
    {
        public string Planta { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public decimal PesoProducto { get; set; }
        public string Proveedor { get; set; }
        public string PlacaEquipo { get; set; }
        public string Producto { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaCreacionBoleta { get; set; }
    }
}
