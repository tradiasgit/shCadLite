using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.SingleSelectBox
{
    public class VM_EntityProperty:ViewModelBase
    {
        protected Models.EntityPropertyConfig _config;
        protected EntityInfo _info;
        public VM_EntityProperty(Models.EntityPropertyConfig config,EntityInfo info)
        {
            _config = config;
            _info = info;
            Title = _config.Title;
            Value = _info.GetType().GetProperty(_config.ProperyName)?.GetValue(_info)?.ToString();
        }

        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Value { get { return GetValue<string>(); } set { SetValue(value); } }
    }

    public class VM_EntityProperty_BlockName : VM_EntityProperty
    {
        public VM_EntityProperty_BlockName(Models.EntityPropertyConfig config, EntityInfo info):base(config, info)
        {
            var br = _info as BlockInfo;
            Title = br.IsDynamicBlock ? "动态块":_config.Title;
            Value = _info.GetType().GetProperty(_config.ProperyName)?.GetValue(_info)?.ToString();
        }
    }
}
