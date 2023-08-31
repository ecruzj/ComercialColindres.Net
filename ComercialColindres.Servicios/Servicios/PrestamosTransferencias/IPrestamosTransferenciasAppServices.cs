using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPrestamosTransferenciasAppServices
    {
        List<PrestamosTransferenciasDTO> Get(GetPrestamosTransferenciasPorPrestamoId request);
        ActualizarResponseDTO Post(PostPrestamoTransferencias request);
    }
}
