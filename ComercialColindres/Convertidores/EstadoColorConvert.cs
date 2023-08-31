using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComercialColindres.Convertidores
{
    public class EstadoColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valor = (string)value;

            if (valor == "Pendiente")
            {
                return new SolidColorBrush(Colors.Red);
            }

            if (valor == "Cerrado")
            {
                return new SolidColorBrush(Colors.Green);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
