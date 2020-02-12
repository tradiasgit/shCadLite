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
    public static class ToolBoxCommand
    {

        [CommandMethod("SHANHE", CommandFlags.UsePickSet)]
        public static void Query()
        {
            try
            {
                //    psConfig.Width = 320;
                //    psConfig.Height = 640;
                
                var ps1= new Views.Repository.PS_Repository();
                ps1.ShowInPalette();

                var ps2 = new Views.Property.PS_Property();
                ps2.ShowInPalette();


            }
            catch (System.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog(ex.Message);
            }
        }

       
    }

}
