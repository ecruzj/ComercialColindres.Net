using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class BoletaOtrasDeducciones : Entity, IValidacionesEntidades
    {
        public BoletaOtrasDeducciones(int boletaId, decimal monto, string motivoDeduccion, string formaPago, int? lineaCreditoId, string noDocumento, bool esDeduccionManual = false)
        {
            this.BoletaId = boletaId;
            this.Monto = monto;
            this.MotivoDeduccion = motivoDeduccion;
            this.FormaDePago = formaPago;
            this.LineaCreditoId = lineaCreditoId;
            this.NoDocumento = noDocumento;
            this.EsDeduccionManual = esDeduccionManual;

            ApplyValuesForManualDeduction();
        }

        protected BoletaOtrasDeducciones() { }

        public int BoletaOtraDeduccionId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public string MotivoDeduccion { get; set; }
        public string FormaDePago { get; set; }
        public int? LineaCreditoId { get; set; }
        public string NoDocumento { get; set; }
        public bool EsDeduccionManual { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual LineasCredito LineasCredito { get; set; }
        
        private void ApplyValuesForManualDeduction()
        {
            if (EsDeduccionManual)
            {
                Monto = Math.Abs(Monto) * -1;
            }

            if (Monto < 0 && !EsDeduccionManual) return;

            LineaCreditoId = null;
            FormaDePago = "N/A";
            NoDocumento = "N/A";
        }
        
        public bool ValidateNewOtherDeduction(Boletas boleta, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(MotivoDeduccion))
            {
                errorMessage = "Debe específicar el Motivo del por qué está agregando Dinero";
                return false;
            }

            if (Monto == 0)
            {
                errorMessage = "La Deducción debe ser mayor o menor a 0";
                return false;
            }

            var totalPagoBoleta = Math.Round(boleta.ObtenerTotalAPagar() - Math.Abs(Monto), 2);

            if (totalPagoBoleta < 0)
            {
                errorMessage = "La deducción deja en Negativo la Boleta";
                return false;
            }

            if (Monto < 0 && !EsDeduccionManual)
            {
                if (string.IsNullOrWhiteSpace(FormaDePago))
                {
                    errorMessage = "Debe especificar la Forma de Pago";
                    return false;
                }
                
                if (LineaCreditoId == 0)
                {
                    errorMessage = "Debe seleccionar una Linea de Crédito";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(NoDocumento))
                {
                    errorMessage = "Debe ingresar el Documento Ref de la Transacción";
                    return false;
                }
            }        

            errorMessage = string.Empty;
            return true;
        }
        
        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (BoletaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BoletaId");
                listaErrores.Add(mensaje);
            }

            if (Monto == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Monto");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(MotivoDeduccion))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "MotivoDeduccion");
                listaErrores.Add(mensaje);
            }

            if (LineaCreditoId >= 0 && !EsDeduccionManual)
            {
                if (string.IsNullOrWhiteSpace(NoDocumento))
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NoDocumento");
                    listaErrores.Add(mensaje);
                }

                if (string.IsNullOrWhiteSpace(FormaDePago))
                {
                    var mensaje = string.Format(MensajesValidacion.Campo_Requerido, nameof(FormaDePago));
                    listaErrores.Add(mensaje);
                }
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }        
    }
}
