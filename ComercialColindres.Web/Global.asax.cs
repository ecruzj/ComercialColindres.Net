using ComercialColindres.Web.Api;
using ServiceStack.WebHost.Endpoints;
using System;
using ServiceStack.Text;
using ComercialColindres.Web.MapeosDTOs;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using Microsoft.Practices.Unity;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ContainerAdapter.Unity;
using Funq;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.DataCore.Logging;
using System.Text;
using ComercialColindres.DTOs.Clases;
using ServiceStack;
using ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre;
using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Servicios.Servicios.AjusteTipos;
using ComercialColindres.Servicios.Servicios.BoletasImg;
using ComercialColindres.Servicios.Servicios.OrdenesCombustibleImg;
using ComercialColindres.ReglasNegocio.DomainServices.Invoice;
using ComercialColindres.Servicios.Servicios.FuelOrderManualPayments;

namespace ComercialColindres.Web
{
    public class Global : System.Web.HttpApplication
    {
        public class AppHost : AppHostBase
        {
            public AppHost() : base("ComercialColindres RestApi Services", typeof(SucursalesRestService).Assembly) { }

            public override void Configure(Container container)
            {
                JsConfig.DateHandler = JsonDateHandler.ISO8601;

                InitializeAutomap.InitializarAutomap();

                SetConfig(new EndpointHostConfig
                {
                    GlobalResponseHeaders =
                    {
                        {"Access-Control-Allow-Origin", "*"},
                        {"Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS"},
                    },
                });
                SetConfig(new EndpointHostConfig
                {
                    EnableFeatures = Feature.All.Remove(Feature.All).Add(Feature.Xml | Feature.Json | Feature.Html),
                });

                //register any dependencies your services use, e.g:
                IUnityContainer unityContainer = new UnityContainer();

                //->Identity Generation
                unityContainer.RegisterType<IIdentityFactory, ADOIdentityGeneratorFactory>(new ContainerControlledLifetimeManager());

                var identityGeneratorFactory = unityContainer.Resolve<IIdentityFactory>();
                IdentityFactory.SetCurrent(identityGeneratorFactory);

                //Settings
                //El metodo ConfigurarSettingFactory debe de ser ejecutado antes de registrar o resolver cualquier dependencia
                unityContainer.RegisterType<ISettingFactory, ADOSettingFactory>();
                var settingFactory = unityContainer.Resolve<ISettingFactory>();
                SettingFactory.SetCurrent(settingFactory);

                LoggerFactory.SetCurrent(new TraceSourceLogFactory());

                unityContainer.RegisterType<ICacheClient, MemoryCacheClient>(new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<ICacheAdapter, CacheAdapter>(new PerResolveLifetimeManager());

                //Handle Exceptions occurring in Services:
                this.ServiceExceptionHandler = (request, dto, exception) => {

                    //logs the exceptions here
                    string requestInfo = GetRequestInfo(request, dto);
                    LoggerFactory.CreateLog().LogError(requestInfo, exception);

                    //Prepare your own custom response
                    var responseStatus = exception.ToResponseStatus();
                    
                    var response = DtoUtils.CreateErrorResponse(request, exception, responseStatus);
                    
                    return response;
                };

                //Handle Unhandled Exceptions occurring outside of Services
                //E.g. Exceptions during Request binding or in filters:
                this.ExceptionHandler = ((req, res, operationName, ex) =>
                {
                    res.Write("Error: {0}: {1}".Fmt(ex.GetType().Name, ex.Message));
                    res.EndRequest(skipHeaders: true);
                });

                //App Services
                unityContainer.RegisterType<ICuadrillasAppServices, CuadrillasAppServices>();
                unityContainer.RegisterType<IClientePlantasAppServices, ClientePlantasAppServices>();
                unityContainer.RegisterType<IBoletasAppServices, BoletasAppServices>();
                unityContainer.RegisterType<IBoletaDetallesAppServices, BoletaDetallesAppServices>();
                unityContainer.RegisterType<IBancosAppServices, BancosAppServices>();
                unityContainer.RegisterType<IProveedoresAppServices, ProveedoresAppServices>();
                unityContainer.RegisterType<IConductoresAppServices, ConductoresAppServices>();
                unityContainer.RegisterType<IEquiposAppServices, EquiposAppServices>();
                unityContainer.RegisterType<IEquiposCategoriasAppServices, EquiposCategoriasAppServices>();
                unityContainer.RegisterType<ICuentasBancariasAppServices, CuentasBancariasAppServices>();
                unityContainer.RegisterType<IFacturasAppServices, FacturasAppServices>();
                unityContainer.RegisterType<IOpcionesAppServices, OpcionesAppServices>();
                unityContainer.RegisterType<ISucursalesAppServices, SucursalesAppServices>();
                unityContainer.RegisterType<IUsuariosAppServices, UsuariosAppServices>();
                unityContainer.RegisterType<IOpcionesAppServices, OpcionesAppServices>();
                unityContainer.RegisterType<ICategoriaProductosAppServices, CategoriaProductosAppServices>();
                unityContainer.RegisterType<IPrecioProductosAppServices, PrecioProductosAppServices>();
                unityContainer.RegisterType<IPrecioDescargasAppServices, PrecioDescargasAppServices>();
                unityContainer.RegisterType<IGasolinerasAppServices, GasolinerasAppServices>();
                unityContainer.RegisterType<IGasolineraCreditosAppServices, GasolineraCreditosAppServices>();
                unityContainer.RegisterType<IGasolineraCreditoPagosAppServices, GasolineraCreditoPagosAppServices>();
                unityContainer.RegisterType<IOrdenesCombustibleAppServices, OrdenesCombustibleAppServices>();
                unityContainer.RegisterType<IDescargadoresAppServices, DescargadoresAppServices>();
                unityContainer.RegisterType<IBoletaCierresAppServices, BoletaCierresAppServices>();
                unityContainer.RegisterType<IPrestamosAppServices, PrestamosAppServices>();
                unityContainer.RegisterType<IPrestamosTransferenciasAppServices, PrestamosTransferenciasAppServices>();
                unityContainer.RegisterType<IPagoPrestamosAppServices, PagoPrestamosAppServices>();
                unityContainer.RegisterType<IPagoDescargadoresAppServices, PagoDescargadoresAppServices>();
                unityContainer.RegisterType<IPagoDescargaDetallesAppServices, PagoDescargaDetallesAppServices>();
                unityContainer.RegisterType<IFacturaDetalleBoletasAppServices, FacturaDetalleBoletasAppServices>();
                unityContainer.RegisterType<IRecibosAppServices, RecibosAppServices>();
                unityContainer.RegisterType<IFacturasCategoriasAppServices, FacturasCategoriasAppServices>();
                unityContainer.RegisterType<IReportesAppServices, ReportesAppServices>();
                unityContainer.RegisterType<ILineasCreditoAppServices, LineasCreditoAppServices>();
                unityContainer.RegisterType<ILineasCreditoDeduccionesAppServices, LineasCreditoDeduccionesAppServices>();
                unityContainer.RegisterType<ICuentasFinancierasAppServices, CuentasFinancierasAppServices>();
                unityContainer.RegisterType<ICuentasFinancieraTiposAppServices, CuentasFinancieraTiposAppServices>();
                unityContainer.RegisterType<IOrdenesCompraProductoAppServices, OrdenesCompraProductoAppServices>();
                unityContainer.RegisterType<IOrdenesCompraProductoDetalleAppServices, OrdenesCompraProductoDetalleAppServices>();
                unityContainer.RegisterType<IOrdenesCompraDetalleBoletaAppServices, OrdenesCompraDetalleBoletaAppServices>();
                unityContainer.RegisterType<IMaestroBiomasaAppServices, MaestroBiomasaAppServices>();
                unityContainer.RegisterType<IBoletaOtrasDeduccionesAppServices, BoletaOtrasDeduccionesAppServices>();
                unityContainer.RegisterType<ILoggingAppServices, LoggingAppServices>();
                unityContainer.RegisterType<IDescargasPorAdelantadoAppServices, DescargasPorAdelantadoAppServices>();
                unityContainer.RegisterType<IBoletaHumedadAppServices, BoletaHumedadAppServices>();
                unityContainer.RegisterType<IBoletaHumedadPagoAppServices, BoletaHumedadPagoAppServices>();
                unityContainer.RegisterType<IBoletasHumedadAsignacionAppServices, BoletasHumedadAsignacionAppServices>();
                unityContainer.RegisterType<IBoletaDeduccionManualAppServices, BoletaDeduccionManualAppServices>();
                unityContainer.RegisterType<IFacturaDetalleItemAppServices, FacturaDetalleItemAppServices>();
                unityContainer.RegisterType<ISubPlantaServices, SubPlantaServices>();
                unityContainer.RegisterType<IFacturaPagoAppServices, FacturaPagoAppServices>();
                unityContainer.RegisterType<INotaCreditoServices, NotaCreditoServices>();
                unityContainer.RegisterType<IBonificacionProductoAppServices, BonificacionProductoAppServices>();
                unityContainer.RegisterType<IAjusteBoletaAppServices, AjusteBoletaAppServices>();
                unityContainer.RegisterType<IAjusteBoletaPagoAppServices, AjusteBoletaPagoAppServices>();
                unityContainer.RegisterType<IAjusteTipoAppServices, AjusteTipoAppServices>();
                unityContainer.RegisterType<IAjusteBoletaDetalleAppServices, AjusteBoletaDetalleAppServices>();
                unityContainer.RegisterType<IBoletaImgAppServices, BoletaImgAppServices>();
                unityContainer.RegisterType<IOrdenCombustibleImgAppServices, OrdenCombustibleImgAppServices>();
                unityContainer.RegisterType<IStorageService, StorageService>();
                unityContainer.RegisterType<IFuelOrderManualPaymentAppServices, FuelOrderManualPaymentAppServices>();

                //Domain Services
                unityContainer.RegisterType<IUsuariosDomainServices, UsuariosDomainServices>();
                unityContainer.RegisterType<IBoletasDetalleDomainServices, BoletasDetalleDomainServices>();
                unityContainer.RegisterType<ILineasCreditoDeduccionesDomainServices, LineasCreditoDeduccionesDomainServices>();
                unityContainer.RegisterType<IBoletasDomainServices, BoletasDomainServices>();
                unityContainer.RegisterType<IOrdenesCompraDetalleBoletaDomainServices, OrdenesCompraDetalleBoletaDomainServices>();
                unityContainer.RegisterType<IPrestamoDomainServices, PrestamoDomainServices>();
                unityContainer.RegisterType<IPagoPrestamosDomainServices, PagoPrestamosDomainServices>();
                unityContainer.RegisterType<IDescargadoresDomainServices, DescargadoresDomainServices>();
                unityContainer.RegisterType<IGasolineraCreditoDomainServices, GasolineraCreditoDomainServices>();
                unityContainer.RegisterType<IOrdenCombustibleDomainServices, OrdenCombustibleDomainServices>();
                unityContainer.RegisterType<IBoletaCierreDomainServices, BoletaCierreDomainServices>();
                unityContainer.RegisterType<IBoletaHumedadDomainServices, BoletaHumedadDomainServices>();
                unityContainer.RegisterType<IBoletaHumedadAsignacionDomainServices, BoletaHumedadAsignacionDomainServices>();
                unityContainer.RegisterType<IBoletaHumedadPagoDomainServices, BoletaHumedadPagoDomainServices>();
                unityContainer.RegisterType<IFacturaDetalleBoletaDomainServices, FacturaDetalleBoletaDomainServices>();
                unityContainer.RegisterType<IAjusteBoletaDomainServices, AjusteBoletaDomainServices>();
                unityContainer.RegisterType<IInvoiceDomainServices, InvoiceDomainServices>();

                var unityAdapter = new UnityContainerAdapter(unityContainer);
                container.Adapter = unityAdapter;
            }
            
        }
        
