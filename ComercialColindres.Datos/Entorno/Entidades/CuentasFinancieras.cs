using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class CuentasFinancieras : Entity, IValidacionesEntidades
    {
        public CuentasFinancieras(int? bancoId, int cuentaFinancieraTipoId, string cuentaNo, string nombreAbonado, string cedula, bool esCuentaAdministrativa)
        {
            this.BancoId = bancoId;
            this.CuentaFinancieraTipoId = cuentaFinancieraTipoId;
            this.CuentaNo = cuentaNo;
            this.NombreAbonado = nombreAbonado;
            this.Cedula = cedula;
            this.EsCuentaAdministrativa = esCuentaAdministrativa;
            this.Estado = Estados.ACTIVO;
            this.LineasCreditoes = new List<LineasCredito>();
        }

        protected CuentasFinancieras() { }

        public int CuentaFinancieraId { get; set; }
        public int? BancoId { get; set; }
        public int CuentaFinancieraTipoId { get; set; }
        public string CuentaNo { get; set; }
        public string NombreAbonado { get; set; }
        public string Cedula { get; set; }
        public string Estado { get; set; }
        public bool EsCuentaAdministrativa { get; set; }
        public virtual Bancos Banco { get; set; }
        public virtual CuentasFinancieraTipos CuentasFinancieraTipos { get; set; }
        public virtual ICollection<LineasCredito> LineasCreditoes { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();
                        
            if (CuentaFinancieraTipoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuentaFinancieraTipoId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CuentaNo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuentaNo");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(NombreAbonado))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NombreAbonado");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Cedula))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Cedula");
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
