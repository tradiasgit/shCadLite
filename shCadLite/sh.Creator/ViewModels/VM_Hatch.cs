using Autodesk.AutoCAD.DatabaseServices;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_Hatch : ViewModelBase
    {
        public VM_Hatch(sh.Cad.HacthStyle style)
        {
            PatternAngle = style.PatternAngle;
            PatternDouble =style.PatternDouble ? "是" : "否";
            PatternName = style.PatternName;
            PatternScale = style.PatternScale;
            PatternSpace = style.PatternSpace;
            switch (style.PatternType) { case HatchPatternType.CustomDefined: PatternType = "自定义"; break; case HatchPatternType.PreDefined: PatternType = "预定义"; break; case HatchPatternType.UserDefined: PatternType = "用户定义"; break; }
            switch (style.HatchStyle) { case Autodesk.AutoCAD.DatabaseServices.HatchStyle.Normal: HatchStyle = "普通"; break; case Autodesk.AutoCAD.DatabaseServices.HatchStyle.Outer: HatchStyle = "外部"; break; case Autodesk.AutoCAD.DatabaseServices.HatchStyle.Ignore: HatchStyle = "忽略"; break; }
            Associative = style.Associative?"是":"否";
            OriginX = style.Origin.X;
            OriginY = style.Origin.Y;
        }
        public double PatternScale { get; set; } = 1;

        /// <summary>
        /// 填充角度
        /// </summary>
        public double PatternAngle { get; set; }

        /// <summary>
        /// Cad索引颜色，默认为7：白色，-1为ByLayer
        /// </summary>
        public short ColorIndex { get; set; } = 7;

        /// <summary>
        /// CAD预设样式名称
        /// </summary>
        public string PatternName { get; set; } = "SOLID";

        public string HatchStyle { get; set; }

        public string Associative { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string PatternType { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        public double PatternSpace { get; set; } = 1;

        /// <summary>
        /// 双向
        /// </summary>
        public string PatternDouble { get; set; }

        public double OriginX { get; set; }

        public double OriginY { get; set; }
    }
}
