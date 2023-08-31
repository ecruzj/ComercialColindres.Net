using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Deducciones
    {
        public Deducciones()
        {
            this.BoletaDetalles = new List<BoletaDetalles>();
        }

        public int DeduccionId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<BoletaDetalles> BoletaDetalles { get; set; }
    }
}
