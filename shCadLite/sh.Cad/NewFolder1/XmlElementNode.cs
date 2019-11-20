using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace sh.XmlResourcesParsing.XmlNodeViewModels
{
    public class XmlElementNode : NodeBase
    {
        public XmlElement Element { get; protected set; }

        public XmlElementNode(XmlElement ele)
        {
            Element = ele;
            Initialize();
        }
        public override bool Initialize()
        {
            if (Element != null)
            {
                foreach (XmlAttribute att in Element.Attributes)
                {
                    SetPropertyFromAttribute(this, Element, att.Name);
                }
                foreach (XmlNode oNode in Element.ChildNodes)
                {
                    if(oNode is XmlElement)
                    {
                        var obj = CreateNode(oNode as XmlElement);
                        if (obj != null)
                        {
                            AddChild(obj);
                        }
                    }
                    
                }
            }
            return true;
        }

        private static INode CreateNode(XmlElement ele)
        {
            switch (ele.Name)
            {
                case "Node":return new TreeNode(ele);
                //case "Field": return new Field(ele);
                //case "QueryField": return new Fields.QueryField(ele);
                //case "MultiField": return new Fields.MultiField(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
                //case "Node": return new TreeNode(ele);
            }



            var nsList = new List<string>();
            nsList.Add("sh.XmlResourcesParsing");
            nsList.Add("sh.XmlResourcesParsing.Models");
            nsList.Add("sh.XmlResourcesParsing.Fields");
            nsList.Add("sh.XmlResourcesParsing.XmlNodeViewModels");
            nsList.Add("sh.XmlResourcesParsing.Actions");
            nsList.Add("sh.XmlResourcesParsing.Nodes");
            foreach (var ns in nsList)
            {
                var type = Type.GetType($"{ns}.{ele.Name}");
                if (type != null)
                {
                    return Activator.CreateInstance(type, ele) as INode;
                }
            }
            return null;
        }

        public override Dictionary<string, string> GetData()
        {
            var result = new Dictionary<string, string>();
            if (Parent != null)
            {
                //var dic = Parent.GetData();
                //foreach (var kv in dic)
                //{
                //    result.Add(kv.Key, kv.Value);
                //}
            }
            if (Element != null)
            {
                var data = Element.SelectNodes("Data");
                if (data != null && data.Count > 0)
                {
                    foreach (XmlNode d in data)
                    {
                        var remove = d.Attributes["RemoveKey"];
                        if (remove != null)
                        {
                            result.Remove(remove.Value);
                        }
                        else
                        {
                            var k = d.Attributes["Key"].Value;
                            var v = d.Attributes["Value"].Value;
                            if (result.ContainsKey(k))
                                result[k] = v;
                            else
                                result.Add(k, v);
                        }

                    }
                }
            }
            return result;
        }
    }
}
