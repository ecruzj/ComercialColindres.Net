using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteTipos
{
    public class AjusteTipoDto : BaseDTO
    {
        public int AjusteTipoId { get; set; }
        public string Descripcion { get; set; }
        public bool UseCreditLine { get; set; }
    }
}
