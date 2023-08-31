using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class AjusteBoletaPago : Entity, IValidacionesEntidades
    {
        public AjusteBoletaPago(AjusteBoletaDetalle ajusteBoletaDetalle, Boletas boleta, decimal monto)
        {
            AjusteBoletaDetalle = ajusteBoletaDetalle;
            AjusteBoletaDetalleId = ajusteBoletaDetalle.AjusteBoletaDetalleId;
            Boleta = boleta;
            BoletaId = boleta.BoletaId;
            Monto = monto;

            FechaAbono = DateTime.Now;
        }

        protected AjusteBoletaPago() { }

        public int AjusteBoletaPagoId { get; set; }
        public int AjusteBoletaDetalleId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaAbono { get; set; }

        public virtual AjusteBoletaDetalle AjusteBoletaDetalle { get; set; }
        public virtual Boletas Boleta { get; set; }

        [NotMapped]
        public AjusteBoleta AjusteBoleta
        {
            get
            {
                return AjusteBoletaDetalle.AjusteBoleta;
            }
            private set { }
        }

        public void UpdateAjusteBoletaPago(decimal monto)
        {
            Monto = monto;
            FechaAbono = DateTime.Now;
        }

        public decimal GetPaymentsByDetail()
        {
            if (AjusteBoletaDetalle.AjusteBoletaPagos == null) return 0;

            return Math.Round(AjusteBoletaDetalle.AjusteBoletaPagos.Sum(p => p.Monto), 2);
        }
        
        public void ReactiveAdjustment()
        {
            AjusteBoleta.ReactiveStatus();
        }

        public string GetCodigoBoletaPayment()
        {
            if (Boleta == null) return string.Empty;

            return Boleta.CodigoBoleta;
        }

        public string GetNumeroEnvioPayment()
        {
            if (Boleta == null) return string.Empty;

            if (Boleta.IsShippingNumberRequired())
            {
                return Boleta.NumeroEnvio;
            }

            return "N/A";
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (AjusteBoletaDetalleId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "AjusteBoletaId");
                listaErrores.Add(mensaje);
            }

            if (BoletaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BoletaId");
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
