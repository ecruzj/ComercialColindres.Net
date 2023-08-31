using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class CuentasBancarias : IValidacionesEntidades
    {
        public CuentasBancarias(int proveedorId, int banco, string abonado, string cedulaNo, string cuentaNo)
        {
            ProveedorId = proveedorId;
            BancoId = banco;
            NombreAbonado = abonado;
            CedulaNo = cedulaNo ?? string.Empty;
            CuentaNo = cuentaNo;
            Estado = Estados.ACTIVO;
        }
        protected CuentasBancarias() { }
        public int CuentaId { get; set; }
        public int ProveedorId { get; set; }
        public int BancoId { get; set; }
        public string CuentaNo { get; set; }
        public string NombreAbonado { get; set; }
        public string CedulaNo { get; set; }
        public string Estado { get; set; }
        public virtual Bancos Banco { get; set; }
        public virtual Proveedores Proveedor { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (ProveedorId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ClienteId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CuentaNo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuentaNo");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(NombreAbonado))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Nombre Abonado");
                listaErrores.Add(mensaje);
            }

            if (BancoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Banco");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();

            return listaErrores;
        }

        public void ActualizarCuentaBancaria(string nombreAbonado, string cedulaNo, int bancoId, string cuentaNo, string estado)
        {
            NombreAbonado = nombreAbonado;
            CedulaNo = !string.IsNullOrWhiteSpace(cedulaNo) ? cedulaNo : string.Empty;
            BancoId = bancoId;
            CuentaNo = cuentaNo;
            Estado = estado;
        }
    }
}
