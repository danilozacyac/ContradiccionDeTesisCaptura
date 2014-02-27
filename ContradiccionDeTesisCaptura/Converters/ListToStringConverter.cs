using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace ContradiccionDeTesisCaptura.Converters
{
    public class ListToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                List<int> lista = (List<int>)value;

                return string.Join(",", lista.ToArray());
            }
            catch (Exception)
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}