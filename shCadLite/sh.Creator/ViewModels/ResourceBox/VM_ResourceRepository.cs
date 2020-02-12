using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sh.Cad;
using System.Collections.ObjectModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Xml;
using Newtonsoft.Json;
using sh.ResourceRepository;

namespace sh.Creator.ViewModels
{
    public class VM_ResourceRepository : CadViewModelBase, IProcessHandler
    {
        public string Name { get; set; }

        DirectoryInfo Local;
        public VM_ResourceRepository(string name,DirectoryInfo dir)
        {
            Local = dir;
            Name = name;
            LoadStatus();
        }

        private void LoadStatus()
        {
            
        }

        public string Status { get { return GetValue<string>(); } set { SetValue(value); } }//Create,Clone,Sync


        public ObservableCollection<VM_TreeItem> ResourceTree { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }




        public ICommand Cmd_Refresh
        {
            get
            {
                return RegisterCommandAsync(async p =>
                {
                    SetBusy("正在加载...");
                    await Task.Delay(200);
                    Load();
                    ClearBusy();
                });
            }
        }


        private void Load()
        {
            if (Local.Exists)
            {
                ResourceTree = new ObservableCollection<VM_TreeItem>(LoadFromDir(Local));
            }
        }

        private IEnumerable<VM_TreeItem> LoadFromDir(DirectoryInfo dir)
        {
            var result = new List<VM_TreeItem>();
            if (dir.Exists)
            {
                foreach (var cd in dir.GetDirectories())
                {
                    if (cd.Name == "budgetsheet") continue;
                    else if (cd.Name == ".git") continue;
                    var vmcd = new VM_TreeFolder(cd) { Text = cd.Name };
                    vmcd.Children = new ObservableCollection<VM_TreeItem>(LoadFromDir(cd));
                    vmcd.IsExpanded = vmcd.Children.Count > 0;
                    result.Add(vmcd);
                }
                foreach (var cf in dir.GetFiles())
                {
                    if (cf.Extension.ToLower() == ".enf")
                    {
                        var text = File.ReadAllText(cf.FullName);
                        var info = EntityInfo.Get(cf);
                        if (info == null) continue;
                        else if (info is LayoutInfo) result.Add(new VM_TreeLayoutInfo(cf, info as LayoutInfo));
                        else if (info is BlockInfo) result.Add(new VM_TreeBlockInfo(cf, info as BlockInfo));
                        else if (info is HatchInfo) result.Add(new VM_TreeHatchInfo(cf, info as HatchInfo));
                        else if (info is PolylineInfo) result.Add(new VM_TreePolylineInfo(cf, info as PolylineInfo));
                        else if (info is EntityInfo) result.Add(new VM_TreeEntityInfo(cf, info as EntityInfo));
                    }
                }
            }
            return result;
        }



        public bool OnProgress(string message)
        {
            BusyMessage = message;
            WriteLine(message);
            return true;
        }






    }
}
