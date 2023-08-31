using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Recibos : Entity, IValidacionesEntidades
    {
        public Recibos(int sucursalId, bool esAnticipo, string aplicaA, int prestamoId, int facturaId, string numeroRecibo, 
                       DateTime fecha, decimal monto, string observaciones)
        {
            SucursalId = sucursalId;
            EsAnticipo = esAnticipo;
            AplicaA = aplicaA;
            PrestamoId = prestamoId;
            FacturaId = facturaId;
            NumeroRecibo = numeroRecibo;
            Fecha = fecha;
            Monto = monto;
            Observaciones = observaciones;
        }

        protected Recibos() { }

        public int ReciboId { get; set; }
        public int SucursalId { get; set; }
        public bool EsAnticipo { get; set; }
        public string AplicaA { get; set; }
        public int? PrestamoId { get; set; }
        public int? FacturaId { get; set; }
        public string NumeroRecibo { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual Prestamos Prestamo { get; set; }
        public virtual Sucursales Sucursal { get; set; }

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
