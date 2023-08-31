using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class FacturaPago : Entity, IValidacionesEntidades
    {
        public FacturaPago(Factura invoice, Bancos bank, string formaDePago, string refNo, decimal monto, DateTime fechaDePago)
        {
            Invoice = invoice;
            FacturaId = invoice.FacturaId;
            Bank = bank;
            BancoId = bank.BancoId;
            FormaDePago = formaDePago ?? string.Empty;
            ReferenciaBancaria = refNo;
            Monto = monto;
            FechaDePago = fechaDePago;
        }

        protected FacturaPago() { }

        public int FacturaPagoId { get; set; }
        public int FacturaId { get; set; }
        public string FormaDePago { get; set; }
        public int BancoId { get; set; }
        public DateTime FechaDePago { get; set; }
        public string ReferenciaBancaria { get; set; }
        public decimal Monto { get; set; }

        public virtual Factura Invoice { get; set; }
        public virtual Bancos Bank { get; set; }

        public void UpdateInvoicePayment(Bancos bank, string referenciaBancaria, decimal monto)
        {
            Bank = bank;
            BancoId = bank.BancoId;
            ReferenciaBancaria = referenciaBancaria;
            Monto = monto;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            List<string> listaErrores = new List<string>();

            if (FacturaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FacturaId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(FormaDePago))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FormaDePago");
                listaErrores.Add(mensaje);
            }

            if (BancoId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BancoId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(ReferenciaBancaria))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ReferenciaBancaria");
                listaErrores.Add(mensaje);
            }

            if (Monto <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
                listaErrores.Add(mensaje);
            }

            if (FechaDePago == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaDePago");
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
