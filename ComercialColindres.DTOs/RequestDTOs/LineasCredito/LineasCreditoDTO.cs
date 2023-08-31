using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCredito
{
    public class LineasCreditoDTO : BaseDTO
    {
        public int LineaCreditoId { get; set; }
        public string CodigoLineaCredito { get; set; }
        public int SucursalId { get; set; }
        public int CuentaFinancieraId { get; set; }
        public string Estado { get; set; }
        public string NoDocumento { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal Saldo { get; set; }
        public string CreadoPor { get; set; }
        public bool EsLineaCreditoActual { get; set; }

        public bool RequiereBanco { get; set; }
        public int BancoId { get; set; }
        public string NombreBanco { get; set; }
        public string NombreSucursal { get; set; }
        public string CuentaFinanciera { get; set; }
        public string TipoCredito { get; set; }
        public decimal CreditoDisponible { get; set; }
        public decimal DeduccionTotal { get; set; }
        public int CuentaFinancieraTipoId { get; set; }
    }
}
