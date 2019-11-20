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
    public class VM_CadPart:ViewModelBase
    {

        public VM_CadPart(FileInfo f)
        {
            File = f;
        }
        public FileInfo File { get; set; }

        public override string ToString()
        {
            return File.Name;
        }



        public ICommand Cmd_Import
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
