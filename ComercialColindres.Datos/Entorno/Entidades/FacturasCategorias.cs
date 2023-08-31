using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class FacturasCategorias
    {
        public FacturasCategorias()
        {
            this.Facturas = new List<Factura>();
        }

        public int FacturaCategoriaId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
