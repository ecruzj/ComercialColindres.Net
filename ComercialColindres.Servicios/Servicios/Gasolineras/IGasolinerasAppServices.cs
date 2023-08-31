using ComercialColindres.DTOs.RequestDTOs.Gasolineras;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IGasolinerasAppServices
    {
        GasolinerasDTO Get(GetGasolinera request);
        List<GasolinerasDTO> Get(GetGasolinerasPorValorBusqueda request);
        ActualizarResponseDTO Post(PostGasolinera request);
        ActualizarResponseDTO Put(PutGasolinera request);
    }
}
