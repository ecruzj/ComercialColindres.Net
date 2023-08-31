namespace ComercialColindres.Datos.Entorno.Modelos.Reportes
{
    public class RptPrestamosPendientes
    {
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public decimal TotalPrestamo { get; set; }
        public decimal TotalAbono { get; set; }

        public decimal ObtenerSaldoPendientePorProveedor()
        {
            return TotalPrestamo - TotalAbono;
        }
    }
}
