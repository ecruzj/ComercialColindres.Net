using ComercialColindres.DTOs.RequestDTOs.FacturaDetalle;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IFacturaDetalleBoletasAppServices
    {
        List<FacturaDetalleBoletasDTO> Get(GetDetalleBoletasPorFacturaId request);
        List<FacturaDetalleBoletasDTO> SaveInvoiceDetailBoletas(PostDetalleBoletas request);
        List<FacturaDetalleBoletasDTO> Put(PutValidarDetalleBoletasMasivo request);
    }
}
