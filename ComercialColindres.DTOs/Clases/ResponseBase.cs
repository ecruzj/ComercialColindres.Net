namespace ComercialColindres.DTOs.Clases
{
    public abstract class ResponseBase
    {
        public string Message { get; set; }
        public string ValidationErrorMessage { get; set; }
    }
}
