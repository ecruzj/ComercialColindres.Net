using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.BoletaCierres
{
    [Route("/boleta-cierres/busqueda/{boletaId}", "GET")]
    public class GetBoletasCierrePorBoletaId : IReturn<List<BoletaCierresDTO>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/boleta-cierres/", "POST")]
    public class PostBoletasCierre : IReturn<ActualizarResponseDTO>
    {
        public int BoletaId { get; set; }
        public string UserId { get; set; }
        public int SucursalId { get; set; }
        public List<BoletaCierresDTO> BoletaCierres { get; set; }
        public List<BoletaDetallesDTO> BoletaDetalles { get; set; }
    }

    [Route("/boleta-cierres/", "PUT")]
    public class CloseBoletaMasive : RequestBase, IReturn<List<BoletaCierresDTO>>
    {
        public int VendorId { get; set; }
        public bool IsPartialPayment { get; set; }
        public List<int> BoletasToPay { get; set; }
        public List<BoletaCierresDTO> BoletaCierres { get; set; }
    }
}
