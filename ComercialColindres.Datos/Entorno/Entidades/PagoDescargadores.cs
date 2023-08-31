using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class PagoDescargadores : Entity, IValidacionesEntidades
    {
        public PagoDescargadores(string codigoPagoDescarga, int cuadrillaId, string creadoPor, DateTime datePayment)
        {
            CodigoPagoDescarga = codigoPagoDescarga;
            CuadrillaId = cuadrillaId;
            CreadoPor = creadoPor;
            Estado = Estados.ACTIVO;
            FechaPago = datePayment;

            this.Descargadores = new List<Descargadores>();
            this.DescargasPorAdelantado = new List<DescargasPorAdelantado>();
            this.PagoDescargaDetalles = new List<PagoDescargaDetalles>();
        }

        protected PagoDescargadores() { }

        public int PagoDescargaId { get; set; }
        public string CodigoPagoDescarga { get; set; }
        public int CuadrillaId { get; set; }
        public decimal TotalPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public virtual Cuadrillas Cuadrilla { get; set; }
        public virtual ICollection<Descargadores> Descargadores { get; set; }
        public virtual ICollection<DescargasPorAdelantado> DescargasPorAdelantado { get; set; }
        public virtual ICollection<PagoDescargaDetalles> PagoDescargaDetalles { get; set; }
        public object Equipos { get; set; }

        public decimal ObtenerTotalPago()
        {
            return Math.Round(Descargadores.Where(d => !d.EsDescargaPorAdelanto).Sum(p => p.PagoDescarga) + DescargasPorAdelantado.Sum(p => p.PrecioDescarga), 2);
        }

        public decimal ObtenerPagoTotalJustificado()
        {
            return Math.Round(PagoDescargaDetalles.Sum(j => j.Monto), 2);
        }

        public int ObtenerTotalDescargas()
        {
            return Descargadores.Count() + DescargasPorAdelantado.Where(b => b.BoletaId == null).Count();
        }

        public void ActualizarEstadoPagoDescarga()
        {
            if (PagoDescargaDetalles != null && PagoDescargaDetalles.Any())
            {
                var totalPagar = Math.Round(ObtenerTotalPago(), 2);
                var totalAbono = Math.Round(PagoDescargaDetalles.Sum(c => c.Monto), 2);

                if (totalPagar == totalAbono)
                {
                    Estado = Estados.CERRADO;

                    foreach (var descarga in Descargadores)
                    {
                        descarga.Estado = Estados.CERRADO;
                    }
                }
                else
                {
                    Estado = Estados.ENPROCESO;
                }
            }
            else
            {
                Estado = Estados.ACTIVO;
            }
        }

        public void EliminarPagoDescargas()
        {
            foreach (var descarga in Descargadores)
            {
                descarga.PagoDescargaId = null;
                descarga.Estado = Estados.ACTIVO;
            }
        }

        public bool HasShippingNumber()
        {
            if (Cuadrilla == null) return false;
            ClientePlantas planta = Cuadrilla.ClientePlanta;

            return planta.IsShippingNumberRequired(); 
        }

        public void AddDescargaToPayment(Descargadores descarga, decimal pagoDescarga)
        {
            if (Descargadores == null) return;

            descarga.PagoDescargaId = PagoDescargaId;
            descarga.PagoDescarga = pagoDescarga;
            descarga.Estado = Estados.ENPROCESO;
            Descargadores.Add(descarga);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (CuadrillaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuadrillaId");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public void AddDescargaPorAdelanto(DescargasPorAdelantado descargaPorAdelantado)
        {
            if (DescargasPorAdelantado == null) return;

            DescargasPorAdelantado.Add(descargaPorAdelantado);
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();

            if (Estado == Estados.CERRADO)
            {
                var mensaje = "Ya esta Cerrado el Pago de Descargas";
                listaErrores.Add(mensaje);
            }

            if (Estado == Estados.ENPROCESO)
            {
                var mensaje = "Ya esta Cerrado el Pago de Descargas";
                listaErrores.Add(mensaje);
            }

            if (FechaPago == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(FechaPago));
                listaErrores.Add(mensaje);
            }

            if (PagoDescargaDetalles.Any())
            {
                var mensaje = "Antes debe Eliminar las Justificaciones de Pago de las Descargas";
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }
    }
}
