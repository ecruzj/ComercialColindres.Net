using System.Collections.Generic;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaImg : Entity, IValidacionesEntidades
    {
        public BoletaImg(Boletas boleta, byte[] imagen)
        {
            Boleta = boleta;
            BoletaId = boleta.BoletaId;
            Imagen = imagen;
        }

        protected BoletaImg() { }

        public int BoletaId { get; set; }
        public byte[] Imagen { get; set; }

        public virtual Boletas Boleta { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (Imagen == null)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Imagen");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new System.NotImplementedException();
        }
    }
}
