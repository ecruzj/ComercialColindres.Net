using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Entorno.Mapeos;
using ComercialColindres.Datos.Entorno.Modelos;
using ServidorCore.Aplicacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ComercialColindres.Datos.Entorno
{
    public class ComercialColindresContext : DbContext
    {
        static ComercialColindresContext()
        {
            Database.SetInitializer<ComercialColindresContext>(null);
        }

        public ComercialColindresContext()
            :base("Name=ComercialColindresContext")
        {
        }

        public DbSet<Bancos> Bancos { get; set; }
        public DbSet<BoletaCierres> BoletaCierres { get; set; }
        public DbSet<BoletaDetalles> BoletaDetalles { get; set; }
        public DbSet<Boletas> Boletas { get; set; }
        public DbSet<BoletaImg> BoletasImg { get; set; }
        public DbSet<CategoriaProductos> CategoriaProductos { get; set; }
        public DbSet<ClientePlantas> ClientePlantas { get; set; }
        public DbSet<Deducciones> Deducciones { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Conductores> Conductores { get; set; }
        public DbSet<Configuraciones> Configuraciones { get; set; }
        public DbSet<ConfiguracionesDetalles> ConfiguracionesDetalles { get; set; }
        public DbSet<Correlativos> Correlativos { get; set; }
        public DbSet<Cuadrillas> Cuadrillas { get; set; }
        public DbSet<CuentasBancarias> CuentasBancarias { get; set; }
        public DbSet<Descargadores> Descargadores { get; set; }
        public DbSet<PagoDescargadores> PagoDescargadores { get; set; }
        public DbSet<PagoDescargaDetalles> PagoDescargaDetalles { get; set; }
        public DbSet<Equipos> Equipos { get; set; }
        public DbSet<EquiposCategorias> EquiposCategorias { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturasCategorias> FacturasCategorias { get; set; }
        public DbSet<FacturaDetalleItem> FacturaDetalleItem { get; set; }
        public DbSet<FacturaPago> FacturaPago { get; set; }
        public DbSet<FacturaDetalleBoletas> FacturaDetalleBoletas { get; set; }
        public DbSet<Gasolineras> Gasolineras { get; set; }
        public DbSet<GasolineraCreditos> GasolineraCreditos { get; set; }
        public DbSet<GasolineraCreditoPagos> GasolineraCreditoPagos { get; set; }
        public DbSet<Opciones> Opciones { get; set; }
        public DbSet<OrdenesCombustible> OrdenesCombustible { get; set; }
        public DbSet<OrdenCombustibleImg> OrdenesCombustibleImg { get; set; }
        public DbSet<PagoPrestamos> PagoPrestamos { get; set; }
        public DbSet<PrecioDescargas> PrecioDescargas { get; set; }
        public DbSet<PrecioProductos> PrecioProductos { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Recibos> Recibos { get; set; }
        public DbSet<PrestamosTransferencias> PrestamosTransferencias { get; set; }
        public DbSet<Sucursales> Sucursales { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<UsuariosOpciones> UsuariosOpciones { get; set; }
        public DbSet<PagoOrdenesCombustible> PagoOrdenesCombustible { get; set; }
        public DbSet<LineasCredito> LineasCredito { get; set; }
        public DbSet<LineasCreditoDeducciones> LineasCreditoDeducciones { get; set; }
        public DbSet<CuentasFinancieras> CuentasFinancieras { get; set; }
        public DbSet<CuentasFinancieraTipos> CuentasFinancieraTipos { get; set; }
        public DbSet<MaestroBiomasa> MaestroBiomasa { get; set; }
        public DbSet<OrdenesCompraProducto> OrdenesCompraProducto { get; set; }
        public DbSet<OrdenesCompraProductoDetalle> OrdenesCompraProductoDetalle { get; set; }
        public DbSet<OrdenesCompraDetalleBoleta> OrdenesCompraDetalleBoleta { get; set; }
        public DbSet<BoletaOtrasDeducciones> BoletaOtrasDeducciones { get; set; }
        public DbSet<DescargasPorAdelantado> DescargasPorAdelantado { get; set; }
        public DbSet<BoletaHumedad> BoletasHumedad { get; set; }
        public DbSet<BoletaHumedadPago> BoletasHumedadPago { get; set; }
        public DbSet<BoletaHumedadAsignacion> BoletaHumedadAsignacion { get; set; }
        public DbSet<BoletaDeduccionManual> BoletaDeduccionManual { get; set; }
        public DbSet<SubPlanta> SubPlantas { get; set; }
        public DbSet<NotaCredito> NotaCredito { get; set; }
        public DbSet<BonificacionProducto> BonifacionProducto { get; set; }
        public DbSet<AjusteTipo> AjusteTipos { get; set; }
        public DbSet<AjusteBoleta> AjusteBoleta { get; set; }
        public DbSet<AjusteBoletaDetalle> AjusteBoletaDetalle { get; set; }
        public DbSet<AjusteBoletaPago> AjusteBoletaPago { get; set; }
        public DbSet<FuelOrderManualPayment> FuelOrderManualPayments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BancosMap());
            modelBuilder.Configurations.Add(new BoletaCierresMap());
            modelBuilder.Configurations.Add(new BoletaDetallesMap());
            modelBuilder.Configurations.Add(new BoletasMap());
            modelBuilder.Configurations.Add(new BoletasImgMap());
            modelBuilder.Configurations.Add(new CategoriaProductosMap());
            modelBuilder.Configurations.Add(new ClientePlantasMap());
            modelBuilder.Configurations.Add(new ConductoreMap());
            modelBuilder.Configurations.Add(new ConfiguracionesMap());
            modelBuilder.Configurations.Add(new ConfiguracionesDetallesMap());
            modelBuilder.Configurations.Add(new CorrelativosMap());
            modelBuilder.Configurations.Add(new CuadrillasMap());
            modelBuilder.Configurations.Add(new CuentasBancariasMap());
            modelBuilder.Configurations.Add(new DeduccionesMap());
            modelBuilder.Configurations.Add(new DescargadoresMap());
            modelBuilder.Configurations.Add(new EquiposMap());
            modelBuilder.Configurations.Add(new EquiposCategoriasMap());
            modelBuilder.Configurations.Add(new FacturasMap());
            modelBuilder.Configurations.Add(new FacturasCategoriasMap());
            modelBuilder.Configurations.Add(new FacturaDetalleItemsMap());
            modelBuilder.Configurations.Add(new FacturaDetalleBoletasMap());
            modelBuilder.Configurations.Add(new GasolinerasMap());
            modelBuilder.Configurations.Add(new GasolineraCreditosMap());
            modelBuilder.Configurations.Add(new GasolineraCreditoPagosMap());
            modelBuilder.Configurations.Add(new OpcionesMap());
            modelBuilder.Configurations.Add(new OrdenesCombustibleMap());
            modelBuilder.Configurations.Add(new OrdenesCombustibleImgMap());
            modelBuilder.Configurations.Add(new PagoPrestamosMap());
            modelBuilder.Configurations.Add(new PrecioDescargasMap());
            modelBuilder.Configurations.Add(new PrecioProductosMap());
            modelBuilder.Configurations.Add(new PrestamosMap());
            modelBuilder.Configurations.Add(new RecibosMap());
            modelBuilder.Configurations.Add(new PrestamosTransferenciasMap());
            modelBuilder.Configurations.Add(new ProveedoresMap());
            modelBuilder.Configurations.Add(new SucursalesMap());
            modelBuilder.Configurations.Add(new UsuariosMap());
            modelBuilder.Configurations.Add(new UsuariosOpcionesMap());
            modelBuilder.Configurations.Add(new PagoOrdenesCombustibleMap());
            modelBuilder.Configurations.Add(new PagoDescargadoresMap());
            modelBuilder.Configurations.Add(new PagoDescargaDetalleMap());
            modelBuilder.Configurations.Add(new LineasCreditoMap());
            modelBuilder.Configurations.Add(new LineasCreditoDeduccionesMap());
            modelBuilder.Configurations.Add(new CuentasFinancierasMap());
            modelBuilder.Configurations.Add(new CuentasFinancieraTiposMap());
            modelBuilder.Configurations.Add(new MaestroBiomasaMap());
            modelBuilder.Configurations.Add(new OrdenesCompraProductoMap());
            modelBuilder.Configurations.Add(new OrdenesCompraProductoDetalleMap());
            modelBuilder.Configurations.Add(new OrdenesCompraDetalleBoletaMap());
            modelBuilder.Configurations.Add(new BoletaOtrasDeduccionesMap());
            modelBuilder.Configurations.Add(new DescargasPorAdelantadoMap());
            modelBuilder.Configurations.Add(new BoletaHumedadMap());
            modelBuilder.Configurations.Add(new BoletaHumedadPagoMap());
            modelBuilder.Configurations.Add(new BoletaHumedadAsignacionMap());
            modelBuilder.Configurations.Add(new BoletaDeduccionManualMap());
            modelBuilder.Configurations.Add(new SubPlantaMap());
            modelBuilder.Configurations.Add(new FacturaPagoMap());
            modelBuilder.Configurations.Add(new NotaCreditoMap());
            modelBuilder.Configurations.Add(new BonificacionProductoMap());
            modelBuilder.Configurations.Add(new AjusteTipoMap());
            modelBuilder.Configurations.Add(new AjusteBoletaMap());
            modelBuilder.Configurations.Add(new AjusteBoletaPagoMap());
            modelBuilder.Configurations.Add(new AjusteBoletaDetalleMap());
            modelBuilder.Configurations.Add(new FuelOrderManualPaymentMap());
        }

        public override int SaveChanges()
        {
            VerificarEntidadesRegistrosEliminados();  

            var saveChanges = (-1);
            try
            {
                saveChanges = base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

            return saveChanges;
        }

        public int SaveChanges(TransaccionInformacion transaccionInformacion)
        {
            var addedAuditedEntities = ChangeTracker.Entries<Entity>().Where(p => p.State == EntityState.Added).Select(p => p.Entity);
            var modifiedAuditedEntities = ChangeTracker.Entries<Entity>().Where(p => p.State == EntityState.Modified).Select(p => p.Entity);

            var now = DateTime.Now;

            foreach (var added in addedAuditedEntities)
            {
                added.FechaTransaccion = now;
                added.ModificadoPor = transaccionInformacion.Usuario;
                added.DescripcionTransaccion = transaccionInformacion.DescripcionTransaccion;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.FechaTransaccion = now;
                modified.ModificadoPor = transaccionInformacion.Usuario;
                modified.DescripcionTransaccion = transaccionInformacion.DescripcionTransaccion;
            }

            VerificarEntidadesRegistrosEliminados();

            var saveChanges = (-1);
            try
            {
                saveChanges = base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

            return saveChanges;
        }

        private void VerificarEntidadesRegistrosEliminados()
        {
            var changesInfo = ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).ToList();
            if (changesInfo != null)
            {
                var entidades = (from reg in changesInfo
                                 select new
                                 {
                                     Entidad = reg.Entity.GetType().BaseType.ToString()
                                 }).Distinct().ToList();
                foreach (var entityEntry in entidades)
                {
                    var entidadModificada = entityEntry.Entidad;

                    if (EntidadModificada("UsuariosOpciones", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<UsuariosOpciones>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Usuario == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("CuentasBancarias", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<CuentasBancarias>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Proveedor == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("Equipos", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<Equipos>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Proveedor == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaCierres", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaCierres>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaDetalles", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaDetalles>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("PrestamosTransferencias", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<PrestamosTransferencias>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Prestamo == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("Prestamos", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<Prestamos>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Proveedor == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("PagoPrestamos", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<PagoPrestamos>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("Descargadores", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<Descargadores>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("PagoDescargadores", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<PagoDescargadores>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Descargadores == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("PagoDescargaDetalles", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<PagoDescargaDetalles>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.PagoDescargador == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("GasolineraCreditos", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<GasolineraCreditos>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Gasolinera == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("GasolineraCreditoPagos", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<GasolineraCreditoPagos>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.GasolineraCredito == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("OrdenesCombustible", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<OrdenesCombustible>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.GasolineraCredito == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("OrdenCombustibleImg", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<OrdenCombustibleImg>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.OrdenCombustible == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("FacturaDetalleBoletas", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<FacturaDetalleBoletas>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Factura == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("LineasCreditoDeducciones", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<LineasCreditoDeducciones>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.LineasCredito == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("OrdenesCompraProductoDetalle", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<OrdenesCompraProductoDetalle>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.OrdenesCompraProducto == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("OrdenesCompraDetalleBoleta", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<OrdenesCompraDetalleBoleta>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("Boletas", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<Boletas>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Proveedor == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaImg", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaImg>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaOtrasDeducciones", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaOtrasDeducciones>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("DescargasPorAdelantado", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<DescargasPorAdelantado>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.PagoDescargador == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("Descargadores", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<Descargadores>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaHumedad", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaHumedad>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.ClientePlanta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaHumedadAsignacion", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaHumedadAsignacion>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaHumedadPago", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaHumedadPago>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada("BoletaDeduccionManual", entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<BoletaDeduccionManual>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(FacturaDetalleItem), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<FacturaDetalleItem>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Factura == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(FacturaPago), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<FacturaPago>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Invoice == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }
                    
                    if (EntidadModificada(nameof(NotaCredito), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<NotaCredito>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Invoice == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(AjusteBoleta), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<AjusteBoleta>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(AjusteBoletaDetalle), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<AjusteBoletaDetalle>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.AjusteBoleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(AjusteBoletaPago), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<AjusteBoletaPago>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.Boleta == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }

                    if (EntidadModificada(nameof(FuelOrderManualPayment), entidadModificada))
                    {
                        var entity = ChangeTracker.Entries<FuelOrderManualPayment>().Where(a => a.State == EntityState.Modified);
                        foreach (var entry in entity.Where(r => r.Entity.FuelOrder == null))
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }
                }
            }
        }

        private bool EntidadModificada(string nombreEntidad, string entityEntry)
        {
            return entityEntry.Contains(nombreEntidad);
        }

        #region Implementation of IUnitOfWork

        public virtual void Commit()
        {
            //Actualiza la FechaTransaccion de cada entidad agregada, modificada o eliminada con la fecha del servidor
            foreach (var entry in ChangeTracker.Entries().Where(
                e => e.Entity is Entity && (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)))
            {
                ((Entity)entry.Entity).FechaTransaccion = DateTime.Now;
            }

            base.SaveChanges();
        }

        public virtual void Commit(TransactionInfo transactionInfo)
        {
            #region Check Prerequisites

            if (transactionInfo == null) throw new ArgumentNullException("transactionInfo");
            if (string.IsNullOrWhiteSpace(transactionInfo.DescripcionTransaccion)) throw new ArgumentException("transactionInfo.DescripcionTransaccion");
            if (string.IsNullOrWhiteSpace(transactionInfo.ModificadoPor)) throw new ArgumentException("transactionInfo.ModificadoPor");
            
            #endregion

            Transaction transaction = BuildTransactionInfo(transactionInfo);
            VerificarEntidadesRegistrosEliminados();

            Commit(transaction);
        }

        #endregion

        private Transaction BuildTransactionInfo(TransactionInfo transactionInfo)
        {
            var transaccionId = IdentityFactory.CreateIdentity().NewSequentialTransactionIdentity();

            return new Transaction
            {
                TransactionId = transaccionId.TransactionId,
                TransactionDate = transaccionId.TransactionDate,
                TransactionDateUtc = transaccionId.TransactionUtcDate,                
                TransactionType = transactionInfo.TipoTransaccion,
                ModifiedBy = transactionInfo.ModificadoPor
            };
        }

        public virtual void Commit(Transaction transaction)
        {
            #region Check Prerequisites

            if (transaction == null) throw new ArgumentNullException("transaction");
            if (string.IsNullOrWhiteSpace(transaction.TransactionType)) throw new ArgumentException("transactionInfo.TipoTransaccion");
            if (string.IsNullOrWhiteSpace(transaction.ModifiedBy)) throw new ArgumentException("transactionInfo.ModificadoPor");

            #endregion

            ObjectContext objectContext = ((IObjectContextAdapter)this).ObjectContext;
            try
            {
                objectContext.Connection.Open();

                // Resetenado el detalla de las transacciones.
                transaction.TransactionDetail = new List<TransactionDetail>();
                
                using (var scope = TransactionScopeFactory.GetTransactionScope())
                {
                    var changedEntities = new List<ModifiedEntityEntry>();
                    var tableMapping = new List<EntityMapping>();
                    var sqlCommandInfos = new List<SqlCommandInfo>();

                    // Get ebtities with changes.
                    IEnumerable<DbEntityEntry> changedDbEntityEntries = GetChangedDbEntityEntries();

                    // Apply transaction info.
                    foreach (DbEntityEntry entry in changedDbEntityEntries)
                    {

                        ApplyTransactionInfo(transaction, entry);

                        // Get the deleted records info first
                        if (entry.State == EntityState.Deleted)
                        {
                            EntityMapping entityMapping = GetEntityMappingConfiguration(tableMapping, entry);
                            SqlCommandInfo sqlCommandInfo = GetSqlCommandInfo(transaction, entry, entityMapping);
                            if (sqlCommandInfo != null) sqlCommandInfos.Add(sqlCommandInfo);

                            transaction.AddDetail(entityMapping.TableName, entry.State.ToString(), transaction.TransactionType);
                        }
                        else
                        {
                            changedEntities.Add(new ModifiedEntityEntry(entry, entry.State.ToString()));
                        }
                    }

                    base.SaveChanges();


                    // Get the Added and Mdified records after changes, that way we will be able to get the generated .
                    foreach (ModifiedEntityEntry entry in changedEntities)
                    {
                        EntityMapping entityMapping = GetEntityMappingConfiguration(tableMapping, entry.EntityEntry);
                        SqlCommandInfo sqlCommandInfo = GetSqlCommandInfo(transaction, entry.EntityEntry, entityMapping);
                        if (sqlCommandInfo != null) sqlCommandInfos.Add(sqlCommandInfo);

                        transaction.AddDetail(entityMapping.TableName, entry.State, transaction.TransactionType);
                    }

                    // Adding Audit Detail Transaction CommandInfo.
                    sqlCommandInfos.AddRange(GetAuditRecords(transaction));


                    // Insert Transaction and audit records.
                    foreach (SqlCommandInfo sqlCommandInfo in sqlCommandInfos)
                    {
                        Database.ExecuteSqlCommand(sqlCommandInfo.Sql, sqlCommandInfo.Parameters);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objectContext.Connection.Close();
            }
        }

        private IEnumerable<DbEntityEntry> GetChangedDbEntityEntries()
        {
            return ChangeTracker.Entries().Where(
                e =>
                e.Entity is Entity &&
                (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted));
        }

        private void ApplyTransactionInfo(Transaction transaction, DbEntityEntry entry)
        {
            ((Entity)entry.Entity).FechaTransaccion = transaction.TransactionDate;
            ((Entity)entry.Entity).DescripcionTransaccion = entry.State.ToString();
            ((Entity)entry.Entity).ModificadoPor = transaction.ModifiedBy;

            AplicarInformacionTransaccion(entry, "TipoTransaccion", transaction.TransactionType);
            AplicarInformacionTransaccion(entry, "TransaccionUId", transaction.TransactionId);
            AplicarInformacionTransaccion(entry, "FechaTransaccionUtc", transaction.TransactionDateUtc);
        }

        private void AplicarInformacionTransaccion(DbEntityEntry item, string nombrePropiedad, object valorPropiedad)
        {
            if (item != null && item.Entity != null)
            {
                PropertyInfo propInfoEntity = item.Entity.GetType().GetProperty(nombrePropiedad);
                if (propInfoEntity != null)
                {
                    propInfoEntity.SetValue(item.Entity, valorPropiedad, null);
                }
            }
        }

        private EntityMapping GetEntityMappingConfiguration(List<EntityMapping> tableMapping, DbEntityEntry entry)
        {
            var type = GetDomainEntityType(entry);

            EntityMapping entityMapping = tableMapping.FirstOrDefault(m => m.EntityType == type);
            if (entityMapping == null)
            {
                string sql = Set(type).ToString();
                var regex = new Regex("FROM (?<table>.*) AS");

                Match match = regex.Match(sql);
                string tname = match.Groups["table"].Value;

                entityMapping = CreateTableMapping(type, tname);
                tableMapping.Add(entityMapping);
            }

            return entityMapping;
        }

        private Type GetDomainEntityType(DbEntityEntry entry)
        {
            Type type = entry.Entity.GetType();
            if (type.FullName != null)
            {
                if (type.FullName.Contains("Colindres"))
                {
                    return type;
                }

                if (type.BaseType != null)
                {
                    return type.BaseType;
                }
            }

            return null;
        }

        private EntityMapping CreateTableMapping(Type type, string tname)
        {
            return new EntityMapping { EntityType = type, TableName = tname, TransactionTableName = GetTransactionTableName(tname) };
        }

        private string GetTransactionTableName(string tname)
        {
            if (tname.Contains("_Transacciones"))
            {
                return tname;
            }

            string[] nameArray = tname.Split('.');
            string name = nameArray[1].Replace("[", "").Replace("]", "");

            int place = tname.LastIndexOf(name, StringComparison.Ordinal);

            string result = tname.Remove(place, name.Length).Insert(place, string.Format("{0}_Transacciones", name));
            return result;
        }

        private SqlCommandInfo GetSqlCommandInfo(Transaction transaction, DbEntityEntry entry, EntityMapping entityMapping)
        {
            if (entityMapping.TableName.Contains("_Transacciones"))
            {
                return null;
            }

            string sqlInsert;
            object[] param;
            CreateTransactionInsertStatement(entityMapping, entry, transaction, out sqlInsert, out param);

            var sqlCommandInfo = new SqlCommandInfo(sqlInsert, param);
            return sqlCommandInfo;
        }

        private void CreateTransactionInsertStatement(EntityMapping entityMapping, DbEntityEntry entry,
                                                      Transaction transaction, out string sqlInsert, out object[] objects)
        {
            var insert = new StringBuilder();
            var fields = new StringBuilder();
            var paramNames = new StringBuilder();
            var values = new List<Object>();

            insert.AppendLine(string.Format("Insert Into {0} ", entityMapping.TransactionTableName));

            int index = 0;
            IEnumerable<string> propertyNames = entry.State == EntityState.Deleted
                                                    ? entry.OriginalValues.PropertyNames
                                                    : entry.CurrentValues.PropertyNames;

            foreach (string property in propertyNames)
            {
                string prop = property;
                if (prop != "RowVersion")
                {
                    if (fields.Length == 0)
                    {
                        fields.Append(string.Format(" ({0}", prop));
                        paramNames.Append(string.Format(" values ({0}{1}{2}", "{", index, "}"));
                    }
                    else
                    {
                        fields.Append(string.Format(", {0}", prop));
                        paramNames.Append(string.Format(", {0}{1}{2}", "{", index, "}"));
                    }

                    values.Add(GetEntityPropertyValue(entry, prop, transaction));
                    index++;
                }
            }

            fields.Append(string.Format(") "));
            paramNames.Append(string.Format(") "));

            insert.AppendLine(fields.ToString());
            insert.AppendLine(paramNames.ToString());

            sqlInsert = insert.ToString();
            objects = values.ToArray();
        }

        private object GetEntityPropertyValue(DbEntityEntry entry, string prop, Transaction transaction)
        {
            object value;
            TryGeTransactionInfo(prop, transaction, out value);
            if (value != null)
            {
                return value;
            }

            if (entry.State == EntityState.Deleted || entry.State == EntityState.Detached)
            {
                return prop == "DescripcionTransaccion"
                           ? EntityState.Deleted.ToString()
                           : entry.Property(prop).OriginalValue;
            }
            return entry.Property(prop).CurrentValue;
        }

        private void TryGeTransactionInfo(string property, Transaction transaction, out object value)
        {
            switch (property)
            {
                case "TransaccionUId":
                    value = transaction.TransactionId;
                    break;
                case "TipoTransaccion":
                    value = transaction.TransactionType;
                    break;
                case "FechaTransaccion":
                    value = transaction.TransactionDate;
                    break;
                case "FechaTransaccionUtc":
                    value = transaction.TransactionDateUtc;
                    break;
                case "ModificadoPor":
                    value = transaction.ModifiedBy;
                    break;
                default:
                    value = null;
                    break;
            }
        }

        private IEnumerable<SqlCommandInfo> GetAuditRecords(Transaction transaction)
        {
            var auditCommands = new List<SqlCommandInfo>();

            // Adding Audit Header Transaction CommandInfo.
            auditCommands.Add(GetAuditHeaderCommandInfo(transaction));

            // Adding Audit Detail Transaction CommandInfo
            foreach (var transactionDetail in transaction.TransactionDetail)
            {
                auditCommands.Add(GetAuditDetailCommandInfo(transactionDetail));
            }

            return auditCommands;
        }

        private SqlCommandInfo GetAuditHeaderCommandInfo(Transaction transaction)
        {
            const string sqlInsert =
                "insert into  dbo.LogTransacciones(TransaccionUId, TipoTransaccion, FechaTransaccion, FechaTransaccionUtc, " +
                "ModificadoPor) " +
                "values({0}, {1}, {2}, {3}, {4})";

            var param = new object[]
                                 {
                                     transaction.TransactionId, transaction.TransactionType, transaction.TransactionDate,
                                     transaction.TransactionDateUtc, transaction.ModifiedBy
                                 };

            return new SqlCommandInfo(sqlInsert, param);
        }

        private SqlCommandInfo GetAuditDetailCommandInfo(TransactionDetail transactionDetail)
        {
            const string sqlInsert =
                "insert into  dbo.LogTransaccionesDetalle(TransaccionUId,TipoTransaccion, EntidadDominio, DescripcionTransaccion) " +
                                       "values({0}, {1}, {2},{3})";

            var param = new object[]
                                 {
                                     transactionDetail.TransactionId,transactionDetail.TransactionType, transactionDetail.TableName, transactionDetail.CrudOperation
                                 };

            return new SqlCommandInfo(sqlInsert, param);
        }

    }
}
