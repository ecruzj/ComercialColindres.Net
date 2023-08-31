using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IProveedoresAppServices
    {
        ProveedoresDTO Get(GetProveedor request);
        BusquedaProveedoresDTO Get(GetByValorProveedores request);
        List<ProveedoresDTO> Get(GetProveedoresPorValorBusqueda request);
        ActualizarResponseDTO Put(PutProveedor request);
        ActualizarResponseDTO Post(PostProveedor request);
        EliminarResponseDTO Delete(DeleteProveedor request);
    }
}
