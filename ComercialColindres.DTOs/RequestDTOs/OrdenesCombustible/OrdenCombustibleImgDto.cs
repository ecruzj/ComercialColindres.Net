using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    public class OrdenCombustibleImgDto : BaseDTO
    {
        public int OrdenCombustibleId { get; set; }
        public byte[] Imagen { get; set; }
    }
}
