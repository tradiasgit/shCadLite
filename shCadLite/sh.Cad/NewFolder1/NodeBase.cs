
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sh.XmlResourcesParsing.XmlNodeViewModels
{
    public abstract class NodeBase : INode
    {
        public string Id { get; set; }

        private string _name;
        public string Name{ get { return GetExpressionValue(_name);} set { _name = value; }}

        public string Tooltips { get; set; }

        public string ParentName { get { if (Parent != null) return Parent.Name; else return null; } }

        protected bool IsExpression(string value) { return value!=null&& value.Contains("{") && value.Contains("}"); }

        protected string GetExpressionValue(string value)
        {
            return value;
            //if (IsExpression(_name))
            //{
            //    StringExpression exp = new StringExpression(value, this);
            //    value = exp.ToString();
            //    return value;
            //}
            //else return value;
        }
        public abstract bool Initialize();
        public INode Parent { get; set; }
        public bool IsExpanded { get; set; }
        public abstract Dictionary<string, string> GetData();

        private List<INode> _children = new List<INode>();
        public IEnumerable<INode> Children { get { return _children; } set { _children = value.ToList(); } }

        protected void AddChild(INode child)
        {
            if (child != null)
            {
                //child.Parent = this;
                _children.Add(child);
            }
        }




        protected static void SetPropertyFromAttribute(object target, XmlNode node, string name)
        {
            var att = node.Attributes[name];
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

        public string GetString([CallerMemberName] string name = null, bool readBase = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INode> GetNodeArray(string childrenFieldName)
        {
            throw new NotImplementedException();
        }
    }
}
