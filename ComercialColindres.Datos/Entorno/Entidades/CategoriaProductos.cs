using ComercialColindres.Datos.Clases;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class CategoriaProductos : IValidacionesEntidades
    {
        public CategoriaProductos(int biomasaId, string descripcion)
        {
            this.BiomasaId = biomasaId;
            this.Descripcion = descripcion;
            this.Boletas = new List<Boletas>();
            this.PrecioProductos = new List<PrecioProductos>();
            this.FacturaDetalleItems = new List<FacturaDetalleItem>();
            this.BonificacionesProducto = new List<BonificacionProducto>();
        }

        protected CategoriaProductos() { }

        public int CategoriaProductoId { get; set; }
        public int BiomasaId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Boletas> Boletas { get; set; }
        public virtual MaestroBiomasa MaestroBiomasa { get; set; }
        public virtual ICollection<PrecioProductos> PrecioProductos { get; set; }
        public virtual ICollection<FacturaDetalleItem> FacturaDetalleItems { get; set; }
        public virtual ICollection<BonificacionProducto> BonificacionesProducto { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (BiomasaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BiomasaId");
                listaErrores.Add(mensaje);
            }

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
