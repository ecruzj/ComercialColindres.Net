using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Conductores : IValidacionesEntidades
    {
        public Conductores(string nombreConductor, int proveedorId, string telefonos)
        {
            Nombre = nombreConductor;
            ProveedorId = proveedorId;
            Telefonos = telefonos ?? string.Empty;
        }

        protected Conductores() { }

        public int ConductorId { get; set; }
        public string Nombre { get; set; }
        public int ProveedorId { get; set; }
        public string Telefonos { get; set; }
        public Proveedores Proveedor { get; set; }

        public void ActualizarConductor(string nombre, string telefonos)
        {
            Nombre = nombre;
            Telefonos = telefonos ?? string.Empty;
        }
        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Nombre");
                listaErrores.Add(mensaje);
            }

            if (ProveedorId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ProveedorId");
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
