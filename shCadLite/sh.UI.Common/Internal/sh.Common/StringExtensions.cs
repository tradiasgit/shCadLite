using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal static class StringExtensions
    {
        public static FileInfo ExAsFile(this string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return new FileInfo(@"c:\不存在的文件.文件");
            return new FileInfo(path);
        }

       

        public static string ExToString_FileSize(this long size)
        {
            if (size < 1024) return $"{size}byte";
            else if (size < 1048576) return $"{((double)size) / 1024:f1}KB";
            else if (size < 1073741824) return $"{((double)size) / 1048576:f1}MB";
            else return $"{((double)size) / 1073741824:f1}GB";
        }

        public static string ExToString_Percent(this double value, string format = "f2")
        {
            if (value >= 0 && value <= 1) return (value * 100).ToString(format) + "%";
            else if (value > 1 && value <= 100) return value.ToString(format) + "%";
            else return "未能识别的数字格式：值=" + value;
        }
    }
}
