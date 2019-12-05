using Autodesk.AutoCAD.DatabaseServices;
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
    class VM_TreeCadBrush : VM_TreeItem
    {

        public string Value { get; set; }

        public VM_TreeCadBrush(FileInfo f)
        {
            var doc = new XmlDocument();
            doc.Load(f.FullName);
            Text = f.Name;
            Brush = new Cad.EntityConfig(f);
        }


        public ICommand Command
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Brush.Brush();
                });
            }
        }


        public sh.Cad.EntityConfig Brush { get; set; }

       
        
    }
}
