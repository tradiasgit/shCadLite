
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace sh.DesignHub.ViewModels
{
    public class VM_TreeFolder : VM_TreeItem
    {
        public override string ItemType => "Folder";

        DirectoryInfo info;
        public VM_TreeFolder(DirectoryInfo dir):base(dir)
        {
            info = dir;
            Refresh();
        }

        private void Refresh()
        {
            Children = new ObservableCollection<VM_TreeItem>(info.GetFileSystemInfos().Select(p=>New_VM_TreeItem(p)));
        }

        

        public ObservableCollection<VM_TreeItem> Children { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }



        public bool IsExpanded { get { return GetValue<bool>(); } set { SetValue(value); } }

        
        //public string Icon { get { return "\xf07c"; } }

    }
}
