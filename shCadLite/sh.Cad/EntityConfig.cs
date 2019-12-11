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
        public string EntityTypeName { get; protected set; }

        public int ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public Dictionary<string, string> Data { get; protected set; }

        public HacthInfo Hatch { get; protected set; }

        public string BlockName { get; protected set; }


       


        


    }
}
