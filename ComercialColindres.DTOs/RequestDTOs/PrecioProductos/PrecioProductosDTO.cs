using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.PrecioProductos
{
    public class PrecioProductosDTO : BaseDTO
    {
        public int PrecioProductoId { get; set; }
        public int CategoriaProductoId { get; set; }
        public int PlantaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool EsPrecioActual { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    }
}
