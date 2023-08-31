using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletasHumedadAsignacionAppServices
    {
        List<BoletaHumedadAsignacionDto> GetBoletasHumidityByVendor(GetBoletasHumedadByVendor request);
    }
}
