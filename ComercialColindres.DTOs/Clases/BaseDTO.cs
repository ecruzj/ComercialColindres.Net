namespace ComercialColindres.DTOs.Clases
{
    public class BaseDTO : ResponseBase
    {
        public string MensajeError { get; set; }
        public bool EstaEditandoRegistro { get; set; }
    }
}
