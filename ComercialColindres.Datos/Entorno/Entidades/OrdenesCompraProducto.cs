using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class OrdenesCompraProducto : Entity, IValidacionesEntidades
    {
        public OrdenesCompraProducto(string ordenCompraNo, string noExoneracionDei, int plantaId, DateTime fechaCreacion, string creadoPor, 
                                     decimal montoDollar, decimal conversionDollarToLps)
        {
            this.OrdenCompraNo = ordenCompraNo;
            this.NoExoneracionDEI = noExoneracionDei;
            this.PlantaId = plantaId;
            this.FechaCreacion = fechaCreacion;
            this.CreadoPor = creadoPor;
            this.MontoDollar = montoDollar;
            this.ConversionDollarToLps = conversionDollarToLps;
            this.Estado = Estados.NUEVO;
            this.EsOrdenCompraActual = false;
            this.OrdenesCompraDetalleBoletas = new List<OrdenesCompraDetalleBoleta>();
            this.OrdenesCompraProductoDetalles = new List<OrdenesCompraProductoDetalle>();
        }

        protected OrdenesCompraProducto() { }

        public int OrdenCompraProductoId { get; set; }
        public string OrdenCompraNo { get; set; }
        public string NoExoneracionDEI { get; set; }
        public int PlantaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public bool EsOrdenCompraActual { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string CreadoPor { get; set; }
        public decimal MontoDollar { get; set; }
        public decimal ConversionDollarToLps { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual ICollection<OrdenesCompraDetalleBoleta> OrdenesCompraDetalleBoletas { get; set; }
        public virtual ICollection<OrdenesCompraProductoDetalle> OrdenesCompraProductoDetalles { get; set; }

        public string CerrarOrdenCompraProducto()
        {
            if (Estado == Estados.CERRADO)
            {
                return "La OrdenCompraProductoId ya esta CERRADA";
            }

            if (Estado != Estados.ACTIVO)
            {
                return "Solo puede Cerrar OrdenesCompraProducto ACTIVAS";
            }

            if (!OrdenesCompraDetalleBoletas.Any())
            {
                return "La Orden de Compra de Producto no tiene ninguna Boleta Asignada";
            }

            Estado = Estados.CERRADO;
            FechaCierre = DateTime.Now;
            EsOrdenCompraActual = false;

            return string.Empty;
        }

        public string ActivarOrdenCompraProducto()
        {
            if (Estado == Estados.ACTIVO)
            {
                return "La Orden de Compra de Producto Ya está ACTIVA!";
            }

            if (Estado != Estados.NUEVO)
            {
                return "Solo puede Activar Ordenes de Compra de Producto en estados NUEVO!";
            }

            if (!OrdenesCompraProductoDetalles.Any())
            {
                return "La Orden de Compra de Producto no tiene ningún requerimiento de Biomasa";
            }

            foreach (var productoDetalle in OrdenesCompraProductoDetalles)
            {
                if (productoDetalle.PrecioDollar <= 0)
                {
                    return string.Format("La Categoría de Biomasa {0} no tiene precio de Dollar asignado", productoDetalle.MaestroBiomasa.Descripcion);
                }

                if (productoDetalle.PrecioLps <= 0)
                {
                    return string.Format("La Categoría de Biomasa {0} no tiene precio de LPS asignado", productoDetalle.MaestroBiomasa.Descripcion);
                }
            }

            var totalDollar = OrdenesCompraProductoDetalles.Sum(d => d.Toneladas * d.PrecioDollar);
            var totalLpsDetalle = Math.Round(OrdenesCompraProductoDetalles.Sum(d => d.Toneladas * d.PrecioLps), 2);
            var totalLpsPO = CalcularTotalLpsPO();

            if (totalDollar != MontoDollar)
            {
                return string.Format("La cantidad total de Dollares de la orden {0} no coinciden con el total del detalle de Biomasa {1}", MontoDollar, totalDollar);
            }

            if (totalLpsDetalle != totalLpsPO)
            {
                return string.Format("La cantidad total de LPS de la orden {0} no coinciden con el total del detalle de Biomasa {1}", totalLpsPO, totalLpsDetalle);
            }

            Estado = Estados.ACTIVO;
            FechaActivacion = DateTime.Now;
            EsOrdenCompraActual = true;

            return string.Empty;
        }

        public void ActualizarOrdenCompraProducto(string ordenCompraNo, string noExoneracionDEI, DateTime fechaCreacion, decimal montoDollar, decimal conversionDollarToLps)
        {
            this.OrdenCompraNo = ordenCompraNo;
            this.NoExoneracionDEI = noExoneracionDEI;
            this.FechaCreacion = fechaCreacion;
            this.MontoDollar = montoDollar;
            this.ConversionDollarToLps = conversionDollarToLps;
        }

        public decimal CalcularRendimientoEntrega()
        {
            var toneladasRequeridas = OrdenesCompraProductoDetalles.Sum(r => r.Toneladas);
            var toneladasAsignadas = OrdenesCompraDetalleBoletas.Sum(t => t.CantidadFacturada);
            
            if (toneladasAsignadas == 0) return 0;

            return Math.Round((toneladasAsignadas / toneladasRequeridas) * 100, 2);
        }

        public decimal CalcularTotalLpsPO()
        {
            return Math.Round(MontoDollar * ConversionDollarToLps, 2);
        }

        public decimal CalcularTotalDollaresDetallePO()
        {
            return OrdenesCompraProductoDetalles.Sum(d => d.PrecioDollar * d.Toneladas);
        }

        public decimal CalcularTotalLpsDetallePO()
        {
            return OrdenesCompraProductoDetalles.Sum(d => d.PrecioLps * d.Toneladas);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(OrdenCompraNo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "OrdenCompraNo");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(NoExoneracionDEI))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoExoneracionDEI");
                listaErrores.Add(mensaje);
            }

            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                listaErrores.Add(mensaje);
            }

            if (FechaCreacion == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaCreacion");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CreadoPor))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CreadoPor");
                listaErrores.Add(mensaje);
            }

            if (MontoDollar <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoDollar");
                listaErrores.Add(mensaje);
            }

            if (ConversionDollarToLps <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoLPS");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();

            if (Estado == Estados.CERRADO)
            {
                var mensaje = "La OrdenCompraProductoId ya esta CERRADA";
                listaErrores.Add(mensaje);
            }

            if (Estado != Estados.NUEVO)
            {
                var mensaje = "Solo puede eliminar Ordenes de Compra de Producto en estado NUEVOS";
                listaErrores.Add(mensaje);
            }

            if (OrdenesCompraDetalleBoletas.Any())
            {
                var mensaje = "La Orden de Compra de Productos tiene Boletas asignadas";
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }        
    }
}
