using ComercialColindres.Clases;
using ComercialColindres.Enumeraciones;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace ComercialColindres.Comportamientos
{
    public class Seguridad : Behavior<FrameworkElement>
    {
        public Seguridad()
        {
        }

        public static readonly DependencyProperty LlaveSeguridadProperty = DependencyProperty.Register(
            "LlaveSeguridad", typeof(string), typeof(Seguridad), new PropertyMetadata(default(string)));

        public string LlaveSeguridad
        {
            get { return (string)GetValue(LlaveSeguridadProperty); }
            set { SetValue(LlaveSeguridadProperty, value); }
        }


        public TipoSeguridadPropiedad TipoSeguridadPropiedad
        {
            get { return (TipoSeguridadPropiedad)GetValue(TipoSeguridadPropiedadProperty); }
            set { SetValue(TipoSeguridadPropiedadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TipoSeguridadPropiedad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipoSeguridadPropiedadProperty =
            DependencyProperty.Register("TipoSeguridadPropiedad", typeof(TipoSeguridadPropiedad), typeof(Seguridad), new PropertyMetadata(TipoSeguridadPropiedad.Habilitado));

        protected override void OnAttached()
        {
            var control = AssociatedObject;
            control.Loaded += control_Loaded;
        }

        void control_Loaded(object sender, RoutedEventArgs e)
        {
            //if (TipoSeguridadPropiedad == TipoSeguridadPropiedad.Habilitado)
            //{
            //    AssociatedObject.IsEnabled = false;
            //}

            AssociatedObject.IsEnabled = false;

            if (TipoSeguridadPropiedad == TipoSeguridadPropiedad.Visible)
            {
                AssociatedObject.Visibility = Visibility.Collapsed;
            }

            var permiso =
                InformacionSistema.UsuarioActivo.UsuariosOpciones.FirstOrDefault(r => r.NombreOpcion == LlaveSeguridad);
            if (permiso != null)
            {
                if (permiso.TipoPropiedad == "HABILITADO" || string.IsNullOrWhiteSpace(permiso.TipoPropiedad))
                {
                    AssociatedObject.IsEnabled = true;
                }
                else
                {
                    AssociatedObject.IsEnabled = true;
                    AssociatedObject.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
