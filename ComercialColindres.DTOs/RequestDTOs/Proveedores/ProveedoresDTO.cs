using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Proveedores
{
    public class ProveedoresDTO : BaseDTO
    {
        public int ProveedorId { get; set; }
        public string CedulaNo { get; set; }
        public string RTN { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Estado { get; set; }
        public bool IsExempt { get; set; }
    }
}
