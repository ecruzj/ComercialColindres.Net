namespace ComercialColindres.Servicios.Clases
{
    public interface IStorageService
    {
        FileSaveResult SaveFile(string boletaStorageRoot, byte[] fileBytes, string filteName);
    }
}
