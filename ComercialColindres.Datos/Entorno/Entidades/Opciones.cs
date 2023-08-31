using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Opciones : IValidacionesEntidades
    {
        public Opciones(string nombre, string tipoAcceso, string tipoPropiedad)
        {
            Nombre = nombre;
            TipoAcceso = tipoAcceso;
            TipoPropiedad = tipoPropiedad;

            this.UsuariosOpciones = new List<UsuariosOpciones>();
        }

        protected Opciones() { }

        public int OpcionId { get; set; }
        public string Nombre { get; set; }
        public string TipoAcceso { get; set; }
        public string TipoPropiedad { get; set; }
        public virtual ICollection<UsuariosOpciones> UsuariosOpciones { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var validaciones = new List<string>();
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Nombre de la Opcion");
                validaciones.Add(mensaje);
            }
            return validaciones;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var validaciones = new List<string>();
            if (UsuariosOpciones.Any())
            {

                validaciones.Add("Existen Usuarios que tienen asignada esta opcion");
            }
            return validaciones;
        }
    }
}
