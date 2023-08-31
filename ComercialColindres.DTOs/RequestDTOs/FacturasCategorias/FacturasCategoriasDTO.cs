using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.FacturasCategorias
{
    public class FacturasCategoriasDTO : BaseDTO
    {
        public int FacturaCategoriaId { get; set; }
        public string Descripcion { get; set; }
    }
}
