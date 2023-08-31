using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class LineasCreditoDeducciones : Entity, IValidacionesEntidades
    {
        public LineasCreditoDeducciones(int lineaCreditoId, string descripcion, decimal monto, bool esGastoOperativo, string noDocumento, 
                                        DateTime fechaCreacion, bool requiereBanco)
        {
            this.LineaCreditoId = lineaCreditoId;
            this.Descripcion = descripcion;
            this.Monto = monto;
            this.EsGastoOperativo = esGastoOperativo;
            this.NoDocumento = noDocumento ?? string.Empty;
            this.FechaCreacion = fechaCreacion;
            this.RequiereBanco = requiereBanco;
        }

        protected LineasCreditoDeducciones() { }

        public int LineaCreditoDeduccionId { get; set; }
        public int LineaCreditoId { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public bool EsGastoOperativo { get; set; }
        public string NoDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool RequiereBanco { get; set; }
        public virtual LineasCredito LineasCredito { get; set; }

        public void ActualizarDeduccionLineaCredito(string descripcion, decimal monto, string noDocumento, DateTime fechaCreacion, bool requiereBanco)
        {
            this.Descripcion = descripcion;
            this.Monto = monto;
            this.NoDocumento = noDocumento ?? string.Empty;
            this.FechaCreacion = fechaCreacion;
            this.RequiereBanco = requiereBanco;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (LineaCreditoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "LineaCreditoId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Descripcion");
                listaErrores.Add(mensaje);
            }

            if (Monto <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
                listaErrores.Add(mensaje);
            }

            if (DateTime.MinValue == FechaCreacion)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaCreacion");
                listaErrores.Add(mensaje);
            }

            if (RequiereBanco)
            {
                if (string.IsNullOrWhiteSpace(NoDocumento))
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoDocumento");
                    listaErrores.Add(mensaje);
                }
            }
            
            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }        
    }
}
