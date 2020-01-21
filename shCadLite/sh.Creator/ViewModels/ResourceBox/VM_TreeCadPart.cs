using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_TreeCadPart : VM_TreeItem
    {

        public VM_TreeCadPart(FileInfo f,EntityGroupInfo info):base(f)
        {
        }

        public EntityGroupInfo Model { get { return GetValue<EntityGroupInfo>(); }set { SetValue(value); } }

        public ICommand Cmd_ImportCadPart
        {
            get
            {
                return RegisterCommand(p =>
                {
                    var dwgfile = new FileInfo(ConfigFile.FullName.Replace(ConfigFile.Name,Model.DwgFileName));
                    if (!dwgfile.Exists) return;
                    sh.Cad.DatabaseManager.CopyAllEntity(ConfigFile, Model.BasePoint.ToPoint3d());
                });
            }
        }
    }
}
