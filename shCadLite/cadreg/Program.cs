using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadreg
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("正在注册cad插件[sh.Creator.dll]");
            var str = @"shcadlite\sh.Creator.dll";

            try
            {
                var dir = new DirectoryInfo(Environment.CurrentDirectory);
                var file = new FileInfo($@"{dir.FullName}\{str}");
                if (file.Exists)
                {

                    var key16 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Autodesk\AutoCAD\R20.1\ACAD-F001:804\Applications\shanhesoftware";
                    var key18 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Autodesk\AutoCAD\R22.0\ACAD-1001:804\Applications\shanhesoftware";

                    WriteFile(new FileInfo($@"{dir.FullName}\regcad2016.reg"), file, key16);
                    WriteFile(new FileInfo($@"{dir.FullName}\regcad2018.reg"), file, key18);
                    Console.WriteLine($"注册表文件已生成请按版本导入注册表regcad[v].reg");
                    Console.ReadKey();
                }
                else throw new Exception("没有找到插件文件");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误：{ex.Message}");
                Console.ReadKey();
            }
        }



        private static void WriteFile(FileInfo file,FileInfo plugin,string key)
        {
            List<string> lines = new List<string>()
            {
                "Windows Registry Editor Version 5.00",
                "",
                $"[-{key}]",
                $"[{key}]",
                "\"DESCRIPTION\"=\"shanhesoftware\"",
                "\"LOADCTRLS\"=dword:0000000e",
                "\"MANAGED\"=dword:00000001",
                "\"LOADER\"=\""+plugin.FullName.Replace("\\","\\\\")+"\"",
                ""
            };
            File.WriteAllLines(file.FullName, lines);

        }
    }
}
