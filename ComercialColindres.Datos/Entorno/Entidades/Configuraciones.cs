using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Configuraciones
    {
        public Configuraciones()
        {
            this.ConfiguracionesDetalles = new List<ConfiguracionesDetalles>();
        }

        public string CodigoConfiguracion { get; set; }
        public string Descripcion { get; set; }
        public bool DefinidaPorUsuario { get; set; }
        public virtual ICollection<ConfiguracionesDetalles> ConfiguracionesDetalles { get; set; }
    }
}
