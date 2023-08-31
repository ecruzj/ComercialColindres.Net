using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs
{
    [Route("/boletas-humedad/buscar-por-valor", "GET")]
    public class GetByValorBoletasHumedad : BusquedaRequestBaseDTO, IReturn<BusquedaBoletasHumedadDto>
    {
    }

    [Route("/boletas-humedad/", "PUT")]
    public class PutBoletasHumedad : RequestBase, IReturn<List<BoletaHumedadDto>>
    {
        public int FacilityDestination { get; set; }
        public List<BoletaHumedadDto> BoletasHumedad { get; set; }
    }

    [Route("/boletas-humedad/", "DELETE")]
    public class DeleteBoletasHumedad : RequestBase, IReturn<BoletaHumedadDto>
    {
        public int BoletaHumedadId { get; set; }
    }
}
