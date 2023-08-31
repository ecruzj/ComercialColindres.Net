using System;
using System.Collections.Generic;
using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Descargadores : Entity, IValidacionesEntidades
    {
        public Descargadores(int boletaId, int cuadrillaId, decimal precioDescarga, DateTime fechaDescarga, bool esDescargaPorAdelantado, bool esIngresoManual = false)
        {
            BoletaId = boletaId;
            CuadrillaId = cuadrillaId;
            PrecioDescarga = Math.Abs(precioDescarga);
            FechaDescarga = fechaDescarga;
            this.EsDescargaPorAdelanto = esDescargaPorAdelantado;
            EsIngresoManual = esIngresoManual;
            PagoDescarga = esIngresoManual ? precioDescarga : 0;

            AsignarEstado(esDescargaPorAdelantado);            
        }
        
        protected Descargadores() { }

        public int DescargadaId { get; set; }
        public int BoletaId { get; set; }
        public int CuadrillaId { get; set; }
        public decimal PrecioDescarga { get; set; }
        public decimal PagoDescarga { get; set; }
        public int? PagoDescargaId { get; set; }
        public bool EsDescargaPorAdelanto { get; set; }
        public bool EsIngresoManual { get; set; }
        public string Estado { get; set; }
        public DateTime FechaDescarga { get; set; }

        public virtual Boletas Boleta { get; set; }
        public virtual Cuadrillas Cuadrilla { get; set; }
        public virtual PagoDescargadores PagoDescargador { get; set; }

        private void AsignarEstado(bool esDescargaPorAdelantado)
        {
            if (esDescargaPorAdelantado)
            {
                this.Estado = Estados.ENPROCESO;
            }
            else
            {
                this.Estado = Estados.ACTIVO;
            }
        }

        public void CerrarDescarga()
        {
            Estado = Estados.CERRADO;
        }

        public void EliminarDescarga()
        {
            Boleta = null;
        }

        public void ActualizarDescarga(int boletaId, int cuadrillaId, decimal precioDescarga, DateTime fechaDescarga)
        {
            BoletaId = boletaId;
            CuadrillaId = cuadrillaId;
            PrecioDescarga = Math.Abs(precioDescarga);
            FechaDescarga = fechaDescarga == null ? DateTime.Now : fechaDescarga;
        }

        public void ActualizarPrecioDescargaProducto(decimal precioDescarga)
        {
            this.PrecioDescarga = Math.Abs(precioDescarga);
        }

        public void AsignarDescargaAPago(PagoDescargadores pagoDescargas)
        {
            if (pagoDescargas == null) return;

            PagoDescargaId = pagoDescargas.PagoDescargaId;
            Estado = Estados.ENPROCESO;
        }

        public string GetCuadrillaName()
        {
            if (Cuadrilla == null) return string.Empty;

            return Cuadrilla.NombreEncargado;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (BoletaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "BoletaId");
                listaErrores.Add(mensaje);
            }

            if (CuadrillaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CuadrillaId");
                listaErrores.Add(mensaje);
            }

            if (PrecioDescarga <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioDescarga");
                listaErrores.Add(mensaje);
            }

            if (FechaDescarga == DateTime.MinValue)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "FechaDescarga");
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
