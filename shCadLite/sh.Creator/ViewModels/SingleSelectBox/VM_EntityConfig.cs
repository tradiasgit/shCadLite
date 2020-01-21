using Newtonsoft.Json;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.SingleSelectBox
{
    public class VM_EntityConfig : ViewModelBase<sh.Cad.EntityConfig>
    {
        public VM_EntityConfig(DirectoryInfo dir)
        {
            foreach (var file in dir.GetFiles())
            {
                if (file.Extension.ToLower() == ".enf")
                {
                    var info = EntityInfo.Get(file);
                    Prefabs.Add(new VM_EntityPrefab() { Model = info as EntityInfo, Key = file.Name });
                }
            }
        }

        public string Header { get { return $"预制配置：{Model?.ToString()}"; } }


        public List<VM_EntityPrefab> Prefabs { get; set; } = new List<VM_EntityPrefab>();
    }
}
