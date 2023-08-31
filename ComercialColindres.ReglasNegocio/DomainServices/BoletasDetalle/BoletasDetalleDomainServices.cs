using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.ReglasNegocio.Recursos;
using ComercialColindres.Datos.Recursos;
using System;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class BoletasDetalleDomainServices : IBoletasDetalleDomainServices
    {
        public string ActualizarBoletaDetalle(Boletas boleta, List<Deducciones> deducciones, string tipoDeduccion, string noCumento, 
                                              string observaciones)
        {
            var datosDescarga = boleta.Descargador;
            var deduccion = deducciones.FirstOrDefault(d => d.Descripcion == tipoDeduccion);

            if (deduccion == null)
            {
                return "No existe DeduccionId para " + tipoDeduccion;
            }

            var boletaDetalle = new BoletaDetalles(boleta.BoletaId, deduccion.DeduccionId, datosDescarga.PrecioDescarga, noCumento, observaciones);
            var mensajeError = TryValidationBoletaDetalle(boletaDetalle, true);
            
            if (!string.IsNullOrWhiteSpace(mensajeError))
            {
                return mensajeError;
            }

            boletaDetalle.ActualizarBoletaDetalle(boletaDetalle.DeduccionId,
                          boletaDetalle.MontoDeduccion, boletaDetalle.NoDocumento, boletaDetalle.Observaciones);

            return string.Empty;
        }
        
        public string AgregarBoletaDetalle(Boletas boleta, List<Deducciones> deducciones, decimal montoDeduccion, string tipoDeduccion, string noCumento, 
                                           string observaciones)
        {
            var deduccion = deducciones.FirstOrDefault(d => d.Descripcion == tipoDeduccion);

            if (deduccion == null)
            {
                return "No existe DeduccionId para " + tipoDeduccion;
            }

            var boletaDetalle = new BoletaDetalles(boleta.BoletaId, deduccion.DeduccionId, montoDeduccion, noCumento, observaciones);
            var mensajeError = TryValidationBoletaDetalle(boletaDetalle, false);

            if (!string.IsNullOrWhiteSpace(mensajeError))
            {
                return mensajeError;
            }

            boleta.BoletaDetalles.Add(boletaDetalle);

            return string.Empty;            
        }

        public List<BoletaDetalles> ObtenerBoletaDeducciones(Boletas boleta, List<Deducciones> listaDeducciones)
        {
            var detallesDeducciones = new List<BoletaDetalles>();

            var tasaSeguridad = new BoletaDetalles
            {
                DescripcionDeduccion = "Tasa Seguridad",
                FechaTransaccion = boleta.FechaTransaccion,
                MontoDeduccion = boleta.ObtenerTasaSeguridad() * -1,
                ModificadoPor = boleta.ModificadoPor,
                NoDocumento = "N/A",
                DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.TasaSeguridad).DeduccionId
            };

            detallesDeducciones.Add(tasaSeguridad);

            foreach (BoletaHumedadPago humidityPay in boleta.BoletasHumedadPagos)
            {
                BoletaDetalles deduction = new BoletaDetalles
                {
                    DescripcionDeduccion = KeyDeducciones.BoletaHumedad,
                    MontoDeduccion = humidityPay.BoletaHumedad.CalculateHumidityPricePayment() * -1,
                    FechaTransaccion = humidityPay.FechaTransaccion,
                    ModificadoPor = humidityPay.ModificadoPor,
                    Observaciones = $"#Envío {humidityPay.BoletaHumedad.NumeroEnvio} Humedad {humidityPay.BoletaHumedad.HumedadPromedio}/{humidityPay.BoletaHumedad.PorcentajeTolerancia}%",
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.BoletaHumedad).DeduccionId
                };

                detallesDeducciones.Add(deduction);
            }

            if (boleta.Descargador != null && !boleta.Descargador.EsIngresoManual)
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Descarga Producto",
                    MontoDeduccion = boleta.Descargador.PrecioDescarga * -1,
                    FechaTransaccion = boleta.Descargador.FechaDescarga,
                    ModificadoPor = boleta.Descargador.ModificadoPor,
                    Observaciones = string.Format("Descargado por {0}", boleta.Descargador.GetCuadrillaName()),
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.Descargadores).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (var ordenCombustible in boleta.OrdenesCombustible)
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Orden Combustible",
                    NoDocumento = string.Format("#Fact {0}", ordenCombustible.CodigoFactura),
                    MontoDeduccion = ordenCombustible.Monto * -1,
                    FechaTransaccion = ordenCombustible.FechaCreacion,
                    ModificadoPor = ordenCombustible.ModificadoPor,
                    Observaciones = ordenCombustible.Observaciones,
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.OrdenCombustible).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (var pagoPrestamo in boleta.PagoPrestamos)
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Abono Por Boleta",
                    NoDocumento = string.Format("Prestamo {0}", pagoPrestamo.Prestamo.CodigoPrestamo),
                    MontoDeduccion = pagoPrestamo.MontoAbono * -1,
                    ModificadoPor = pagoPrestamo.ModificadoPor,
                    FechaTransaccion = pagoPrestamo.FechaTransaccion,
                    Observaciones = string.Format("Abono al Prestamo {0}", pagoPrestamo.Prestamo.CodigoPrestamo),
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.AbonoPorBoleta).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (var boletaOtraDeduccion in boleta.BoletaOtrasDeducciones.Where(m => m.Monto < 0 && !m.EsDeduccionManual))
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Boleta Otras Deducciones",
                    NoDocumento = boletaOtraDeduccion.NoDocumento,
                    MontoDeduccion = boletaOtraDeduccion.Monto,
                    Observaciones = boletaOtraDeduccion.MotivoDeduccion,
                    ModificadoPor = boletaOtraDeduccion.ModificadoPor,
                    FechaTransaccion = boletaOtraDeduccion.FechaTransaccion,
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.BoletaOtrasDeducciones).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (var boletaOtraDeduccion in boleta.BoletaOtrasDeducciones.Where(m => m.EsDeduccionManual))
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Boleta Deducción Manual",
                    NoDocumento = "N/A",
                    MontoDeduccion = Math.Abs(boletaOtraDeduccion.Monto) * -1,
                    Observaciones = boletaOtraDeduccion.MotivoDeduccion,
                    ModificadoPor = boletaOtraDeduccion.ModificadoPor,
                    FechaTransaccion = boletaOtraDeduccion.FechaTransaccion,
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.BoletaDeduccionManual).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (var boletaOtraDeduccion in boleta.BoletaOtrasDeducciones.Where(m => m.Monto > 0 && !m.EsDeduccionManual))
            {
                var deduccion = new BoletaDetalles
                {
                    DescripcionDeduccion = "Boleta Otros Ingresos",
                    NoDocumento = boletaOtraDeduccion.NoDocumento,
                    MontoDeduccion = boletaOtraDeduccion.Monto,
                    Observaciones = boletaOtraDeduccion.MotivoDeduccion,
                    ModificadoPor = boletaOtraDeduccion.ModificadoPor,
                    FechaTransaccion = boletaOtraDeduccion.FechaTransaccion,
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.BoletaOtrosIngresos).DeduccionId
                };

                detallesDeducciones.Add(deduccion);
            }

            foreach (AjusteBoletaPago payment in boleta.AjusteBoletaPagos)
            {
                BoletaDetalles deduction = new BoletaDetalles
                {
                    DescripcionDeduccion = $"Ajuste | {payment.AjusteBoletaDetalle.AjusteTipo.Descripcion}",
                    NoDocumento = "N/A",
                    MontoDeduccion = payment.Monto * -1,
                    Observaciones = $"{payment.AjusteBoletaDetalle.Observaciones} en Boleta #{payment.AjusteBoletaDetalle.AjusteBoleta.Boleta.CodigoBoleta}",
                    ModificadoPor = payment.ModificadoPor,
                    FechaTransaccion = payment.FechaTransaccion,
                    DeduccionId = listaDeducciones.FirstOrDefault(d => d.Descripcion == KeyDeducciones.AjusteBoletas).DeduccionId
                };

                detallesDeducciones.Add(deduction);
            }

            return detallesDeducciones;
        }

        public string RegistrarBoletaDeducciones(Boletas boleta, List<Deducciones> listaDeducciones)
        {
            var listaBoletaDetalle = ObtenerBoletaDeducciones(boleta, listaDeducciones);

            foreach (var deduccion in listaBoletaDetalle)
            {
                var boletaDetalle = new BoletaDetalles(deduccion.CodigoBoleta, deduccion.DeduccionId, deduccion.MontoDeduccion, deduccion.NoDocumento, deduccion.Observaciones);
                var mensajeError = TryValidationBoletaDetalle(boletaDetalle, false);

                if (!string.IsNullOrWhiteSpace(mensajeError))
                {
                    return mensajeError;
                }

                boleta.AddBoletaDetalle(boletaDetalle);
            }
                                   
            return string.Empty;
        }

        public string ActualizarEstadoBoleta(Boletas boleta)
        {
            var montoJustificado = boleta.BoletaCierres.Sum(c => c.Monto);
            var totalAPagar = boleta.ObtenerTotalAPagar();

            if (montoJustificado > totalAPagar)
            {
                return string.Format("La Justificación de Pago L. {0} supera el total a Pagar de la Boleta L. {1}", montoJustificado, totalAPagar);              
            }

            boleta.ActualizarEstadoBoleta();

            return string.Empty;
        }

        public void RemoveBoletaDetalle(Boletas boleta)
        {
            if (boleta.Estado != Estados.CERRADO) return;
            if (!boleta.BoletaDetalles.Any()) return;

            List<BoletaDetalles> boletaDetalles = boleta.BoletaDetalles.ToList();

            foreach (BoletaDetalles boletaDetalle in boletaDetalles)
            {
                boleta.RemoveBoletaDetalle(boletaDetalle);
            }
        }

        private string TryValidationBoletaDetalle(BoletaDetalles boletaDetalle, bool esActualizacion)
        {
            if (esActualizacion)
            {
                if (boletaDetalle.CodigoBoleta == 0)
                {
                    return "Requerido CodigoBoleta";
                }
            }

            if (boletaDetalle.DeduccionId == 0)
            {
                return "Requerido DeduccionId";
            }

            if (boletaDetalle.MontoDeduccion == 0)
            {
                return "Requerido MonoDeduccion";
            }

            return string.Empty;
        }
    }    
}
