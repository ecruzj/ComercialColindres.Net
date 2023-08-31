using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    [Route("/ordenes-combustible/buscar-por-valor", "GET")]
    public class GetByValorOrdenesCombustible : BusquedaRequestBaseDTO, IReturn<BusquedaOrdenesCombustibleDTO>
    {
    }

    [Route("/ordenes-combustible/busqueda-asignaciones/{proveedorId}", "GET")]
    public class GetOrderFuelByVendorId : RequestBase, IReturn<List<OrdenesCombustibleDTO>>
    {
        public int ProveedorId { get; set; }
    }

    [Route("/ordenes-combustible/busqueda-porcredito/{gasCreditoId}", "GET")]
    public class GetOrdenesCombustiblePorGasCreditoId : IReturn<List<OrdenesCombustibleDTO>>
    {
        public int GasCreditoId { get; set; }
    }

    [Route("/ordenes-combustible/busqueda/{boletaId}", "GET")]
    public class GetOrdenesCombustibleByBoletaId : RequestBase, IReturn<List<OrdenesCombustibleDTO>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/ordenes-combustible/a-boleta", "PUT")]
    public class PutOrdenesCombustibleABoleta : RequestBase, IReturn<OrdenesCombustibleDTO>
    {
        public int BoletaId { get; set; }
        public List<OrdenesCombustibleDTO> OrdenesCombustible { get; set; }
    }

    [Route("/ordenes-combustible/a-cliente", "POST")]
    public class PostOrdenesCombustible : RequestBase, IReturn<OrdenesCombustibleDTO>
    {
        public OrdenesCombustibleDTO OrdenCombustible { get; set; }
    }

    [Route("/ordenes-combustible/", "PUT")]
    public class PutOrdenesCombustible : IReturn<ActualizarResponseDTO>
    {
        public string UserId { get; set; }
        public OrdenesCombustibleDTO OrdenCombustible { get; set; }
    }

    [Route("/ordenes-combustible/", "DELETE")]
    public class DeleteOrdenesCombustible : IReturn<EliminarResponseDTO>
    {
        public int OrdenCombustibleId { get; set; }
        public string UserId { get; set; }
    }
}
