using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaCierres : Entity, IValidacionesEntidades
    {
        public BoletaCierres(int boletaId, string formaPago, int lineaCreditoId, string noDocumento, decimal monto, DateTime fechaPago)
        {
            BoletaId = boletaId;
            FormaDePago = formaPago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
            FechaPago = fechaPago;
        }

        protected BoletaCierres() { }

        public int BoletaCierreId { get; set; }
        public int BoletaId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual LineasCredito LineasCredito { get; set; }

        public void ActualizarBoletaCierre(string formaPago, int lineaCreditoId, string noDocumento, decimal monto, DateTime fechaPago)
        {
            FormaDePago = formaPago;
            LineaCreditoId = lineaCreditoId;
            NoDocumento = noDocumento ?? string.Empty;
            Monto = monto;
            FechaPago = fechaPago;
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

            if (FechaPago == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaPago");
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
