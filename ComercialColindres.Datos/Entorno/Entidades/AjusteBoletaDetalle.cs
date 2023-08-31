using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class AjusteBoletaDetalle : Entity, IValidacionesEntidades
    {
        public AjusteBoletaDetalle(AjusteBoleta ajusteBoleta, AjusteTipo ajusteTipo, decimal monto, LineasCredito lineaCredito, string noDocumento, string observaciones)
        {
            AjusteBoleta = ajusteBoleta;
            AjusteBoletaId = ajusteBoleta.BoletaId;
            AjusteTipo = ajusteTipo;
            AjusteTipoId = ajusteTipo.AjusteTipoId;
            Monto = monto;
            LineaCredito = lineaCredito;
            LineaCreditoId = lineaCredito != null ? lineaCredito.LineaCreditoId : (int?)null;
            NoDocumento = noDocumento ?? string.Empty;
            Observaciones = observaciones;
            FechaCreacion = DateTime.Now;

            this.AjusteBoletaPagos = new List<AjusteBoletaPago>();
        }

        protected AjusteBoletaDetalle() { }

        public int AjusteBoletaDetalleId { get; set; }
        public int AjusteBoletaId { get; set; }
        public int AjusteTipoId { get; set; }
        public decimal Monto { get; set; }
        public int? LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual AjusteBoleta AjusteBoleta { get; set; }
        public virtual AjusteTipo AjusteTipo { get; set; }
        public virtual LineasCredito LineaCredito { get; set; }
        public virtual ICollection<AjusteBoletaPago> AjusteBoletaPagos { get; set; }

        public void UpdateAjusteBoletaDetalle(AjusteTipo ajusteTipo, decimal monto, string observaciones)
        {
            AjusteTipo = ajusteTipo;
            AjusteTipoId = ajusteTipo.AjusteTipoId;
            Monto = monto;
            Observaciones = observaciones;
        }

        public bool HasPayment()
        {
            return AjusteBoletaPagos.Any();
        }

        public void AddAjusteBoletaPago(AjusteBoletaPago ajustePago)
        {
            if (AjusteBoletaPagos == null) return;

            AjusteBoletaPagos.Add(ajustePago);
        }

        public decimal GetTotalPayments()
        {
            return Math.Round(AjusteBoletaPagos.Sum(p => p.Monto), 2);
        }

        public bool IsBoletaInPayment(int boletaPaymentId)
        {
            if (AjusteBoletaPagos == null) return false;

            return AjusteBoletaPagos.Any(b => b.Boleta.BoletaId == boletaPaymentId);
        }

        public string GetStatus()
        {
            decimal totalPayment = GetTotalPayments();

            if (totalPayment == Monto)
            {
                return Estados.CERRADO;
            }

            if (HasPayment())
            {
                return Estados.ENPROCESO;
            }

            return Estados.ACTIVO;
        }

        public decimal GetSaldoPendiente()
        {
            decimal payments = GetTotalPayments();

            return Math.Round((Monto - payments), 2);
        }

        public string GetCadenaAjuste()
        {
            return $"{AjusteTipo.Descripcion} | {Observaciones}";
        }

        public string GetDocumentRef()
        {
            if (AjusteTipo.UseCreditLine)
            {
                return NoDocumento;
            }

            return "N/A";
        }

        public object GetCreditLine()
        {
            if (AjusteTipo.UseCreditLine)
            {
                return LineaCredito.CodigoLineaCredito;
            }

            return "N/A";
        }

        public void RemoveAjustePayment(AjusteBoletaPago ajustePayment)
        {
            if (AjusteBoletaPagos == null) return;

            AjusteBoletaPagos.Remove(ajustePayment);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (AjusteBoletaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "AjusteBoletaId");
                listaErrores.Add(mensaje);
            }

            if (AjusteTipoId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "AjusteTipoId");
                listaErrores.Add(mensaje);
            }

            if (AjusteTipo.UseCreditLine && LineaCreditoId == null)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "LineaCreditoId");
                listaErrores.Add(mensaje);
            }

            if (AjusteTipo.UseCreditLine && string.IsNullOrWhiteSpace(NoDocumento))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoDocumento");
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

