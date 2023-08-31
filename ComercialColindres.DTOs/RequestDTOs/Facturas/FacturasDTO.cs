using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Facturas
{
    public class FacturasDTO : BaseDTO
    {
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
        public bool IsExonerated { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal Total { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public bool IsForeignCurrency { get; set; }
        public decimal LocalCurrencyAmount { get; set; }
        public bool HasUnitPriceItem { get; set; }

        public string NombrePlanta { get; set; }
        public string NombreSubPlanta { get; set; }
        public string NombreSucursal { get; set; }
        public bool RequiereOrdenCompra { get; set; }
        public bool RequiereProForm { get; set; }
        public bool ShippingNumberRequired { get; set; }
        public bool HasSubPlanta { get; set; }

        public decimal Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal _tax;

        public decimal SubTotalDollar
        {
            get { return _subTotalDollar; }
            set { _subTotalDollar = value; }
        }
        private decimal _subTotalDollar;

        public decimal SubTotalLps
        {
            get { return _subTotalLps; }
            set { _subTotalLps = value; }
        }
        private decimal _subTotalLps;

        public decimal InvoiceForeignTotal
        {
            get { return _invoiceForeignTotal; }
            set { _invoiceForeignTotal = value; }
        }
        private decimal _invoiceForeignTotal;

        public decimal InvoiceLocalTotal
        {
            get { return _invoiceLocalTotal; }
            set { _invoiceLocalTotal = value; }
        }
        private decimal _invoiceLocalTotal;

        public string ExonerationNo
        {
            get { return _exonerationNo; }
            set { _exonerationNo = value; }
        }
        private string _exonerationNo;

        public decimal Saldo { get; set; }

        public string ConteoFacturas
        {
            get { return _conteoFacturas; }
            set
            {
                _conteoFacturas = value;                
            }
        }
        private string _conteoFacturas;
    }
}
