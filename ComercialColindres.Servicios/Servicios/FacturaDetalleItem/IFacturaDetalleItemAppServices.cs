using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IFacturaDetalleItemAppServices
    {
        List<FacturaDetalleItemDto> GetDetailItemsByInvoiceId(GetDetailItemsByInvoiceId request);
        FacturaDetalleItemDto SaveInvoiceDetailItems(PostInvoiceDetailItems request);
    }
}
