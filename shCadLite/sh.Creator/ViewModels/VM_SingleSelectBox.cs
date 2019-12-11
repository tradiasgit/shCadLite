using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_SingleSelectBox : ViewModelBase<EntityInfo>, sh.Cad.IEntitySelectionListener
    {

        public string AreaText { get { return Model?.Area != null ? string.Format("{0:f2}平米", 0.000001 * Model.Area) : "/"; } }
        public string LengthText { get { return Model?.Length != null ? string.Format("{0:f2}米", 0.001 * Model.Length) : "/"; } }
        public string EntityTypeText { get { return Model?.EntityTypeName; } }


        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection != null && selection.Count == 1)
            {
                var ent = selection.GetEntity();
                Model = ent;
                if (ent != null)
                {
                    if (Model.Data != null)
                    {
                        Data = new ObservableCollection<VM_Data>(Model.Data.Select(p => new VM_Data(p.Key, p.Value)).OrderBy(p => p.Key));
                        NewData = new VM_Data();
                    }
                    RaiseAllPropertyChanged();
                    IsVisible = true;
                }
            }
        }

        protected override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged("AreaText");
            RaisePropertyChanged("LengthText");
            RaisePropertyChanged("LengthVisibility");
            RaisePropertyChanged("AreaVisibility");
            RaisePropertyChanged("EntityTypeText");
        }


        public ICommand Cmd_Brush
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Model?.Brush();
                });
            }
        }
        public ICommand Cmd_PutBlock
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Model?.PutBlock();
                });
            }
        }


        public ICommand Cmd_SaveAs
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                    if (Convert.ToInt16(dwgtitled) == 0)
                    {
                        ShowMessage("请保存图纸。");
                        return;
                    }
                    var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    var db_source = HostApplicationServices.WorkingDatabase;
                    var dir = new FileInfo(db_source.Filename).Directory;
                    dir = new DirectoryInfo($@"{dir.FullName}\support");
                    dir.Create();
                    var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                    op_file.InitialDirectory = $@"{dir.FullName}"; ;
                    op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";

                    var result_file = ed.GetFileNameForSave(op_file);
                    if (result_file.Status == PromptStatus.OK)
                    {
                        if (Model.BlockName != null)
                        {
                            var file = new FileInfo(result_file.StringResult.Replace(".enf","_block.dwg"));
                            
                            DatabaseManager.ExportBlock(file,Model.BlockName);
                        }
                        File.WriteAllText(result_file.StringResult, JsonConvert.SerializeObject(Model));
                    }
                });
            }
        }



        #region Data


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
                    sh.Cad.DataManager.WriteDictionary(Model.EntityHandle, new Dictionary<string, string> { { NewData.Key, NewData.Value } });
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
                    sh.Cad.DataManager.RemoveKey(Model.EntityHandle, data.Key);
                    Data.Remove(data);
                });
            }
        }

        #endregion

    }
}
