using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IOrdenesCombustibleAppServices
    {
        BusquedaOrdenesCombustibleDTO Get(GetByValorOrdenesCombustible request);
        List<OrdenesCombustibleDTO> Get(GetOrderFuelByVendorId request);
        List<OrdenesCombustibleDTO> Get(GetOrdenesCombustiblePorGasCreditoId request);
        List<OrdenesCombustibleDTO> Get(GetOrdenesCombustibleByBoletaId request);
        OrdenesCombustibleDTO AssignFuelOrdersToBoleta(PutOrdenesCombustibleABoleta request);
        OrdenesCombustibleDTO Post(PostOrdenesCombustible request);
        ActualizarResponseDTO Put(PutOrdenesCombustible request);
        EliminarResponseDTO Delete(DeleteOrdenesCombustible request);
    }
}
