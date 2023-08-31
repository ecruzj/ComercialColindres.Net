using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class BoletasDomainServices : IBoletasDomainServices
    {
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;
        IPagoPrestamosDomainServices _pagoPrestamosDomainServices;
        IDescargadoresDomainServices _descargadoresDomainServices;
        IOrdenCombustibleDomainServices _ordenCombustibleDomainServices;
        IBoletasDetalleDomainServices _boletasDetalleDomainServices;
        IBoletaCierreDomainServices _boletaCierreDomainServices;
        IAjusteBoletaDomainServices _ajusteBoletaDomainServices;
        IBoletaHumedadDomainServices _boletaHumedadDomainServices;

        public BoletasDomainServices(ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices,
                                     IPagoPrestamosDomainServices pagoPrestamosDomainServices,
                                     IDescargadoresDomainServices descargadoresDomainServices,
                                     IOrdenCombustibleDomainServices ordenCombustibleDomainServices,
                                     IBoletasDetalleDomainServices boletasDetalleDomainServices,
                                     IBoletaCierreDomainServices boletaCierreDomainServices,
                                     IAjusteBoletaDomainServices ajusteBoletaDomainServices,
                                     IBoletaHumedadDomainServices boletaHumedadDomainServices)
        {
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
            _pagoPrestamosDomainServices = pagoPrestamosDomainServices;
            _descargadoresDomainServices = descargadoresDomainServices;
            _ordenCombustibleDomainServices = ordenCombustibleDomainServices;
            _boletasDetalleDomainServices = boletasDetalleDomainServices;
            _boletaCierreDomainServices = boletaCierreDomainServices;
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
            _boletaHumedadDomainServices = boletaHumedadDomainServices;
        }

        public bool CanAssignBonusProduct(Boletas boleta, List<BonificacionProducto> bonificacionesProducto, out string errorMessage)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));
            if (bonificacionesProducto == null) throw new ArgumentNullException(nameof(bonificacionesProducto));

            errorMessage = string.Empty;

            if (boleta.Bonus == 0) return true;

            BonificacionProducto bonusProduct = bonificacionesProducto.FirstOrDefault(b => b.PlantaId == boleta.PlantaId && b.CategoriaProductoId == boleta.CategoriaProductoId);

            if (bonusProduct == null && boleta.BoletaId > 0)
            {
                errorMessage = $"El Producto {boleta.CategoriaProducto.Descripcion} no está configurado para Bonificaciones en {boleta.ClientePlanta.NombrePlanta}";
                return false;
            }

            if (bonusProduct == null)
            {
                errorMessage = "El Producto no está configurado para Bonificaciones";
                return false;
            }

            if (!bonusProduct.IsEnable && boleta.BoletaId > 0)
            {
                errorMessage = $"El Producto {boleta.CategoriaProducto.Descripcion} está desactivado para Bonificaciones en {boleta.ClientePlanta.NombrePlanta}";
                return false;
            }

            if (!bonusProduct.IsEnable)
            {
                errorMessage = "El Producto está desactivado para Bonificaciones";
                return false;
            }

            return true;
        }

        public string ActualizarPrecioVentaPorOrdenCompra(Boletas boleta, OrdenesCompraProducto ordenCompraProducto, bool aplicarPrecioManual, decimal precioVenta)
        {
            if (ordenCompraProducto == null)
            {
                return "No existe una Orden de Compra Disponible";
            }

            if (aplicarPrecioManual)
            {
                boleta.PrecioProductoVenta = precioVenta;
                return string.Empty;
            }

            var biomasa = ordenCompraProducto.OrdenesCompraProductoDetalles.FirstOrDefault(p => p.BiomasaId == boleta.CategoriaProducto.BiomasaId);

            if (biomasa == null)
            {
                return string.Format("No existe un Precio de Venta definido para el tipo de Biomasa {0}", boleta.CategoriaProducto.MaestroBiomasa.Descripcion);
            }

            boleta.PrecioProductoVenta = Math.Round(biomasa.PrecioLps, 2);

            return string.Empty;
        }

        public bool TryEliminarBoleta(Boletas boleta, out string mensajeValidacion)
        {
            if (TryEliminarDescargaProducto(boleta, out mensajeValidacion))
            {
                if (TryDeleteLoanPayments(boleta, out mensajeValidacion))
                {
                    if (TryDeleteFuelOrders(boleta, out mensajeValidacion))
                    {
                        if (TryDeleteBoletaOtherDeduccions(boleta, out mensajeValidacion))
                        {
                            if (TryEliminarBoletaOrdenCompraProducto(boleta, out mensajeValidacion))
                            {
                                if (TryDeleteBoletaHumidity(boleta, out mensajeValidacion))
                                {
                                    if (TryToDeleteAjustmentBoletaPayment(boleta, out mensajeValidacion))
                                    {
                                        TryEliminarBoletaDeFactura(boleta);

                                        boleta.DeleteImagen();
                                        mensajeValidacion = string.Empty;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool TryCierreForzado(Boletas boleta, List<Deducciones> deductions, out string mensajeValidacion)
        {
            if (boleta.Estado == Estados.CERRADO)
            {
                mensajeValidacion = "La Boleta Ya Está Cerrada!";
                return false;
            }

            if (boleta.ObtenerTotalAPagar() != 0)
            {
                mensajeValidacion = "Todavía existe un Saldo Pendiente de Pagar";
                return false;
            }
            
            boleta.Estado = Estados.CERRADO;

            //Registrar detalle del cierre de boleta
            mensajeValidacion = _boletasDetalleDomainServices.RegistrarBoletaDeducciones(boleta, deductions);

            if (!string.IsNullOrWhiteSpace(mensajeValidacion))
            {
                return false;
            }

            //verificar si aplica cerrar prestamos
            _pagoPrestamosDomainServices.CloseLoan(boleta.PagoPrestamos);

            //verificar si aplica cerrar Boletas con humedad
            _boletaHumedadDomainServices.CloseBoletaHumidity(boleta.BoletasHumedadPagos.ToList());

            //verificar si aplica cerrar ajustes
            _ajusteBoletaDomainServices.TryCloseAjusteBoleta(boleta.AjusteBoletaPagos.ToList());

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryOpenBoletaById(Boletas boleta, out string mensajeValidacion)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            if (boleta.Estado != Estados.CERRADO)
            {
                mensajeValidacion = "El estado de la Boleta debe ser CERRADO!";
                return false;
            }

            if (!_boletaCierreDomainServices.RemoveBoletaCierre(boleta, out string errorMessage))
            {
                mensajeValidacion = errorMessage;
                return false;
            }

            _boletasDetalleDomainServices.RemoveBoletaDetalle(boleta);
            _pagoPrestamosDomainServices.TryReactiveLoanByBoleta(boleta);
            _ajusteBoletaDomainServices.TryRectiveAdjustment(boleta);

            boleta.ActualizarEstadoBoleta();

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryToDeleteAjustmentBoletaPayment(Boletas boleta, out string mensajeValidacion)
        {
            List<AjusteBoletaPago> payments = boleta.AjusteBoletaPagos.ToList();

            foreach (AjusteBoletaPago payment in payments)
            {
                if (!_ajusteBoletaDomainServices.TryRemoveAjusteBoletaPayment(boleta, payment, out string errorMessage))
                {
                    mensajeValidacion = errorMessage;
                    return false;
                }
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        private bool TryDeleteBoletaHumidity(Boletas boleta, out string mensajeValidacion)
        {
            List<BoletaHumedadPago> boletasHumedadPago = boleta.BoletasHumedadPagos.ToList();
            List<BoletaHumedadAsignacion> boletaHumiditiesAssignment = boleta.BoletasHumedadAsignacion.ToList();

            foreach (BoletaHumedadPago humidityPay in boletasHumedadPago)
            {
                if (humidityPay.BoletaHumedad.Estado != Estados.ACTIVO)
                {
                    mensajeValidacion = "El estado de la Boleta con Humedad debe estar Activo";
                    return false;
                }

                boleta.RemoveHumidityPayment(humidityPay);
            }

            foreach (BoletaHumedadAsignacion humidityAssignment in boletaHumiditiesAssignment)
            {
                boleta.RemoveHumidityAssignment(humidityAssignment);
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        private bool TryEliminarDescargaProducto(Boletas boleta, out string mensajeValidacion)
        {
            if (boleta.Descargador != null)
            {
                if (!_descargadoresDomainServices.PuedeEliminarDescargaProducto(boleta.Descargador, out mensajeValidacion))
                {
                    return false;
                }

                boleta.RemoverDescargaProducto();
            }
            mensajeValidacion = string.Empty;
            return true;
        }

        private bool TryDeleteLoanPayments(Boletas boleta, out string mensajeValidacion)
        {
            var pagoPrestamos = boleta.PagoPrestamos.ToList();

            foreach (var itemPago in pagoPrestamos)
            {
                if (!_pagoPrestamosDomainServices.PuedeRemoverAbonoPrestamo(itemPago, out mensajeValidacion))
                {
                    return false;
                }

                boleta.RemoverAbonoPrestamo(itemPago);
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        private bool TryDeleteFuelOrders(Boletas boleta, out string mensajeValidacion)
        {
            var ordenesCombustible = boleta.OrdenesCombustible.ToList();

            foreach (var itemOrden in ordenesCombustible)
            {
                if (!_ordenCombustibleDomainServices.TryRemoveFuelOrderFromBoleta(itemOrden, out mensajeValidacion))
                {
                    return false;
                }
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        private void TryEliminarBoletaDeFactura(Boletas boleta)
        {
            var detalleFactura = boleta.FacturaDetalleBoletas.ToList();

            foreach (var itemBoletaFactura in detalleFactura)
            {
                boleta.RemoverFacturaDetalleBoleta(itemBoletaFactura);
            }
        }

        private bool TryDeleteBoletaOtherDeduccions(Boletas boleta, out string mensajeValidacion)
        {
            var boletOtrasDeducciones = boleta.BoletaOtrasDeducciones.ToList();

            foreach (var itemBoletaOtraDeduccion in boletOtrasDeducciones)
            {
                if (itemBoletaOtraDeduccion.LineasCredito == null) continue;

                if (!_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(itemBoletaOtraDeduccion.LineasCredito))
                {
                    mensajeValidacion = string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                                        itemBoletaOtraDeduccion.LineasCredito.CodigoLineaCredito, itemBoletaOtraDeduccion.LineasCredito.Estado);
                    return false;
                }

                boleta.RemoverOtraDeduccion(itemBoletaOtraDeduccion);

            }

            mensajeValidacion = string.Empty;
            return true;
        }

        private bool TryEliminarBoletaOrdenCompraProducto(Boletas boleta, out string mensajeValidacion)
        {
            var OrdenesCompraDetalleBoletas = boleta.OrdenesCompraDetalleBoletas.ToList();

            foreach (var itemOrdenCompraProducto in OrdenesCompraDetalleBoletas)
            {
                if (itemOrdenCompraProducto.OrdenesCompraProducto.Estado != "ACTIVO")
                {
                    mensajeValidacion = "El estado de la Orden de Compra debe ser ACTIVO";
                    return false;
                }

                boleta.RemoverBoletaOrdenCompraProducto(itemOrdenCompraProducto);
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        /// <summary>
        /// special tool for try to change or update boleta's information, thoug the boleta is closed
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="codigoBoleta">codigoBoleta</param>
        /// <param name="numeroEnvio">numeroEnvio</param>
        /// <param name="vendor">vendor</param>
        /// <param name="placaEquipo">placaEquipo</param>
        /// <param name="motorista">motorista</param>
        /// <param name="product">product</param>
        /// <param name="factory">factory</param>
        /// <param name="pesoEntrada">pesoEntrada</param>
        /// <param name="pesoSalida">pesoSalida</param>
        /// <param name="cantidadPenalizada">cantidadPenalizada</param>
        /// <param name="bonus">bonus</param>
        /// <param name="precioProductoCompra">precioProductoCompra</param>
        /// <param name="fechaSalida">fechaSalida</param>
        /// <param name="imagen">imagen</param>
        /// <param name="mensajeValidacion">mensajeValidacion</param>
        /// <returns>True/False</returns>
        public bool TryUpdateProperties(Boletas boleta, string codigoBoleta, string numeroEnvio, Proveedores vendor, string placaEquipo, string motorista,
                                        CategoriaProductos product, ClientePlantas factory, decimal pesoEntrada, decimal pesoSalida, decimal cantidadPenalizada,
                                        decimal bonus, decimal precioProductoCompra, DateTime fechaSalida, byte[] imagen, out string mensajeValidacion)
        {

            if (boleta == null) { throw new ArgumentNullException(nameof(boleta)); }
            if (vendor == null) { throw new ArgumentNullException(nameof(vendor)); }
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            if (product == null) { throw new ArgumentNullException(nameof(product)); }

            boleta.CodigoBoleta = codigoBoleta;
            boleta.NumeroEnvio = numeroEnvio;
            boleta.PlacaEquipo = placaEquipo;
            boleta.Motorista = motorista;
            boleta.FechaSalida = fechaSalida;

            if (boleta.BoletaImg != null && imagen != null)
            {
                boleta.BoletaImg.Imagen = imagen;
            }

            if (!CanBoletaChangeFactory(boleta, factory, out mensajeValidacion)) return false;
            if (!CanBoletaChangeVendor(boleta, vendor, out mensajeValidacion)) return false;
            if (!CanBoletaChangePriceTag(boleta, product, factory, precioProductoCompra, pesoEntrada, pesoSalida, cantidadPenalizada, bonus, out mensajeValidacion)) return false;
            TryRemoveWronInvoiceBoleta(boleta, factory);

            mensajeValidacion = string.Empty;
            return true;
        }

        /// <summary>
        /// Validate that you can change the vendor
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="vendor">New Vendor</param>
        /// <param name="errorMessage">Message Validation</param>
        /// <returns>True/False</returns>
        private bool CanBoletaChangeVendor(Boletas boleta, Proveedores vendor, out string errorMessage)
        {
            ///validar diferentes escenarios en los que el proveedor puede cambiar.
            if (boleta.Proveedor == vendor)
            {
                errorMessage = string.Empty;
                return true;
            }

            //y si tiene prestamos, ordenes de diesel, ajustes y/o humedades, no se puede cambiar de proveedor
            if (boleta.Estado != Estados.ACTIVO &&
                (boleta.HasAssignedCreditToLoan() ||
                 boleta.HasAssignedFuelOrders() ||
                 boleta.HasAjusteBoleta() ||
                 boleta.HasAssignedHumidity() ||
                 boleta.HasAssignedAdjustment()))
            {
                errorMessage = "No puede cambiar de proveedor ya que tiene asignado miscelaneos propias del mismo";
                return false;
            }

            if (!TryDeleteLoanPayments(boleta, out errorMessage)) { return false; }
            if (!TryDeleteFuelOrders(boleta, out errorMessage)) { return false; }
            if (!TryDeleteBoletaOtherDeduccions(boleta, out errorMessage)) { return false; }
            if (!TryDeleteBoletaHumidity(boleta, out errorMessage)) { return false; }
            if (!TryToDeleteAjustmentBoletaPayment(boleta, out errorMessage)) { return false; }

            boleta.Proveedor = vendor;

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Vlidate that you can change the factory
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="factory">factory</param>
        /// <param name="errorMessage">Message Validation</param>
        /// <returns>True/False</returns>
        private bool CanBoletaChangeFactory(Boletas boleta, ClientePlantas factory, out string errorMessage)
        {
            ///Validate if is possible chance factory
            ///
            if (boleta.ClientePlanta == factory)
            {
                errorMessage = string.Empty;
                return true;
            }

            ///planta que no requiere #Envío y no tiene bonificación
            if (!factory.IsShippingNumberRequired())
            {
                boleta.NumeroEnvio = string.Empty;
            }

            BonificacionProducto productBonus = factory.BonificacionesProducto.Where(b => b.PlantaId == factory.PlantaId &&
                                                                                     b.CategoriaProductoId == boleta.CategoriaProductoId).FirstOrDefault();
            if (boleta.Estado != Estados.ACTIVO)
            {
                if (productBonus == null || !productBonus.IsEnable)
                {
                    if (boleta.Bonus > 0)
                    {
                        errorMessage = "No puede cambiar la planta actualmente tiene bonificación y la planta actualizar no bonifica";
                        return false;
                    }
                    else
                    {
                        boleta.Bonus = 0;
                    }
                }
            }

            boleta.ClientePlanta = factory;

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validate that you can chance the properties that have to do with price
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="product">product</param>
        /// <param name="precioCompra">precioCompra</param>
        /// <param name="pesoEntrada">pesoEntrada</param>
        /// <param name="pesoSalida">pesoSalida</param>
        /// <param name="penalidad">penalidad</param>
        /// <param name="bonus">bonus</param>
        /// <returns>True/False</returns>
        private bool CanBoletaChangePriceTag(Boletas boleta, CategoriaProductos product, ClientePlantas factory, decimal precioCompra, decimal pesoEntrada, decimal pesoSalida, decimal penalidad, decimal bonus, out string errorMessage)
        {
            //Cuando boleta esa cerrada no se puede modificar sus valores de peso, precio y producto            
            if (boleta.Estado == Estados.CERRADO)
            {
                if (boleta.CategoriaProducto != product)
                {
                    errorMessage = "No puede cambiar de producto, Boleta cerrada";
                    return false;
                }

                decimal productTon = boleta.GetProductWeight();

                if (boleta.PesoProducto != productTon)
                {
                    errorMessage = "No puede cambiar las toneladas, Boleta cerrada";
                    return false;
                }

                if (boleta.PrecioProductoCompra != precioCompra)
                {
                    errorMessage = "No puede cambiar el precio, Boleta cerrada";
                    return false;
                }
            }

            boleta.CategoriaProducto = product;
            boleta.PesoEntrada = pesoEntrada;
            boleta.PesoSalida = pesoSalida;
            boleta.CantidadPenalizada = penalidad;
            boleta.Bonus = bonus;

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validate that boleta isn't assigned to invoice with wron information
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="factory">factory</param>
        private void TryRemoveWronInvoiceBoleta(Boletas boleta, ClientePlantas factory)
        {
            ///Evaluar que la boleta no esta facturada con datos incorrectos
            if (!boleta.IsAssignedToInvoice()) return;

            FacturaDetalleBoletas invoiceDetail = boleta.FacturaDetalleBoletas.FirstOrDefault();
            decimal boletaWeight = GetBoletaWeight(boleta, factory);

            if (invoiceDetail != null)
            {
                if (((invoiceDetail.Planta != factory)
                    || (invoiceDetail.FechaIngreso != boleta.FechaSalida)
                    || (invoiceDetail.PesoProducto != boletaWeight)))
                {
                    boleta.RemoverFacturaDetalleBoleta(invoiceDetail);
                }
            }
        }

        /// <summary>
        /// Gets the boleta weight depending its scenario
        /// </summary>
        /// <param name="boleta">boleta</param>
        /// <param name="factory">factory</param>
        /// <returns>Boleta Weight</returns>
        private decimal GetBoletaWeight(Boletas boleta, ClientePlantas factory)
        {
            bool bonusEnable = factory.BonificacionesProducto.Any(p => p.CategoriaProducto == boleta.CategoriaProducto && p.IsEnable);
            return bonusEnable ? boleta.GetProductWeight() : boleta.GetWeightWithoutBonus(); ;
        }
    }
}
