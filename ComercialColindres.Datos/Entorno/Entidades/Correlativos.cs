using ComercialColindres.Datos.Clases;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Correlativos : IValidacionesEntidades
    {
        public int CorrelativoId { get; set; }
        public string CodigoCorrelativo { get; set; }
        public int SucursalId { get; set; }
        public string Prefijo { get; set; }
        public string Letra { get; set; }
        public int Tamaño { get; set; }
        public int CorrelativoActual { get; set; }
        public bool ControlarPorPrefijo { get; set; }
        public bool ControlarPorRango { get; set; }
        public int CorrelativoInicialPermitido { get; set; }
        public int CorrelativoFinalPermitido { get; set; }
        public bool DefinidoPorUsuario { get; set; }
        public virtual Sucursales Sucursal { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var validacion = new List<string>();
            if (ControlarPorRango)
            {
                if (CorrelativoInicialPermitido > CorrelativoFinalPermitido)
                {
                    validacion.Add("El Correlativo Inicial debe de ser menor o igual al correlativo final permitido");
                }
            }
            return validacion;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
