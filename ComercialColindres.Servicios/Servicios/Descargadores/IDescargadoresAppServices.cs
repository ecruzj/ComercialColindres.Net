using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IDescargadoresAppServices
    {
        List<DescargadoresDTO> Get(GetDescargasPorPagoDescargaId request);
        List<DescargadoresDTO> Get(GetDescargasPorCuadrillaId request);
        List<DescargadoresDTO> Get(GetDescargasAplicaPagoPorCuadrillaId request);
        BusquedaDescargadoresDTO Get(GetByValorDescargadores request);
        EliminarResponseDTO Put(PutDescargaAnular request);
        ActualizarResponseDTO Put(PutDescarga request);
        ActualizarResponseDTO Post(PostDescargadores request);
    }
}
