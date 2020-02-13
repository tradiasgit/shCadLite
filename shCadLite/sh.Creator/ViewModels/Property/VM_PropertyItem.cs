using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Property
{
    class VM_PropertyItem:CadViewModelBase, IVM_ProperyItem
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public string Category { get; set; } = "默认";

        public bool IsEditable { get; set; }

    }
}
