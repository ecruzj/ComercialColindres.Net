using ComercialColindres.DTOs.RequestDTOs.Facturas;
using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class FacturaDetalleItemModel : ObservableObject
    {
        public int FacturaDetalleItemId { get; set; }
        public int FacturaId { get; set; }

        public decimal Cantidad
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                RaisePropertyChanged(nameof(Cantidad));
            }
        }
        private decimal _cantidad;

        public int CategoriaProductoId
        {
            get { return _categoriaProductoId; }
            set
            {
                _categoriaProductoId = value;
                RaisePropertyChanged(nameof(CategoriaProductoId));
            }
        }
        private int _categoriaProductoId;

        public decimal Precio
        {
            get { return _precio; }
            set
            {
                _precio = value;
                RaisePropertyChanged(nameof(Precio));
            }
        }
        private decimal _precio;

        public string ProductoDescripcion
        {
            get { return _productoDescripcion; }
            set
            {
                _productoDescripcion = value;
                RaisePropertyChanged(nameof(ProductoDescripcion));
            }
        }
        private string _productoDescripcion;

        public decimal SubTotal
        {
            get { return _subTotal; }
            set
            {
                _subTotal = value;
                RaisePropertyChanged(nameof(SubTotal));
            }
        }
        private decimal _subTotal;

        public decimal Tax
        {
            get { return _tax; }
            set
            {
                _tax = value;
                RaisePropertyChanged(nameof(Tax));
            }
        }
        private decimal _tax;

        public decimal Total
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged(nameof(Total));
            }
        }
        private decimal _total;

        public decimal LocalCurrencyAmount
        {
            get { return _localCurrencyAmount; }
            set
            {
                _localCurrencyAmount = value;
                RaisePropertyChanged(nameof(LocalCurrencyAmount));
            }
        }
        private decimal _localCurrencyAmount;

        public decimal ForeignCurrencyAmount
        {
            get { return _foreignCurrencyAmount; }
            set
            {
                _foreignCurrencyAmount = value;
                RaisePropertyChanged(nameof(ForeignCurrencyAmount));
            }
        }
        private decimal _foreignCurrencyAmount;

        public void GetSubTotalItem(FacturasDTO currentInvoice)
        {
            LocalCurrencyAmount = GetLocalCurrencyItemAmount(currentInvoice);
            ForeignCurrencyAmount = GetForeignCurrencyItemAmount(currentInvoice);
            Tax = GetTaxItem(currentInvoice);

            if (currentInvoice.IsForeignCurrency)
            {
                SubTotal = ForeignCurrencyAmount;
            }
            else
            {
                SubTotal = LocalCurrencyAmount;
            }
        }

        private decimal GetTaxItem(FacturasDTO currentInvoice)
        {
            if (currentInvoice.IsExonerated) return 0;

            decimal tax;
            tax = GetLocalCurrencyItemAmount(currentInvoice) * (currentInvoice.TaxPercent / 100);

            return Math.Round(tax, 2);
        }

        private decimal GetLocalCurrencyItemAmount(FacturasDTO currentInvoice)
        {
            if (currentInvoice.IsForeignCurrency)
            {
                decimal rate = (currentInvoice.LocalCurrencyAmount / currentInvoice.InvoiceForeignTotal);
                return Math.Round((GetForeignCurrencyItemAmount(currentInvoice) * rate), 2);
            }

            return Math.Round((Cantidad * Precio), 2);
        }

        private decimal GetForeignCurrencyItemAmount(FacturasDTO currentInvoice)
        {
            if (!currentInvoice.IsForeignCurrency)
            {
                return 0;
            }

            return Math.Round((Cantidad * Precio), 2);
        }
    }
}
