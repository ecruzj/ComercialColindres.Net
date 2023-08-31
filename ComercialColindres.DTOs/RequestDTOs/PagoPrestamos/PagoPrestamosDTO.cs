using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.PagoPrestamos
{
    public class PagoPrestamosDTO : BaseDTO
    {
        public int PagoPrestamoId { get; set; }
        public int PrestamoId { get; set; }
        public string FormaDePago { get; set; }
        public int? BancoId { get; set; }
        public string NoDocumento { get; set; }
        public int? BoletaId { get; set; }
        public decimal MontoAbono { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public string CodigoBoleta { get; set; }
        public bool PuedeEditarAbonoPorBoleta { get; set; }
        public string PlantaDestino { get; set; }
        public string CodigoPrestamo { get; set; }
        public string NombreBanco { get; set; }
    }
}
