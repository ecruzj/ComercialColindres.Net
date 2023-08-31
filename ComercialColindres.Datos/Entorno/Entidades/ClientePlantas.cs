using ComercialColindres.Datos.Clases;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.Datos.Entorno.DataCore.Setting;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class ClientePlantas : IValidacionesEntidades
    {
        public ClientePlantas(string rtn, string nombrePlanta, string telefonos, string direccion)
        {
            RTN = rtn ?? string.Empty;
            NombrePlanta = nombrePlanta;
            Telefonos = telefonos;
            Direccion = direccion;
            RequiresPurchaseOrder = false;
            HasSubPlanta = false;

            this.Boletas = new List<Boletas>();
            this.Cuadrillas = new List<Cuadrillas>();
            this.DescargasPorAdelantado = new List<DescargasPorAdelantado>();
            this.Facturas = new List<Factura>();
            this.OrdenesCompraProductos = new List<OrdenesCompraProducto>();
            this.PrecioDescargas = new List<PrecioDescargas>();
            this.PrecioProductos = new List<PrecioProductos>();
            this.BoletasHumedad = new List<BoletaHumedad>();
            this.SubPlantas = new List<SubPlanta>();
            this.FacturaDetalleBoletas = new List<FacturaDetalleBoletas>();
            this.BonificacionesProducto = new List<BonificacionProducto>();
        }

        protected ClientePlantas() { }

        public int PlantaId { get; set; }
        public string RTN { get; set; }
        public string NombrePlanta { get; set; }
        public string Telefonos { get; set; }
        public string Direccion { get; set; }
        public bool RequiresPurchaseOrder { get; set; }
        public bool RequiresWeekNo { get; set; }
        public bool RequiresProForm { get; set; }
        public bool IsExempt { get; set; }
        public bool HasSubPlanta { get; set; }
        public bool HasExonerationNo { get; set; }
        public bool ImgHorizontalFormat { get; set; }

        public virtual ICollection<Boletas> Boletas { get; set; }
        public virtual ICollection<Cuadrillas> Cuadrillas { get; set; }
        public virtual ICollection<DescargasPorAdelantado> DescargasPorAdelantado { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
        public virtual ICollection<FacturaDetalleBoletas> FacturaDetalleBoletas { get; set; }
        public virtual ICollection<OrdenesCompraProducto> OrdenesCompraProductos { get; set; }
        public virtual ICollection<PrecioDescargas> PrecioDescargas { get; set; }
        public virtual ICollection<PrecioProductos> PrecioProductos { get; set; }
        public virtual ICollection<BoletaHumedad> BoletasHumedad { get; set; }
        public virtual ICollection<SubPlanta> SubPlantas { get; set; }
        public virtual ICollection<BonificacionProducto> BonificacionesProducto { get; set; }

        public void ActualizarPlanta(string rtn, string nombrePlanta, string telefonos, string direccion)
        {
            RTN = !string.IsNullOrWhiteSpace(rtn) ? rtn : string.Empty;
            NombrePlanta = nombrePlanta;
            Telefonos = telefonos;
            Direccion = direccion;
        }

        public bool IsShippingNumberRequired()
        {
            string result = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("NumeroEnvio", PlantaId.ToString(), string.Empty);

            return !string.IsNullOrWhiteSpace(result) ? result == "1" ? true : false : false;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombrePlanta))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NombrePlanta");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Telefonos))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Telefonos");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Direccion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Direccion");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
