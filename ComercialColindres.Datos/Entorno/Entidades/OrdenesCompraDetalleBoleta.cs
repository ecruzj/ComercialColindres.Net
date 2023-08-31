using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class OrdenesCompraDetalleBoleta : Entity, IValidacionesEntidades
    {
        public OrdenesCompraDetalleBoleta(int ordenCompraProductoId, int boletaId, decimal cantidadFacturada)
        {
            this.OrdenCompraProductoId = ordenCompraProductoId;
            this.BoletaId = boletaId;
            this.CantidadFacturada = cantidadFacturada;
        }

        protected OrdenesCompraDetalleBoleta() { }

        public int OrdenCompraDetalleBoletaId { get; set; }
        public int OrdenCompraProductoId { get; set; }
        public int BoletaId { get; set; }
        public decimal CantidadFacturada { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual OrdenesCompraProducto OrdenesCompraProducto { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
