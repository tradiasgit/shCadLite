using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    class VM_TreeCadPart: VM_TreeItem
    {

        public VM_TreeCadPart(FileInfo f)
        {
            File = f;
            Text = File.Name;
        }
        public FileInfo File { get; set; }

        public ICommand Command
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var node = new sh.XmlResourcesParsing.Models.CopyAllEntity(null);
                    node.SourceFileName = File.Name;
                    node.Execute();
                });
            }
        }
    }
}
