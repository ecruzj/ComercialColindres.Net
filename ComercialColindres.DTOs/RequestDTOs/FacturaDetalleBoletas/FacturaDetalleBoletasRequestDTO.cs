using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.FacturaDetalle
{
    [Route("/factura-detalle/busqueda-boletas/{facturaId}", "GET")]
    public class GetDetalleBoletasPorFacturaId : IReturn<List<FacturaDetalleBoletasDTO>>
    {
        public int FacturaId { get; set; }
    }

    [Route("/factura-detalle/", "POST")]
    public class PostDetalleBoletas : RequestBase, IReturn<List<FacturaDetalleBoletasDTO>>
    {
        public int FacturaId { get; set; }
        public List<FacturaDetalleBoletasDTO> DetalleBoletas { get; set; }
        public bool EsImportacionMasiva { get; set; }
    }
    
    [Route("/factura-detalle/validacion-masivo", "PUT")]
    public class PutValidarDetalleBoletasMasivo : RequestBase, IReturn<List<FacturaDetalleBoletasDTO>>
    {
        public int PlantaId { get; set; }
        public int FacturaId { get; set; }
        public List<FacturaDetalleBoletasDTO> DetalleBoletas { get; set; }
    }
}
