using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class PrestamoModel : ObservableObject
    {
        public int PrestamoId { get; set; }
        public int SucursalId { get; set; }
        public string CodigoPrestamo { get; set; }
        public int ProveedorId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string AutorizadoPor { get; set; }
        public decimal PorcentajeInteres { get; set; }
        public bool EsInteresMensual { get; set; }
        public decimal MontoPrestamo { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }

        public string NombreProveedor { get; set; }
        public string NombreSucursal { get; set; }
        public decimal TotalACobrar { get; set; }
        public decimal TotalAbono { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal CantidadAbono { get; set; }
        public int DiasTranscurridos { get; set; }
        public decimal Intereses { get; set; }
    }
}
