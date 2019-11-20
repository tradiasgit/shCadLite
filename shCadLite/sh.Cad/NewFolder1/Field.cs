using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.XmlResourcesParsing
{
    public class Field:XmlNodeViewModels.XmlElementNode
    {
        public Field(XmlElement ele) : base(ele)
        {
        }

        public double Value { get; set; }

        public string Format { get; set; } = "";

        public double Ratio { get; set; } = 1;        

        public virtual double GetValue() { return Value; }

        public virtual string GetText() { return string.Format(Format, GetValue() * Ratio); }
    }
}
