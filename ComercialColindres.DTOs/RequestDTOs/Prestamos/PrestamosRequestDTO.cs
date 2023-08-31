using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Prestamos
{
    [Route("/prestamos/buscar-por-valor", "GET")]
    public class GetByValorPrestamos : BusquedaRequestBaseDTO, IReturn<BusquedaPrestamosDTO>
    {
    }

    [Route("/prestamos/ultimo-correlativo", "GET")]
    public class GetPrestamoUltimo : IReturn<PrestamosDTO>
    {
        public int SucursalId { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Route("/prestamos/{proveedorId}", "GET")]
    public class GetPrestamoPorProveedorId : IReturn<List<PrestamosDTO>>
    {
        public int ProveedorId { get; set; }
    }

    [Route("/prestamos/", "PUT")]
    public class PutPrestamo : IReturn<ActualizarResponseDTO>
    {
        public PrestamosDTO Prestamo { get; set; }
        public string UserId { get; set; }
    }

    [Route("/prestamos/", "POST")]
    public class PostPrestamo : IReturn<ActualizarResponseDTO>
    {
        public PrestamosDTO Prestamo { get; set; }
        public string UserId { get; set; }
    }

    [Route("/prestamos/anular", "PUT")]
    public class PutPrestamoAnular : IReturn<ActualizarResponseDTO>
    {
        public int PrestamoId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/prestamos/activar", "PUT")]
    public class PutActivarPrestamo : IReturn<ActualizarResponseDTO>
    {
        public int PrestamoId { get; set; }
        public string UserId { get; set; }
    }
}
