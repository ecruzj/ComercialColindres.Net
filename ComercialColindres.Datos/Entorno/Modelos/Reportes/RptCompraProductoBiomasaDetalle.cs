using System;

namespace ComercialColindres.Datos.Entorno.Modelos.Reportes
{
    public class RptCompraProductoBiomasaDetalle
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
