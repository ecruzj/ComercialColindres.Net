using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using System.Collections.Generic;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class CuentasFinancieraTipos : Entity, IValidacionesEntidades
    {
        public CuentasFinancieraTipos(string descripcion, bool requiereBanco)
        {
            this.Descripcion = descripcion;
            this.RequiereBanco = requiereBanco;
            this.CuentasFinancieras = new List<CuentasFinancieras>();
        }

        protected CuentasFinancieraTipos() { }

        public int CuentaFinancieraTipoId { get; set; }
        public string Descripcion { get; set; }
        public bool RequiereBanco { get; set; }
        public virtual ICollection<CuentasFinancieras> CuentasFinancieras { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Descripcion");
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
