using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_Layer:ViewModelBase
    {
        public VM_Layer(string name) { LayerName = name;IsSelected = true; }

        public string LayerName { get { return GetValue<string>(); }set { SetValue(value); } }

        public bool IsSelected { get { return GetValue<bool>(); } set { SetValue(value); } }

        public override string ToString()
        {
            return LayerName;
        }
    }
}
