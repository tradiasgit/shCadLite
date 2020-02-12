using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Repository
{
    class VM_TreeDirectory:VM_TreeItem
    {
        public VM_TreeDirectory(DirectoryInfo dir)
        {
            if (!dir.Exists) return;
            Text = dir.Name;
            Children = new ObservableCollection<VM_TreeItem>();
            foreach (var cd in dir.GetDirectories())
            {
                if (cd.Name == "budgetsheet") continue;
                else if (cd.Name == ".git") continue;
                var vmcd = new VM_TreeDirectory(cd);
                Children.Add(vmcd);
            }
            foreach (var cf in dir.GetFiles())
            {
                var vmcf = new VM_TreeFile(cf);
                Children.Add(vmcf);
            }
            //IsExpanded = Children.Count > 0;
        }


        public bool IsExpanded { get { return GetValue<bool>(); } set { SetValue(value); } }

        public ObservableCollection<VM_TreeItem> Children { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }
    }
}
