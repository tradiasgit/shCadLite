using Autodesk.AutoCAD.Runtime;
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
            try
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage($"【山河软件】创造者插件已加载({Assembly.GetExecutingAssembly().GetName().Version.ToString()},Location:{Assembly.GetExecutingAssembly().Location})" + Environment.NewLine);
                sh.Cad.EventManager.Start();
                CadCommands.Query();
            } 
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("【山河异常】" + ex.Message + Environment.NewLine);
            }
        }

        


      


        public void Terminate() { }
    }
}
