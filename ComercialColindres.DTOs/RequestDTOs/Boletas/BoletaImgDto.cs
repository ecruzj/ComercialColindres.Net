using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.Boletas
{
    public class BoletaImgDto : BaseDTO
    {
        public int BoletaId { get; set; }
        public byte[] Imagen { get; set; }
    }
}
