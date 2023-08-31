using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class OrdenCombustibleImg : Entity, IValidacionesEntidades
    {
        public OrdenCombustibleImg(OrdenesCombustible ordenCombustible, byte[] imagen)
        {
            OrdenCombustible = OrdenCombustible;
            OrdenCombustibleId = ordenCombustible.OrdenCombustibleId;
            Imagen = imagen;
        }

        protected OrdenCombustibleImg() { }

        public int OrdenCombustibleId { get; set; }
        public byte[] Imagen { get; set; }

        public virtual OrdenesCombustible OrdenCombustible { get; set; }

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
            throw new NotImplementedException();
        }
    }
}
