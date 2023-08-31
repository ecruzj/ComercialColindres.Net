using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PagoDescargadores
{
    [Route("/pago-descargas/buscar-por-valor", "GET")]
    public class GetByValorPagosDescargas : BusquedaRequestBaseDTO, IReturn<BusquedaPagoDescargadoresDTO>
    {
    }
    
    [Route("/pago-descargas/", "POST")]
    public class PostPagosDescargas : RequestBase, IReturn<List<PagoDescargadoresDTO>>
    {
        public PagoDescargadoresDTO PagoDescargas { get; set; }
        public List<DescargadoresDTO> Descargas { get; set; }
    }

    [Route("/pago-descargas/ultimo-correlativo", "GET")]
    public class GetPagoDescargadoresUltimo : IReturn<PagoDescargadoresDTO>
    {
        public int SucursalId { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Route("/pago-descargas/actualizar", "PUT")]
    public class PutPagoDescargas : RequestBase, IReturn<List<PagoDescargadoresDTO>>
    {
        public PagoDescargadoresDTO PagoDescargas { get; set; }
        public List<DescargadoresDTO> Descargas { get; set; }
    }

    [Route("/pago-descargas/eliminar", "DELETE")]
    public class DeletePagoDescargas : IReturn<EliminarResponseDTO>
    {
        public int PagoDescargaId { get; set; }
        public string UserId { get; set; }
    }
}
