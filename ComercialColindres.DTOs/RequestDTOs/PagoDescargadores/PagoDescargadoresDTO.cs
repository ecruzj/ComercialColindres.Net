using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.PagoDescargadores
{
    public class PagoDescargadoresDTO : BaseDTO
    {
        public int PagoDescargaId { get; set; }
        public string CodigoPagoDescarga { get; set; }
        public int CuadrillaId { get; set; }
        public decimal TotalPago { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPago { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public string EncargadoCuadrilla { get; set; }
        public string NombrePlanta { get; set; }
        public int PlantaId { get; set; }
        public decimal TotalJustificado { get; set; }
        public int TotalBoletaDescargas { get; set; }
        public bool EsActualizacion { get; set; }
        public bool HasShippingNumber { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoBoleta { get; set; }

        public string ConteoDescargas { get; set; }
    }
}
