using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class NotaCredito : Entity, IValidacionesEntidades
    {
        public NotaCredito(Factura invoice, string notaCreditoNo, decimal monto, string observaciones)
        {
            Invoice = invoice;
            FacturaId = invoice.FacturaId;
            NotaCreditoNo = notaCreditoNo;
            Monto = monto;
            Observaciones = observaciones;
        }

        protected NotaCredito() { }

        public int NotaCreditoId { get; set; }
        public int FacturaId { get; set; }
        public string NotaCreditoNo { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }

        public virtual Factura Invoice { get; set; }

        public void UpdateNotaCredito(string notaCreditoNo, decimal monto, string observaciones)
        {
            NotaCreditoNo = notaCreditoNo;
            Monto = monto;
            Observaciones = observaciones;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (FacturaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FacturaId");
                listaErrores.Add(mensaje);
            }

            if (Monto <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Observaciones))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Observaciones");
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
