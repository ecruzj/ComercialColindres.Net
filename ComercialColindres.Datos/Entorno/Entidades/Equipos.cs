using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Equipos : IValidacionesEntidades
    {
        public Equipos(int categoriaId, int proveedorId, string placaCabezal)
        {
            EquipoCategoriaId = categoriaId;
            ProveedorId = proveedorId;
            PlacaCabezal = placaCabezal;
            Estado = Estados.ACTIVO;            
        }

        protected Equipos() { }

        public int EquipoId { get; set; }
        public int EquipoCategoriaId { get; set; }
        public int ProveedorId { get; set; }
        public string PlacaCabezal { get; set; }
        public string Estado { get; set; }
        
        public virtual Proveedores Proveedor { get; set; }
        public virtual EquiposCategorias EquiposCategoria { get; set; }

        public void ActualizarEquipo(int equipoCategoria, string placaCabezal)
        {
            EquipoCategoriaId = equipoCategoria;
            PlacaCabezal = placaCabezal;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (EquipoCategoriaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CategoriaEquipoId");
                listaErrores.Add(mensaje);
            }

            if (ProveedorId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ProveedorId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(PlacaCabezal))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlacaCabezal");
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
