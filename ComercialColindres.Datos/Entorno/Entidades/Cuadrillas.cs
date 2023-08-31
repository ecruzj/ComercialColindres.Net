using ComercialColindres.Datos.Clases;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Cuadrillas : IValidacionesEntidades
    {
        public Cuadrillas(string nombreEncargado, int plantaId, bool aplicaPago)
        {
            NombreEncargado = nombreEncargado;
            PlantaId = plantaId;
            AplicaPago = aplicaPago;
            Estado = Estados.ACTIVO;
            this.Descargadores = new List<Descargadores>();
            this.PagoDescargadores = new List<PagoDescargadores>();
        }

        protected Cuadrillas() { }

        public int CuadrillaId { get; set; }
        public string NombreEncargado { get; set; }
        public int PlantaId { get; set; }
        public bool AplicaPago { get; set; }
        public string Estado { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual ICollection<Descargadores> Descargadores { get; set; }
        public virtual ICollection<PagoDescargadores> PagoDescargadores { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreEncargado))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NombreEncargado");
                listaErrores.Add(mensaje);
            }

            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
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
