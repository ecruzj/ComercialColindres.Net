using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto
{
    public class OrdenesCompraProductoDTO : BaseDTO
    {
        public int OrdenCompraProductoId { get; set; }
        public string OrdenCompraNo { get; set; }
        public string NoExoneracionDEI { get; set; }
        public int PlantaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public bool EsOrdenCompraActual { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string CreadoPor { get; set; }
        public decimal MontoDollar { get; set; }
        public decimal ConversionDollarToLps { get; set; }

        public string NombrePlanta { get; set; }
        public decimal PorcentajeCumplimiento { get; set; }
        public decimal MontoLPS { get; set; }

        //Detalle Biomasa
        public decimal TotalDetalleDollares { get; set; }
        public decimal TotalDetalleLps { get; set; }
    }
}
