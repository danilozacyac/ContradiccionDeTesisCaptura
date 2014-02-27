using System;
using System.Linq;
using System.Windows.Data;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionDeTesisCaptura.Converters
{
    class TipoAsuntoConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                String tipoAsunto = (from n in TipoAsuntoSingleton.TipoAsunto
                                    where n.IdTipo == (Int32)value
                                    select n.Descripcion).ToList()[0];

                return tipoAsunto;
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