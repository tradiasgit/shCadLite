using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class EntityGroupInfo : IEntityInfo
    {
        public string EntityTypeName { get; set; }
        public string DwgFileName { get; set; }

        public PointInfo BasePoint { get; set; }
    }
}
