using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class EquiposCategorias : IValidacionesEntidades
    {
        public EquiposCategorias(string descripcion)
        {
            Descripcion = descripcion;
            this.Equipos = new List<Equipos>();
            this.PrecioDescargas = new List<PrecioDescargas>();
        }

        protected EquiposCategorias() { }

        public int EquipoCategoriaId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Equipos> Equipos { get; set; }
        public virtual ICollection<PrecioDescargas> PrecioDescargas { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Descripcion");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
