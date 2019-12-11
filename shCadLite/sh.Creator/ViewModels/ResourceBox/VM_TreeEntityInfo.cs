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
    public class VM_TreeEntityInfo : VM_TreeItem
    {
       protected  FileInfo File;
        public VM_TreeEntityInfo(FileInfo file,EntityInfo info)
        {
            Model = info;
            Text = file.Name;
            var query = new sh.Cad.EntityQuery(info);
            File = file;
            CountText = query.Count().ToString();
            LengthText =string.Format("{0:f2}米", query.SumLength()*0.001);
            AreaText = string.Format("{0:f2}平米", query.SumArea() * 0.000001);
        }

        public new EntityInfo Model { get { return GetValue<EntityInfo>(); } set { SetValue(value); } }


        public string CountText { get { return GetValue<string>(); }set { SetValue(value); } }
        public string AreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LengthText { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd_Brush
        {
            get {
                return CommandFactory.RegisterCommand(p=>
                {
                    Model?.Brush();
                });
            }
        }
    }
}
