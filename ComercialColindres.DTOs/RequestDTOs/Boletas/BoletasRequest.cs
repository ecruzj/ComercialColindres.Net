using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Boletas
{
    [Route("/boletas/{boletaNo}", "GET")]
    public class GetBoleta : IReturn<BoletasDTO>
    {
        public string BoletaNo { get; set; }
        public int PlantaId { get; set; }
    }

    [Route("/boletas/buscar-por-valor", "GET")]
    public class GetByValorBoletas : BusquedaRequestBaseDTO, IReturn<BusquedaBoletasDTO>
    {
    }

    [Route("/boletas/pendientes-facturar/{plantaId}", "GET")]
    public class GetBoletasPendientesDeFacturar : IReturn<List<BoletasDTO>>
    {
        public int PlantaId { get; set; }
    }

    [Route("/boletas/in-process", "GET")]
    public class GetBoletasInProcess : RequestBase, IReturn<List<BoletasDTO>>
    {
    }

    [Route("/boletas/busqueda-valorbusqueda", "GET")]
    public class GetBoletasPorValorBusqueda : IReturn<List<BoletasDTO>>
    {
        public int PlantaId { get; set; }
        public string ValorBusqueda { get; set; }
    }

    [Route("/boletas/", "PUT")]
    public class PutBoleta : IReturn<ActualizarResponseDTO>
    {
        public BoletasDTO Boleta { get; set; }
        public string UserId { get; set; }
    }

    [Route("/boletas/cierre-forzado", "PUT")]
    public class PutCierreForzadoBoleta : IReturn<ActualizarResponseDTO>
    {
        public int BoletaId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/boletas/open-boleta-by-id", "PUT")]
    public class OpenBoletaById : RequestBase, IReturn<BoletasDTO>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boletas/boleta-properties", "PUT")]
    public class UpdateBoletaProperties : RequestBase, IReturn<BoletasDTO>
    {
        public BoletasDTO Boleta { get; set; }
    }

    [Route("/boletas/", "POST")]
    public class PostBoleta : IReturn<ActualizarResponseDTO>
    {
        public BoletasDTO Boleta { get; set; }
        public string UserId { get; set; }
    }

    [Route("/boletas/eliminar", "DELETE")]
    public class DeleteBoleta : IReturn<EliminarResponseDTO>
    {
        public int BoletaId { get; set; }
        public string UserId { get; set; }
    }
}
