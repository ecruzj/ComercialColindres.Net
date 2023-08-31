using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class BoletaDetalles : Entity, IValidacionesEntidades
    {
        public BoletaDetalles(int boletaId, int deduccionId, decimal montoDeduccion, string noDocumento, string observaciones)
        {
            CodigoBoleta = boletaId;
            DeduccionId = deduccionId;
            MontoDeduccion = montoDeduccion;
            NoDocumento = noDocumento ?? string.Empty;
            Observaciones = observaciones ?? string.Empty;
        }

        public BoletaDetalles() { }
        
        public int PagoBoletaId { get; set; }
        public int CodigoBoleta { get; set; }
        public int DeduccionId { get; set; }
        public decimal MontoDeduccion { get; set; }
        public string NoDocumento { get; set; }
        public string Observaciones { get; set; }
        public string DescripcionDeduccion { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual Deducciones Deduccion { get; set; }

        public void ActualizarBoletaDetalle(int deduccionId, decimal montoDeduccion, string noDocumento, string observaciones)
        {
            DeduccionId = deduccionId;
            MontoDeduccion = montoDeduccion;
            NoDocumento = noDocumento ?? string.Empty;
            Observaciones = observaciones ?? string.Empty;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (CodigoBoleta == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BoletaId");
                listaErrores.Add(mensaje);
            }

            if (DeduccionId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "DeduccionId");
                listaErrores.Add(mensaje);
            }

            if (MontoDeduccion == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoDeduccion");
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
