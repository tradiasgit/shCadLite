using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_SingleSelectBox : ViewModelBase
    {
        public VM_SingleSelectBox(EntityInfo ent)
        {
            Info = ent;
        }

        public EntityInfo Info { get; private set; }

        public string AreaText
        {
            get {return string.Format("{0:f2}平米", 0.000001 *Info.GetArea()); }
        }
        public string LengthText
        {
            get { return string.Format("{0:f2}米", 0.001 * Info.GetLength()); }
        }
        public string LayerName { get { return Info.LayerName; } }

        public string TypeName
        {
            get
            {
                switch (Info.DxfName)
                {
                    default: return Info.DxfName;
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
        }

        




        






    }
}
