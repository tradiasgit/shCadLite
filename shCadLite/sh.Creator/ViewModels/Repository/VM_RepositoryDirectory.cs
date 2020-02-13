using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Repository
{
    class VM_RepositoryDirectory:CadViewModelBase
    {
        public VM_RepositoryDirectory(string name, string dir)
        {
            Name = name;
            Directory =new DirectoryInfo( dir);
        }

        public string Name { get { return GetValue<string>(); } set { SetValue(value); } }

        public DirectoryInfo Directory { get { return GetValue<DirectoryInfo>(); } set { SetValue(value); } }


        public ObservableCollection<VM_TreeItem> TreeRoot { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }


        public void LoadTree()
        {
            var root = new VM_TreeDirectory(Directory);
            root.IsExpanded = true;
            TreeRoot = new ObservableCollection<VM_TreeItem>();
            TreeRoot.Add(root);
        }

    }
}
