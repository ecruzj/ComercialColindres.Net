using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletasAppServices
    {
        BoletasDTO Get(GetBoleta request);
        BusquedaBoletasDTO Get(GetByValorBoletas request);
        List<BoletasDTO> Get(GetBoletasPorValorBusqueda request);
        List<BoletasDTO> Get(GetBoletasPendientesDeFacturar request);
        List<BoletasDTO> Get(GetBoletasInProcess request);
        ActualizarResponseDTO Put(PutBoleta request);
        ActualizarResponseDTO Put(PutCierreForzadoBoleta request);
        BoletasDTO OpenBoletaById(OpenBoletaById request);
        ActualizarResponseDTO Post(PostBoleta request);
        EliminarResponseDTO Delete(DeleteBoleta request);
        BoletasDTO UpdateBoletaProperties(UpdateBoletaProperties request);
        bool ExisteBoleta(string codigoBoleta, string numeroEnvio, int plantaId);
    }
}
