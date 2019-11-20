using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.XmlResourcesParsing.XmlNodeViewModels
{
    public class TreeNode:XmlElementNode
    {

        public TreeNode(XmlElement ele) : base(ele) { }
        public IEnumerable<TreeNode> ChildNodes { get { return Children.Where(p=>p is TreeNode).Cast<TreeNode>(); } }
        public IEnumerable<ActionNode> ActionNodes { get { return Children.Where(p => p is ActionNode).Cast<ActionNode>(); } }
        public IEnumerable<Field> FieldNodes { get { return Children.Where(p => p is Field).Cast<Field>(); } }

   
    }
}
