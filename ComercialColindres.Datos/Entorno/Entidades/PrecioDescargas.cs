using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class PrecioDescargas : Entity
    {
        public int PrecioDescargaId { get; set; }
        public int PlantaId { get; set; }
        public int EquipoCategoriaId { get; set; }
        public decimal PrecioDescarga { get; set; }
        public bool EsPrecioActual { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual EquiposCategorias EquiposCategoria { get; set; }
    }
}
