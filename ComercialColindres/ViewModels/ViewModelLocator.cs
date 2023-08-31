using ComercialColindres.Clases;
using ComercialColindres.ViewModels.Reportes;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ComercialColindres.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IServiciosComunes, ServiciosComunes>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UsuariosViewModel>();
            SimpleIoc.Default.Register<ProveedoresViewModel>();
            SimpleIoc.Default.Register<BoletasViewModel>();
            SimpleIoc.Default.Register<PrestamosViewModel>();
            SimpleIoc.Default.Register<DescargadoresViewModel>();
            SimpleIoc.Default.Register<GasolinerasViewModel>();
            SimpleIoc.Default.Register<FacturasViewModel>();
            SimpleIoc.Default.Register<LineasCreditoViewModel>();
            SimpleIoc.Default.Register<OrdenesCompraProductoViewModel>();            
            SimpleIoc.Default.Register<MonitorBoletasInProcessViewModel>();
            SimpleIoc.Default.Register<AjusteBoletasViewModel>();


            //Reports
            SimpleIoc.Default.Register<ReportesMainViewModel>();
            SimpleIoc.Default.Register<RptPrestamosPorProveedorViewModel>();
            SimpleIoc.Default.Register<RptBoletasPorProveedorViewModel>();
            SimpleIoc.Default.Register<RptPrestamosPentiendesViewModel>();
            SimpleIoc.Default.Register<RptCompraProductoBiomasaResumenViewModel>();
            SimpleIoc.Default.Register<RptCompraBiomasaDetalleViewModel>();
            SimpleIoc.Default.Register<RptBoletaPaymentPendingViewModel>();
            SimpleIoc.Default.Register<BoletasHumedadViewModel>();
            SimpleIoc.Default.Register<RptHumidityPendingPaymentViewModel>();
            SimpleIoc.Default.Register<RptPendingInvoiceViewModel>();
            SimpleIoc.Default.Register<RptBoletasWithoutInvoiceViewModel>();
            SimpleIoc.Default.Register<RptBillsWithWeightsErrorViewModel>();
            SimpleIoc.Default.Register<RptHistoryOfInvoiceBalancesViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public UsuariosViewModel UsuariosViewModel
        {
            get { return ServiceLocator.Current.GetInstance<UsuariosViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProveedoresViewModel ProveedoresViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ProveedoresViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BoletasViewModel BoletasViewModel
        {
            get { return ServiceLocator.Current.GetInstance<BoletasViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PrestamosViewModel PrestamosViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PrestamosViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DescargadoresViewModel DescargadoresViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DescargadoresViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public GasolinerasViewModel GasolinerasViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GasolinerasViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public FacturasViewModel FacturasViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FacturasViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LineasCreditoViewModel LineasCreditoViewModel
        {
            get { return ServiceLocator.Current.GetInstance<LineasCreditoViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public OrdenesCompraProductoViewModel OrdenesCompraProductoViewModel
        {
            get { return ServiceLocator.Current.GetInstance<OrdenesCompraProductoViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ReportesMainViewModel ReportesViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ReportesMainViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public RptPrestamosPorProveedorViewModel RptPrestamosPorProveedorViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptPrestamosPorProveedorViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public RptBoletasPorProveedorViewModel RptBoletasPorProveedorViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptBoletasPorProveedorViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptPrestamosPentiendesViewModel RptPrestamosPentiendesViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptPrestamosPentiendesViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public MonitorBoletasInProcessViewModel MonitorBoletasInProcessViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MonitorBoletasInProcessViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptCompraProductoBiomasaResumenViewModel RptCompraProductoBiomasaResumenViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptCompraProductoBiomasaResumenViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptCompraBiomasaDetalleViewModel RptCompraBiomasaDetalleViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptCompraBiomasaDetalleViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptBoletaPaymentPendingViewModel RptBoletaPaymentPendingViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptBoletaPaymentPendingViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public BoletasHumedadViewModel BoletasHumedadViewModel
        {
            get { return ServiceLocator.Current.GetInstance<BoletasHumedadViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptHumidityPendingPaymentViewModel RptHumidityPendingPaymentViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptHumidityPendingPaymentViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptPendingInvoiceViewModel RptPendingInvoiceViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptPendingInvoiceViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptBoletasWithoutInvoiceViewModel RptBoletasWithoutInvoiceViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptBoletasWithoutInvoiceViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptBillsWithWeightsErrorViewModel RptBillsWithWeightsErrorViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptBillsWithWeightsErrorViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public AjusteBoletasViewModel AjusteBoletasViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AjusteBoletasViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public RptHistoryOfInvoiceBalancesViewModel RptHistoryOfInvoiceBalancesViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RptHistoryOfInvoiceBalancesViewModel>(); }
        }

        public static void Cleanup()
        {
        }
    }
}
