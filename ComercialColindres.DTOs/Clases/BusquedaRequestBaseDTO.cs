namespace ComercialColindres.DTOs.Clases
{
    public class BusquedaRequestBaseDTO
    {
        public int CantidadRegistros { get; set; }
        public int PaginaActual { get; set; }
        public string Filtro { get; set; }
    }
}
