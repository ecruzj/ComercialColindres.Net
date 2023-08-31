using ComercialColindres.DTOs.RequestDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletaHumedadAppServices
    {
        BusquedaBoletasHumedadDto GetBoletasHumedadPaged(GetByValorBoletasHumedad request);
        List<BoletaHumedadDto> CreateBoletasHumedad(PutBoletasHumedad request);
        BoletaHumedadDto DeleteBoletaHumedad(DeleteBoletasHumedad request);
    }
}
