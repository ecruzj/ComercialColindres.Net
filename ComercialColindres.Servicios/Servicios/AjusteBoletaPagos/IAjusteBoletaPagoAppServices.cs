using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IAjusteBoletaPagoAppServices
    {
        List<AjusteBoletaPagoDto> GetAjusteBoletaPagoByBoletaId(GetAjusteBoletaPagoByBoletaId request);
        List<AjusteBoletaPagoDto> GetAjusteBoletaPagoByDetailId(GetAjusteBoletaPagoByDetailId request);
        AjusteBoletaPagoDto SaveAjusteBoletaPagos(PostAjusteBoletaPagoByBoleta request);
    }
}
