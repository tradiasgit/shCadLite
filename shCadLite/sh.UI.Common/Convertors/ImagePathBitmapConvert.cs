using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace sh.UI.Common
{
    public class ImagePathBitmapConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var directoryName = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;

            var path = $@"{directoryName}\Resources\Images\defaultImage.png";
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                if (File.Exists(value.ToString()))
                {
                    path = value.ToString();
                }
            }

            // Read byte[] from png file
            BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));
            FileInfo fileInfo = new FileInfo(path);
            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();

            // Init bitmap
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
