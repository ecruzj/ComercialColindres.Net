using System;

namespace ComercialColindres.Datos.Entorno.Modelos
{
    public class RptOrdenesCombustibleOperativo
    {
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoFactura { get; set; }
        public string AutorizadoPor { get; set; }
        public string PlacaEquipo { get; set; }
        public string NombreCliente { get; set; }
        public string Motorista { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
