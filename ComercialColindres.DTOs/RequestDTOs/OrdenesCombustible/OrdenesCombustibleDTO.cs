using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    public class OrdenesCombustibleDTO : BaseDTO
    {
        public int OrdenCombustibleId { get; set; }
        public string CodigoFactura { get; set; }
        public int GasCreditoId { get; set; }
        public string AutorizadoPor { get; set; }
        public int? BoletaId { get; set; }
        public int? ProveedorId { get; set; }
        public string PlacaEquipo { get; set; }   
        public decimal Monto { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public bool EsOrdenPersonal { get; set; }
        public string EstadoFacturacion { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }

        public string Proveedor { get; set; }
        public string NombreGasolinera { get; set; }
        public string CodigoCreditoCombustible { get; set; }
        public string CodigoBoleta { get; set; }        
        public string EstadoBoleta { get; set; }
        public byte[] Imagen { get; set; }
        public string NombreProveedor { get; set; }
        public string FuelOrderSpecification { get; set; }
        public decimal Payments { get; set; }
        public decimal OutStandingBalance { get; set; }
        public bool IsClosed { get; set; }

        public string GetOrderFuelName()
        {
            string factory = NombreGasolinera.Substring(0);

            return $"{factory}-{CodigoFactura}.JPG";
        }
    }
}
