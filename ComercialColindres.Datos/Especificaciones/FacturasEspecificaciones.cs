using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using System.Linq;

namespace ComercialColindres.Datos.Especificaciones
{
    public class FacturasEspecificaciones
    {
        public static Specification<Factura> Filtrar(string valorBusqueda)
        {
            var especification = new Specification<Factura>(o => o.FacturaId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification =
                    new Specification<Factura>(o => o.NumeroFactura.ToUpper().Contains(valorBuscar)
                                               || o.OrdenCompra.ToUpper().Contains(valorBuscar)
                                               || o.ExonerationNo.ToUpper().Contains(valorBuscar)
                                               || o.ClientePlanta.NombrePlanta.ToUpper().Contains(valorBuscar)
                                               || o.ProFormaNo.ToUpper().Contains(valorBuscar)
                                               || o.Observaciones.ToUpper().Contains(valorBuscar)
                                               || (o.FacturaDetalleBoletas.Any(e => e.NumeroEnvio.ToUpper().Contains(valorBuscar) 
                                               || o.FacturaDetalleBoletas.Any(c => c.CodigoBoleta.ToUpper().Contains(valorBuscar))))
                                               || (o.FacturaPagos.Any(p => p.ReferenciaBancaria.ToUpper().Contains(valorBuscar)))
                                               );
                especification &= valorBusquedaSpecification;
            }            

            return especification;
        }
    }
}
