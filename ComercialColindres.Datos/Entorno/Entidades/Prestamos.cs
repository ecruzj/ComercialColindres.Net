using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Prestamos : Entity, IValidacionesEntidades
    {
        public Prestamos(string codigoPrestamo, int sucrusalId, int proveedorId, DateTime fechaCreacion, string autorizadoPor, decimal porcentajeInteres,
                         decimal montoPrestamo, string observaciones, bool esInteresMensual = false)
        {
            CodigoPrestamo = codigoPrestamo;
            SucursalId = sucrusalId;
            ProveedorId = proveedorId;
            FechaCreacion = fechaCreacion;
            AutorizadoPor = autorizadoPor;
            PorcentajeInteres = porcentajeInteres;
            MontoPrestamo = montoPrestamo;
            Estado = Estados.NUEVO;
            Observaciones = observaciones;
            EsInteresMensual = esInteresMensual;

            this.PagoPrestamos = new List<PagoPrestamos>();
            this.PrestamosTransferencias = new List<PrestamosTransferencias>();
            this.Recibos = new List<Recibos>();
        }

        protected Prestamos() { }

        public int PrestamoId { get; set; }
        public string CodigoPrestamo { get; set; }
        public int SucursalId { get; set; }
        public int ProveedorId { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string AutorizadoPor { get; set; }
        public decimal PorcentajeInteres { get; set; }
        public bool EsInteresMensual { get; set; }
        public decimal MontoPrestamo { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public virtual ICollection<PagoPrestamos> PagoPrestamos { get; set; }
        public virtual Proveedores Proveedor { get; set; }
        public virtual Sucursales Sucursal { get; set; }
        public virtual ICollection<PrestamosTransferencias> PrestamosTransferencias { get; set; }
        public virtual ICollection<Recibos> Recibos { get; set; }

        public void ActualizarPrestamo(int sucrusalId, int proveedorId, DateTime fechaPrestamo, string autorizadoPor, decimal porcentajeInteres,
                                       decimal montoPrestamo, string observaciones)
        {
            SucursalId = sucrusalId;
            ProveedorId = proveedorId;
            FechaCreacion = fechaPrestamo;
            AutorizadoPor = autorizadoPor;
            PorcentajeInteres = porcentajeInteres;
            MontoPrestamo = montoPrestamo;
            Observaciones = observaciones;
        }

        public decimal ObtenerTotalACobrar()
        {
            decimal intereses = GetInterests();

            return Math.Round((MontoPrestamo + intereses), 2);
        }

        public decimal GetInterests()
        {
            if (PorcentajeInteres == 0) return 0;

            if (EsInteresMensual)
            {
                return CalculateInteresMensual();
            }

            return Math.Round((MontoPrestamo * (PorcentajeInteres / 100)), 2);
        }

        private decimal CalculateInteresMensual()
        {
            decimal interesMensual = Math.Round((MontoPrestamo * (PorcentajeInteres / 100)), 2);
            decimal interesDiario = Math.Round((interesMensual / 30), 2);
            int diasTranscurridos = GetIntervalDays();

            return Math.Round((diasTranscurridos * interesDiario), 2);
        }

        public int GetIntervalDays()
        {
            if (Estado != Estados.CERRADO)
            {
                return (DateTime.Now.Date - FechaCreacion.Date).Days;
            }

            PagoPrestamos lastPayment = GetLastPayment();

            return (lastPayment.FechaTransaccion.Date - FechaCreacion.Date).Days;
        }

        private PagoPrestamos GetLastPayment()
        {
            return PagoPrestamos.OrderByDescending(f => f.FechaTransaccion).FirstOrDefault();
        }
        
        public decimal ObtenerTotalAbono()
        {
            if (PagoPrestamos != null && PagoPrestamos.Any())
            {
                return Math.Round(PagoPrestamos.Sum(p => p.MontoAbono), 2);
            }

            return 0;
        }

        public decimal ObtenerSaldoPendiente()
        {
            var saldoPendiente = ObtenerTotalACobrar();

            if (PagoPrestamos != null && PagoPrestamos.Any())
            {
                return Math.Round(saldoPendiente - PagoPrestamos.Sum(p => p.MontoAbono), 2);
            }

            return saldoPendiente;
        }

        private decimal ObtenerAbonosPorBoletas()
        {
            if (PagoPrestamos == null) return 0;

            var totalAbonos = PagoPrestamos.Where(b => b.Boleta != null && b.Boleta.Estado == Estados.CERRADO).Sum(a => a.MontoAbono);

            return Math.Round(totalAbonos, 2);
        }

        private decimal ObtenerOtrosAbonos()
        {
            if (PagoPrestamos == null) return 0;

            var otrosAbonos = Math.Round(PagoPrestamos.Where(a => a.Boleta == null).Sum(m => m.MontoAbono), 2);

            return Math.Round(otrosAbonos, 2);
        }

        private decimal ObtenerTotalAbonosParaCierre()
        {
            if (PagoPrestamos == null) return 0;

            var abonosPorBoletas = ObtenerAbonosPorBoletas();
            var otrosAbonos = ObtenerOtrosAbonos();

            return Math.Round((abonosPorBoletas + otrosAbonos), 2);
        }

        public string Anular()
        {
            if (Estado != Estados.NUEVO)
            {
                return "Solo se pueden Anular Prestamos en estado " + Estados.NUEVO;
            }

            Estado = Estados.ANULADO;
            return string.Empty;
        }

        public void ActualizarEstadoPrestamo()
        {
            if (PrestamosTransferencias != null && PrestamosTransferencias.Any())
            {
                Estado = Estados.ENPROCESO;
            }
            else
            {
                Estado = Estados.NUEVO;
            }
        }

        public void ReactiveLoan()
        {
            if (Estado == Estados.CERRADO)
            {
                Estado = Estados.ACTIVO;
            }
        }

        public string Activar()
        {
            var transferenciaPrestamo = Math.Round(PrestamosTransferencias.Sum(p => p.Monto), 2);
            var transferenciaRestante = Math.Round(MontoPrestamo - transferenciaPrestamo, 2);

            if (MontoPrestamo == transferenciaPrestamo)
            {
                Estado = Estados.ACTIVO;
            }
            else
            {
                return string.Format("Hace falta transferir {0}", transferenciaRestante);
            }

            return string.Empty;
        }

        public void CerrarPrestamo()
        {
            if (PagoPrestamos == null || !PagoPrestamos.Any()) return;

            var totalDeduda = ObtenerTotalACobrar();
            var totalAbonos = ObtenerTotalAbonosParaCierre();

            if (totalAbonos == totalDeduda)
            {
                Estado = Estados.CERRADO;
            }
        }

        public void AgregarOtroAbono(PagoPrestamos otroAbono)
        {
            if (PagoPrestamos == null) return;

            PagoPrestamos.Add(otroAbono);
        }

        public void RemoverOtroAbono(PagoPrestamos otroAbono)
        {
            if (PagoPrestamos == null) return;

            PagoPrestamos.Remove(otroAbono);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(CodigoPrestamo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoPrestamo");
                listaErrores.Add(mensaje);
            }

            if (ProveedorId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ProveedorId");
                listaErrores.Add(mensaje);
            }

            if (SucursalId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "SucursalId");
                listaErrores.Add(mensaje);
            }

            if (FechaCreacion == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaPrestamo");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(AutorizadoPor))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "AutorizadoPor");
                listaErrores.Add(mensaje);
            }

            if (MontoPrestamo <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoPrestamo");
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
