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
    public class VM_MultiSelectBox : ViewModelBase
    {
        public VM_MultiSelectBox(IEnumerable<EntityInfo> ents)
        {
            Selection = ents;
        }

        public IEnumerable<EntityInfo> Selection { get; private set; }



        public int Count { get { return Selection.Count(); } }
        public string AreaText
        {
            get {return string.Format("{0:f2}平米", 0.000001 *(Selection.Sum(i=>i.GetArea()))); }
        }
        public string LengthText
        {
            get { return string.Format("{0:f2}米", 0.001 * (Selection.Sum(i => i.GetLength()))); }
        }


    }
}
