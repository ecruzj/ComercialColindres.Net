using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class DescargadoresDomainServices : IDescargadoresDomainServices
    {
        readonly IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        public DescargadoresDomainServices(IAjusteBoletaDomainServices ajusteBoletaDomainServices)
        {
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
        }

        public void TryAsignarDescargaProductoPorAdelantado(Boletas boleta, List<DescargasPorAdelantado> descargasPorAdelantado)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            if (descargasPorAdelantado == null || !descargasPorAdelantado.Any()) return;

            DescargasPorAdelantado descargaAdelanto;

            if (boleta.IsShippingNumberRequired())
            {
                descargaAdelanto = descargasPorAdelantado.FirstOrDefault(d => d.NumeroEnvio == boleta.NumeroEnvio && d.Estado != Estados.ASIGNADO);
            }
            else
            {
                descargaAdelanto = descargasPorAdelantado.FirstOrDefault(d => d.CodigoBoleta == boleta.CodigoBoleta && d.Estado != Estados.ASIGNADO);
            }

            if (descargaAdelanto == null) return;

            descargaAdelanto.AssignBoleta(boleta);
            boleta.Descargador = AssigDescargaPorAdelanto(boleta, descargaAdelanto);       
        }
        
        public bool TryActualizarPrecioDescargaProducto(Boletas boleta, decimal precioDescarga, out string mensajeValidacion)
        {
            if (boleta == null) throw new ArgumentNullException("boleta");
            
            if (boleta.Estado != Estados.ACTIVO)
            {
                mensajeValidacion = "El Estado de la Boleta debe ser ACTIVO";
                return false;
            }

            var descargaProducto = boleta.Descargador;

            if (descargaProducto != null)
            {
                var pagoDescargasProducto = descargaProducto.PagoDescargador;

                if (pagoDescargasProducto != null)
                {
                    if (pagoDescargasProducto.Estado != Estados.ACTIVO)
                    {
                        mensajeValidacion = "El Estado de la Orden de Pago del Descargador debe estar ACTIVO";
                        return false;
                    }
                }

                if (precioDescarga <= 0)
                {
                    mensajeValidacion = "El Precio de la descarga debe ser mayor a 0";
                    return false;
                }

                descargaProducto.ActualizarPrecioDescargaProducto(precioDescarga);

                mensajeValidacion = string.Empty;
                return true;
            }

            mensajeValidacion = "No existe Descarga de Producto Asignada a la Boleta";
            return false;            
        }

        /// <summary>
        /// Metodo se encarga de remover descargas y descargas programadas por adelantado
        /// </summary>
        /// <param name="pagoDescarga">pagoDescarga</param>
        /// <param name="descargaPorAdelanto">descargaPorAdelanto</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns>true/false</returns>
        public bool TryRemoverDescargas(PagoDescargadores pagoDescarga, DescargasPorAdelantado descargaPorAdelanto, out string errorMessage)
        {
            if (pagoDescarga == null) throw new ArgumentNullException(nameof(pagoDescarga));
            if (descargaPorAdelanto == null) throw new ArgumentNullException(nameof(descargaPorAdelanto));

            Boletas boleta = descargaPorAdelanto.Boleta;

            if (boleta != null)
            {
                if (boleta.Estado != Estados.ACTIVO)
                {
                    errorMessage = $"El Estado de la boleta {boleta.CodigoBoleta} debe ser ACTIVO!";
                    return false;
                }

                boleta.RemoverDescargaProducto();
            }

            pagoDescarga.DescargasPorAdelantado.Remove(descargaPorAdelanto);

            errorMessage = string.Empty;
            return true;
        }

        public bool PuedeEliminarDescargaProducto(Descargadores descargaProducto, out string mensajeValidacion)
        {
            if (descargaProducto == null) throw new ArgumentNullException("descargaProducto");

            if (descargaProducto.PagoDescargador != null && descargaProducto.PagoDescargador.Estado != Estados.ACTIVO)
            {
                mensajeValidacion = "El Estado del Pago de la Descarga deber ser ACTIVO!";
                return false;
            }

            if (descargaProducto.Boleta != null && descargaProducto.Boleta.Estado != Estados.ACTIVO)
            {
                mensajeValidacion = $"El Estado de la boleta {descargaProducto.Boleta.CodigoBoleta} debe ser ACTIVO!";
                return false;
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryAssigneDescargaToPay(PagoDescargadores pagoDescarga, Boletas boleta, AjusteTipo ajusteTipo, DescargasPorAdelantado descargaPorAdelanto,  string numeroEnvio, string codigoBoleta, decimal montoPagoDescarga, out string errorMessage)
        {
            if (pagoDescarga == null) throw new ArgumentNullException(nameof(pagoDescarga));

            if (boleta != null)
            {
                return AssigneDescargaToPay(pagoDescarga, boleta, ajusteTipo, montoPagoDescarga, out errorMessage);
            }

            //Evaluar si la descarga ya pertenece como Descarga por Adelanto
            if (descargaPorAdelanto != null)
            {
                string descarga = pagoDescarga.Cuadrilla.ClientePlanta.IsShippingNumberRequired() ? numeroEnvio : codigoBoleta;
                errorMessage = $"La Descarga {descarga} ya pertenece al pago de Adeltanto {descargaPorAdelanto.PagoDescargador.CodigoPagoDescarga}";
                return false;
            }

            //En caso contrario, crearlo como descarga por adelanto
            DescargasPorAdelantado descargaAdelantoNew = new DescargasPorAdelantado(numeroEnvio, codigoBoleta, pagoDescarga, montoPagoDescarga, DateTime.Now, false);

            if (descargaAdelantoNew.GetValidationErrors().Any())
            {
                errorMessage = descargaAdelantoNew.GetValidationErrors().FirstOrDefault();
                return false;
            }

            pagoDescarga.AddDescargaPorAdelanto(descargaAdelantoNew);

            errorMessage = string.Empty;
            return true;
        }

        private bool AssigneDescargaToPay(PagoDescargadores pagoDescarga, Boletas boleta, AjusteTipo ajusteTipo, decimal montoPagoDescarga, out string errorMessage)
        {
            if (pagoDescarga == null) throw new ArgumentNullException(nameof(pagoDescarga));
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            if (boleta.Descargador == null)
            {
                ///se crea la descarga manualmente para que se pueda contabilizar que esta pagado, luego agregarlo al detalle de descargas a pagar
                boleta.Descargador = CreateDescargaProducto(boleta, pagoDescarga.CuadrillaId, montoPagoDescarga, DateTime.Now, false, true);
                pagoDescarga.AddDescargaToPayment(boleta.Descargador, montoPagoDescarga);

                //posteriormente se debe crear un ajuste
                return TryCreateAjusteByDescarga(boleta, ajusteTipo, montoPagoDescarga, "Se olvido cobrar la descarga", out errorMessage);
            }

            Descargadores descarga = boleta.Descargador;

            //Verificar que la descarga no se encuentre en el detalle de un pago de descargas
            //Puede cambiar de pago de descargas unicamente que éste tenga estado ACTIVO
            if (descarga.PagoDescargaId != null) //&& descarga.PagoDescargador.Estado != Estados.ACTIVO)
            {
                errorMessage = $"La descarga ya pertenece al pago {descarga.PagoDescargador.CodigoPagoDescarga}";
                return false;
            }

            pagoDescarga.AddDescargaToPayment(descarga, montoPagoDescarga);
            decimal saldoDescarga = montoPagoDescarga - descarga.PrecioDescarga;

            //Se debe crear ajuste de descarga
            if (saldoDescarga <= 0)
            {
                errorMessage = string.Empty;
                return true;
            }
            
            string observaciones = "No se cobro la descarga completa";

            return TryCreateAjusteByDescarga(boleta, ajusteTipo, saldoDescarga, observaciones, out errorMessage);
        }

        private bool TryCreateAjusteByDescarga(Boletas boleta, AjusteTipo tipoAjuste, decimal saldoDescarga, string observaciones, out string errorMessage)
        {
            if (!_ajusteBoletaDomainServices.TryCreateBoletaAjuste(boleta, out errorMessage))
            {
                return false;
            }

            if (!_ajusteBoletaDomainServices.TryCreateBoletaAjusteDetalle(boleta.GetAjusteBoleta(), tipoAjuste, saldoDescarga, observaciones, null, null,  out errorMessage))
            {
                return false;
            }

            if (!_ajusteBoletaDomainServices.TryActiveAjusteBoleta(boleta.GetAjusteBoleta(), out errorMessage))
            {
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        private Descargadores CreateDescargaProducto(Boletas boleta, int cuadrillaId, decimal precioDescarga, DateTime fechaDescarga, bool esDescargaAdelantado, bool esIngresoManual = false)
        {
            return new Descargadores(boleta.BoletaId, cuadrillaId, precioDescarga, fechaDescarga, esDescargaAdelantado, esIngresoManual);
        }

        private Descargadores AssigDescargaPorAdelanto(Boletas boleta, DescargasPorAdelantado descargaPorAdelantado)
        {
            Descargadores descarga = new Descargadores(boleta.BoletaId, descargaPorAdelantado.PagoDescargador.Cuadrilla.CuadrillaId, descargaPorAdelantado.PrecioDescarga, descargaPorAdelantado.FechaCreacion, true)
            {
                PagoDescargaId = descargaPorAdelantado.PagoDescargaId,
                PagoDescarga = descargaPorAdelantado.PrecioDescarga,
                Estado = descargaPorAdelantado.PagoDescargador.Estado
            };

            return descarga;
        }
    }
}
