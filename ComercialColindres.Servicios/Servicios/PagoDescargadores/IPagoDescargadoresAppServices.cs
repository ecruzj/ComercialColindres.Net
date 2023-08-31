using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios
{
    public interface IPagoDescargadoresAppServices
    {
        PagoDescargadoresDTO Get(GetPagoDescargadoresUltimo request);
        BusquedaPagoDescargadoresDTO Get(GetByValorPagosDescargas request);
        List<PagoDescargadoresDTO> Post(PostPagosDescargas request);
        List<PagoDescargadoresDTO> Put(PutPagoDescargas request);
        EliminarResponseDTO Delete(DeletePagoDescargas request);  
    }
}
