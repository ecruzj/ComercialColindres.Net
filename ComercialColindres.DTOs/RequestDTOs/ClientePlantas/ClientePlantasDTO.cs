using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.ClientePlantas
{
    public class ClientePlantasDTO : BaseDTO
    {
        public int PlantaId { get; set; }
        public string RTN { get; set; }
        public string NombrePlanta { get; set; }
        public string Telefonos { get; set; }
        public string Direccion { get; set; }
        public bool RequiresPurchaseOrder { get; set; }
        public bool RequiresWeekNo { get; set; }
        public bool RequiresProForm { get; set; }
        public bool IsExempt { get; set; }
        public bool HasSubPlanta { get; set; }
        public bool ImgHorizontalFormat { get; set; }

        public bool IsShippingNumberRequired { get; set; }
    }
}
