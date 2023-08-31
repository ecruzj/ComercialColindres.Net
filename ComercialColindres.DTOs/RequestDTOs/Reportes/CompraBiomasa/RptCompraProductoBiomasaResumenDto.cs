using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptCompraProductoBiomasaResumenDto
    {
        public RptCompraProductoBiomasaResumenDto()
        {
            CompraBiosaResumen = new List<CompraProductoBiomasaResumenDato>();
        }

        public List<CompraProductoBiomasaResumenDato> CompraBiosaResumen { get; set; }
        public decimal CompraTotal { get; set; }

        public void ObtenerCompraTotal()
        {
            if (CompraBiosaResumen == null) return;

            CompraTotal = CompraBiosaResumen.Sum(t => t.TotalCompra);
        }
    }

    public class CompraProductoBiomasaResumenDato
    {
        public string Planta { get; set; }
        public string TipoProducto { get; set; }
        public decimal TotalCompra { get; set; }
    }
}
