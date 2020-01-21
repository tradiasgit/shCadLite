using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_TreeLayoutInfo:VM_TreeItem
    {
        public VM_TreeLayoutInfo(FileInfo file, LayoutInfo info):base(file)
        {
            Model = info;
        }
        public LayoutInfo Model { get { return GetValue<LayoutInfo>(); } set { SetValue(value); } }

        public ICommand Cmd_Import
        {
            get
            {
                return RegisterCommand(p=>
                {
                    var dwgfile = new FileInfo(ConfigFile.FullName.Replace(ConfigFile.Name, Model.DwgFileName));
                    if (!dwgfile.Exists) throw new FileNotFoundException("图纸不存在", dwgfile.FullName);
                    var basepoint = Model.BasePoint.ToPoint3d();
                    var point = sh.Cad.DatabaseManager.CopyAllEntity(dwgfile, basepoint);
                    if (point == null) return;
                    db.ExImportLayout(dwgfile, Model.LayoutName, point.Value- basepoint);
                });
            }
        }
    }
}
