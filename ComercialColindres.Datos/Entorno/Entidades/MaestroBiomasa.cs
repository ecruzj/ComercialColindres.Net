using ComercialColindres.Datos.Clases;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class MaestroBiomasa : IValidacionesEntidades
    {
        public MaestroBiomasa(string descripcionBiomasa)
        {
            this.Descripcion = descripcionBiomasa;
            this.CategoriaProductos = new List<CategoriaProductos>();
            this.OrdenesCompraProductoDetalles = new List<OrdenesCompraProductoDetalle>();
        }

        protected MaestroBiomasa() { }

        public int BiomasaId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<CategoriaProductos> CategoriaProductos { get; set; }
        public virtual ICollection<OrdenesCompraProductoDetalle> OrdenesCompraProductoDetalles { get; set; }

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
