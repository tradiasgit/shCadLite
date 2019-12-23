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
using System.Windows;
using System.Xml;

namespace sh.Cad
{
    public class EntityConfig
    {
        public string EntityType { get;  set; }

        public Dictionary<string,EntityInfo> Prefabs { get; set; }
    }
}
