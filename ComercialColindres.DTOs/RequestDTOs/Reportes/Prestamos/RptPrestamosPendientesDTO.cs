using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptPrestamosPendientesDTO
    {
        public RptPrestamosPendientesDTO()
        {
            this.ListaPrestamosPendientes = new List<RptDatosPrestamosPendientes>();
        }

        public List<RptDatosPrestamosPendientes> ListaPrestamosPendientes { get; set; }
        public decimal SaldoPendiente { get; set; }

        public decimal ObtenerSaldoPendiente()
        {
            return ListaPrestamosPendientes.Sum(s => s.TotalPrestamo) - ListaPrestamosPendientes.Sum(s => s.TotalAbono);
        }
    }

    public class RptDatosPrestamosPendientes
    {
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public decimal TotalPrestamo { get; set; }
        public decimal TotalAbono { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}
