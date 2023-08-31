using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Clases
{
    public class Sucursal
    {
        public static int SucursalId { get; set; }
        public static string CodigoSucursal { get; set; }
        public static string Nombre { get; set; }
        public static string Direccion { get; set; }
        public static string Telefonos { get; set; }
        public static string Estado { get; set; }

        public static void Asignar(int sucursalId, string codigoSucursal, string nombre, string direccion, string telefonos, string estado)
        {
            SucursalId = sucursalId;
            CodigoSucursal = codigoSucursal;
            Nombre = nombre;
            Direccion = direccion;
            Telefonos = telefonos;
            Estado = estado;
        }
    }
}
