using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre
{
    public class BoletaCierreDomainServices : IBoletaCierreDomainServices
    {
        ILineasCreditoDeduccionesDomainServices _lineaCreditoDomainServices;

        public BoletaCierreDomainServices(ILineasCreditoDeduccionesDomainServices lineaCreditoDomainServices)
        {
            _lineaCreditoDomainServices = lineaCreditoDomainServices;
        }

        public decimal BuildAveragePaymentMasive(decimal totalPaymentPending, decimal creditLineDeduction, Boletas boleta)
        {
            decimal averagePaymentPorcent = Math.Round((creditLineDeduction / totalPaymentPending), 2);
            decimal averagePayment = averagePaymentPorcent * boleta.ObtenerTotalAPagar();

            return Math.Round(averagePayment, 2);
        }

        public bool RemoveBoletaCierre(Boletas boleta, out string errorMessage)
        {
            if (boleta.Estado != Estados.CERRADO)
            {
                errorMessage = "La Boleta debe estar CERRADO!";
                return false;
            }

            if (!boleta.BoletaCierres.Any())
            {
                errorMessage = string.Empty;
                return true;
            }

            List<BoletaCierres> boletaCierres = boleta.BoletaCierres.ToList();

            foreach (BoletaCierres boletaCierre in boletaCierres)
            {
                if (!_lineaCreditoDomainServices.AplicaRemoverDeduccionCredito(boletaCierre.LineasCredito))
                {
                    errorMessage = $"La linea de crédito {boletaCierre.LineasCredito.CodigoLineaCredito} está cerrada, contactarse con el administrador";
                    return false;
                }

                boleta.RemoveBoletaCierre(boletaCierre);
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}