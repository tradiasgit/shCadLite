using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;

namespace sh.Creator
{
    public static class CadCommands
    {
        [CommandMethod("query", CommandFlags.UsePickSet)]
        public static void Query()
        {
            try
            {
                var psConfig = new Cad.PaletteSetConfig();
                psConfig.Name = "sh编辑器";
                psConfig.Width = 320;
                psConfig.Height = 640;
                psConfig.PaletteConfigs = new List<Cad.PaletteConfig>
                {
                    Views.shQueryEditor.Instance.GetPaletteConfig(),
                    Views.shResourceBox.Instance.GetPaletteConfig()
                };
                var ps = new sh.Cad.PaletteSetManager();
                ps.ShowPaletteSet(psConfig);

            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog(ex.Message);

            }
        }






    }

}
