using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_TreeItem:ViewModelBase
    {
        public string Text { get { return GetValue<string>(); } set { SetValue(value); } }

        
    }
}
