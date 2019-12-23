using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace update
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);

            Console.Write($"按任意键开始更新"); Console.ReadKey(); Console.Write($"{Environment.NewLine}");
            var settingfile = new FileInfo(dir + @"\update.json");
            if (settingfile.Exists)
            {
                try
                {
                    var package = JsonConvert.DeserializeObject<FilePackage>(File.ReadAllText(settingfile.FullName));
                    foreach (var f in package.Files)
                    {
                        Console.Write($"Downloading ................{f}");
                        package.Download(dir, f);
                        Console.Write($"succuss{Environment.NewLine}");
                    }
                    Console.Write($"完成{Environment.NewLine}");

                }
                catch (Exception ex)
                {
                    Console.Write($"failed:{ex.Message}{Environment.NewLine}");
                }
                finally
                {
                    Console.Write($"按任意键退出"); Console.ReadKey();
                }
            }

        }
    }
}
