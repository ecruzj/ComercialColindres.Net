using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasBancarias
{
    public class CuentasBancariasDTO : BaseDTO
    {
        public int CuentaId { get; set; }
        public int ProveedorId { get; set; }
        public int BancoId { get; set; }
        public string CuentaNo { get; set; }
        public string NombreAbonado { get; set; }
        public string CedulaNo { get; set; }
        public string Estado { get; set; }

        public string NombreBanco { get; set; }
    }
}
