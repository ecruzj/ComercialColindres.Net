using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PagoPrestamos
{
    [Route("/pago-prestamos/busqueda/{boletaId}", "GET")]
    public class GetPagoPrestamosPorBoletaId : IReturn<List<PagoPrestamosDTO>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/pago-prestamos/busqueda-pagos/", "GET")]
    public class GetPagoPrestamos : RequestBase, IReturn<List<PagoPrestamosDTO>>
    {
        public int PrestamoId { get; set; }
        public bool FiltrarAbonosPorBoletas { get; set; }
        public bool MostrarTodosLosAbonos { get; set; }
    }

    [Route("/pago-prestamos/por-boleta", "POST")]
    public class PostPagosPrestamoPorBoletaId : RequestBase, IReturn<PagoPrestamosDTO>
    {
        public int BoletaId { get; set; }
        public List<PagoPrestamosDTO> PagoPrestamos { get; set; }
    }

    [Route("/pago-prestamos/otros-abonos", "POST")]
    public class PostOtrosAbonosPrestamo : RequestBase, IReturn<PagoPrestamosDTO>
    {
        public int PrestamoId { get; set; }
        public List<PagoPrestamosDTO> PagoPrestamos { get; set; }
    }

    [Route("/pago-prestamos/por-boletas", "PUT")]
    public class PutAbonosPrestamoPorBoletas : RequestBase, IReturn<PagoPrestamosDTO>
    {
        public int PrestamoId { get; set; }
        public List<PagoPrestamosDTO> PagoPrestamos { get; set; }
    }
}
