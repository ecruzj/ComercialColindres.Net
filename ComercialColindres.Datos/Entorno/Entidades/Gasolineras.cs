using ComercialColindres.Datos.Clases;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Gasolineras : IValidacionesEntidades
    {
        public Gasolineras(string descripcion, string nombreContacto, string telefonoContacto)
        {
            Descripcion = descripcion;
            NombreContacto = nombreContacto;
            TelefonoContacto = telefonoContacto;
            this.GasolineraCreditos = new List<GasolineraCreditos>();
        }

        protected Gasolineras() { }

        public int GasolineraId { get; set; }
        public string Descripcion { get; set; }
        public string NombreContacto { get; set; }
        public string TelefonoContacto { get; set; }
        public virtual ICollection<GasolineraCreditos> GasolineraCreditos { get; set; }

        public void ActualizarGasolinera(string descripcion, string nombreContacto, string telefonoContacto)
        {
            Descripcion = descripcion;
            NombreContacto = nombreContacto;
            TelefonoContacto = telefonoContacto;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Descripcion");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(NombreContacto))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NombreContacto");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(TelefonoContacto))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "TelefonoContacto");
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
