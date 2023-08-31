using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Boletas
{
    public class BoletasDTO : BaseDTO
    {
        public int BoletaId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }        
        public string Motorista { get; set; }
        public int CategoriaProductoId { get; set; }
        public int EquipoId { get; set; }
        public int PlantaId { get; set; }
        public int DescargaId { get; set; }
        public decimal PrecioDescarga { get; set; }
        public decimal PesoEntrada { get; set; }
        public decimal PesoSalida { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal Bonus { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal PrecioProductoVenta { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaCreacionBoleta { get; set; }
        public string Estado { get; set; }
        public string BoletaPath { get; set; }
        public byte[] Imagen { get; set; }
        public string ModificadoPor { get; set; }

        public int ProveedorId { get; set; }
        public string PlacaEquipo { get; set; }
        public string NombreProveedor { get; set; }
        public string DescripcionTipoProducto { get; set; }
        public string NombrePlanta { get; set; }
        public int EquipoCategoriaId { get; set; }
        public string DescripcionTipoEquipo { get; set; }
        public bool AplicarDescarga { get; set; }
        public int CuadrillaId { get; set; }
        public string NombreDescargador { get; set; }
        public bool AplicarOrdenCombustible { get; set; }
        public string CodigoBoletaCombustible { get; set; }
        public decimal MontoOrdenCombustible { get; set; }
        public decimal TotalToneladas { get; set; }
        public bool ClientHasLoan { get; set; }
        public bool ClientHasOutStandingHumidity { get; set; }
        public bool ClientHasPendingAdjustments { get; set; }
        public bool HasFuelOrderPending { get; set; }
        public bool ApplieToForceClosing { get; set; }
        public bool ApplieToOpen { get; set; }
        public string FacturaNo { get; set; }
        public bool IsAssignedInvoice { get; set; }
        public bool HasBonus { get; set; }
        public bool IsBonusEnable { get; set; }
        public bool IsShippingNumberRequired { get; set; }
        public bool IsDescargaAdelanto { get; set; }
        public bool IsHorizontalImg { get; set; }

        public decimal TotalFacturaCompra
        {
            get { return _totalFacturaCompra; }
            set { _totalFacturaCompra = value; }
        }
        private decimal _totalFacturaCompra;
        
        public decimal TotalFacturaVenta
        {
            get { return _totalFacturaVenta; }
            set { _totalFacturaVenta = value; }
        }
        private decimal _totalFacturaVenta;

        public decimal TotalDeduccion
        {
            get { return _totalDeduccion; }
            set { _totalDeduccion = value; }
        }
        private decimal _totalDeduccion;
        
        public decimal TotalAPagar
        {
            get { return _totalAPagar; }
            set { _totalAPagar = value; }
        }
        private decimal _totalAPagar;
        
        public decimal TotalDeuda
        {
            get
            {
                return _totalDeduda;
            }
            set
            {
                _totalDeduda = value;
            }
        }
        private decimal _totalDeduda;
        
        public decimal TotalAPagarSinAbono
        {
            get { return _totalAPagarSinAbono; }
            set { _totalAPagarSinAbono = value; }
        }
        private decimal _totalAPagarSinAbono;
        
        public decimal TotalBoletaOtrasDeducciones
        {
            get { return _totalBoletaOtrasDeducciones; }
            set { _totalBoletaOtrasDeducciones = value; }
        }
        private decimal _totalBoletaOtrasDeducciones;
        
        public decimal TotalOtrosIngresosBoleta
        {
            get { return _totalOtrosIngresosBoleta; }
            set { _totalOtrosIngresosBoleta = value; }
        }
        private decimal _totalOtrosIngresosBoleta;

        private int _diasAntiguos;

        public int DiasAntiguos
        {
            get { return _diasAntiguos; }
            set { _diasAntiguos = value; }
        }

        public string GetBoletaName()
        {
            string factory = NombrePlanta.Substring(0);
            string boletaCode = IsShippingNumberRequired ? NumeroEnvio : CodigoBoleta;

            return $"{factory}-{boletaCode}.JPG";
        }
    }
}
