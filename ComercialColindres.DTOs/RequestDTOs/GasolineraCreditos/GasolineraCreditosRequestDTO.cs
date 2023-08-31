using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos
{
    [Route("/gas-creditos/buscar-por-valor", "GET")]
    public class GetByValorGasCreditos : BusquedaRequestBaseDTO, IReturn<BusquedaGasolineraCreditosDTO>
    {
    }

    [Route("/gas-creditos/", "POST")]
    public class PostGasolineraCreditos : IReturn<ActualizarResponseDTO>
    {
        public string UserId { get; set; }
        public int SucursalId { get; set; }
        public GasolineraCreditosDTO GasolineraCredito { get; set; }
    }

    [Route("/gas-creditos/ultimo-correlativo", "GET")]
    public class GetGasolineraCreditoUltimo : IReturn<GasolineraCreditosDTO>
    {
        public int SucursalId { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Route("/gas-creditos/credito-actual", "GET")]
    public class GetGasolineraCreditoActual : IReturn<GasolineraCreditosDTO>
    {
        public int GasolineraId { get; set; }
    }

    [Route("/gas-creditos/activar", "PUT")]
    public class PutActivarGasolineraCreditos : IReturn<ActualizarResponseDTO>
    {
        public int GasCreditoId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/gas-creditos/", "PUT")]
    public class PutGasolineraCreditos : IReturn<ActualizarResponseDTO>
    {
        public GasolineraCreditosDTO GasolineraCredito { get; set; }
        public string UserId { get; set; }
    }

    [Route("/gas-creditos/", "DELETE")]
    public class DeleteGasolineraCredito : IReturn<EliminarResponseDTO>
    {
        public int GasolineraCreditoId { get; set; }
        public string UserId { get; set; }
    }
}
