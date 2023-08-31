using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Proveedores : IValidacionesEntidades
    {
        public Proveedores(string rtn, string nombre, string cedulaNo, string direccion, string telefonos, string correoElectronico)
        {
            RTN = !string.IsNullOrWhiteSpace(rtn) ? rtn : string.Empty ;
            CedulaNo = !string.IsNullOrWhiteSpace(cedulaNo) ? cedulaNo : string.Empty;
            Nombre = nombre;
            Direccion = direccion;
            Telefonos = telefonos;
            CorreoElectronico = !string.IsNullOrWhiteSpace(correoElectronico) ? correoElectronico : string.Empty;
            Estado = Estados.ACTIVO;
            IsExempt = false;

            this.Boletas = new List<Boletas>();
            Conductores = new List<Conductores>();
            CuentasBancarias = new List<CuentasBancarias>();
            Equipos = new List<Equipos>();
            this.Prestamos = new List<Prestamos>();
        }
        
        protected Proveedores() { }

        public int ProveedorId { get; set; }
        public string RTN { get; set; }
        public string CedulaNo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Estado { get; set; }
        public bool IsExempt { get; set; }
        public virtual ICollection<Boletas> Boletas { get; set; }
        public virtual ICollection<OrdenesCombustible> OrdenesCombustible { get; set; }
        public virtual ICollection<Conductores> Conductores { get; set; }
        public virtual ICollection<CuentasBancarias> CuentasBancarias { get; set; }
        public virtual ICollection<Equipos> Equipos { get; set; }
        public virtual ICollection<Prestamos> Prestamos { get; set; }

        [NotMapped]
        public ICollection<BoletaHumedadAsignacion> BoletaHumedadAsignaciones
        {
            get
            {
                return Boletas.SelectMany(boleta => boleta.BoletasHumedadAsignacion).ToList();
            }
            private set { }
        }

        [NotMapped]
        public ICollection<AjusteBoleta> AjusteBoletas
        {
            get
            {
                return Boletas.SelectMany(boleta => boleta.AjusteBoletas).ToList();
            }
            private set { }
        }

        public bool HasOutStandingHumidity(Boletas boletaPayment)
        {
            if (BoletaHumedadAsignaciones == null) return false;

            List<BoletaHumedadAsignacion> outStandingHumidities = BoletaHumedadAsignaciones.Where(h => h.BoletaHumedad.Estado == Estados.ACTIVO
                                                                                                  && h.BoletaHumedad.BoletaIngresada
                                                                                                  && !h.BoletaHumedad.BoletaHumedadAsignacion.IsHumidityPaid(boletaPayment)).ToList();

            return outStandingHumidities.Any();
        }

        public bool HasPendingAdjustments(Boletas boletaPayment)
        {
            if (AjusteBoletas == null) return false;

            List<AjusteBoleta> ajusteBoletas = AjusteBoletas.Where(a => a.Estado != Estados.CERRADO &&
                                                                  (a.GetAjusteBoletaPendingAmount() > 0
                                                                   || a.AjusteBoletaPagos.Any(p => p.BoletaId == boletaPayment.BoletaId))).ToList();

            return ajusteBoletas.Any();
        }

        internal bool HasFuelOrderPending(Boletas boletaPayment)
        {
            if (OrdenesCombustible == null) return false;

            return OrdenesCombustible.Any(b => b.BoletaId == null || b.BoletaId == boletaPayment.BoletaId);
        }

        public void AddNewLoan(Prestamos newLoan)
        {
            if (Prestamos == null) return;

            Prestamos.Add(newLoan);
        }

        public decimal GetPendingLoan()
        {
            var totalDeduda = 0m;

            if (Prestamos != null && Prestamos.Any())
            {
                var listaPrestamos = Prestamos.Where(p => p.Estado == Estados.ACTIVO);
                var totalAbono = 0m;

                foreach (var prestamo in listaPrestamos)
                {
                    foreach (var abono in prestamo.PagoPrestamos)
                    {
                        totalAbono += abono.MontoAbono;
                    }

                    totalDeduda += (prestamo.MontoPrestamo * (1 + prestamo.PorcentajeInteres)) - totalAbono;
                    totalAbono = 0m;
                }
            }

            return totalDeduda;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();
            
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Nombre");
                listaErrores.Add(mensaje);
            }
            
            if (string.IsNullOrWhiteSpace(Direccion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Direccion");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Telefonos))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Telefonos");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }
        
        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();
            
            return listaErrores;
        }

        public void ActualizarProveedor(string cedulaNo, string rtn, string nombre, string direccion, string telefonos, string correoElectronico, string estado)
        {
            RTN = !string.IsNullOrWhiteSpace(rtn) ? rtn : string.Empty;
            CedulaNo = !string.IsNullOrWhiteSpace(cedulaNo) ? cedulaNo : string.Empty;
            Nombre = nombre;
            Direccion = direccion;
            Telefonos = telefonos;
            CorreoElectronico = !string.IsNullOrWhiteSpace(correoElectronico) ? correoElectronico : string.Empty;
            Estado = estado;
        }
    }
}
