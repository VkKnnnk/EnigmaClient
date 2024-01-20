using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Enigma_Client_V2.Converters
{
    class SessionTimeLeftConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null)
            {
                DateTime endTime = (DateTime)value;
                return Math.Round((endTime.TimeOfDay.TotalSeconds - DateTime.Now.TimeOfDay.TotalSeconds) / 60, 0);
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
