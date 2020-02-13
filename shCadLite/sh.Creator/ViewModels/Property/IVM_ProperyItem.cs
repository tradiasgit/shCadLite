using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Property
{
    interface IVM_ProperyItem
    {
        string Label { get; set; }

        string Value { get; set; }

        string Category { get; set; }

        bool IsEditable { get; set; }
    }
}
