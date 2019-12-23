using Autodesk.AutoCAD.DatabaseServices;
using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_HatchBox : ViewModelBase,IEntitySelectionListener
    {
        public double PatternScale { get { return GetValue<double>(); } set { SetValue(value); } }
        /// <summary>
        /// 填充角度
        /// </summary>
        public double PatternAngle { get { return GetValue<double>(); } set { SetValue(value); } }

        /// <summary>
        /// Cad索引颜色，默认为7：白色，-1为ByLayer
        /// </summary>
        public short ColorIndex { get { return GetValue<short>(); } set { SetValue(value); } }

        /// <summary>
        /// CAD预设样式名称
        /// </summary>
        public string PatternName { get { return GetValue<string>(); } set { SetValue(value); } }

        public string HatchStyle { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Associative { get { return GetValue<string>(); } set { SetValue(value); } }

        /// <summary>
        /// 类型
        /// </summary>
        public string PatternType { get { return GetValue<string>(); } set { SetValue(value); } }

        /// <summary>
        /// 间隙
        /// </summary>
        public double PatternSpace { get { return GetValue<double>(); } set { SetValue(value); } }

        /// <summary>
        /// 双向
        /// </summary>
        public string PatternDouble { get { return GetValue<string>(); } set { SetValue(value); } }

        public double OriginX { get { return GetValue<double>(); } set { SetValue(value); } }

        public double OriginY { get { return GetValue<double>(); } set { SetValue(value); } }

        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection!=null&&selection.Count == 1)
            {
                var ent = selection.GetEntity<HatchInfo>() ;
                if (ent!=null)
                {
                    PatternAngle = ent.PatternAngle;
                    PatternDouble = ent.PatternDouble ? "是" : "否";
                    PatternName = ent.PatternName;
                    PatternScale = ent.PatternScale;
                    PatternSpace = ent.PatternSpace;
                    switch (ent.PatternType) { case "CustomDefined": PatternType = "自定义"; break; case "PreDefined": PatternType = "预定义"; break; case "UserDefined": PatternType = "用户定义"; break; }
                    switch (ent.HatchStyle) { case "Normal": HatchStyle = "普通"; break; case "Outer": HatchStyle = "外部"; break; case "Ignore": HatchStyle = "忽略"; break; }
                    Associative = ent.Associative ? "是" : "否";
                    IsVisible = true;
                }
            }
        }


       
    }
}
