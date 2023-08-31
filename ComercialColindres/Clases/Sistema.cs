using System;
using System.ComponentModel;

namespace ComercialColindres.Clases
{
    public class Sistema
    {
        public Sistema()
        {
            TituloAplicacion = "Comercial Colindres";
        }

        public static string Uri_ApiService { get; set; }
        public static string RutaReporte { get; set; }
        public static string DEITemplateExcelDMC { get; set; }

        public static string TituloAplicacion
        {
            get { return _tituloAplicacion; }
            set
            {
                if (_tituloAplicacion != value)
                {
                    _tituloAplicacion = value;
                    RaiseStaticPropertyChanged("TituloAplicacion");
                }
            }
        }
        private static string _tituloAplicacion;


        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string propName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
                handler(null, new PropertyChangedEventArgs(propName));
        }
    }
}
