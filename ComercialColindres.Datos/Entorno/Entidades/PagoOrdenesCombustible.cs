using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class PagoOrdenesCombustible : Entity, IValidacionesEntidades
    {
        public int PagoOrdenCombustibleId { get; set; }
        public int OrdenCombustibleId { get; set; }
        public string FormaDePago { get; set; }
        public int LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public int? BoletaId { get; set; }
        public decimal MontoAbono { get; set; }
        public string Observaciones { get; set; }
        public virtual LineasCredito LineaCredito { get; set; }
        public virtual Boletas Boleta { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
