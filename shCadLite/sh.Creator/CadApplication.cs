using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

[assembly: ExtensionApplication(typeof(sh.Creator.CadApplication))]//注册插件
namespace sh.Creator
{
    public class CadApplication : IExtensionApplication
    {


        public void Initialize()
        {
            // 加载DLL, Cad不能加载引用的引用
            var dllPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); ;
            if(File.Exists(dllPath + "\\System.Windows.Interactivity.dll"))
                Assembly.LoadFrom(dllPath + "\\System.Windows.Interactivity.dll");

            try
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"【山河软件】创造者插件已加载({Assembly.GetExecutingAssembly().GetName().Version.ToString()},Location:{Assembly.GetExecutingAssembly().Location})" + Environment.NewLine);
                sh.Cad.EventManager.Start();
                ToolBoxCommand.Query();
            } 
            catch (System.Exception ex)
            {
               Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("【山河异常】" + ex.Message + Environment.NewLine);
            }
        }

        


      


        public void Terminate() { }
    }
}
