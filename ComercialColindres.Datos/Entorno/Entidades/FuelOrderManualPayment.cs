using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class FuelOrderManualPayment : Entity, IValidacionesEntidades
    {
        public FuelOrderManualPayment(int fuelOrdermanualPayment, OrdenesCombustible fuelOrder, string wayToPay, Bancos bank, DateTime paymentDate, string bankReference, decimal amount, string observations)
        {
            FuelOrderManualPaymentId = fuelOrdermanualPayment;
            FuelOrder = FuelOrder;
            FuelOrderId = fuelOrder.OrdenCombustibleId;
            WayToPay = wayToPay;
            Bank = bank;
            BankId = bank?.BancoId ?? null;
            PaymentDate = paymentDate;
            BankReference = bankReference ?? string.Empty;
            Amount = amount;
            Observations = observations;
        }

        protected FuelOrderManualPayment() { }

        public int FuelOrderManualPaymentId { get; set; }
        public int FuelOrderId { get; set; }
        public string WayToPay { get; set; }
        public int? BankId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BankReference { get; set; }
        public decimal Amount { get; set; }
        public string Observations { get; set; }

        public virtual OrdenesCombustible FuelOrder { get; set; }
        public virtual Bancos Bank { get; set; }

        public void UpdateManualPayment(Bancos bank, string bankReference, DateTime paymentDate, string wayToPay, decimal amount, string observations)
        {
            this.Bank = bank;
            BankReference = bankReference;
            PaymentDate = paymentDate;
            WayToPay = wayToPay;
            Amount = amount;
            Observations = observations;
        }

        public string GetBankName()
        {
            if (Bank == null) return "N/A";

            return Bank.Descripcion;
        }
        public IEnumerable<string> GetValidationErrors()
        {
            List<string> errors = new List<string>();

            if (FuelOrderId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(FuelOrderId));
                errors.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(WayToPay))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(WayToPay));
                errors.Add(mensaje);
            }

            if (Bank != null)
            {
                if (BankId == null)
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(BankId));
                    errors.Add(mensaje);
                }

                if (string.IsNullOrWhiteSpace(BankReference))
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(BankReference));
                    errors.Add(mensaje);
                }
            }

            if (Amount <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Invalid_Value, nameof(Amount));
                errors.Add(mensaje);
            }

            if (PaymentDate == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_FechaValida, nameof(PaymentDate));
                errors.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Observations))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(Observations));
                errors.Add(mensaje);
            }

            return errors;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
