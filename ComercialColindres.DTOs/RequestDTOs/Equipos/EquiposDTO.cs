using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Equipos
{
    public class EquiposDTO : BaseDTO
    {
        public int EquipoId { get; set; }
        public int EquipoCategoriaId { get; set; }
        public int ProveedorId { get; set; }
        public string PlacaCabezal { get; set; }
        public string Estado { get; set; }

        public string DescripcionCategoria { get; set; }
    }
}
