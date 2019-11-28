using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_DataBox:ViewModelBase
    {
        EntityInfo Info;

        public VM_DataBox(EntityInfo info)
        {
            Info = info;
            var d = info.GetData();
            Data = new ObservableCollection<VM_Data>(d.Select(p=>new VM_Data(p.Key,p.Value)).OrderBy(p=>p.Key));
            NewData = new VM_Data();
        }


        public ObservableCollection<VM_Data> Data { get { return GetValue<ObservableCollection<VM_Data>>(); } set { SetValue(value); } }

        public VM_Data NewData { get { return GetValue<VM_Data>(); } set { SetValue(value); } }

        public ICommand Cmd_AddData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    NewData.IsNew = true;
                    Data.Insert(0, NewData);
                    Info.WriteData(NewData.Key, NewData.Value);
                    NewData = new VM_Data();

                });
            }
        }

        public ICommand Cmd_RemoveData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    Info.RemoveData(data.Key);
                    Data.Remove(data);
                });
            }
        }


    }
}
