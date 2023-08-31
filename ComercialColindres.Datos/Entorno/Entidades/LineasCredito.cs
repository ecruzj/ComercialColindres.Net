using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class LineasCredito : Entity, IValidacionesEntidades
    {
        public LineasCredito(string codigoLineaCredito, int sucursalId, int cuentaFinancieraId, string noDocumento, string observaciones, DateTime fechaCreacion, decimal montoInicial,
                             decimal saldo, string creadoPor)
        {
            this.CodigoLineaCredito = codigoLineaCredito;
            this.SucursalId = sucursalId;
            this.CuentaFinancieraId = cuentaFinancieraId;
            this.NoDocumento = noDocumento;
            this.Observaciones = observaciones;
            this.FechaCreacion = fechaCreacion;
            this.MontoInicial = montoInicial;
            this.Saldo = saldo;
            this.CreadoPor = creadoPor;
            this.EsLineaCreditoActual = false;
            this.Estado = Estados.NUEVO;
            this.BoletaCierres = new List<BoletaCierres>();
            this.BoletaOtrasDeducciones = new List<BoletaOtrasDeducciones>();
            this.GasolineraCreditoPagos = new List<GasolineraCreditoPagos>();
            this.LineasCreditoDeducciones = new List<LineasCreditoDeducciones>();
            this.PagoDescargaDetalles = new List<PagoDescargaDetalles>();
            this.PagoOrdenesCombustibles = new List<PagoOrdenesCombustible>();
            this.PrestamoTransferencias = new List<PrestamosTransferencias>();
            this.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>();
        }

        protected LineasCredito() { }

        public int LineaCreditoId { get; set; }
        public string CodigoLineaCredito { get; set; }
        public int SucursalId { get; set; }
        public int CuentaFinancieraId { get; set; }
        public string Estado { get; set; }
        public string NoDocumento { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal Saldo { get; set; }
        public string CreadoPor { get; set; }
        public bool EsLineaCreditoActual { get; set; }
        public virtual ICollection<BoletaCierres> BoletaCierres { get; set; }
        public virtual ICollection<BoletaOtrasDeducciones> BoletaOtrasDeducciones { get; set; }
        public virtual CuentasFinancieras CuentasFinanciera { get; set; }
        public virtual ICollection<GasolineraCreditoPagos> GasolineraCreditoPagos { get; set; }
        public virtual Sucursales Sucursal { get; set; }
        public virtual ICollection<LineasCreditoDeducciones> LineasCreditoDeducciones { get; set; }
        public virtual ICollection<PagoDescargaDetalles> PagoDescargaDetalles { get; set; }
        public virtual ICollection<PagoOrdenesCombustible> PagoOrdenesCombustibles { get; set; }
        public virtual ICollection<PrestamosTransferencias> PrestamoTransferencias { get; set; }
        public virtual ICollection<AjusteBoletaDetalle> AjusteBoletaDetalles { get; set; }

        public string AnularLineaCredito()
        {
            if (EsLineaCreditoActual)
            {
                return "No puede eliminar la Linea de Crédito porque es Crédito Actual";
            }

            if (Estado != Estados.NUEVO)
            {
                return "El Estado de la Linea de Crédito debe ser NUEVO";
            }

            Estado = Estados.ANULADO;

            return string.Empty;
        }

        public void ActivarLineaCredito(LineasCredito ultimaLineaCredito)
        {
            if (ultimaLineaCredito != null)
            {
                ultimaLineaCredito.Estado = Estados.CERRADO;
                ultimaLineaCredito.FechaCierre = DateTime.Now;
                ultimaLineaCredito.EsLineaCreditoActual = false;
                Saldo = Math.Round(((ultimaLineaCredito.MontoInicial + ultimaLineaCredito.Saldo) - ultimaLineaCredito.ObtenerDeduccionTotal()), 2);
            }

            Estado = Estados.ACTIVO;
            EsLineaCreditoActual = true;
        }

        public string ActualizarLineaCredito(int cuentaFinancieraId, string noDocumento, string observaciones, DateTime fechaCreacion, decimal montoInicial,
                                             decimal saldo, string creadoPor)
        {
            if (Estado == Estados.ACTIVO)
            {
                return "La Linea de Crédito debe ser NUEVA para actualizar";
            }

            if (Estado == Estados.CERRADO)
            {
                return "La Linea de Crédito ya está CERRADA";
            }

            if (Estado == Estados.NUEVO)
            {
                this.CuentaFinancieraId = cuentaFinancieraId;
                this.NoDocumento = noDocumento;
                this.Observaciones = observaciones;
                this.FechaCreacion = fechaCreacion;
                this.MontoInicial = montoInicial;
                this.Saldo = saldo;
                this.CreadoPor = creadoPor;
            }

            return string.Empty;
        }

        public decimal ObtenerDeduccionTotal()
        {
            var totalDeduccion = 0.00m;

            totalDeduccion += BoletaCierres.Sum(d => d.Monto);
            totalDeduccion += PrestamoTransferencias.Sum(d => d.Monto);
            totalDeduccion += PagoDescargaDetalles.Sum(d => d.Monto);
            totalDeduccion += PagoOrdenesCombustibles.Sum(d => d.MontoAbono);
            totalDeduccion += LineasCreditoDeducciones.Sum(d => d.Monto);
            totalDeduccion += GasolineraCreditoPagos.Sum(d => d.Monto);
            totalDeduccion += AjusteBoletaDetalles.Sum(d => d.Monto);
            totalDeduccion += Math.Abs(BoletaOtrasDeducciones.Where(m => m.Monto < 0).Sum(d => d.Monto));

            return Math.Round(totalDeduccion, 2);
        }
        
        public decimal ObtenerCreditoDisponible()
        {
            var deduccionTotal = ObtenerDeduccionTotal();

            return Math.Round((MontoInicial + Saldo) - deduccionTotal, 2);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(CodigoLineaCredito))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoLineaCredito");
                listaErrores.Add(mensaje);
            }

            if (SucursalId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "SucursalId");
                listaErrores.Add(mensaje);
            }

            if (CuentaFinancieraId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuentaFinancieraId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(NoDocumento))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoDocumento");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Observaciones))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Observaciones");
                listaErrores.Add(mensaje);
            }

            if (DateTime.MinValue == FechaCreacion)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaCreacion");
                listaErrores.Add(mensaje);
            }

            if (MontoInicial <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoInicial");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CreadoPor))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CreadoPor");
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
