using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.Property
{
    class VM_Function : CadViewModelBase
    {
        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Command { get { return GetValue<ICommand>(); } set { SetValue(value); } }

        public object CommandParameter { get { return GetValue<object>(); } set { SetValue(value); } }
    }
}
