using System;
using System.Collections.Generic;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class OrdenesCompraProductoDetalle : Entity, IValidacionesEntidades
    {
        public OrdenesCompraProductoDetalle(int ordenCompraProductoId, int biomasaId, decimal toneladas, decimal precioDollar, decimal precioLps)
        {
            this.OrdenCompraProductoId = ordenCompraProductoId;
            this.BiomasaId = biomasaId;
            this.Toneladas = toneladas;
            this.PrecioDollar = precioDollar;
            this.PrecioLps = precioLps;
        }

        protected OrdenesCompraProductoDetalle() { }

        public int OrdenCompraProductoDetalleId { get; set; }
        public int OrdenCompraProductoId { get; set; }
        public int BiomasaId { get; set; }
        public decimal Toneladas { get; set; }
        public decimal PrecioDollar { get; set; }
        public decimal PrecioLps { get; set; }
        public virtual MaestroBiomasa MaestroBiomasa { get; set; }
        public virtual OrdenesCompraProducto OrdenesCompraProducto { get; set; }

        public void ActualizarOrdenProductoDetalle(int biomasaId, decimal toneladas, decimal precioDollar, decimal precioLPS)
        {
            this.BiomasaId = biomasaId;
            this.Toneladas = toneladas;
            this.PrecioDollar = precioDollar;
            this.PrecioLps = precioLPS;
        }

        public decimal ObtenerTotalDollaresPO()
        {
            return Toneladas * PrecioDollar;
        }

        public decimal ObtenerTotalLpsPO()
        {
            return Toneladas * PrecioLps;
        }

        public decimal CalcularCumplimientoDetalleEntrega()
        {
            var toneladasAsingadas = OrdenesCompraProducto.OrdenesCompraDetalleBoletas
                                     .Where(b => b.Boleta.CategoriaProducto.BiomasaId == BiomasaId)
                                     .Sum(t => t.CantidadFacturada);

            if (toneladasAsingadas == 0) return 0;

            return Math.Round((toneladasAsingadas / Toneladas) * 100, 2);
        }
        
        public decimal CalcularPrecioLpsPorTonelada()
        {
            return PrecioDollar * OrdenesCompraProducto.ConversionDollarToLps;
        }

        public decimal ObtenerTotalToneladasProductoEntregado()
        {
            return OrdenesCompraProducto.OrdenesCompraDetalleBoletas
                                     .Where(b => b.Boleta.CategoriaProducto.BiomasaId == BiomasaId)
                                     .Sum(t => t.CantidadFacturada);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (OrdenCompraProductoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "OrdenCompraProductoId");
                listaErrores.Add(mensaje);
            }

            if (BiomasaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BiomasaId");
                listaErrores.Add(mensaje);
            }

            if (Toneladas <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Toneladas");
                listaErrores.Add(mensaje);
            }

            if (PrecioDollar <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioDollar");
                listaErrores.Add(mensaje);
            }

            if (PrecioLps <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioLPS");
                listaErrores.Add(mensaje);
            }

            if (PrecioDollar >= PrecioLps)
            {
                var mensaje = "El Precio del Producto por Tonelada en $ debe ser menor al precio Lps";
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
