using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Descargadores
{
    [Route("/descargas/busqueda-porCuadrillaId/", "GET")]
    public class GetDescargasPorCuadrillaId : IReturn<List<DescargadoresDTO>>
    {
        public int CuadrillaId { get; set; }
    }

    [Route("/descargas/busqueda-pagos/{pagoDescargaId}", "GET")]
    public class GetDescargasPorPagoDescargaId : IReturn<List<DescargadoresDTO>>
    {
        public int PagoDescargaId { get; set; }
    }

    [Route("/descargas/pago/busqueda-porCuadrillaId/", "GET")]
    public class GetDescargasAplicaPagoPorCuadrillaId : IReturn<List<DescargadoresDTO>>
    {
        public int CuadrillaId { get; set; }
    }

    [Route("/descargadores/buscar-por-valor", "GET")]
    public class GetByValorDescargadores : BusquedaRequestBaseDTO, IReturn<BusquedaDescargadoresDTO>
    {
    }

    [Route("/descargadores/anular", "PUT")]
    public class PutDescargaAnular : IReturn<EliminarResponseDTO>
    {
        public int DescargadaId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/descargadores/", "PUT")]
    public class PutDescarga : IReturn<ActualizarResponseDTO>
    {
        public DescargadoresDTO Descarga { get; set; }
        public string UserId { get; set; }
    }

    [Route("/descargadores/", "POST")]
    public class PostDescargadores : IReturn<ActualizarResponseDTO>
    {
        public DescargadoresDTO Descarga { get; set; }
        public string UserId { get; set; }
    }
}
