using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaHumedad : Entity, IValidacionesEntidades
    {
        public BoletaHumedad(string numeroEnvio, ClientePlantas facility, bool boletaIngresada, decimal humedadPromedio, decimal porcentajeTolerancia, DateTime fechaHumedad)
        {
            NumeroEnvio = numeroEnvio;
            ClientePlanta = facility;
            PlantaId = facility.PlantaId;
            BoletaIngresada = boletaIngresada;
            HumedadPromedio = humedadPromedio;
            PorcentajeTolerancia = porcentajeTolerancia;
            FechaHumedad = fechaHumedad;
            SetStatus();
        }

        protected BoletaHumedad() { }

        public int BoletaHumedadId { get; set; }
        public string NumeroEnvio { get; set; }
        public int PlantaId { get; set; }
        public bool BoletaIngresada { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
        public DateTime FechaHumedad { get; set; }
        public string Estado { get; set; }
        public virtual BoletaHumedadPago BoletaHumedadPago { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual BoletaHumedadAsignacion BoletaHumedadAsignacion { get; set; }


        public decimal GetWrongPercentageOfHumidity()
        {
            return Math.Round((HumedadPromedio - PorcentajeTolerancia), 2);
        }

        public decimal CalculateHumidityPricePayment()
        {
            decimal wrongHumidityPorcentage = GetWrongPercentageOfHumidity();

            if (wrongHumidityPorcentage <= 0) return 0;

            return GetHumidityPrice(wrongHumidityPorcentage, BoletaHumedadAsignacion.Boleta.PesoProducto, BoletaHumedadAsignacion.Boleta.PrecioProductoCompra);
        }

        private decimal GetHumidityPrice(decimal wrongHumidityPorcentage, decimal boletaTons, decimal purchasePrice)
        {
            decimal tons = Math.Round((wrongHumidityPorcentage * boletaTons), 2);

            int decimal_places = 0;
            string tonsString = Convert.ToString(tons);
            Regex regex = new Regex("(?<=[\\.])[0-9]+");

            if (regex.IsMatch(tonsString))
            {
                decimal_places = Convert.ToInt32(regex.Match(tonsString).Value);
            }

            if (decimal_places >= 50)
            {
                return Math.Round((Math.Ceiling(tons) * purchasePrice) / 100, 2);
            }

            return Math.Round((Math.Round(tons) * purchasePrice) / 100, 2);
        }

        public decimal CalculateHumidityPriceForAssignment(decimal boletaTons, decimal purchasePrice)
        {
            decimal wrongHumidityPorcentage = GetWrongPercentageOfHumidity();

            return GetHumidityPrice(wrongHumidityPorcentage, boletaTons, purchasePrice);
        }
                        
        private void SetStatus()
        {
            if (GetWrongPercentageOfHumidity() > 0)
            {
                Estado = Estados.ACTIVO;
            }
            else
            {
                Estado = Estados.CERRADO;
            }
        }

        public void CloseBoletaHumidity()
        {
            if (BoletaHumedadPago == null) return;

            if (BoletaHumedadPago.Boleta.IsBoletaClose())
            {
                Estado = Estados.CERRADO;
            }
        }

        public void RemoveAssignment()
        {
            if (BoletaHumedadAsignacion == null) return;

            BoletaHumedadAsignacion = null;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(NumeroEnvio))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "NumeroEnvio");
                listaErrores.Add(mensaje);
            }

            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                listaErrores.Add(mensaje);
            }

            if (PorcentajeTolerancia <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PorcentajeTolerancia");
                listaErrores.Add(mensaje);
            }

            if (HumedadPromedio <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "HumedadPromedio");
                listaErrores.Add(mensaje);
            }

            if (FechaHumedad == null || FechaHumedad == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaHumedad");
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
