using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletas
{
    [Route("/ajuste-boletas/", "GET")]
    public class GetByValorAjusteBoletas : BusquedaRequestBaseDTO, IReturn<BusquedaAjusteBoletas>
    {
        public string ValorBusqueda { get; set; }
    }

    [Route("/ajuste-boletas/{boletaId}", "POST")]
    public class PostAjusteBoleta : RequestBase, IReturn<AjusteBoletaDto>
    {
        public int BoletaId { get; set; }
    }

    [Route("/ajuste-boletas/activate/{ajusteBoletaId}", "POST")]
    public class PostActiveAjusteBoleta : RequestBase, IReturn<AjusteBoletaDto>
    {
        public int AjusteBoletaId { get; set; }
    }

    [Route("/ajuste-boletas/{ajusteBoletaId}", "DELETE")]
    public class DeleteAjusteBoleta : RequestBase, IReturn<AjusteBoletaDto>
    {
        public int AjusteBoletaId { get; set; }
    }
}
