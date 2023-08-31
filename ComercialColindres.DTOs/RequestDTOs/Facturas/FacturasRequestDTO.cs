using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Facturas
{
    [Route("/facturas/{facturaId}", "GET")]
    public class GetFactura : IReturn<FacturasDTO>
    {
        public int FacturaId { get; set; }
    }
    
    [Route("/facturas/busqueda/{proveedorId}", "GET")]
    public class GetFacturasPorProveedorId : IReturn<List<FacturasDTO>>
    {
        public int ProveedorId { get; set; }
    }
    
    [Route("/facturas/buscar-por-valor", "GET")]
    public class GetByValorFacturas : BusquedaRequestBaseDTO, IReturn<BusquedaFacturasDTO>
    {
    }

    [Route("/facturas/update-info", "PUT")]
    public class UpdateInfoInvoice : RequestBase, IReturn<FacturasDTO>
    {
        public FacturasDTO Factura { get; set; }
    }

    [Route("/facturas/{invoiceId}", "PUT")]
    public class ActiveInvoiceById : RequestBase, IReturn<FacturasDTO>
    {
        public int InvoiceId { get; set; }
    }

    [Route("/facturas/", "POST")]
    public class PostFactura : RequestBase, IReturn<ActualizarResponseDTO>
    {
        public FacturasDTO Factura { get; set; }
    }

    [Route("/facturas/anular", "PUT")]
    public class PutFacturaAnular : IReturn<ActualizarResponseDTO>
    {
        public int FacturaId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/facturas/", "DELETE")]
    public class DeleteFactura : IReturn<EliminarResponseDTO>
    {
        public int FacturaId { get; set; }
    }
}
