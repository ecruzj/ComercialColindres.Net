using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCredito
{
    [Route("/lineas-credito/busqueda/porbanco-portipocuenta", "GET")]
    public class GetLineasCreditoPorBancoPorTipoCuenta : IReturn<List<LineasCreditoDTO>>
    {
        public int BancoId { get; set; }
        public int SucursalId { get; set; }
        public int UsuarioId { get; set; }
        public int CuentaFinancieraTipoId { get; set; }
    }

    [Route("/lineas-credito/busqueda/valores/", "GET")]
    public class GetByValorLineasCredito : BusquedaRequestBaseDTO, IReturn<BusquedaLineasCreditoDTO>
    {
        public int SucursalId { get; set; }
        public int UsuarioId { get; set; }
    }

    [Route("/lineas-credito/busqueda/", "GET")]
    public class GetLineasCreditoCajaChica : IReturn<List<LineasCreditoDTO>>
    {
        public int SucursalId { get; set; }
        public int UsuarioId { get; set; }
    }

    [Route("/lineas-credito/busqueda/ultimo-correlativo/", "GET")]
    public class GetLineaCreditoUltimo : IReturn<LineasCreditoDTO>
    {
        public int SucursalId { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Route("/lineas-credito/", "POST")]
    public class PostLineaCredito : IReturn<ActualizarResponseDTO>
    {
        public LineasCreditoDTO LineaCredito { get; set; }
        public string UserId { get; set; }
        public int SucursalId { get; set; }
    }

    [Route("/lineas-credito/", "PUT")]
    public class PutLineaCredito : IReturn<ActualizarResponseDTO>
    {
        public LineasCreditoDTO LineaCredito { get; set; }
        public string UserId { get; set; }
    }

    [Route("/lineas-credito/activar/", "PUT")]
    public class PutActivarLineaCredito : IReturn<ActualizarResponseDTO>
    {
        public int LineaCreditoId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/lineas-credito/anular/", "PUT")]
    public class PutLineaCreditoAnular : IReturn<ActualizarResponseDTO>
    {
        public int LineaCreditoId { get; set; }
        public string UserId { get; set; }
    }
}
