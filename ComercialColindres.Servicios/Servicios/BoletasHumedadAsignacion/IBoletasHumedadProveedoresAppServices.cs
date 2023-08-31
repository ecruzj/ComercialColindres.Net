using ComercialColindres.DTOs.RequestDTOs.BoletasHumedadProveedores;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBoletasHumedadProveedoresAppServices
    {
        List<BoletaHumedadProveedorDto> GetBoletasHumidityByVendor(GetBoletasHumidityByVendorId request);
    }
}
