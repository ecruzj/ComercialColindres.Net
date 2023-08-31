using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptPrestamosPorProveedorResumenDTO
    {
        public List<RptPrestamosPorProveedorEncabezadoDTO> EncabezadoPrestamos { get; set; }
        public List<RptPrestamosPorProveedorDetalleDTO> AbonosPorBoletas { get; set; }

        public decimal TotalPrestamos { get; set; }
        public decimal TotalAbonos { get; set; }
        public decimal SaldoPendiente { get; set; }

        public decimal ObtenerTotalPrestamos()
        {
            var totalPrestamo = 0m;

            foreach (var prestamo in EncabezadoPrestamos)
            {
                totalPrestamo += prestamo.TotalCobrar;
            }

            return totalPrestamo;
        }

        public decimal ObtenerTotalAbonos()
        {
            return Math.Abs(AbonosPorBoletas.Sum(b => b.MontoAbono));
        }

        public decimal ObtenerSaldoPendiente()
        {
            return ObtenerTotalPrestamos() - ObtenerTotalAbonos();
        }
    }
}
