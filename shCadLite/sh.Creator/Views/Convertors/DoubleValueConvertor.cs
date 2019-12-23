using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sh.Creator.Views
{
    public class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v;
            if (value != null && double.TryParse(value.ToString(), out v))
            {
                if ((string)parameter == "米") return $"{(v * 0.001):f2}{parameter}";
                else if ((string)parameter == "平米")return  $"{(v*0.000001):f2}{parameter}";
                else return $"{v:f2}{parameter}";
            }
            return "/";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
