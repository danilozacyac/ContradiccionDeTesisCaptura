using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace ContradiccionDeTesisCaptura.Converters
{
    class FinalStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isFinish = System.Convert.ToBoolean(value);

            if (!isFinish)
                return new SolidColorBrush(Colors.White);
            else 
                return new SolidColorBrush(Colors.Orange);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
           
        }
    }
}
