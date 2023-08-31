using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class AjusteBoleta : Entity, IValidacionesEntidades
    {
        public AjusteBoleta(Boletas boleta)
        {
            Boleta = boleta;
            BoletaId = boleta.BoletaId;
            Estado = Estados.NUEVO;

            this.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>();
        }

        protected AjusteBoleta() { }

        public int AjusteBoletaId { get; set; }
        public int BoletaId { get; set; }
        public string Estado { get; set; }

        public virtual Boletas Boleta { get; set; }

        public virtual ICollection<AjusteBoletaDetalle> AjusteBoletaDetalles { get; set; }

        [NotMapped]
        public List<AjusteBoletaPago> AjusteBoletaPagos
        {
            get
            {
                return AjusteBoletaDetalles.SelectMany(p => p.AjusteBoletaPagos).ToList();
            }
            private set { }
        }

        public string GetNumeroEnvio()
        {
            if (Boleta == null) return string.Empty;

            return !string.IsNullOrEmpty(Boleta.NumeroEnvio) ? Boleta.NumeroEnvio : "N/A";
        }

        public decimal GetAjusteBoletaTotal()
        {
            if (AjusteBoletaDetalles == null) return 0;

            return Math.Round(AjusteBoletaDetalles.Sum(a => a.Monto), 2);
        }

        public decimal GetAjusteBoletaPayments()
        {
            if (AjusteBoletaDetalles == null) return 0;

            decimal payments = 0m;

            foreach (AjusteBoletaDetalle detail in AjusteBoletaDetalles)
            {
                payments += detail.GetTotalPayments();
            }

            return Math.Round(payments, 2);
        }

        internal void ReactiveStatus()
        {
            Estado = Estados.ENPROCESO;
        }

        public decimal GetAjusteBoletaPendingAmount()
        {
            decimal total = GetAjusteBoletaTotal();
            decimal payments = GetAjusteBoletaPayments();

            return Math.Round((total - payments), 2);
        }

        public void RemoveAjusteDetalle(AjusteBoletaDetalle ajusteDetalle)
        {
            if (AjusteBoletaDetalles == null) return;

            AjusteBoletaDetalles.Remove(ajusteDetalle);
        }

        public bool HasPayments()
        {
            if (AjusteBoletaDetalles == null) return false;

            return AjusteBoletaDetalles.Any(p => p.HasPayment());
        }

        public bool HasAjusteDetail()
        {
            if (AjusteBoletaDetalles == null) return false;

            return AjusteBoletaDetalles.Any();
        }

        public void AddAjusteBoletaDetalle(AjusteBoletaDetalle ajusteBoletaDetalle)
        {
            if (AjusteBoletaDetalles == null) return;

            AjusteBoletaDetalles.Add(ajusteBoletaDetalle);
        }

        public void UpdateStatus()
        {
            if (AjusteBoletaDetalles == null || !AjusteBoletaDetalles.Any())
            {
                Estado = Estados.NUEVO;
                return;
            }

            decimal ajusteTotal = GetAjusteBoletaTotal();
            decimal payments = GetAjusteBoletaPayments();
            bool boletaPaymentOpen = AjusteBoletaPagos.Any(b => b.Boleta.Estado != Estados.CERRADO);

            if ((ajusteTotal == payments) && !boletaPaymentOpen)
            {
                Estado = Estados.CERRADO;
                return;
            }

            if (payments > 0)
            {
                Estado = Estados.ENPROCESO;
                return;
            }

            Estado = Estados.ACTIVO;
        }
        
        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (BoletaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BoletaId");
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
