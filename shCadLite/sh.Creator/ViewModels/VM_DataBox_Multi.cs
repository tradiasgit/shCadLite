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
    public class VM_DataBox_Multi : ViewModelBase
    {
        public VM_DataBox_Multi()
        {
            NewData = new VM_Data();
            Data = new ObservableCollection<VM_Data>();
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
                    Data.Remove(data);
                });
            }
        }
       
       


     

        public ICommand Cmd_SaveData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    if (data.IsNew)
                    {
                        var doc = Application.DocumentManager.MdiActiveDocument;
                        var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                        var db_source = HostApplicationServices.WorkingDatabase;
                        using (var l = doc.LockDocument())
                        {
                            var op_ent = new PromptSelectionOptions();
                            var result_ent = ed.GetSelection(op_ent);

                            if (result_ent.Status == PromptStatus.OK)
                            {
                                var ids = result_ent.Value.GetObjectIds();
                                foreach (var oid in ids)
                                {
                                    sh.Cad.DataManager.WriteDictionary(oid, new Dictionary<string, string> { { data.Key, data.Value } });
                                }
                            }
                            ed.SetImpliedSelection(result_ent.Value);
                        }
                    }
                    data.IsNew = false;
                });
            }
        }

        public ICommand Cmd_DeleteData
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var data = p as VM_Data;
                    var doc = Application.DocumentManager.MdiActiveDocument;
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    using (var l = doc.LockDocument())
                    {
                        var op_ent = new PromptSelectionOptions();
                        var result_ent = ed.GetSelection(op_ent);

                        if (result_ent.Status == PromptStatus.OK)
                        {
                            var ids = result_ent.Value.GetObjectIds();
                            foreach (var oid in ids)
                            {
                                sh.Cad.DataManager.RemoveKey(oid, data.Key);
                            }
                        }
                        ed.SetImpliedSelection(result_ent.Value);
                    }
                });
            }
        }

    }
}
