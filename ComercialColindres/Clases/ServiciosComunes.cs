using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.Logging;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.Enumeraciones;
using ComercialColindres.Recursos;
using ComercialColindres.ViewModels.Reportes;
using NotifyMessageControl;
using ServiceStack.ServiceClient.Web;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using WPFCore.Funciones;

namespace ComercialColindres.Clases
{
    public class ServiciosComunes : IServiciosComunes
    {
        LeerConfiguracion _configuracionManager;
        const string CARPETA_PRINCIPAL = "ComercialColindres";

        public ServiciosComunes()
        {
            _configuracionManager = new LeerConfiguracion(@"C:\" + CARPETA_PRINCIPAL + @"\Config.ini");
        }

        public void MostrarNotificacion(EventType tipoNotificacion, string titulo, string mensaje)
        {
            var tipoNotificacionMensaje = NotifyMessageType.Successful;
            if (tipoNotificacion == EventType.Error)
            {
                tipoNotificacionMensaje = NotifyMessageType.Error;
            }
            if (tipoNotificacion == EventType.Information)
            {
                tipoNotificacionMensaje = NotifyMessageType.Information;
            }
            if (tipoNotificacion == EventType.Successful)
            {
                tipoNotificacionMensaje = NotifyMessageType.Successful;
            }
            if (tipoNotificacion == EventType.Warning)
            {
                tipoNotificacionMensaje = NotifyMessageType.Warning;
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                var msg = new NotifyMessage(titulo, mensaje, tipoNotificacionMensaje, () => { });
                var notifyMessageMgr = new NotifyMessageManager
                   (
                       Screen.Width,
                       Screen.Height,
                       200,
                       150
                   );
                notifyMessageMgr.Start();

                notifyMessageMgr.EnqueueMessage(msg);
            });
        }

        public void MostrarNotificacion(EventType tipoNotificacion, string mensaje)
        {
            var tipoNotificacionMensaje = NotifyMessageType.Successful;
            if (tipoNotificacion == EventType.Error)
            {
                tipoNotificacionMensaje = NotifyMessageType.Error;
            }
            if (tipoNotificacion == EventType.Information)
            {
                tipoNotificacionMensaje = NotifyMessageType.Information;
            }
            if (tipoNotificacion == EventType.Successful)
            {
                tipoNotificacionMensaje = NotifyMessageType.Successful;
            }
            if (tipoNotificacion == EventType.Warning)
            {
                tipoNotificacionMensaje = NotifyMessageType.Warning;
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                var msg = new NotifyMessage("Mensaje:", mensaje, tipoNotificacionMensaje, () => { });
                var notifyMessageMgr = new NotifyMessageManager
                    (
                    Screen.Width,
                    Screen.Height,
                    200,
                    150
                    );
                notifyMessageMgr.Start();

                notifyMessageMgr.EnqueueMessage(msg);
            });
        }

        public string ObtenerRutaReporte()
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (path == @"file:\C:\MisSistemas\Constructora\Constructora.WPF\bin\Debug")
            {
                return @"C:\MisSistemas\Constructora\Constructora.Reportes\";
            }
            else
            {
                return @"C:\" + CARPETA_PRINCIPAL + @"\Reportes";
            }
        }

        public string ObtenerUriApiService()
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (path.Contains(@"\bin\Debug"))
            {
                return Configuracion.Uri_ApiServiceLocal;
            }
            else
            {
                var uriApiServer = _configuracionManager.ObtenerValorConfiguracion("ApiServer");
                return uriApiServer + "/api/";
            }
        }

        public string GetPathBoletasImg()
        {
            return @"C:\" + CARPETA_PRINCIPAL + @"\BoletasImg\";
        }

        public string GetPathFuelOrder()
        {
            return @"C:\" + CARPETA_PRINCIPAL + @"\FuerlOrders\";
        }

        public RequestUserInfo GetRequestUserInfo()
        {
            return new RequestUserInfo
            {
                UserId = InformacionSistema.UsuarioActivo.Usuario,
                SucursalId = InformacionSistema.SucursalActiva.SucursalId
            };
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogError(string message, Exception exception)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(message);

            if (exception != null)
            {
                errorMessage.AppendLine();
                errorMessage.AppendLine(exception.Message);

                if (exception.InnerException != null)
                {
                    errorMessage.AppendLine();
                    errorMessage.AppendLine(exception.InnerException.ToString());
                }

                if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                {
                    errorMessage.AppendLine();
                    errorMessage.AppendLine(exception.StackTrace);
                }
            }
            try
            {
                LogError(errorMessage.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at Service:" + ex.Message);
            }
        }

        private void LogError(string errorMessage)
        {
            try
            {
                var uri = InformacionSistema.Uri_ApiService;
                var client = new JsonServiceClient(uri);
                var request = new PostLogging
                {
                    ErrorMessage = errorMessage
                };

                client.Post(request);
            }
            catch { }
        }

        public RptBoletaPaymentPendingResumenDto GetPendingBoletasPayment(RptBoletaPaymentPendingResumenDto info)
        {
            return new RptBoletaPaymentPendingResumenDto
            {
                BoletaQuantity = info.BoletaQuantity,
                BoletasPending = info.BoletasPending,
                SaldoPendiente = info.SaldoPendiente
            };
        }
    }
}
