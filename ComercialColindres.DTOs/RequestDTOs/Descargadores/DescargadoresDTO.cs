using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Descargadores
{
    public class DescargadoresDTO : BaseDTO
    {
        public int DescargadaId { get; set; }
        public int BoletaId { get; set; }
        public int CuadrillaId { get; set; }
        public decimal PrecioDescarga { get; set; }
        public decimal PagoDescarga { get; set; }
        public int? PagoDescargaId { get; set; }
        public bool EsDescargaPorAdelanto { get; set; }
        public string Estado { get; set; }
        public string EstadoBoleta { get; set; }
        public System.DateTime FechaDescarga { get; set; }

        public string EncargadoCuadrilla { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string PlacaEquipo { get; set; }
        public string NombreProveedor { get; set; }
        public string DescripcionTipoProducto { get; set; }
        public string NombrePlanta { get; set; }
        public string Motorista { get; set; }
        public string DescripcionTipoEquipo { get; set; }
        public string CodigoPagoDescarga { get; set; }
        public bool HasShippingNumber { get; set; }
    }
}
