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


namespace sh.Creator.Cad
{
    public abstract class EntityInfo
    {
        public string EntityTypeName { get; set; }

        public int ColorIndex { get; set; } = 256;

        public string LayerName { get; set; }

        public Dictionary<string, string> Data { get; set; }

        [JsonIgnore]
        public string EntityHandle { get; set; }

        [JsonIgnore]
        public double? Area { get; set; }

        [JsonIgnore]
        public double? Length { get; set; }
        [JsonIgnore]
        public bool IsFromDocument { get { return !String.IsNullOrWhiteSpace(EntityHandle); } }


        #region Block
        public string BlockName { get; set; }

        public string BlockVisibilityName { get; set; }

        public bool IsDynamicBlock { get; set; }


        #endregion

        #region Hatch

        public double PatternScale { get; set; } = 1;

        /// <summary>
        /// 填充角度
        /// </summary>
        public double PatternAngle { get; set; }

        /// <summary>
        /// CAD预设样式名称
        /// </summary>
        public string PatternName { get; set; } = "SOLID";

        public string HatchStyle { get; set; } = "Normal";

        public bool Associative { get; set; } = false;

        /// <summary>
        /// 类型
        /// </summary>
        public string PatternType { get; set; } = "PreDefined";

        /// <summary>
        /// 间隙
        /// </summary>
        public double PatternSpace { get; set; } = 1;

        /// <summary>
        /// 双向
        /// </summary>
        public bool PatternDouble { get; set; }

        #endregion

        public string LayoutName { get; set; }

        public string DwgFileName { get; set; }


        public string GetDataValue(string key)
        {
            if (Data == null || !Data.ContainsKey(key)) return null;
            else return Data[key];
        }
        public abstract void SetDataValue(string key, string value);
    }
}
