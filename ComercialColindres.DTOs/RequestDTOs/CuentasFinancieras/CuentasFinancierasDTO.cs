using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras
{
    public class CuentasFinancierasDTO : BaseDTO
    {
        public int CuentaFinancieraId { get; set; }
        public int? BancoId { get; set; }
        public int CuentaFinancieraTipoId { get; set; }
        public string CuentaNo { get; set; }
        public string NombreAbonado { get; set; }
        public string Cedula { get; set; }
        public string Estado { get; set; }
    }
}
