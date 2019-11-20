using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.XmlResourcesParsing.XmlNodeViewModels
{
    public abstract class ActionNode : XmlElementNode
    {
        public ActionNode(XmlElement ele) : base(ele) { }

        public abstract void Execute();
    }
}
