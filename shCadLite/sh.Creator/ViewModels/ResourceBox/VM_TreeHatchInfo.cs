using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using sh.Cad;

namespace sh.Creator.ViewModels
{
    public class VM_TreeHatchInfo : VM_TreeEntityInfo<HatchInfo>
    {
        public VM_TreeHatchInfo(FileInfo file, HatchInfo info) : base(file, info) { }

    }
}
