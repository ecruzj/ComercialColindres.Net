using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class Boletas : Entity, IValidacionesEntidades
    {
        public Boletas(string codigoBoleta, string numeroEnvio, Proveedores vendor, string placaEquipo, string motorista, int categoriaProducto,
                       int plantaId, decimal pesoEntrada, decimal pesoSalida, decimal cantidadPenalizada, decimal bonus, decimal precioCompra, decimal precioVenta, 
                       DateTime fechaSalida, byte[] imagen, string boletaPath)
        {
            CodigoBoleta = codigoBoleta;
            NumeroEnvio = numeroEnvio ?? string.Empty;
            this.Proveedor = vendor;
            ProveedorId = vendor.ProveedorId;
            PlacaEquipo = placaEquipo;
            Motorista = motorista;
            CategoriaProductoId = categoriaProducto;
            PlantaId = plantaId;
            PesoEntrada = pesoEntrada;
            PesoSalida = pesoSalida;
            Bonus = bonus;            
            CantidadPenalizada = cantidadPenalizada;
            PrecioProductoCompra = precioCompra;
            PrecioProductoVenta = precioVenta;
            FechaSalida = fechaSalida;

            PesoProducto = GetProductWeight();
            Estado = Estados.ACTIVO;
            FechaCreacionBoleta = DateTime.Now;
            BoletaPath = boletaPath;
            AssignImage(imagen);

            this.BoletaCierres = new List<BoletaCierres>();
            this.BoletaDeduccionesManual = new List<BoletaDeduccionManual>();
            this.BoletaDetalles = new List<BoletaDetalles>();
            this.BoletaOtrasDeducciones = new List<BoletaOtrasDeducciones>();
            this.FacturaDetalleBoletas = new List<FacturaDetalleBoletas>();
            this.OrdenesCombustible = new List<OrdenesCombustible>();
            this.OrdenesCompraDetalleBoletas = new List<OrdenesCompraDetalleBoleta>();
            this.PagoOrdenesCombustible = new List<PagoOrdenesCombustible>();
            this.PagoPrestamos = new List<PagoPrestamos>();
            this.BoletasHumedadPagos = new List<BoletaHumedadPago>();
            this.BoletasHumedadAsignacion = new List<BoletaHumedadAsignacion>();
            this.AjusteBoletaPagos = new List<AjusteBoletaPago>();
            this.AjusteBoletas = new List<AjusteBoleta>();
            this.DescargasPorAdelantado = new List<DescargasPorAdelantado>();
        }

        protected Boletas() { }

        public int BoletaId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public int ProveedorId { get; set; }
        public string PlacaEquipo { get; set; }
        public string Motorista { get; set; }
        public int CategoriaProductoId { get; set; }
        public int PlantaId { get; set; }
        public decimal PesoEntrada { get; set; }
        public decimal PesoSalida { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal Bonus { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal PrecioProductoVenta { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaCreacionBoleta { get; set; }
        public string Estado { get; set; }
        public string BoletaPath { get; set; }

        public virtual ICollection<BoletaCierres> BoletaCierres { get; set; }
        public virtual ICollection<BoletaDeduccionManual> BoletaDeduccionesManual { get; set; }
        public virtual ICollection<BoletaDetalles> BoletaDetalles { get; set; }
        public virtual ICollection<BoletaOtrasDeducciones> BoletaOtrasDeducciones { get; set; }
        public virtual CategoriaProductos CategoriaProducto { get; set; }
        public virtual Proveedores Proveedor { get; set; }
        public virtual BoletaImg BoletaImg { get; set; }
        public virtual ClientePlantas ClientePlanta { get; set; }
        public virtual Descargadores Descargador { get; set; }
        public virtual ICollection<DescargasPorAdelantado> DescargasPorAdelantado { get; set; }
        public virtual ICollection<BoletaHumedadAsignacion> BoletasHumedadAsignacion { get; set; }
        public virtual ICollection<FacturaDetalleBoletas> FacturaDetalleBoletas { get; set; }
        public virtual ICollection<OrdenesCombustible> OrdenesCombustible { get; set; }
        public virtual ICollection<OrdenesCompraDetalleBoleta> OrdenesCompraDetalleBoletas { get; set; }
        public virtual ICollection<PagoOrdenesCombustible> PagoOrdenesCombustible { get; set; }
        public virtual ICollection<PagoPrestamos> PagoPrestamos { get; set; }
        public virtual ICollection<BoletaHumedadPago> BoletasHumedadPagos { get; set; }
        public virtual ICollection<AjusteBoleta> AjusteBoletas { get; set; }
        public virtual ICollection<AjusteBoletaPago> AjusteBoletaPagos { get; set; }

        [NotMapped]
        public Factura Invoice 
        {
            get
            {
                FacturaDetalleBoletas invoiceDetail = FacturaDetalleBoletas.FirstOrDefault();
                return invoiceDetail?.Factura;
            }
            set { }
        }

        public bool HasAssignedCreditToLoan()
        {
            if (PagoPrestamos == null) return false;

            return PagoPrestamos.Any();
        }

        private void AssignImage(byte[] imagen)
        {
            BoletaImg = new BoletaImg(this, imagen);
        }

        public void RemoveAjusteBoleta()
        {
            AjusteBoletas = null;
        }

        public bool HasAjusteBoleta()
        {
            return (AjusteBoletas != null && AjusteBoletas.Any()) ? true : false;
        }

        public bool BoletaHasBonus()
        {
            return Bonus > 0;
        }
        
        /// <summary>
        /// Get weight with all scenaries
        /// </summary>
        /// <returns>Boleta Weight</returns>
        public decimal GetProductWeight()
        {
            return ((PesoEntrada - PesoSalida) - CantidadPenalizada) + Bonus;
        }

        /// <summary>
        /// Get weight that it is in tha physical boleta
        /// without bonus
        /// without penalty
        /// </summary>
        /// <returns>Boleta Weight</returns>
        public decimal GetWeightWithoutBonus()
        {
            return ((PesoProducto - Bonus) + CantidadPenalizada);
        }

        internal bool IsBoletaClose()
        {
            return Estado == Estados.CERRADO;
        }

        public void DeleteImagen()
        {
            if (BoletaImg == null) return;

            BoletaImg.Boleta = null;
        }

        public bool IsAssignedToInvoice()
        {
            return Invoice != null;
        }

        public string GetInoviceNo()
        {
            return Invoice == null ? Estados.PENDIENTE : Invoice.NumeroFactura;
        }
        
        public void RemoveHumidityAssignment(BoletaHumedadAsignacion boletaHumedadAsignacion)
        {
            if (BoletasHumedadAsignacion == null) return;

            BoletasHumedadAsignacion.Remove(boletaHumedadAsignacion);
        }

        public AjusteBoleta GetAjusteBoleta()
        {
            return AjusteBoletas.FirstOrDefault();
        }

        public void AddBoletaHumidityAssignment(BoletaHumedadAsignacion boletaHumidityAssignment)
        {
            if (BoletasHumedadAsignacion == null) return;

            BoletasHumedadAsignacion.Add(boletaHumidityAssignment);
        }

        public void RemoveHumidityPayment(BoletaHumedadPago humidityPayment)
        {
            if (BoletasHumedadPagos == null) return;

            BoletasHumedadPagos.Remove(humidityPayment);
        }

        public void RemoveAjusteBoletaPayment(AjusteBoletaPago ajustePayment)
        {
            if (AjusteBoletaPagos == null) return;

            AjusteBoletaPagos.Remove(ajustePayment);
        }

        public bool AppliesToForceClosing()
        {
            if (Estado == Estados.CERRADO) return false;

            return ObtenerTotalAPagar() == 0;
        }

        public bool ClientHasLoan()
        {
            if (Estado != Estados.ACTIVO) return false;

            return Proveedor.GetPendingLoan() > 0;
        }

        public bool ClientHasOutStandingHumidiy()
        {
            if (Estado != Estados.ACTIVO) return false;

            return Proveedor.HasOutStandingHumidity(this);
        }

        public bool ClientHasPendingAdjustments()
        {
            if (Estado != Estados.ACTIVO) return false;

            return Proveedor.HasPendingAdjustments(this);
        }

        public bool HasFuelOrderPending()
        {
            if (Estado != Estados.ACTIVO) return false;

            return Proveedor.HasFuelOrderPending(this);
        }

        public bool HasAssignedFuelOrders()
        {
            if (OrdenesCombustible == null) return false;

            return OrdenesCombustible.Any();
        }

        public void AddAjusteBoleta(AjusteBoleta ajusteBoleta)
        {
            if (AjusteBoletas == null) return;

            AjusteBoletas.Add(ajusteBoleta);
        }

        public void RemoverBoletaOrdenCompraProducto(OrdenesCompraDetalleBoleta boletaOrdenCompraProducto)
        {
            if (OrdenesCompraDetalleBoletas != null)
            {
                OrdenesCompraDetalleBoletas.Remove(boletaOrdenCompraProducto);
            }
        }

        public void RemoverOtraDeduccion(BoletaOtrasDeducciones boletaOtraDeduccion)
        {
            if (boletaOtraDeduccion != null)
            {
                BoletaOtrasDeducciones.Remove(boletaOtraDeduccion);
            }
        }

        public bool IsHorizontalImg()
        {
            return ClientePlanta.ImgHorizontalFormat == true;
        }

        public bool IsDescargaAdelanto()
        {
            if (Descargador == null) return false;

            return Descargador.EsDescargaPorAdelanto;
        }

        public void RemoverFacturaDetalleBoleta(FacturaDetalleBoletas boletaFactura)
        {
            if (FacturaDetalleBoletas != null)
            {
                boletaFactura.Boleta = null;
                boletaFactura.BoletaId = null;
            }
        }

        public void AgregarOrdenCombustible(OrdenesCombustible ordenCombustible)
        {
            if (OrdenesCombustible == null) return;

            OrdenesCombustible.Add(ordenCombustible);
        }

        public void RemoverAbonoPrestamo(PagoPrestamos abonoPrestamo)
        {
            if (PagoPrestamos == null) return;

            PagoPrestamos.Remove(abonoPrestamo);
        }

        public void RemoveBoletaDetalle(BoletaDetalles boletaDetalle)
        {
            if (BoletaDetalles == null) return;

            BoletaDetalles.Remove(boletaDetalle);
        }

        public bool IsClose()
        {
            return Estado == Estados.CERRADO;
        }
        public void AgregarAbonoPrestamo(PagoPrestamos abonoPrestamo)
        {
            if (PagoPrestamos == null) return;

            PagoPrestamos.Add(abonoPrestamo);
        }

        public void AddBoletaHumidityPay(BoletaHumedadPago boletaHumedadPago)
        {
            if (BoletasHumedadPagos == null) return;

            BoletasHumedadPagos.Add(boletaHumedadPago);
        }

        public int GetFacturaIdAssigned()
        {
            if (FacturaDetalleBoletas == null || !FacturaDetalleBoletas.Any()) return 0;

            return FacturaDetalleBoletas.FirstOrDefault().Factura.FacturaId;
        }

        public void ActualizarBoleta(int plantaId, string codigoBoleta, string numeroEnvio, int proveedorId, string placaEquipo, string motorista, 
                                     int categoriaProducto, decimal pesoEntrada, decimal pesoSalida, decimal cantidadPenalizada, decimal bonus, decimal precioProducto, 
                                     DateTime fechaSalida, byte[] image)
        {
            PlantaId = plantaId;
            CodigoBoleta = codigoBoleta;
            NumeroEnvio = numeroEnvio ?? string.Empty;
            ProveedorId = proveedorId;
            PlacaEquipo = placaEquipo;
            Motorista = motorista;
            CategoriaProductoId = categoriaProducto;
            PesoEntrada = pesoEntrada;
            PesoSalida = pesoSalida;
            CantidadPenalizada = cantidadPenalizada;
            Bonus = bonus;
            PrecioProductoCompra = precioProducto;
            FechaSalida = fechaSalida;
            BoletaImg.Imagen = image;

            PesoProducto = GetProductWeight();
        }

        public void AgregarOtraDeduccion(BoletaOtrasDeducciones otraDeduccion)
        {
            if (BoletaOtrasDeducciones == null) return;

            BoletaOtrasDeducciones.Add(otraDeduccion);
        }

        public void AddBoletaDetalle(BoletaDetalles boletaDetalle)
        {
            if (BoletaDetalles == null) return;

            BoletaDetalles.Add(boletaDetalle);
        }

        public void ActualizarEstadoBoleta()
        {
            if (BoletaCierres != null && BoletaCierres.Any())
            {
                var totalPagar = Math.Round(ObtenerTotalAPagar(), 2);
                var totalAbono = Math.Round(BoletaCierres.Sum(c => c.Monto), 2);

                if (totalPagar == totalAbono)
                {
                    Estado = Estados.CERRADO;                 
                }
                else
                {
                    Estado = Estados.ENPROCESO;
                }
            }
            else
            {
                Estado = Estados.ACTIVO;
            }
        }
                
        public decimal ObtenerTotalFacturaCompra()
        {
            return Math.Round(PrecioProductoCompra * PesoProducto, 2);
        }

        public decimal ObtenerTotalFacturaVenta()
        {
            return Math.Round(PrecioProductoVenta * PesoProducto, 2);
        }

        public decimal ObtenerTotalDeduccion()
        {
            if (Estado == Estados.CERRADO)
            {
                return Math.Round(BoletaDetalles.Where(d => d.MontoDeduccion < 0).Sum(m => m.MontoDeduccion) * -1, 2);
            }

            var deducciones = 0m;
            decimal descarga = Descargador != null
                               ? !Descargador.EsIngresoManual
                                  ? Descargador.PrecioDescarga 
                                  : 0 
                               : 0;
                                
            deducciones += ObtenerTasaSeguridad();
            deducciones += GetHumidityPayments();
            deducciones += descarga;
            deducciones += OrdenesCombustible.Sum(p => p.Monto);
            deducciones += PagoPrestamos.Sum(p => p.MontoAbono);
            deducciones += ObtenerTotalOtrasDeduccionesNegativas();
            deducciones += GetAjusteBoletaPayments();
            
            return Math.Round(deducciones, 2);
        }
        
        public decimal GetAjusteBoletaPayments()
        {
            if (AjusteBoletaPagos == null) return 0;

            return Math.Round(AjusteBoletaPagos.Sum(p => p.Monto), 2);
        }

        public bool HasAssignedHumidity()
        {
            if (BoletasHumedadPagos == null) return false;

            return BoletasHumedadPagos.Any();
        }

        public bool HasAssignedAdjustment()
        {
            if (AjusteBoletaPagos == null) return false;

            return AjusteBoletaPagos.Any();
        }

        private decimal GetHumidityPayments()
        {
            if (BoletasHumedadPagos == null) return 0;

            decimal humidityPayments = 0m;
            
            foreach (BoletaHumedadPago humidityPay in BoletasHumedadPagos)
            {
                humidityPayments += humidityPay.BoletaHumedad.CalculateHumidityPricePayment();
            }

            return Math.Round(humidityPayments, 2);
        }

        public void RemoveBoletaCierre(BoletaCierres boletaCierre)
        {
            if (BoletaCierres == null) return;

            BoletaCierres.Remove(boletaCierre);
        }

        public decimal ObtenerTotalOtrasDeduccionesNegativas()
        {
            return Math.Abs(BoletaOtrasDeducciones.Where(m => m.Monto < 0).Sum(d => d.Monto));
        }

        public decimal ObtenerTotalOtrasIngresosBoleta()
        {
            return Math.Round(BoletaOtrasDeducciones.Where(m => m.Monto > 0).Sum(d => d.Monto), 2);
        }

        public decimal ObtenerTotalAPagar()
        {
            var pago = Math.Round(PrecioProductoCompra * PesoProducto, 2) + ObtenerTotalOtrasIngresosBoleta();

            pago -= ObtenerTotalDeduccion();

            return Math.Round(pago, 2);
        }

        private string ValidarEliminarBoleta()
        {
            var validarEstado = ValidarEstadoEliminarBoleta();

            if (!string.IsNullOrWhiteSpace(validarEstado))
            {
                return validarEstado;
            }

            var deduccionesBoleta = ValidarDeduccionesEliminarBoleta();

            if (!string.IsNullOrWhiteSpace(deduccionesBoleta))
            {
                return deduccionesBoleta;
            }

            return string.Empty;
        }

        private string ValidarDeduccionesEliminarBoleta()
        {
            if (Descargador != null)
            {
                return string.Format("La Boleta tiene una descarga asignada a la Cuadrilla de {0}", Descargador.Cuadrilla.NombreEncargado);
            }

            if (PagoPrestamos != null && PagoPrestamos.Any())
            {
                return "La Boleta tiene registrada Abonos a Préstamos";
            }

            if (OrdenesCombustible != null && OrdenesCombustible.Where(b => b.BoletaId != null).Any())
            {
                return "Existen Ordenes de Combustible asignadas a la Boleta";
            }

            return string.Empty;
        }

        private string ValidarEstadoEliminarBoleta()
        {
            if (Estado == Estados.ENPROCESO)
            {
                return "La Boleta está en Proceso de Pago";
            }

            if (Estado == Estados.CERRADO)
            {
                return "La Boleta YA está Cerrada";
            }

            return string.Empty;
        }

        public string ObtenerDescripcionEquipo()
        {
            var equipo = Proveedor.Equipos.Where(e => e.PlacaCabezal == PlacaEquipo).FirstOrDefault();

            if (equipo == null) return "N/A";

            return equipo.EquiposCategoria.Descripcion;
        }

        public int ObtenerCategoriaEquipo()
        {
            var equipo = Proveedor.Equipos.Where(e => e.PlacaCabezal == PlacaEquipo).FirstOrDefault();

            if (equipo == null) return 0;

            return equipo.EquiposCategoria.EquipoCategoriaId;
        }

        public void RemoverDescargaProducto()
        {
            if (Estado != Estados.ACTIVO) return;

            if (Descargador.EsDescargaPorAdelanto)
            {
                DescargasPorAdelantado descargaPorAdelanto = DescargasPorAdelantado.FirstOrDefault(b => b.BoletaId == BoletaId);

                if (descargaPorAdelanto != null)
                {
                    descargaPorAdelanto.Estado = Estados.PENDIENTE;
                    descargaPorAdelanto.BoletaId = null;
                }
            }

            Descargador = null;
        }

        public int ObtenerAntiguedadBoleta()
        {
            TimeSpan difference = DateTime.Now - FechaCreacionBoleta;

            return (int)difference.TotalDays;
        }

        public void AddItemBoletaClose(BoletaCierres newCloseItem)
        {
            if (BoletaCierres == null) return;

            BoletaCierres.Add(newCloseItem);
        }

        public decimal ObtenerTasaSeguridad()
        {
            if (Proveedor.IsExempt) return 0.10m;

            var thounsands = Math.Round((ObtenerTotalFacturaCompra() / 1000), 2);

            if (!IsInteger(thounsands))
            {
                thounsands++;
            }
            
            return Math.Round((thounsands * 2), 0, MidpointRounding.AwayFromZero);
        }

        public void DeleteWrongClose()
        {
            BoletaCierres = null;
            BoletaDetalles = null;

            ActualizarEstadoBoleta();
        }

        public void CloseBoleta()
        {
            Estado = Estados.CERRADO;
        }

        private bool IsInteger(decimal thounsands)
        {
            return Math.Round(thounsands, 2) == Math.Round(thounsands);
        }

        public bool IsShippingNumberRequired()
        {
            string result = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("NumeroEnvio", PlantaId.ToString(), string.Empty);

            return !string.IsNullOrWhiteSpace(result) ? result == "1" ? true : false : false;
        }

        private bool IsWrongCode(string code)
        {
            if (code.Any(x => Char.IsWhiteSpace(x))) return true;

            return false;
        }

        public void AddAjusteBoletaPago(AjusteBoletaPago ajusteBoletaPago)
        {
            if (AjusteBoletaPagos == null) return;

            AjusteBoletaPagos.Add(ajusteBoletaPago);
        }

        public void AddBoletaCierre(BoletaCierres boletacierre)
        {
            if (BoletaCierres == null) return;

            BoletaCierres.Add(boletacierre);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (BoletaImg == null || BoletaImg.Imagen == null)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Imagen");
                listaErrores.Add(mensaje);
            }

            if (FechaSalida.Date > DateTime.Now.Date)
            {
                var mensaje = string.Format("La fecha seleccionada es mayor al día de hoy");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(CodigoBoleta))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CodigoBoleta");
                listaErrores.Add(mensaje);
            }

            if (ProveedorId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "ProveedorId");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(PlacaEquipo))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlacaEquipo");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Motorista))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Motorista");
                listaErrores.Add(mensaje);
            }
            
            if (PlantaId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PlantaId");
                listaErrores.Add(mensaje);
            }

            if (CategoriaProductoId == 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "CategoriaProductoId");
                listaErrores.Add(mensaje);
            }

            if (PesoSalida <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PesoSalida");
                listaErrores.Add(mensaje);
            }

            if (PesoEntrada <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PesoEntrada");
                listaErrores.Add(mensaje);
            }

            if (PesoProducto <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PesoProducto");
                listaErrores.Add(mensaje);
            }

            if (PrecioProductoCompra <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioProductoCompra");
                listaErrores.Add(mensaje);
            }

            if (PrecioProductoVenta <= 0)
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "PrecioProductoVenta");
                listaErrores.Add(mensaje);
            }

            if (IsShippingNumberRequired() && string.IsNullOrWhiteSpace(NumeroEnvio))
            {
                string message = string.Format(MensajesValidacion.Campo_Requerido, "NumeroEnvio");
                listaErrores.Add(message);
            }

            return listaErrores;
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();
            
            var validarEliminarBoleta = ValidarEliminarBoleta();

            if (!string.IsNullOrWhiteSpace(validarEliminarBoleta))
            {
                listaErrores.Add(validarEliminarBoleta);
            }
            
            return listaErrores;
        }
    }
}
