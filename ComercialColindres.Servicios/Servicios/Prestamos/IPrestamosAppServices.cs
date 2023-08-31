using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPrestamosAppServices
    {
        BusquedaPrestamosDTO Get(GetByValorPrestamos request);
        PrestamosDTO Get(GetPrestamoUltimo request);
        List<PrestamosDTO> Get(GetPrestamoPorProveedorId request);
        ActualizarResponseDTO Put(PutPrestamo request);
        ActualizarResponseDTO Put(PutActivarPrestamo request);
        ActualizarResponseDTO Post(PostPrestamo request);
        ActualizarResponseDTO Put(PutPrestamoAnular request);
    }
}
