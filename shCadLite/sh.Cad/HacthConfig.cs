using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.Cad
{
    public class HacthConfig
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

        //public Point2d Origin { get; set; }

        public XmlElement ToXml(XmlDocument doc)
        {
            var result = doc.CreateElement("Hatch");
            result.SetAttribute("PatternScale", PatternScale.ToString());
            result.SetAttribute("PatternAngle", PatternAngle.ToString());
            result.SetAttribute("PatternName", PatternName.ToString());
            result.SetAttribute("HatchStyle", HatchStyle.ToString());
            result.SetAttribute("Associative", Associative.ToString());
            result.SetAttribute("PatternType", PatternType.ToString());
            result.SetAttribute("PatternSpace", PatternSpace.ToString());
            result.SetAttribute("PatternDouble", PatternDouble.ToString());
            return result;
        }
        public HacthConfig()
        { }
        public HacthConfig(XmlElement ele)
        {
            SetPropertyFromAttribute(this, ele, "PatternScale");
            SetPropertyFromAttribute(this, ele, "PatternAngle");
            SetPropertyFromAttribute(this, ele, "PatternName");
            SetPropertyFromAttribute(this, ele, "HatchStyle");
            SetPropertyFromAttribute(this, ele, "Associative");
            SetPropertyFromAttribute(this, ele, "PatternType");
            SetPropertyFromAttribute(this, ele, "PatternSpace");
            SetPropertyFromAttribute(this, ele, "PatternDouble");
        }


        public void SetHatch(Hatch h)
        {
            var type = HatchPatternType.PreDefined;
            Enum.TryParse(PatternType, out type);
            h.SetHatchPattern(type, PatternName);
            h.PatternScale = PatternScale;
            h.PatternAngle = PatternAngle;
            h.HatchStyle = Autodesk.AutoCAD.DatabaseServices.HatchStyle.Normal;
            if (h.PatternType == HatchPatternType.UserDefined)
            {
                h.PatternSpace = PatternSpace;
                h.PatternDouble = PatternDouble;
            }
        }


        protected static void SetPropertyFromAttribute(object target, XmlElement ele, string name)
        {
            var att = ele.Attributes[name];
            if (att != null)
            {
                var prop = target.GetType().GetProperty(name);
                if (prop != null && prop.SetMethod != null)
                {
                    string value = att.Value;

                    object obj = null;
                    if (prop.PropertyType == typeof(string))
                        obj = value;
                    else if (prop.PropertyType == typeof(Guid))
                    {
                        Guid v = Guid.Empty;
                        if (Guid.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        int v = 0;
                        if (int.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(short))
                    {
                        short v = 0;
                        if (short.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        double v = 0;
                        if (double.TryParse(value, out v))
                            obj = v;
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        bool v = false;
                        if (bool.TryParse(value, out v))
                            obj = v;
                    }
                    if (obj != null)
                        prop.SetValue(target, obj);
                }

            }
        }
    }
}
