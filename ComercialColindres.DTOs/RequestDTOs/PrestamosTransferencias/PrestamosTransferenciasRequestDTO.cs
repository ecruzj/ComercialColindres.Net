using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias
{
    [Route("/prestamos-transferencias/busqueda/{prestamoId}", "GET")]
    public class GetPrestamosTransferenciasPorPrestamoId : IReturn<List<PrestamosTransferenciasDTO>>
    {
        public int PrestamoId { get; set; }
    }

    [Route("/prestamos-transferencias/", "POST")]
    public class PostPrestamoTransferencias : IReturn<ActualizarResponseDTO>
    {
        public int PrestamoId { get; set; }
        public string UserId { get; set; }
        public List<PrestamosTransferenciasDTO> PrestamoTransferencias { get; set; }
    }
}
