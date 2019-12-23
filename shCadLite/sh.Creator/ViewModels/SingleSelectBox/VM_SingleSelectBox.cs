using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using sh.Cad;
using sh.Creator.ViewModels.SingleSelectBox;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_SingleSelectBox : ViewModelBase<EntityInfo>, IEntitySelectionListener
    {

        public string AreaText { get { return Model?.Area != null ? string.Format("{0:f2}平米", 0.000001 * Model.Area) : "/"; } }
        public string LengthText { get { return Model?.Length != null ? string.Format("{0:f2}米", 0.001 * Model.Length) : "/"; } }
        public string EntityTypeText { get { return Model?.EntityTypeName; } }


        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }


        public VM_EntityConfig EntityConfig { get { return GetValue<VM_EntityConfig>(); } set { SetValue(value); } }

        public ObservableCollection<VM_EntityProperty> EntityProperties { get { return GetValue<ObservableCollection<VM_EntityProperty>>(); } set { SetValue(value); } }

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection != null && selection.Count == 1)
            {
                var ent = selection.GetEntity();
                Model = ent;
                if (ent != null)
                {
                    var dir = new DirectoryInfo($@"{new FileInfo(Assembly.GetExecutingAssembly().Location).Directory}\assets\entityconfigs\{ent.EntityTypeName}");
                    if (dir.Exists)
                    {
                        EntityConfig = new VM_EntityConfig(dir);
                    }
                    else { EntityConfig = null; }
                    if (Model.Data != null)
                    {
                        Data = new ObservableCollection<VM_Data>(Model.Data.Select(p => new VM_Data(p.Key, p.Value)).OrderBy(p => p.Key));
                        NewData = new VM_Data();
                    }

                    EntityProperties = new ObservableCollection<VM_EntityProperty>();
                    LoadEntityProperies("Entity");
                    LoadEntityProperies(Model.EntityTypeName);

                    RaiseAllPropertyChanged();
                    IsVisible = true;
                }
            }
        }



        private void LoadEntityProperies(string entityTypeName)
        {
            var entconfig = Models.EntityConfig.Get(entityTypeName);
            if (entconfig != null)
            {
                foreach (var p in entconfig.PropertyConfigs)
                {
                    if (!string.IsNullOrWhiteSpace(p.ViewModelName))
                    {
                        EntityProperties.Add(Activator.CreateInstance(Type.GetType(p.ViewModelName), p, Model) as VM_EntityProperty);
                    }
                    else EntityProperties.Add(new VM_EntityProperty(p, Model));
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
                    Model?.Draw();
                });
            }
        }


        public ICommand Cmd_SaveAs
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Model?.SaveAs();
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
        public ICommand Cmd_RenameBlock
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (Model is BlockInfo)
                    {
                        var br = Model as BlockInfo;
                        sh.Creator.Views.SingleSelectBox.WD_RenameBlock wd = new Views.SingleSelectBox.WD_RenameBlock() { Title = "重命名块", Text = br.BlockName };
                        var r = Application.ShowModalWindow(wd);
                        if (r.HasValue && r.Value)
                        {
                            DatabaseManager.RenameBlock(br.BlockName, wd.Text);
                            br.BlockName = wd.Text;
                            RaisePropertyChanged("Model");
                        }

                    }
                });
            }
        }

        public ICommand Cmd_PutBlockTo0
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (Model is BlockInfo)
                    {
                        var br = Model as BlockInfo;
                        DatabaseManager.PutBlockTo0(br.BlockName);
                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();
                    }
                });
            }
        }


        public ICommand Cmd_SetBasePoint
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var db = HostApplicationServices.WorkingDatabase;
                    var doc = Application.DocumentManager.MdiActiveDocument;
                    var sel = doc.Editor.SelectImplied();
                    if (sel != null && sel.Status == PromptStatus.OK && sel.Value.Count == 1)
                    {
                        DatabaseManager.SetBlockBasePoint(sel.Value[0].ObjectId);
                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();
                    }
                });
            }
        }
    }
}
