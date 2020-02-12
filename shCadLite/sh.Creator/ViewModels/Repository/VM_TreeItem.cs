using sh.Cad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Repository
{
    class VM_TreeItem: CadViewModelBase
    {
        public string Text { get { return GetValue<string>(); } set { SetValue(value); } }


        public virtual void OnSelect()
        {
            
        }
    }
}
