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
    public partial class GasolineraCreditos : Entity, IValidacionesEntidades
    {
        public GasolineraCreditos(string codigoGasCredito, int gasolineraId, decimal credito, decimal saldo, DateTime fechaInicio, string creadoPor)
        {
            CodigoGasCredito = codigoGasCredito;
            GasolineraId = gasolineraId;
            Credito = credito;
            Saldo = saldo;
            FechaInicio = fechaInicio;
            FechaFinal = null;
            CreadoPor = creadoPor;
            EsCreditoActual = false;
            Estado = Estados.NUEVO;
            
            this.GasolineraCreditoPagos = new List<GasolineraCreditoPagos>();
            this.OrdenesCombustibles = new List<OrdenesCombustible>();
        }

        protected GasolineraCreditos() { }

        public int GasCreditoId { get; set; }
        public string CodigoGasCredito { get; set; }
        public int GasolineraId { get; set; }
        public decimal Credito { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public bool EsCreditoActual { get; set; }
        public virtual ICollection<GasolineraCreditoPagos> GasolineraCreditoPagos { get; set; }
        public virtual Gasolineras Gasolinera { get; set; }
        public virtual ICollection<OrdenesCombustible> OrdenesCombustibles { get; set; }

        public void ActualizarEstadoCredito()
        {
            if (GasolineraCreditoPagos.Any())
            {
                var totalAbono = Math.Round(GasolineraCreditoPagos.Sum(c => c.Monto), 2);

                if (Credito == totalAbono)
                {
                    Estado = Estados.ACTIVO;
                }
                else
                {
                    Estado = Estados.ENPROCESO;
                }
            }
            else
            {
                Estado = Estados.NUEVO;
            }
        }

        public void ActivarGasCredito(GasolineraCreditos ultimoCredito)
        {
            if (ultimoCredito != null)
            {
                ultimoCredito.Estado = Estados.CERRADO;
                ultimoCredito.FechaFinal = DateTime.Now;
                ultimoCredito.EsCreditoActual = false;
                Saldo = ((ultimoCredito.Credito + ultimoCredito.Saldo) - (ultimoCredito.OrdenesCombustibles.Sum(o => o.Monto)));
            }

            Estado = Estados.ACTIVO;
            EsCreditoActual = true;       
        }

        public decimal ObtenerSaldoActual()
        {
            return (Credito + Saldo) - OrdenesCombustibles.Sum(s => s.Monto);
        }

        public decimal ObtenerDebitoTotal()
        {
            return OrdenesCombustibles.Sum(s => s.Monto);
        }

        public void ActualizarGasCredito(decimal credito, DateTime fechaInicio)
        {
            Credito = credito;
            FechaInicio = fechaInicio;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(CodigoGasCredito))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoGasCredito");
                listaErrores.Add(mensaje);
            }

            if (GasolineraId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "GasolineraId");
                listaErrores.Add(mensaje);
            }

            if (Credito <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Credito");
                listaErrores.Add(mensaje);
            }

            if (FechaInicio == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaInicio");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();
            
            if (GasolineraCreditoPagos.Any())
            {
                var mensaje = "El Crédito ya tienen Pagos Justificados";
                listaErrores.Add(mensaje);
            }

            if (OrdenesCombustibles.Any())
            {
                var mensaje = "Existen Ordenes de Combustible asociadas al  Crédito de la Gasolinera";
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }        
    }
}
