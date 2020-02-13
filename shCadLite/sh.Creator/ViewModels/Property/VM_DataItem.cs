using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Property
{
    class VM_DataItem:CadViewModelBase, IVM_ProperyItem
    {
        public VM_DataItem(EntityInfo entinfo, Models.EntityDataConfig p)
        {
            Key = p.Key;
            Model = entinfo;
            Label = p.Title;
            //Value = entinfo.GetDataValue(p.Key);
            Category = "数据";
            IsEditable = true;
        }

        public string Label { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Key { get; set; }

        public string Value { get { return GetValue<string>(); } set { Model.SetDataValue(Key, value);SetValue(value); } }

        public string Category { get; set; } = "默认";

        public bool IsEditable { get; set; }

        public EntityInfo Model { get; set; }
    }
}
