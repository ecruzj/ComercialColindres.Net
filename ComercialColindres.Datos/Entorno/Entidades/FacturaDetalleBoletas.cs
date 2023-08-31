using System;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class FacturaDetalleBoletas : Entity, IValidacionesEntidades
    {
        public FacturaDetalleBoletas(Factura invoice, int? boletaId, string numeroEnvio, string codigoBoleta, decimal pesoProducto, DateTime fechaIngreso, decimal unitPrice = 0)
        {
            Factura = invoice;
            FacturaId = invoice.FacturaId;
            Planta = invoice.ClientePlanta;
            PlantaId = invoice.PlantaId;
            BoletaId = boletaId;
            NumeroEnvio = numeroEnvio ?? string.Empty;
            CodigoBoleta = codigoBoleta ?? string.Empty;
            PesoProducto = pesoProducto;
            EstaIngresada = boletaId != null;
            FechaIngreso = fechaIngreso;
            UnitPrice = Factura.HasUnitPriceItem ? unitPrice : 0;
        }

        public int FacturaDetalleBoletaId { get; set; }
        public int FacturaId { get; set; }
        public int PlantaId { get; set; }
        public int? BoletaId { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoBoleta { get; set; }
        public bool EstaIngresada { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime FechaIngreso { get; set; }

        public virtual Boletas Boleta { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual ClientePlantas Planta { get; set; }

        protected FacturaDetalleBoletas() { }

        public bool HasErrorWeightOrPrice()
        {
            if (Boleta == null) return false;

            if (Boleta.PesoProducto > PesoProducto) return true;

            decimal salesPrice = GetSalePrice();

            if (Boleta.PrecioProductoCompra > salesPrice) return true;

            return false;
        }

        private bool IsShippingNumberRequired()
        {
            string result = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("NumeroEnvio", Factura.PlantaId.ToString(), string.Empty);

            return !string.IsNullOrWhiteSpace(result) ? result == "1" ? true : false : false;
        }

        public void AssignBoleta(Boletas boleta)
        {
            if (BoletaId == boleta.BoletaId) return;

            Boleta = boleta;
            BoletaId = boleta.BoletaId;
        }

        public decimal GetSalePrice()
        {
            if (Boleta == null) return 0;            

            FacturaDetalleItem detailItem = GetItemDetail();

            if (detailItem == null) return 0;            
                        
            if (Factura.IsForeignCurrency)
            {
                decimal rate = Factura.LocalCurrencyAmount / Factura.Total;

                return Factura.HasUnitPriceItem ? 
                    Math.Round((UnitPrice * rate), 2) : 
                    Math.Round((detailItem.Precio * rate), 2);
            }

            return Factura.HasUnitPriceItem ? 
                UnitPrice : 
                Math.Round(detailItem.Precio, 2);
        }

        /// <summary>
        /// Obtiene el peso en toneladas de la Boleta
        /// </summary>
        /// <returns>Toneladas</returns>
        public decimal GetPurchaseWeight()
        {
            if (Boleta == null) return 0;

            return Boleta.PesoProducto;
        }

        public decimal GetProfit()
        {
            if (Boleta == null) return 0;

            FacturaDetalleItem detailItem = GetItemDetail();

            if (detailItem == null) return 0;

            decimal salePrice = GetSalePrice();
            decimal boletaPurchase = Boleta.ObtenerTotalFacturaCompra();
            decimal boletaSale = salePrice * PesoProducto;

            return Math.Round(boletaSale - boletaPurchase, 2);
        }

        private FacturaDetalleItem GetItemDetail()
        {
            FacturaDetalleItem detailItem = Factura.FacturaDetalleItems.FirstOrDefault(i => i.CategoriaProductoId == Boleta.CategoriaProductoId);

            if (detailItem == null)
            {
                List<string> derivadosAserrin = new List<string>
                {
                    ProductCategory.Chip,
                    ProductCategory.Piller,
                    ProductCategory.Aserrin_Chip
                };

                bool existeDerivado = derivadosAserrin.Contains(Boleta.CategoriaProducto.Descripcion);

                if (existeDerivado)
                {
                    detailItem = Factura.FacturaDetalleItems.FirstOrDefault(i => i.CategoriaProducto.Descripcion == ProductCategory.Aserrin_Nuevo);
                }
            }

            return detailItem;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (FacturaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FacturaId");
                listaErrores.Add(mensaje);
            }

            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                listaErrores.Add(mensaje);
            }

            if (PesoProducto <= 0)
            {
                var mensaje = string.Format("El Peso del Producto debe ser mayor a 0");
                listaErrores.Add(mensaje);
            }          

            if (IsShippingNumberRequired())
            {
                if (string.IsNullOrWhiteSpace(NumeroEnvio))
                {
                    string mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NumeroEnvio");
                    listaErrores.Add(mensaje);
                }

                if (Factura.HasUnitPriceItem && UnitPrice <= 0)
                {
                    var mensaje = $"El precio item del #Envío {NumeroEnvio}  debe ser mayor a 0";
                    listaErrores.Add(mensaje);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(CodigoBoleta))
                {
                    string mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoBoleta");
                    listaErrores.Add(mensaje);
                }

                if (Factura.HasUnitPriceItem && UnitPrice <= 0)
                {
                    var mensaje = $"El precio item del #Boleta {CodigoBoleta}  debe ser mayor a 0";
                    listaErrores.Add(mensaje);
                }
            }

            if (FechaIngreso == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaIngreso");
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
