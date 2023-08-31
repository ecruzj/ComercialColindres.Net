using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones
{
    [Route("/lineas-credito-deducciones/busqueda/varias/{lineaCreditoId}", "GET")]
    public class GetDeduccionesVariasPorLineaCreditoId : IReturn<List<LineasCreditoDeduccionesDTO>>
    {
        public int LineaCreditoId { get; set; }
    }

    [Route("/lineas-credito-deducciones/busqueda/operativo/{lineaCreditoId}", "GET")]
    public class GetDeduccionesOperativosPorLineaCreditoId : IReturn<List<LineasCreditoDeduccionesDTO>>
    {
        public int LineaCreditoId { get; set; }
    }

    [Route("/lineas-credito-deducciones/", "POST")]
    public class PostDeduccionVarios : IReturn<ActualizarResponseDTO>
    {
        public LineasCreditoDeduccionesDTO LineaCreditoDeduccion { get; set; }
        public string UserId { get; set; }
    }

    [Route("/lineas-credito-deducciones/", "PUT")]
    public class PutDeduccionVarios : IReturn<ActualizarResponseDTO>
    {
        public LineasCreditoDeduccionesDTO LineaCreditoDeduccion { get; set; }
        public string UserId { get; set; }
    }

    [Route("/lineas-credito-deducciones/", "DELETE")]
    public class DeleteDeduccionVarios : IReturn<EliminarResponseDTO>
    {
        public int LineaCreditoDeuccionId { get; set; }
        public string UserId { get; set; }
    }
}
