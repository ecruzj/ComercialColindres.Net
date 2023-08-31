using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.DTOs.ResponseDTOs;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IFacturasAppServices
    {
        FacturasDTO Get(GetFactura request);
        BusquedaFacturasDTO Get(GetByValorFacturas request);
        FacturasDTO UpdateInvoice(UpdateInfoInvoice request);
        FacturasDTO ActiveInvoice(ActiveInvoiceById request);
        ActualizarResponseDTO Post(PostFactura request);
        ActualizarResponseDTO Put(PutFacturaAnular request);
        EliminarResponseDTO Delete(DeleteFactura request);
    }
}
