using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class GasolineraCreditoPagos : Entity, IValidacionesEntidades
    {
        public GasolineraCreditoPagos(int gasCreditoId, string formaPago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            GasCreditoId = gasCreditoId;
            FormaDePago = formaPago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
        }

        protected GasolineraCreditoPagos() { }

        public int GasCreditoPagoId { get; set; }
        public int GasCreditoId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public virtual LineasCredito LineaCredito { get; set; }
        public virtual GasolineraCreditos GasolineraCredito { get; set; }

        public void ActualizarCreditoPago(string formaPago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            FormaDePago = formaPago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(FormaDePago))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FormaDePago");
                listaErrores.Add(mensaje);
            }

            if (LineaCreditoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "LineaCreditoId");
                listaErrores.Add(mensaje);
            }

            if (FormaDePago == "Cheque" || FormaDePago == "Interbanca")
            {
                if (string.IsNullOrWhiteSpace(NoDocumento))
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoDocumento");
                    listaErrores.Add(mensaje);
                }
            }

            if (Monto == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
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
