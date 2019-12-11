using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sh.Creator.Views
{
    public class EntityTypeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (string)value;
            switch (type)
            {
                default: return type;
                case "Polyline": return "多段线";
                case "Line": return "直线";
                case "Circle": return "圆";
                case "Insert": return "块参照";
                case "BlockReference": return "块参照";
                case "Hatch": return "填充";
                case "Dimension": return "标注";
                case "MText": return "多行文字";
                case "MLine": return "多行";
                case "Arc": return "圆弧";
                case "Leader": return "引线";
                case "Text": return "文字";
                case "Attdef": return "属性定义";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
