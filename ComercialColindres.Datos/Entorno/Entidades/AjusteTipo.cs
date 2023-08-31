using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class AjusteTipo
    {
        public AjusteTipo(string descripcion)
        {
            Descripcion = descripcion;

            this.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>();
        }

        protected AjusteTipo() { }

        public int AjusteTipoId { get; set; }
        public string Descripcion { get; set; }
        public bool UseCreditLine { get; set; }

        public virtual ICollection<AjusteBoletaDetalle> AjusteBoletaDetalles { get; set; }
    }
}
