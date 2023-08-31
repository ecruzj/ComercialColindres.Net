using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class DescargasPorAdelantado : Entity, IValidacionesEntidades
    {

        public DescargasPorAdelantado(string numeroEnvio, string codigoBoleta, PagoDescargadores pagoDescarga, decimal precioDescarga, DateTime fechaCreacion, bool hasPayment = true)
        {
            this.NumeroEnvio = numeroEnvio ?? string.Empty;
            this.CodigoBoleta = codigoBoleta ?? string.Empty;
            this.ClientePlanta = pagoDescarga.Cuadrilla.ClientePlanta;
            this.PlantaId = pagoDescarga.Cuadrilla.PlantaId;
            this.PagoDescargador = pagoDescarga;
            this.PagoDescargaId = pagoDescarga.PagoDescargaId;
            this.CreadoPor = pagoDescarga.CreadoPor;
            this.FechaCreacion = fechaCreacion;
            this.PrecioDescarga = precioDescarga;
            this.Estado = Estados.PENDIENTE;
            this.HasPayment = hasPayment;
        }

        protected DescargasPorAdelantado() { }

        public int DescargaPorAdelantadoId { get; set; }
        public int PlantaId { get; set; }
        public int? BoletaId { get; set; }
        public string NumeroEnvio { get; set; }
        public string CodigoBoleta { get; set; }
        public int PagoDescargaId { get; set; }
        public string CreadoPor { get; set; }
        public decimal PrecioDescarga { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }

        public bool HasPayment { get; set; }

        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual PagoDescargadores PagoDescargador { get; set; }

        public void ActualizarPrecioDescargaPorAdelantado(decimal precioDescarga)
        {
            this.PrecioDescarga = precioDescarga;
        }

        public void AssignBoleta(Boletas boleta)
        {
            Boleta = boleta;
            BoletaId = boleta.BoletaId;
            Estado = Estados.ASIGNADO;            
        }

        public string GetCodigoBoleta()
        {
            if (!string.IsNullOrWhiteSpace(CodigoBoleta))
            {
                return CodigoBoleta;
            }

            if (Boleta != null)
            {
                return Boleta.CodigoBoleta;
            }

            return "S/A";
        }

        public string GetNumeroBoleta()
        {
            if (!string.IsNullOrWhiteSpace(NumeroEnvio))
            {
                return NumeroEnvio;
            }

            bool hasShippingNumber = HasShippingNumber();

            if (hasShippingNumber)
            {
                if (Boleta != null)
                {
                    return Boleta.NumeroEnvio;
                }

                return "S/A";
            }

            return "N/A";
        }

        public bool HasShippingNumber()
        {
            return ClientePlanta.IsShippingNumberRequired();
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (ClientePlanta.IsShippingNumberRequired())
            {
                if (string.IsNullOrWhiteSpace(NumeroEnvio))
                {
                    string mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NumeroEnvio");
                    listaErrores.Add(mensaje);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(CodigoBoleta))
                {
                    string mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoBoleta");
                    listaErrores.Add(mensaje);
                }
            }

            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                listaErrores.Add(mensaje);
            }

            if (HasPayment && PagoDescargaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PagoDescargaId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CreadoPor))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CreadoPor");
                listaErrores.Add(mensaje);
            }

            if (PrecioDescarga <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioDescarga");
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
