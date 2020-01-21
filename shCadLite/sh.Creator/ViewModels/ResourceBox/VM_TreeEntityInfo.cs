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
using System.Diagnostics;
using sh.Cad.Json;

namespace sh.Creator.ViewModels
{
    public class VM_TreeEntityInfo : VM_TreeEntityInfo<EntityInfo> { public VM_TreeEntityInfo(FileInfo file, EntityInfo info) : base(file,info) { } }

    public class VM_TreeEntityInfo<T> : VM_TreeItem where T: EntityInfo
    {
        public VM_TreeEntityInfo(FileInfo file, T info):base(file)
        {
            Model = info;
            var query = EntityQuery.Compute(info);
            CountText = query.Count.ToString();
            LengthText = string.Format("{0:f2}米", query.SumLength * 0.001);
            AreaText = string.Format("{0:f2}平米", query.SumArea * 0.000001);
        }

        public T Model { get { return GetValue<T>(); } set { SetValue(value); } }


        public string CountText { get { return GetValue<string>(); }set { SetValue(value); } }
        public string AreaText { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LengthText { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd_Brush
        {
            get {
                return RegisterCommand(p=>
                {
                    Model?.Brush();
                });
            }
        }
        
        public ICommand CmdRefreshQuery
        {
            get {
                return RegisterCommand(p=>
                {
                    var query =EntityQuery.Compute(Model);
                    CountText = query.Count.ToString();
                    LengthText =string.Format("{0:f2}米", query.SumLength*0.001);
                    AreaText = string.Format("{0:f2}平米", query.SumArea * 0.000001);
                });
            }
        }
    }
}
