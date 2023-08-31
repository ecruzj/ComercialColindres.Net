using System;
using System.Collections.Generic;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class PagoPrestamos : Entity, IValidacionesEntidades
    {
        public PagoPrestamos(int prestamoId, string formaDePago, int? bancoId, int? boletaId,  decimal montoAbono, string noDocumento, string observaciones)
        {
            PrestamoId = prestamoId;
            FormaDePago = formaDePago ?? string.Empty;
            BancoId = bancoId;
            BoletaId = boletaId;
            MontoAbono = montoAbono;
            NoDocumento = noDocumento ?? string.Empty;
            Observaciones = observaciones ?? string.Empty;
        }

        protected PagoPrestamos() { }

        public int PagoPrestamoId { get; set; }
        public int PrestamoId { get; set; }
        public string FormaDePago { get; set; }
        public int? BancoId { get; set; }
        public string NoDocumento { get; set; }
        public int? BoletaId { get; set; }
        public decimal MontoAbono { get; set; }        
        public string Observaciones { get; set; }
        public virtual Prestamos Prestamo { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual Bancos Banco { get; set; }
        
        public void ActualizarAbonoPrestamo(string formaDePago, int? bancoId, string noDocumento, int? boletaId, decimal montoAbono, string observaciones)
        {
            FormaDePago = formaDePago ?? string.Empty;
            BancoId = bancoId;
            BoletaId = boletaId;
            MontoAbono = montoAbono;
            NoDocumento = noDocumento ?? string.Empty;
            Observaciones = observaciones ?? string.Empty;
        }

        public object GetNoDocument()
        {
            if (Boleta != null) return Boleta.CodigoBoleta;

            return NoDocumento;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (PrestamoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrestamoId");
                listaErrores.Add(mensaje);
            }

            if (MontoAbono <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MontoAbono");
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
