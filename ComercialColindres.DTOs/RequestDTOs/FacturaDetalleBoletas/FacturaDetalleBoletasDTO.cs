using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.FacturaDetalle
{
    public class FacturaDetalleBoletasDTO : BaseDTO
    {
        public int FacturaDetalleBoletaId { get; set; }
        public int FacturaId { get; set; }
        public int BoletaId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal UnitPrice { get; set; }

        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Motorista { get; set; }
        public decimal PesoProductoCompra { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaSalida { get; set; }
        public string PlacaEquipo { get; set; }
        public string DescripcionTipoProducto { get; set; }
        public string DescripcionTipoEquipo { get; set; }
        public string NombreProveedor { get; set; }
        public decimal Profit { get; set; }
        public bool HasErrorWeightOrPrice { get; set; }
    }
}
