namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BonificacionProducto
    {
        public BonificacionProducto(ClientePlantas planta, CategoriaProductos categoriaProducto, bool isEnable)
        {
            this.ClientePlanta = planta;
            this.PlantaId = planta.PlantaId;
            this.CategoriaProducto = categoriaProducto;
            this.CategoriaProductoId = categoriaProducto.CategoriaProductoId;

            IsEnable = isEnable;
        }

        protected BonificacionProducto() { }

        public int BonificacionId { get; set; }
        public int PlantaId { get; set; }
        public int CategoriaProductoId { get; set; }
        public bool IsEnable { get; set; }

        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual CategoriaProductos CategoriaProducto { get; set; }
    }
}
