using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptCompraProductoBiomasaDetalleDto
    {
        public RptCompraProductoBiomasaDetalleDto()
        {
            CompraBiomasaDetalle = new List<CompraProductoBiomasaDetalleDato>();
        }

        public List<CompraProductoBiomasaDetalleDato> CompraBiomasaDetalle { get; set; }
        public decimal CompraTotal { get; set; }
        public decimal TotalToneladas { get; set; }
        public int TotalBoletas { get; set; }

        public void ObtenerCompraTotal()
        {
            if (CompraBiomasaDetalle == null) return;

            CompraTotal = CompraBiomasaDetalle.Sum(t => t.TotalCompra);
        }

        public void ObtenerTotalToneladas()
        {
            if (CompraBiomasaDetalle == null) return;

            TotalToneladas = CompraBiomasaDetalle.Sum(t => t.PesoProducto);
        }

        public void ObtenerTotalBoletas()
        {
            if (CompraBiomasaDetalle == null) return;

            TotalBoletas = CompraBiomasaDetalle.Count;
        }
    }

    public class CompraProductoBiomasaDetalleDato
    {
        public string Proveedor { get; set; }
        public string Planta { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string PlacaEquipo { get; set; }
        public string TipoProducto { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal TotalCompra { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
