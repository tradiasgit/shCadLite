using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Cad
{
    public class HacthStyle
    {
        public double PatternScale { get; set; } = 1;

        /// <summary>
        /// 填充角度
        /// </summary>
        public double PatternAngle { get; set; }

        /// <summary>
        /// CAD预设样式名称
        /// </summary>
        public string PatternName { get; set; } = "SOLID";

        public HatchStyle HatchStyle { get; set; } = HatchStyle.Normal;

        public bool Associative { get; set; } = false;

        /// <summary>
        /// 类型
        /// </summary>
        public HatchPatternType PatternType { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        public double PatternSpace { get; set; } = 1;

        /// <summary>
        /// 双向
        /// </summary>
        public bool PatternDouble { get; set; }

        public Point2d Origin { get; set; }
    }
}
