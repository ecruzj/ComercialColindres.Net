using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Conductores
{
    public class ConductoresDTO : BaseDTO
    {
        public int ConductorId { get; set; }
        public string Nombre { get; set; }
        public int ProveedorId { get; set; }
        public string Telefonos { get; set; }
    }
}
