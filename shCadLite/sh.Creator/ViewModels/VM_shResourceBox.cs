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

namespace sh.Creator.ViewModels
{
    public class VM_shResourceBox : ViewModelBase, IEntitySelectionListener
    {

        public VM_shResourceBox()
        {
            IsVisible = true;
        }

        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }
        public ObservableCollection<VM_TreeItem> ResourceTree { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }



        private void Load()
        {
            var db_source = HostApplicationServices.WorkingDatabase;
            var dir = new FileInfo(db_source.OriginalFileName).Directory;
            dir = new DirectoryInfo($@"{dir.FullName}\support");
            ResourceTree = new ObservableCollection<VM_TreeItem>(LoadFromDir(dir));
        }

        private IEnumerable<VM_TreeItem> LoadFromDir(DirectoryInfo dir)
        {
            var result = new List<VM_TreeItem>();
            if (dir.Exists)
            {
                foreach (var cd in dir.GetDirectories())
                {
                    var vmcd = new VM_TreeFolder() { Text = cd.Name };
                    vmcd.Children = new ObservableCollection<VM_TreeItem>(LoadFromDir(cd));
                    vmcd.IsExpanded = vmcd.Children.Count > 0;
                    result.Add(vmcd);
                }
                foreach (var cf in dir.GetFiles())
                {
                    if (cf.Extension.ToLower() == ".ecx")
                    {
                        var doc = new XmlDocument();
                        doc.Load(cf.FullName);
                        if (doc.DocumentElement.Name == "EntityConfig")
                        {
                            var vmcf = new VM_TreeCadBrush(cf);
                            result.Add(vmcf);
                        }
                    }
                    else if (cf.Extension.ToLower() == ".dwg" && !cf.Name.ToLower().EndsWith("_recover.dwg"))
                    {
                        var vmcf = new VM_TreeCadPart(cf);
                        result.Add(vmcf);
                    }
                }
            }
            return result;
        }


        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection == null || selection.Count == 0)
            {
                IsVisible = true;
            }
        }
  
        public ICommand Cmd_Refresh
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Load();
                });
            }
        }

    }
}
