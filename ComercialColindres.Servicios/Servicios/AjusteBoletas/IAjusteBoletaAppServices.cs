using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IAjusteBoletaAppServices
    {
        BusquedaAjusteBoletas GetAjusteBoletas(GetByValorAjusteBoletas request);
        AjusteBoletaDto CreateAjusteBoleta(PostAjusteBoleta request);
        AjusteBoletaDto DeleteAjusteBoleta(DeleteAjusteBoleta request);
        AjusteBoletaDto ActiveAjusteBoleta(PostActiveAjusteBoleta request);
    }
}
