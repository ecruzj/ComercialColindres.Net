using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class AjusteBoletaDomainServices : IAjusteBoletaDomainServices
    {
        readonly ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public AjusteBoletaDomainServices(ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }

        public bool TryCreateBoletaAjuste(Boletas boleta, out string errorMessage)
        {
            if (boleta == null)
            {
                errorMessage = "BoletaId NO existe!";
                return false;
            }

            if (boleta.HasAjusteBoleta())
            {
                errorMessage = "La Boleta ya pertenece a un proceso de Ajustes";
                return false;
            }

            AjusteBoleta ajusteBoleta = new AjusteBoleta(boleta);

            boleta.AddAjusteBoleta(ajusteBoleta);

            errorMessage = string.Empty;
            return true;
        }

        public bool TryCreateBoletaAjusteDetalle(AjusteBoleta ajusteBoleta, AjusteTipo ajusteTipo, decimal monto, string observaciones, LineasCredito lineaCredito, string noDocumento, out string errorMessage)
        {
            if (ajusteBoleta == null) throw new ArgumentNullException(nameof(ajusteBoleta));
            if (ajusteTipo == null) throw new ArgumentNullException(nameof(ajusteTipo));

            if (!CanApplyCreditLine(ajusteTipo, lineaCredito, monto, out errorMessage))
            {
                return false;
            }

            AjusteBoletaDetalle ajusteDetail = new AjusteBoletaDetalle(ajusteBoleta, ajusteTipo, monto, lineaCredito, noDocumento, observaciones);

            if (ajusteDetail.GetValidationErrors().Any())
            {
                errorMessage = ajusteDetail.GetValidationErrors().FirstOrDefault();
                return false;
            }

            ajusteBoleta.AddAjusteBoletaDetalle(ajusteDetail);

            errorMessage = string.Empty;
            return true;
        }

        public bool TryDeleteAjusteBoleta(AjusteBoleta ajusteBoleta, out string errorMessage)
        {
            if (ajusteBoleta == null)
            {
                errorMessage = "AjusteBoletaId no existe!";
                return false;
            }

            if (ajusteBoleta.HasPayments())
            {
                errorMessage = "Existen ajustes con abonos";
                return false;
            }

            List<AjusteBoletaDetalle> ajusteDetalle = ajusteBoleta.AjusteBoletaDetalles.ToList();

            foreach (AjusteBoletaDetalle detalle in ajusteDetalle)
            {
                ajusteBoleta.RemoveAjusteDetalle(detalle);
            }

            Boletas boleta = ajusteBoleta.Boleta;
            boleta.RemoveAjusteBoleta();

            errorMessage = string.Empty;
            return true;
        }

        public bool TryDeleteAjusteDetalle(AjusteBoletaDetalle ajusteDetalle, out string errorMessage)
        {
            if (ajusteDetalle.HasPayment())
            {
                errorMessage = $"El Item del tipo {ajusteDetalle.AjusteTipo.Descripcion} del AjusteId {ajusteDetalle.AjusteBoletaDetalleId} ya tiene pago";
                return false;
            }

            AjusteBoleta ajusteBoleta = ajusteDetalle.AjusteBoleta;
            ajusteBoleta.RemoveAjusteDetalle(ajusteDetalle);

            errorMessage = string.Empty;
            return true;
        }

        public bool TryApplyAjusteBoletaPayment(AjusteBoletaPago ajusteBoletaPago, Boletas boletaPayment, out string errorMessage)
        {
            if (ajusteBoletaPago == null) throw new ArgumentNullException(nameof(ajusteBoletaPago));
            if (boletaPayment == null) throw new ArgumentNullException(nameof(boletaPayment));

            AjusteBoletaDetalle ajusteDetail = ajusteBoletaPago.AjusteBoletaDetalle;

            if (boletaPayment.Estado != Estados.ACTIVO)
            {
                errorMessage = "La Boleta de Pago debe estar Activo";
                return false;
            }

            if (ajusteBoletaPago.Monto <= 0)
            {
                errorMessage = "El monto a pagar debe ser mayor que 0";
                return false;
            }
            
            decimal paymentBoleta = boletaPayment.ObtenerTotalAPagar();
            decimal saldo = Math.Round((paymentBoleta - ajusteBoletaPago.Monto), 2);

            if (saldo < 0)
            {
                errorMessage = "El Abono del Ajuste supera el saldo de la boleta";
                return false;
            }

            decimal ajusteAbonos = Math.Round(ajusteBoletaPago.GetPaymentsByDetail() + ajusteBoletaPago.Monto, 2);

            if (ajusteAbonos > ajusteBoletaPago.AjusteBoletaDetalle.Monto)
            {
                errorMessage = $"El Abono supera el monto total del AjusteId #{ajusteBoletaPago.AjusteBoletaDetalle.AjusteBoletaDetalleId}";
                return false;
            }

            boletaPayment.AddAjusteBoletaPago(ajusteBoletaPago);
            ajusteDetail.AddAjusteBoletaPago(ajusteBoletaPago);
            ajusteBoletaPago.AjusteBoleta.UpdateStatus();

            errorMessage = string.Empty;
            return true;
        }

        public bool TryUpdateAjusteBoletaPago(AjusteBoletaPago ajusteBoletaPago, Boletas boletaPayment, decimal monto, out string errorMessage)
        {
            if (ajusteBoletaPago == null) throw new ArgumentNullException(nameof(ajusteBoletaPago));
            if (boletaPayment == null) throw new ArgumentNullException(nameof(boletaPayment));

            AjusteBoleta ajusteBoleta = ajusteBoletaPago.AjusteBoleta;

            if (boletaPayment.Estado != Estados.ACTIVO)
            {
                errorMessage = "La Boleta de Pago debe estar Activo";
                return false;
            }

            if (ajusteBoletaPago.Monto <= 0)
            {
                errorMessage = "El monto a pagar debe ser mayor que 0";
                return false;
            }

            decimal payment = boletaPayment.ObtenerTotalAPagar();
            decimal saldo = Math.Round(((payment - ajusteBoletaPago.Monto) + monto), 2);

            if (saldo < 0)
            {
                errorMessage = "El Abono del Ajuste supera el saldo de la boleta";
                return false;
            }

            decimal currentPayments = ajusteBoletaPago.GetPaymentsByDetail();
            AjusteBoletaPago currentPayment = ajusteBoleta.AjusteBoletaPagos.FirstOrDefault(p => p.AjusteBoletaPagoId == ajusteBoletaPago.AjusteBoletaPagoId);
            decimal ajusteAbonos = Math.Round((ajusteBoletaPago.GetPaymentsByDetail() - currentPayment.Monto) + ajusteBoletaPago.Monto, 2);

            if (ajusteAbonos > ajusteBoletaPago.AjusteBoletaDetalle.Monto)
            {
                errorMessage = $"El Abono supera el monto total del AjusteId #{ajusteBoletaPago.AjusteBoletaDetalle.AjusteBoletaDetalleId}";
                return false;
            }

            ajusteBoletaPago.UpdateAjusteBoletaPago(monto);
            ajusteBoleta.UpdateStatus();

            errorMessage = string.Empty;
            return true;
        }

        public bool TryRemoveAjusteBoletaPayment(Boletas boletaPayment, AjusteBoletaPago ajustePayment, out string errorMessage)
        {
            if (boletaPayment == null) throw new ArgumentNullException(nameof(boletaPayment));
            if (ajustePayment == null) throw new ArgumentNullException(nameof(ajustePayment));

            AjusteBoleta ajusteBoleta = ajustePayment.AjusteBoleta;
            AjusteBoletaDetalle ajusteBoletaDetail = ajustePayment.AjusteBoletaDetalle;

            if (ajusteBoleta == null)
            {
                errorMessage = "No se encontro Ajuste de Boleta";
                return false;
            }

            if (ajusteBoleta.Estado != Estados.ENPROCESO)
            {
                errorMessage = "El estado del Ajuste debe ser EnProceso";
                return false;
            }

            if (boletaPayment.Estado != Estados.ACTIVO)
            {
                errorMessage = "El estado de la Boleta del abono debe ser Activo";
                return false;
            }

            boletaPayment.RemoveAjusteBoletaPayment(ajustePayment);
            ajusteBoletaDetail.RemoveAjustePayment(ajustePayment);
            ajusteBoleta.UpdateStatus();

            errorMessage = string.Empty;
            return true;
        }

        public bool TryActiveAjusteBoleta(AjusteBoleta ajusteBoleta, out string errorMessage)
        {
            if (ajusteBoleta == null) throw new ArgumentNullException(nameof(ajusteBoleta));

            if (ajusteBoleta.Estado != Estados.NUEVO)
            {
                errorMessage = "El estado del ajuste de boleta debe ser Nuevo";
                return false;
            }

            if (!ajusteBoleta.HasAjusteDetail())
            {
                errorMessage = "El ajuste de boleta no tiene un detalle";
                return false;
            }

            ajusteBoleta.UpdateStatus();             

            errorMessage = string.Empty;
            return true;
        }

        public void TryCloseAjusteBoleta(List<AjusteBoletaPago> ajusteBoletaPagos)
        {
            if (ajusteBoletaPagos == null) throw new ArgumentNullException(nameof(ajusteBoletaPagos));

            foreach (AjusteBoletaPago payment in ajusteBoletaPagos)
            {
                payment.AjusteBoleta.UpdateStatus();
            }
        }

        public void TryRectiveAdjustment(Boletas boleta)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            foreach (AjusteBoletaPago ajusteBoletaPago in boleta.AjusteBoletaPagos)
            {
                ajusteBoletaPago.ReactiveAdjustment();
            }
        }

        private bool CanApplyCreditLine(AjusteTipo ajuseTipo, LineasCredito lineaCredito, decimal monto, out string errorMessage)
        {
            if (!ajuseTipo.UseCreditLine)
            {
                errorMessage = string.Empty;
                return true;
            }

            errorMessage = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, monto);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Se evalua que el detalle del ajuste este saldado pero que siga abierto el ajuste
        /// entonces, lo que se pretende es que solo a la boleta con la que se esta pagando 
        /// le aparezca el detalle
        /// </summary>
        /// <param name="ajustmentDetails">Detalle de ajustes pendientes</param>
        /// <param name="boletaPaymentId">boletaPaymentId</param>
        /// <returns></returns>
        public List<AjusteBoletaDetalle> GetAvailableAjustmentDetails(List<AjusteBoletaDetalle> ajustmentDetails, int boletaPaymentId)
        {
            List<AjusteBoletaDetalle> datosFiltered = new List<AjusteBoletaDetalle>();

            foreach (AjusteBoletaDetalle detail in ajustmentDetails)
            {
                if ((detail.GetSaldoPendiente() == 0) && !detail.IsBoletaInPayment(boletaPaymentId)) continue;

                datosFiltered.Add(detail);
            }

            return datosFiltered;
        }
    }
}