using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.XmlResourcesParsing.XmlNodeViewModels
{
    public interface INode
    {

        string Name { get; }

        bool IsExpanded { get; }

        INode Parent { get; set; }

        IEnumerable<INode> Children { get; }

        bool Initialize();

        Dictionary<string, string> GetData();
    }
}
