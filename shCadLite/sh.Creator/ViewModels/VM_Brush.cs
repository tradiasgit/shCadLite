using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_Brush:ViewModelBase
    {
        public string Title { get; set; }

        public string Value { get; set; }

        public VM_Brush(FileInfo f)
        {
            var doc = new XmlDocument();
            doc.Load(f.FullName);
            Brush = new XmlResourcesParsing.Models.BrushAction(doc.DocumentElement);
            Title = f.Name;

        }


        public ICommand Cmd_Brush
        {
            get
            {
                return CommandFactory.RegisterCommand(p=>
                {
                    Brush.Execute();
                });
            }
        }


        public XmlResourcesParsing.Models.BrushAction Brush { get; set; }
    }
}
