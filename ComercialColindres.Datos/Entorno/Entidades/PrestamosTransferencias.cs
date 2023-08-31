using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class PrestamosTransferencias : Entity, IValidacionesEntidades
    {
        public PrestamosTransferencias(int prestamosId, string formaDePago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            PrestamoId = prestamosId;
            FormaDePago = formaDePago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento;
            Monto = monto;
        }

        protected PrestamosTransferencias() { }

        public int PrestamoId { get; set; }
        public int PrestamoTransferenciaId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public virtual LineasCredito LineaCredito { get; set; }
        public virtual Prestamos Prestamo { get; set; }

        public void ActualizarTransferenciaPrestamo(string formaDePago, int lineaCreditoId, string noDocumento, decimal monto)
        {
            FormaDePago = formaDePago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento;
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
