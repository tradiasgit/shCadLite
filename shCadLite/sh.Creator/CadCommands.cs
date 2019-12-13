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
using System.Windows;
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
                    psConfig.IsDock = true;
                    psConfig.PaletteConfigs = new Dictionary<string, FrameworkElement>
                    {
                        {"选择集",  new Views.shQueryEditor() },
                         //{"资源盒子",  new Views.shResourceBox() },
                    };
                    var ps = new sh.Cad.PaletteSetManager();

                    _paletteSet = ps.ShowPaletteSet(psConfig);
                }
                else if (!_paletteSet.Visible)
                {
                    _paletteSet.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog(ex.Message);
            }
        }

        [CommandMethod("YuanSuanBiao")]
        public static void YuSuanBiao()
        {
            try
            {
                //var vm = new sh.Creator.ViewModels.BudgetSheet.VM_BudgetSheet();
                //vm.Show(); 
                var vm = new sh.Creator.ViewModels.BudgetSheet.VM_BudgetMain();
                vm.Show();
            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog(ex.Message);
            }
        }
    }

}
