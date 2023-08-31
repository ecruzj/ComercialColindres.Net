using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptHistorialBoletasDTO
    {
        public RptHistorialBoletasDTO()
        {
            this.HistorialBoletas = new List<RptHistorialDatoBoletasDTO>();
        }

        public List<RptHistorialDatoBoletasDTO> HistorialBoletas { get; set; }
        public decimal TotalToneladas { get; set; }
        public decimal TotalIngreso { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal TotalPendiente { get; set; }
        public int TotalBoletas { get; set; }
        public decimal TotalAbono { get; set; }

        public int ObtenerTotalBoletas()
        {
            return HistorialBoletas.Count();
        }

        public decimal ObtenerTotalAbono()
        {
            return Math.Round(Math.Abs(HistorialBoletas.Sum(b => b.MontoAbonoPrestamo)), 2);
        }

        public decimal ObtenerTotalToneladas()
        {
            return HistorialBoletas.Sum(t => t.PesoProducto);
        }

        public decimal ObtenerTotalIngreso()
        {
            var totalIngreso = 0m;

            foreach (var boleta in HistorialBoletas)
            {
                totalIngreso += boleta.PesoProducto * boleta.PrecioProductoCompra;
            }

            return Math.Round(totalIngreso, 2);
        }

        private decimal ObtenerTotalDeducciones()
        {
            var totalDeducciones = 0m;

            foreach (var boleta in HistorialBoletas)
            {
                totalDeducciones += Math.Abs(boleta.TasaSeguridad) + Math.Abs(boleta.MontoCombustible) + Math.Abs(boleta.PrecioDescarga) + Math.Abs(boleta.MontoAbonoPrestamo) + Math.Abs(boleta.OtrasDeducciones);
            }

            return Math.Round(totalDeducciones, 2);
        }

        private decimal ObtenerTotalAbonosPrestamo()
        {
            return Math.Round(Math.Abs(HistorialBoletas.Sum(d => d.MontoAbonoPrestamo)) , 2);
        }

        public decimal ObtenerTotalPagadoCliente()
        {
            return Math.Round(HistorialBoletas.Sum(p => p.PagoCliente), 2);
        }

        private decimal ObtenerTotalOtrosIngresos()
        {
            return Math.Round(HistorialBoletas.Sum(o => o.OtrosIngresos), 2);
        }

        public decimal ObtenerSaldoPendiente()
        {
            var totalIngreso = ObtenerTotalIngreso();
            var totalDeducciones = ObtenerTotalDeducciones();
            var totalPagoCliente = ObtenerTotalPagadoCliente();
            var totalOtrasDeducciones = ObtenerTotalAbonosPrestamo();

            return (ObtenerTotalIngreso() - ObtenerTotalDeducciones()) - (ObtenerTotalPagadoCliente() - ObtenerTotalOtrosIngresos());
        }
    }

    public class RptHistorialDatoBoletasDTO
    {
        public string NombreProveedor { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Estado { get; set; }
        public string NombrePlanta { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Motorista { get; set; }
        public string PlacaEquipo { get; set; }
        public string TipoProducto { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal TasaSeguridad { get; set; }
        public decimal MontoCombustible { get; set; }
        public decimal PrecioDescarga { get; set; }
        public decimal MontoAbonoPrestamo { get; set; }
        public decimal OtrasDeducciones { get; set; }
        public decimal OtrosIngresos { get; set; }
        public decimal PagoCliente { get; set; }
    }
}
