using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Enigma_Client_V2.Converters
{
    class VideoPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string current_directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var path = Path.Combine(Path.Combine(Path.Combine(current_directory, "Resources", "Images")),"AppDesign");
            return Path.Combine(path, "background_vid.wmv");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
