using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public interface IEntitySelectionListener
    {
        void OnSelectionChanged(EntitySelection selection);
    }


   
}
