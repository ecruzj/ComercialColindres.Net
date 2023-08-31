using GalaSoft.MvvmLight;
using System.Timers;
using System.Windows;
using WPFCore.Modelos;

namespace ComercialColindres.Clases
{
    public class BaseVM : ViewModelBase
    {
        public BaseVM()
        {
            InicializarTimer();
        }

        private void InicializarTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
        }

        void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CambiarTitulo();
        }

        public virtual void CambiarTitulo()
        {
            TituloPantalla = "Comercial Colindres";
        }

        public ConfiguracionDialogoModel DialogSettings
        {
            get { return _dialogSetting; }
            set
            {
                if (_dialogSetting != value)
                {
                    _dialogSetting = value;
                    RaisePropertyChanged("DialogSettings");
                }
            }
        }
        private ConfiguracionDialogoModel _dialogSetting;

        public Visibility MostrarVentanaEsperaPrincipal
        {
            get { return _mostrarVentanaEsperaPrincipal; }
            set
            {
                if (_mostrarVentanaEsperaPrincipal != value)
                {
                    _mostrarVentanaEsperaPrincipal = value;
                    RaisePropertyChanged("MostrarVentanaEsperaPrincipal");
                }
            }
        }
        private Visibility _mostrarVentanaEsperaPrincipal = Visibility.Collapsed;

        public Visibility MostrarVentanaEsperaFlyOut
        {
            get { return _mostrarVentanaEsperaFlyOut; }
            set
            {
                if (_mostrarVentanaEsperaFlyOut != value)
                {
                    _mostrarVentanaEsperaFlyOut = value;
                    RaisePropertyChanged("MostrarVentanaEsperaFlyOut");
                }
            }
        }
        private Visibility _mostrarVentanaEsperaFlyOut = Visibility.Collapsed;

        public bool EnfocarControl
        {
            get { return _enfocarControl; }
            set
            {
                _enfocarControl = value;
                RaisePropertyChanged("EnfocarControl");
            }
        }
        private bool _enfocarControl;

        public string TituloPantalla
        {
            get { return _tituloPantalla; }
            set
            {
                if (_tituloPantalla != value)
                {
                    _tituloPantalla = value;
                    RaisePropertyChanged("TituloPantalla");
                }
            }
        }
        private string _tituloPantalla;


        public string SlidePanelIndice
        {
            get { return _slidePanelIndice; }
            set
            {

                _slidePanelIndice = value;
                RaisePropertyChanged("SlidePanelIndice");
            }
        }
        string _slidePanelIndice;

        public void CargarSlidePanel(string indice)
        {
            SlidePanelIndice = string.Empty;
            SlidePanelIndice = indice;
        }
    }
}
