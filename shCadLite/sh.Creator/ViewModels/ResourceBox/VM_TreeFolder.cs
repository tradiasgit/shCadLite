using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    class VM_TreeFolder : VM_TreeItem
    {
        public ObservableCollection<VM_TreeItem> Children { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }

        public bool IsExpanded { get { return GetValue<bool>(); } set { SetValue(value); } }


    }
}
