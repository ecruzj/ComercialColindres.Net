using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class OrdenesCombustible : Entity, IValidacionesEntidades
    {
        public OrdenesCombustible(string codigoFactura, int gasCreditoId, string autorizadoPor, string placa, 
                                  bool esOrdenPersonal, decimal montoOrdenCombustible, string observaciones, DateTime fechaOrden, 
                                  byte[] imagen, int? proveedorId)
        {
            CodigoFactura = codigoFactura;
            GasCreditoId = gasCreditoId;
            AutorizadoPor = autorizadoPor;
            ProveedorId = esOrdenPersonal ? null : proveedorId;
            PlacaEquipo = placa;
            EsOrdenPersonal = esOrdenPersonal;
            Monto = Math.Abs(montoOrdenCombustible);
            FechaCreacion = fechaOrden;
            Observaciones = !string.IsNullOrWhiteSpace(observaciones) ? observaciones : string.Empty;
            Estado = Estados.ACTIVO;

            AssignImage(imagen);

            this.FuelOrderManualPayments = new List<FuelOrderManualPayment>();
        }
        protected OrdenesCombustible() { }

        public int OrdenCombustibleId { get; set; }
        public string CodigoFactura { get; set; }
        public int GasCreditoId { get; set; }
        public string AutorizadoPor { get; set; }
        public int? BoletaId { get; set; }
        public int? ProveedorId { get; set; }
        public string PlacaEquipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsOrdenPersonal { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public virtual GasolineraCreditos GasolineraCredito { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual Proveedores Proveedor { get; set; }
        public virtual OrdenCombustibleImg OrdenCombustibleImg { get; set; }
        public virtual ICollection<FuelOrderManualPayment> FuelOrderManualPayments { get; set; }

        private void AssignImage(byte[] imagen)
        {
            OrdenCombustibleImg = new OrdenCombustibleImg(this, imagen);
        }

        public void ActualizarOrdenCombustible(string codigoFactura, bool isPersonalOrder, string autorizadoPor, int? boletaId, int? proveedorId,
                                               string placa, decimal monto, string observaciones, DateTime fechaOrden, 
                                               byte[] image)
        {
            CodigoFactura = codigoFactura;
            AutorizadoPor = autorizadoPor;
            ProveedorId = proveedorId;
            BoletaId = boletaId;
            PlacaEquipo = placa;
            Monto = Math.Abs(monto);
            EsOrdenPersonal = isPersonalOrder;
            FechaCreacion = fechaOrden != DateTime.MinValue ? fechaOrden : DateTime.Now;
            Observaciones = !string.IsNullOrWhiteSpace(observaciones) ? observaciones : string.Empty;
            OrdenCombustibleImg.Imagen = image;
        }

        public void EliminarOrdenCombustible()
        {
            GasolineraCredito = null;
            Boleta = null;
        }

        public void RemoveBoletaFromFuel()
        {
            BoletaId = null;
            Boleta = null;
        }

        public bool IsAssignedToBoleta()
        {
            return Boleta != null;
        }

        public void RemoveOrderImg()
        {
            if (OrdenCombustibleImg == null) return;

            OrdenCombustibleImg.OrdenCombustible = null;
        }

        public string GetVendorName()
        {
            if (EsOrdenPersonal)
            {
                return "N/A";
            }

            return Proveedor.Nombre;
        }

        public object GetFuelOrderSpecification()
        {
            return $"{Observaciones} - {PlacaEquipo}";
        }

        public void RemoveManualPayments()
        {
            if (FuelOrderManualPayments == null) return;

            foreach (FuelOrderManualPayment fuelPayment in FuelOrderManualPayments.ToList())
            {
                this.FuelOrderManualPayments.Remove(fuelPayment);
            }
        }

        public void RemoveManualPayment(FuelOrderManualPayment fuelPayment)
        {
            if (FuelOrderManualPayments == null) return;

            this.FuelOrderManualPayments.Remove(fuelPayment);
        }

        public void AddManualPayment(FuelOrderManualPayment newManualPayment)
        {
            if (FuelOrderManualPayments == null) return;

            FuelOrderManualPayments.Add(newManualPayment);
        }

        public bool HasFuelManualPayments()
        {
            return FuelOrderManualPayments != null && FuelOrderManualPayments.Any();
        }

        public void UpdateStatus()
        {
            if (Boleta != null) { Estado = Estados.CERRADO; return; }

            decimal manualPayments = GetFuelOrderManualPayments();
            decimal outstanding = GetOutstandingFuelOrder();

            if (manualPayments > 0 && outstanding > 0)
            {
                Estado = Estados.ENPROCESO;
                return;
            }

            if (outstanding == 0)
            {
                Estado = Estados.CERRADO;
                return;
            }

            Estado = Estados.ACTIVO;
        }

        public decimal GetPayments()
        {
            if (Boleta != null) return this.Monto;

            return GetFuelOrderManualPayments();
        }

        public decimal GetOutStandingBalance()
        {
            decimal payments = GetPayments();

            return Math.Round(this.Monto - payments, 2);
        }

        public bool IsFuelOrderClosed()
        {
            return Estado == Estados.CERRADO;
        }

        private decimal GetOutstandingFuelOrder()
        {
            decimal manualPayments = GetFuelOrderManualPayments();

            return Math.Round((Monto - manualPayments), 2);
        }

        private decimal GetFuelOrderManualPayments()
        {
            return Math.Round(FuelOrderManualPayments.Sum(p => p.Amount), 2);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(CodigoFactura))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoFactura");
                listaErrores.Add(mensaje);
            }

            if (GasCreditoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "GasolineraId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(AutorizadoPor))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "AutorizadoPor");
                listaErrores.Add(mensaje);
            }

            if (!EsOrdenPersonal && ProveedorId == null)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ProveedorId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(PlacaEquipo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlacaEquipo");
                listaErrores.Add(mensaje);
            }

            if (Monto <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
                listaErrores.Add(mensaje);
            }

            if (FechaCreacion == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaCreacion");
                listaErrores.Add(mensaje);
            }

            if (OrdenCombustibleImg.Imagen == null)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Imagen");
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
