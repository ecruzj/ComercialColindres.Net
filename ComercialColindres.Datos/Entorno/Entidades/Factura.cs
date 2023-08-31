using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Factura : Entity, IValidacionesEntidades
    {
        public Factura(int sucursalId, FacturasCategorias facturaCategoria, ClientePlantas planta, int? subPlantaId, string ordenCompra, string semana, string numeroFactura, string proFormaNo,
                        DateTime fecha, decimal total, string exonerationNo, decimal taxPercent, bool isForeignCurrency, decimal localCurrencyAmount, string observaciones, bool hasUnitPriceItem = false)
        {
            SucursalId = sucursalId;
            FacturaCategoriaId = facturaCategoria.FacturaCategoriaId;
            ClientePlanta = planta;
            PlantaId = planta.PlantaId;
            OrdenCompra = ordenCompra ?? string.Empty;
            Semana = semana ?? string.Empty;
            TipoFactura = facturaCategoria.Descripcion;
            NumeroFactura = numeroFactura;
            ProFormaNo = proFormaNo ?? string.Empty;
            Fecha = fecha;
            Total = total;
            ExonerationNo = exonerationNo ?? string.Empty;
            IsExonerated = planta.IsExempt;
            TaxPercent = taxPercent;
            IsForeignCurrency = isForeignCurrency;
            LocalCurrencyAmount = localCurrencyAmount;
            Observaciones = observaciones;
            Estado = Estados.NUEVO;
            HasUnitPriceItem = hasUnitPriceItem;

            TryAssigneSubPlanta(subPlantaId);

            this.FacturaDetalleBoletas = new List<FacturaDetalleBoletas>();
            this.FacturaDetalleItems = new List<FacturaDetalleItem>();
            this.Recibos = new List<Recibos>();
            this.FacturaPagos = new List<FacturaPago>();
            this.NotasCredito = new List<NotaCredito>();
        }
        
        protected Factura() { }

        public int FacturaId { get; set; }
        public int SucursalId { get; set; }
        public int FacturaCategoriaId { get; set; }
        public int PlantaId { get; set; }
        public int? SubPlantaId { get; set; }
        public string OrdenCompra { get; set; }
        public string Semana { get; set; }
        public string TipoFactura { get; set; }
        public string NumeroFactura { get; set; }
        public string ProFormaNo { get; set; }
        public DateTime Fecha { get; set; }
        public string ExonerationNo { get; set; }
        public bool IsExonerated { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal Total { get; set; }
        public bool IsForeignCurrency { get; set; }
        public decimal LocalCurrencyAmount { get; set; }
        public bool HasUnitPriceItem { get; set; }

        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual ICollection<FacturaDetalleBoletas> FacturaDetalleBoletas { get; set; }
        public virtual ICollection<FacturaDetalleItem> FacturaDetalleItems { get; set; }
        public virtual FacturasCategorias FacturasCategoria { get; set; }
        public virtual Sucursales Sucursal { get; set; }
        public virtual ICollection<Recibos> Recibos { get; set; }
        public virtual SubPlanta SubFacility { get; set; }
        public virtual ICollection<FacturaPago> FacturaPagos { get; set; }
        public virtual ICollection<NotaCredito> NotasCredito { get; set; }


        public void AddNotaCredito(NotaCredito newNota)
        {
            if (NotasCredito == null) return;

            NotasCredito.Add(newNota);
        }

        public void RemoveNotaCredito(NotaCredito notaCredito)
        {
            if (NotasCredito == null) return;

            NotasCredito.Remove(notaCredito);
        }

        public void AddDetailItem(FacturaDetalleItem newInvoiceDetail)
        {
            if (FacturaDetalleItems == null) return;

            FacturaDetalleItems.Add(newInvoiceDetail);
        }

        public void AddInvoicePayment(FacturaPago newPayment)
        {
            if (FacturaPagos == null) return;

            FacturaPagos.Add(newPayment);
        }

        public void RemoveInvoicePayment(FacturaPago itemPayment)
        {
            if (FacturaPagos == null) return;

            FacturaPagos.Remove(itemPayment);
        }

        private void TryAssigneSubPlanta(int? subPlantaId)
        {
            if (ClientePlanta.HasSubPlanta)
            {
                SubPlantaId = subPlantaId;
            }
            else
            {
                SubPlantaId = null;
            }
        }
        
        public bool IsShippingNumberRequired()
        {
            string result = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("NumeroEnvio", PlantaId.ToString(), string.Empty);

            return !string.IsNullOrWhiteSpace(result) ? result == "1" ? true : false : false;
        }

        public decimal GetInvoiceNotasCredito()
        {
            decimal notasCredito = 0m;

            foreach (NotaCredito nota in NotasCredito)
            {
                notasCredito += nota.Monto;
            }

            return notasCredito;
        }

        public decimal GetInvoiceSubTotalDollar()
        {
            decimal subTotal = 0m;

            foreach (FacturaDetalleItem item in FacturaDetalleItems)
            {
                subTotal += item.GetForeignCurrencyItemAmount();
            }

            return Math.Round(subTotal - GetInvoiceNotasCredito(), 2);
        }

        public void RemoveDetailItem(FacturaDetalleItem itemDetail)
        {
            if (FacturaDetalleItems == null) return;

            FacturaDetalleItems.Remove(itemDetail);
        }

        public decimal GetInvoiceSubTotalLps()
        {
            decimal subTotal = 0m;

            foreach (FacturaDetalleItem item in FacturaDetalleItems)
            {
                subTotal += item.GetSubTotalLps();
            }
            decimal notasCredito = GetInvoiceNotasCredito();

            if (IsForeignCurrency)
            {
                decimal rate = LocalCurrencyAmount / Total;
                notasCredito = notasCredito * rate;
            }

            return Math.Round(subTotal - notasCredito, 2);
        }

        public decimal GetInvoiceTax()
        {
            decimal tax = 0m;

            foreach (FacturaDetalleItem item in FacturaDetalleItems)
            {
                tax += item.GetTaxItemLps();                
            }

            return Math.Round(tax, 2);
        }

        public decimal GetInvoiceTaxLps()
        {
            if (IsExonerated) return 0;

            decimal tax = 0m;

            foreach (FacturaDetalleItem item in FacturaDetalleItems)
            {
                tax += item.GetTaxItemLps();
            }

            return Math.Round(tax, 2);
        }

        public decimal GetInvoiceForeignCurrencyTotal()
        {
            if (!IsForeignCurrency) return 0;

            return Total - GetInvoiceNotasCredito();
        }

        public decimal GetInvoiceLocalCurrencyTotal()
        {
            return Math.Round((GetInvoiceSubTotalLps() + GetInvoiceTaxLps()), 2);
        }
        
        public void Cerrar()
        {
            Estado = Estados.CERRADO;
        }

        public string Anular()
        {
            if (Estado == Estados.CERRADO)
            {
                return "La factura está en estado " + Estados.CERRADO + " NO se puede Anular";
            }

            if (Estado == Estados.ANULADO)
            {
                return "La factura YA esta Anulada";
            }

            if (FacturaDetalleBoletas.Any())
            {
                RemoverBoletas();
            }

            Estado = Estados.ANULADO;
            Observaciones = Observaciones + " | Factura Anulada";

            return string.Empty;
        }

        private void RemoverBoletas()
        {
            var boletas = FacturaDetalleBoletas.ToList();

            foreach (var boleta in boletas)
            {
                FacturaDetalleBoletas.Remove(boleta);
            }
        }

        public void AddInvoiceDetailBoleta(FacturaDetalleBoletas newBoletaDetail)
        {
            if (FacturaDetalleBoletas == null) return;

            FacturaDetalleBoletas.Add(newBoletaDetail);
        }

        public bool TryActiveInvoice(out string validationMessage)
        {
            if (Estado != Estados.NUEVO)
            {
                validationMessage = "El estado debe ser NUEVO!";
                return false;
            }

            if (FacturaDetalleItems == null || !FacturaDetalleItems.Any())
            {
                validationMessage = "No existe un Detalle Item";
                return false;
            }

            decimal totalItemsDetail = IsForeignCurrency
                                       ? GetInvoiceForeignCurrencyTotal()
                                       : GetInvoiceSubTotalLps();

            if (totalItemsDetail != Total)
            {
                validationMessage = "El SubTotal de la Factura no es el mismo con el Total de los Items";
                return false;
            }

            Estado = Estados.ACTIVO;

            validationMessage = string.Empty;
            return true;
        }

        public void UpdateInfo(ClientePlantas factory, SubPlanta subFactory, string ordenCompra, string semana, string numeroFactura, string proFormaNo, DateTime fecha, 
                               decimal total, string exonerationNo, decimal taxPercent, bool isForeignCurrency, decimal localCurrencyAmount, string observaciones, bool hasUnitPriceItem = false)
        {
            ClientePlanta = factory;

            if (!factory.HasSubPlanta)
            {
                SubFacility = null;
            }
            else
            {
                SubFacility = subFactory;
                SubPlantaId = SubFacility.SubPlantaId;
            }

            OrdenCompra = ordenCompra;
            Semana = semana ?? string.Empty;
            NumeroFactura = numeroFactura;
            ProFormaNo = factory.RequiresProForm ? proFormaNo : string.Empty;
            Fecha = fecha;
            Total = total;
            ExonerationNo = factory.HasExonerationNo ? exonerationNo : string.Empty;
            TaxPercent = factory.IsExempt ? 0 : taxPercent;
            IsForeignCurrency = isForeignCurrency;
            LocalCurrencyAmount = IsForeignCurrency ? localCurrencyAmount : 0;
            Observaciones = observaciones;
            IsExonerated = factory.IsExempt;
            HasUnitPriceItem = hasUnitPriceItem;
        }

        public bool UpdateStatus(out string validation)
        {
            validation = string.Empty;
            decimal invoicePaid = GetInvoicePaid();
            decimal totalInvoice;
            bool hasBoletasDetail = FacturaDetalleBoletas.Any();

            if (IsForeignCurrency)
            {
                totalInvoice = GetInvoiceForeignCurrencyTotal();
            }
            else
            {
                totalInvoice = GetInvoiceLocalCurrencyTotal();
            }

            if (invoicePaid > totalInvoice)
            {
                validation = "El total del pago es mayor al total de la Factura";
                return false;
            }

            if (totalInvoice == invoicePaid)
            {
                Estado = Estados.CERRADO;
                return true;
            }

            if (invoicePaid > 0)
            {
                Estado = Estados.ENPROCESO;
                return true;
            }

            Estado = Estados.ACTIVO;
            return true;
        }

        private decimal GetInvoicePaid()
        {
            if (FacturaPagos == null) return 0;

            return Math.Round(FacturaPagos.Sum(p => p.Monto), 2);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var mensajeValidacion = new List<string>();

            if (SucursalId == 0)
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "SucursalId");
                mensajeValidacion.Add(validacion);
            }

            if (PlantaId == 0)
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                mensajeValidacion.Add(validacion);
            }

            if (string.IsNullOrWhiteSpace(TipoFactura))
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "TipoFactura");
                mensajeValidacion.Add(validacion);
            }

            if (string.IsNullOrWhiteSpace(NumeroFactura))
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "NumeroFactura");
                mensajeValidacion.Add(validacion);
            }

            if (Total <= 0)
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "Total");
                mensajeValidacion.Add(validacion);
            }

            if (IsForeignCurrency)
            {
                if (LocalCurrencyAmount <= 0)
                {
                    var validacion = string.Format(MensajesValidacion.Campo_Requerido, "LocalCurrencyAmount");
                    mensajeValidacion.Add(validacion);
                }
            }

            if (ClientePlanta.HasSubPlanta && (SubPlantaId == null || SubPlantaId == 0))
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "SubPlantaId");
                mensajeValidacion.Add(validacion);
            }

            if (ClientePlanta.RequiresPurchaseOrder)
            {
                if (string.IsNullOrWhiteSpace(OrdenCompra))
                {
                    var validacion = string.Format(MensajesValidacion.Campo_Requerido, "OrdenCompra");
                    mensajeValidacion.Add(validacion);
                }                
            }

            if (ClientePlanta.RequiresProForm)
            {
                if (string.IsNullOrWhiteSpace(ProFormaNo))
                {
                    var validacion = string.Format(MensajesValidacion.Campo_Requerido, "ProFormaNo");
                    mensajeValidacion.Add(validacion);
                }
            }

            if (ClientePlanta.RequiresWeekNo && string.IsNullOrWhiteSpace(Semana))
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "SemanaNo");
                mensajeValidacion.Add(validacion);
            }
            
            if (IsExonerated  && ClientePlanta.HasExonerationNo && string.IsNullOrWhiteSpace(ExonerationNo))            
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "ExonerationNo");
                mensajeValidacion.Add(validacion);
            }

            if (!IsExonerated && TaxPercent <= 0)
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "%ISV");
                mensajeValidacion.Add(validacion);
            }

            if (string.IsNullOrWhiteSpace(Observaciones))
            {
                var validacion = string.Format(MensajesValidacion.Campo_Requerido, "Observaciones");
                mensajeValidacion.Add(validacion);
            }

            return mensajeValidacion;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var mensajeValidacion = new List<string>();
            
            return mensajeValidacion;
        }
    }
}
