using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPagoPrestamosAppServices
    {
        List<PagoPrestamosDTO> Get(GetPagoPrestamosPorBoletaId request);
        List<PagoPrestamosDTO> Get(GetPagoPrestamos request);
        PagoPrestamosDTO Post(PostPagosPrestamoPorBoletaId request);
        PagoPrestamosDTO Post(PostOtrosAbonosPrestamo request);
        PagoPrestamosDTO Put(PutAbonosPrestamoPorBoletas request);
    }
}
