using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionDeTesisCaptura.Converters
{
    public class OrganismosConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                String organismo = (from n in OrganismosSingleton.Colegiados
                        where n.IdOrganismo == (Int32)value
                        select n.Organismo).ToList()[0];

                return organismo;
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
