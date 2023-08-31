using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Sucursales
    {
        public Sucursales(string codigoSucursal, string nombre, string direccion, string telefonos)
        {
            CodigoSucursal = codigoSucursal;
            Nombre = nombre;
            Direccion = direccion;
            Telefonos = telefonos;
            Estado = Estados.ACTIVO;
            
            this.Facturas = new List<Factura>();
            this.Correlativos = new List<Correlativos>();
            this.Recibos = new List<Recibos>();
            this.UsuariosOpciones = new List<UsuariosOpciones>();
            this.Prestamos = new List<Prestamos>();
        }

        protected Sucursales()
        {
        }

        public int SucursalId { get; set; }
        public string CodigoSucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<UsuariosOpciones> UsuariosOpciones { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
        public virtual ICollection<Correlativos> Correlativos { get; set; }
        public virtual ICollection<Prestamos> Prestamos { get; set; }
        public virtual ICollection<Recibos> Recibos { get; set; }

        public void Actualizar(string codigoSucursal, string nombre, string direccion, string telefonos)
        {
            CodigoSucursal = codigoSucursal;
            Nombre = nombre;
            Direccion = direccion;
            Telefonos = telefonos;
        }
    }
}
