using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using sh.Cad;

namespace sh.Creator.ViewModels
{
    class VM_TreeEntityInfo : VM_TreeItem
    {

        public VM_TreeEntityInfo(string text,EntityInfo info)
        {
            Model = info;
            Text = text;
        }

        public new EntityInfo Model { get { return GetValue<EntityInfo>(); } set { SetValue(value); } }


        public ICommand Command
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    //Brush.Brush();
                });
            }
        }

        public ICommand Cmd_Brush
        {
            get {
                return CommandFactory.RegisterCommand(p=>
                {
                    Model?.Brush();
                });
            }
        }
    }
}
