using ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPagoDescargaDetallesAppServices
    {
        List<PagoDescargaDetallesDTO> Get(GetPagoDescargasDetallePorPagoDescargaId request);
        ActualizarResponseDTO Post(PostPagoDescargasDetalle request);
    }
}
