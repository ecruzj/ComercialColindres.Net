using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;
using System.Linq;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class LineasCreditoDeduccionesDomainServices : ILineasCreditoDeduccionesDomainServices
    {
        public string EliminarDeduccionesPorBoleta(LineasCredito lineaCredito, int boletaCierreId)
        {
            if (lineaCredito == null)
            {
                return "LineaCredito NO Existe";
            }
            
            if (!lineaCredito.EsLineaCreditoActual)
            {
                return "LineaCredito NO es Actual";
            }

            return string.Empty;
        }
        
        public string ActualizarDeduccionPorBoleta(LineasCreditoDeducciones lineaCreditoDeduccion, BoletaCierres boletaCierre)
        {
            if (lineaCreditoDeduccion == null)
            {
                return "No existe el registro de CreditoDeduccion que haga referencia al Cierre de la Boleta";
            }
            

            if (lineaCreditoDeduccion.LineaCreditoId != boletaCierre.LineaCreditoId)
            {
                return string.Format("Las Lines de Crédito no Concuerdan {0}/{1}", lineaCreditoDeduccion.LineasCredito.CodigoLineaCredito, 
                                                                                   boletaCierre.LineasCredito.CodigoLineaCredito);
            }

            var creditoDisponible = lineaCreditoDeduccion.LineasCredito.ObtenerCreditoDisponible();

            if ((creditoDisponible - boletaCierre.Monto) < 0)
            {
                return string.Format("La deducción excede el Crédito disponible Lps {0} de la Linea de Crédito {1}",
                       creditoDisponible, lineaCreditoDeduccion.LineasCredito.CodigoLineaCredito + " - " + lineaCreditoDeduccion.LineasCredito.CuentasFinanciera.CuentaNo); 
            }

            lineaCreditoDeduccion.Monto = boletaCierre.Monto;
            lineaCreditoDeduccion.NoDocumento = boletaCierre.NoDocumento;

            return string.Empty;
        }

        public IEnumerable<string> CrearDeduccionPorBoleta(BoletaCierres nuevaBoletaCierre, LineasCredito lineaCredito)
        {
            var listaErrores = new List<string>();
            string mensajeValidacion;
            if (nuevaBoletaCierre == null)
            {
                mensajeValidacion = "El Cierre de la Boleta no contine información";
                listaErrores.Add(mensajeValidacion);

                return listaErrores;
            }

            var deduccionLineaCredito = new LineasCreditoDeducciones(nuevaBoletaCierre.LineaCreditoId, "Pago de Boleta", nuevaBoletaCierre.Monto, true,
                                                                     nuevaBoletaCierre.NoDocumento, DateTime.Now, true);
            
            if (deduccionLineaCredito.GetValidationErrors().Any())
            {
                return deduccionLineaCredito.GetValidationErrors();
            }

            var creditoDisponible = lineaCredito.ObtenerCreditoDisponible();

            if ((creditoDisponible - deduccionLineaCredito.Monto) < 0)
            {
                mensajeValidacion = string.Format("La deducción excede el Crédito disponible Lps {0} de la Linea de Crédito {1}", 
                                                  creditoDisponible, lineaCredito.CodigoLineaCredito + " - " + lineaCredito.CuentasFinanciera.CuentaNo);
                listaErrores.Add(mensajeValidacion);

                return listaErrores;
            }
            
            return listaErrores;
        }

        public bool AplicaRemoverDeduccionCredito(LineasCredito lineaCredito)
        {
            if (lineaCredito == null)
            {
                return false;
            }

            if (lineaCredito.Estado != Estados.ACTIVO)
            {
                return false;
            }

            return true;
        }

        public string AplicarDeduccionCredito(LineasCredito lineaCredito, decimal montoDeduccion)
        {
            if (lineaCredito == null)
            {
                return "LineaCreditoId NO Existe!";
            }

            if (lineaCredito.Estado != Estados.ACTIVO)
            {
                return string.Format("El estado de la Linea de Crédito {0} debe ser ACTIVO", lineaCredito.CodigoLineaCredito);
            }

            var creditoDisponible = lineaCredito.ObtenerCreditoDisponible();

            if ((creditoDisponible - montoDeduccion) < 0)
            {
                return string.Format("La deducción excede el Crédito disponible Lps {0} de la Linea de Crédito {1}",
                       creditoDisponible, lineaCredito.CodigoLineaCredito + " - " + lineaCredito.CuentasFinanciera.CuentaNo);
            }

            return string.Empty;
        }
    }
}
