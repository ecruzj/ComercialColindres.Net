using ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IDescargasPorAdelantadoAppServices
    {
        List<DescargasPorAdelantadoDTO> Get(GetDescargasAdelantadasPendientes request);
        List<DescargasPorAdelantadoDTO> Get(GetDescargasAdelantadasPorPagoDescargadaId request);
        DescargasPorAdelantadoDTO Post(PostDescargasPorAdelantado request);               
    }
}
