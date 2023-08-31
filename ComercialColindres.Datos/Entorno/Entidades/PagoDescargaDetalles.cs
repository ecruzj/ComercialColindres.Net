using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class PagoDescargaDetalles : Entity, IValidacionesEntidades
    {

        public PagoDescargaDetalles(int pagoDescargaId, string formaPago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            PagoDescargaId = pagoDescargaId;
            FormaDePago = formaPago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
        }

        protected PagoDescargaDetalles() { }

        public int PagoDescargaDetalleId { get; set; }
        public int PagoDescargaId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public virtual LineasCredito LineaCredito { get; set; }
        public virtual PagoDescargadores PagoDescargador { get; set; }

        public void ActualizarPagoDescargaDetalle(string formaDePago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            FormaDePago = formaDePago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (LineaCreditoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "LineaCreditoId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(FormaDePago))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FormaDePago");
                listaErrores.Add(mensaje);
            }

            if (Monto <= 0)
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
