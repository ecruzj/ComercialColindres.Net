using System;
using System.Collections.Generic;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class FacturaDetalleItem : Entity, IValidacionesEntidades
    {
        public FacturaDetalleItem(Factura factura, decimal cantidad, CategoriaProductos categoriaProducto, decimal precio)
        {
            Factura = factura;
            FacturaId = factura.FacturaId;
            Cantidad = cantidad;
            CategoriaProducto = categoriaProducto;
            CategoriaProductoId = categoriaProducto.CategoriaProductoId;
            Precio = precio;
        }

        protected FacturaDetalleItem() { }

        public int FacturaDetalleItemId { get; set; }
        public int FacturaId { get; set; }
        public decimal Cantidad { get; set; }
        public int CategoriaProductoId { get; set; }
        public decimal Precio { get; set; }

        public virtual Factura Factura { get; set; }
        public virtual CategoriaProductos CategoriaProducto { get; set; }

        public void UpdateInvoiceItem(decimal cantidad, CategoriaProductos productCategory, decimal precio)
        {
            Cantidad = cantidad;
            CategoriaProducto = productCategory;
            CategoriaProductoId = productCategory.CategoriaProductoId;
            Precio = precio;
        }

        public decimal GetLocalCurrencyItemAmount()
        {
            if (Factura.IsForeignCurrency)
            {
                decimal rate = Factura.LocalCurrencyAmount / Factura.Total;
                return GetForeignCurrencyItemAmount() * rate;
            }

            return Math.Round((Cantidad * Precio), 2);
        }

        public decimal GetSubTotalLps()
        {
            if (Factura.IsForeignCurrency)
            {
                decimal rate = Factura.LocalCurrencyAmount / Factura.Total;
                return (Cantidad * Precio) * rate;
            }

            return Math.Round((Cantidad * Precio), 2);
        }

        public decimal GetForeignCurrencyItemAmount()
        {
            if (!Factura.IsForeignCurrency)
            {
                return 0;
            }

            return Math.Round((Cantidad * Precio), 2);
        }
        
        public decimal GetTaxItemLps()
        {
            if (Factura.IsExonerated || Factura.IsForeignCurrency) return 0;

            return Math.Round(GetLocalCurrencyItemAmount() * (Factura.TaxPercent / 100), 2);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (FacturaId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FacturaId");
                listaErrores.Add(mensaje);
            }

            if (Cantidad <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Cantidad");
                listaErrores.Add(mensaje);
            }

            if (CategoriaProductoId <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CategoriaProductoId");
                listaErrores.Add(mensaje);
            }

            if (Precio <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Precio");
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
