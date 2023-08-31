using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.CategoriaProductos
{
    public class CategoriaProductosDTO : BaseDTO
    {
        public int CategoriaProductoId { get; set; }
        public int BiomasaId { get; set; }
        public string Descripcion { get; set; }

        public string DescripcionBiomasa { get; set; }
    }
}
