using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_Data: ViewModelBase
    {
        public VM_Data()
        {
            IsSelected = true;
        }
        public VM_Data(string k, string v):this()
        {
            Key = k;Value = v;
        }

        public string Key { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Value { get { return GetValue<string>(); } set { SetValue(value); } }

        public bool IsSelected { get { return GetValue<bool>(); } set { SetValue(value); } }

        public bool IsCommon { get; set; }

        public bool IsNew { get { return GetValue<bool>(); } set { SetValue(value); } }

        public override string ToString()
        {
            return $"【{Key}】{Value}";
        }
    }
}
