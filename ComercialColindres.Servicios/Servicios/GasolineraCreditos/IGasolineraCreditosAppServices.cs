using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.DTOs.ResponseDTOs;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IGasolineraCreditosAppServices
    {
        GasolineraCreditosDTO Get(GetGasolineraCreditoUltimo request);
        BusquedaGasolineraCreditosDTO Get(GetByValorGasCreditos request);
        GasolineraCreditosDTO Get(GetGasolineraCreditoActual request);
        ActualizarResponseDTO Post(PostGasolineraCreditos request);
        ActualizarResponseDTO Put(PutGasolineraCreditos request);
        ActualizarResponseDTO Put(PutActivarGasolineraCreditos request);
        EliminarResponseDTO Delete(DeleteGasolineraCredito request);
    }
}
