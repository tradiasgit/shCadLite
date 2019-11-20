using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{

    public class VM_EntityType : ViewModelBase
    {
        public VM_EntityType(string dxf) { DxfName = dxf;IsSelected = true; }
        public string DxfName { get { return GetValue<string>(); } set { SetValue(value); RaisePropertyChanged("Name"); } }

        public override string ToString()
        {
            switch (DxfName)
            {
                default: return DxfName;
                case "LWPOLYLINE": return "多段线";
                case "LINE": return "直线";
                case "CIRCLE": return "圆";
                case "INSERT": return "块参照";
                case "HATCH": return "填充";
                case "DIMENSION": return "标注";
                case "MTEXT": return "多行文字";
                case "MLINE": return "多行";
                case "ARC": return "圆弧";
                case "LEADER": return "引线";
                case "TEXT": return "文字";
                case "ATTDEF": return "属性定义";

            }
        }

        public bool IsSelected { get { return GetValue<bool>(); } set { SetValue(value); } }
    }
}
