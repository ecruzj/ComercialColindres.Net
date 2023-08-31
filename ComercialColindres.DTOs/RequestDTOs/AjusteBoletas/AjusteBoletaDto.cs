using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletas
{
    public class AjusteBoletaDto : BaseDTO
    {
        public int AjusteBoletaId { get; set; }
        public int BoletaId { get; set; }
        public string Estado { get; set; }

        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Proveedor { get; set; }
        public string TipoProducto { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Planta { get; set; }
        public decimal Total { get; set; }
        public decimal Abonos { get; set; }
        public decimal Saldo { get; set; }
    }
}
