using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class PrecioProductos : Entity
    {
        public int PrecioProductoId { get; set; }
        public int CategoriaProductoId { get; set; }
        public int PlantaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool EsPrecioActual { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public virtual CategoriaProductos CategoriaProducto { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
    }
}
