using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IAjusteBoletaDetalleAppServices
    {
        List<AjusteBoletaDetalleDto> GetAjusteBoletaDetallado(GetAjusteBoletaDetalleByVendorId request);
        List<AjusteBoletaDetalleDto> GetAjusteBoletaDetalleByAjusteId(GetAjusteBoletaDetalleByAjusteBoletaId request);
        AjusteBoletaDetalleDto SaveAjusteBoletaDetalle(PostAjusteBoletaDetalle request);
    }
}
