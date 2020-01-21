using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sh.Cad
{
    public class PolylineInfo:EntityInfo
    {
        public PolylineInfo() { }
        public PolylineInfo(Polyline ent, Transaction tr):base(ent,tr)
        {
        }
    }
}
