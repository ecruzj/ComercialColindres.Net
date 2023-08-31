using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Bancos
    {
        public Bancos()
        {
            this.CuentasBancarias = new List<CuentasBancarias>();
            this.CuentasFinancieras = new List<CuentasFinancieras>();
            this.PagoPrestamos = new List<PagoPrestamos>();
            this.FacturaPagos = new List<FacturaPago>();
            this.FuelOrderManualPayments = new List<FuelOrderManualPayment>();
        }

        public int BancoId { get; set; }
        public string Descripcion { get; set; }
        
        public virtual ICollection<CuentasBancarias> CuentasBancarias { get; set; }
        public virtual ICollection<CuentasFinancieras> CuentasFinancieras { get; set; }
        public virtual ICollection<PagoPrestamos> PagoPrestamos { get; set; }
        public virtual ICollection<FacturaPago> FacturaPagos { get; set; }
        public virtual ICollection<FuelOrderManualPayment> FuelOrderManualPayments { get; set; }
    }
}
