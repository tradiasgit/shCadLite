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
using Autodesk.AutoCAD.Windows;

namespace sh.Creator
{
    public static class CadCommands
    {
        private static PaletteSet _paletteSet;

        [CommandMethod("SHANHE", CommandFlags.UsePickSet)]
        public static void Query()
        {
            try
            {
                if (_paletteSet == null)
                {
                    var psConfig = new Cad.PaletteSetConfig();
                    psConfig.Name = "sh编辑器";
                    psConfig.Width = 320;
                    psConfig.Height = 640;
                    psConfig.PaletteConfigs = new List<Cad.PaletteConfig>
                    {
                        new Views.shQueryEditor().GetPaletteConfig(),
                        new Views.shResourceBox().GetPaletteConfig()
                    };
                    var ps = new sh.Cad.PaletteSetManager();
                    _paletteSet = ps.ShowPaletteSet(psConfig);
                }
                else if(!_paletteSet.Visible)
                {
                    _paletteSet.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog(ex.Message);

            }
        }






    }

}
