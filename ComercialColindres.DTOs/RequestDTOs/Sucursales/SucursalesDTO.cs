using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Sucursales
{
    public class SucursalesDTO : BaseDTO
    {
        public int SucursalId { get; set; }
        public string CodigoSucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public string Estado { get; set; }
    }
}