        private static string GetStringsWithParams(string resourceName, object[] args)
        {
            if (args != null && args.Length > 0)
            {
                return string.Format(resourceName, args);
            }

            return resourceName;
        }
        
        private  static string GetRequestInfo(IHttpRequest request, object dto)
        {
            var requestSB = new StringBuilder();

            requestSB.AppendLine("Request User Info:");

            if (dto != null)
            {
                var requestDTO = dto as RequestBase;

                if (requestDTO != null && requestDTO.RequestUserInfo != null)
                {
                    requestSB.AppendLine(string.Format("UserId: {0}", requestDTO.RequestUserInfo.UserId));
                    requestSB.AppendLine(string.Format("SucursalId: {0}", requestDTO.RequestUserInfo.SucursalId));
                }
                else
                {
                    var userId = dto.GetType().GetProperty("UserId")?.GetValue(dto, null);

                    if (userId != null)
                    {
                        requestSB.AppendLine(string.Format("UserId: {0}", userId));
                    }
                }
            }

            requestSB.AppendLine();
            requestSB.AppendLine("Request Info:");
            requestSB.AppendLine(string.Format("HttpMethod: {0}", request.HttpMethod));
            requestSB.AppendLine(string.Format("RawUrl: {0}", request.RawUrl));
            requestSB.AppendLine(string.Format("PathInfo: {0}", request.PathInfo));
            requestSB.AppendLine(string.Format("AbsoluteUri: {0}", request.AbsoluteUri));
            requestSB.AppendLine(string.Format("QueryString: {0}", request.QueryString));
            requestSB.AppendLine(string.Format("AcceptTypes: {0}", request.AcceptTypes));
            requestSB.AppendLine(string.Format("ContentType: {0}", request.ContentType));
            requestSB.AppendLine(string.Format("ResponseContentType: {0}", request.ResponseContentType));
            requestSB.AppendLine(string.Format("OperationName: {0}", request.OperationName));
            requestSB.AppendLine(string.Format("ApplicationFilePath: {0}", request.ApplicationFilePath));
            requestSB.AppendLine(string.Format("ContentLength: {0}", request.ContentLength));
            requestSB.AppendLine(string.Format("UrlReferrer: {0}", request.UrlReferrer));
            requestSB.AppendLine(string.Format("RemoteIp: {0}", request.RemoteIp));
            requestSB.AppendLine(string.Format("UserHostAddress: {0}", request.UserHostAddress));

            requestSB.AppendLine();
            requestSB.AppendLine("Request Headers:");

            if (request.Headers != null && request.Headers.Count > 0)
            {
                foreach (var header in request.Headers.AllKeys)
                {
                    requestSB.AppendLine(string.Format("{0}: {1}", header, request.Headers[header]));
                }
            }

            if (dto != null)
            {
                requestSB.AppendLine();
                requestSB.AppendLine("Request Body:");
                var requestJson = QueryStringSerializer.SerializeToString(dto);

                requestJson = requestJson.Replace("{", "{{");
                requestJson = requestJson.Replace("}", "}}");

                requestSB.AppendLine(requestJson);
            }

            requestSB.AppendLine();
            requestSB.AppendLine("Exception Detail:");
            return requestSB.ToString();
        }
        
        private static bool ExceptionIsDomainValidationException(Exception exception)
        {
            return exception.InnerException != null;
        }
        
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }        
    }
}