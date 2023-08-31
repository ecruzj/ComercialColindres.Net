using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Gasolineras
{
    [Route("/gasolineras/{gasolineraId}", "GET")]
    public class GetGasolinera : IReturn<GasolinerasDTO>
    {
        public int GasolineraId { get; set; }
    }

    [Route("/gasolineras/por-valorbusqueda", "GET")]
    public class GetGasolinerasPorValorBusqueda : IReturn<List<GasolinerasDTO>>
    {
        public string ValorBusqueda { get; set; }
        public bool ConGasCredito { get; set; }
    }

    [Route("/gasolineras/", "PUT")]
    public class PutGasolinera : IReturn<ActualizarResponseDTO>
    {
        public GasolinerasDTO Gasolinera { get; set; }
    }

    [Route("/gasolineras/", "POST")]
    public class PostGasolinera : IReturn<ActualizarResponseDTO>
    {
        public GasolinerasDTO Gasolinera { get; set; }
    }
}
