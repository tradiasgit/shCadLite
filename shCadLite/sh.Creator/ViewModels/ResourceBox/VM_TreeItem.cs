using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace sh.Creator.ViewModels
{
    public class VM_TreeItem:CadViewModelBase
    {
        public FileInfo ConfigFile { get;private set; }

        public string Status { get { return GetValue<string>(); } set { SetValue(value); } }

        public VM_TreeItem(FileInfo file)
        {
            ConfigFile = file;
            if (ConfigFile.Exists)
            {
                Text = ConfigFile.Name;
            }
            Icon = UI.Common.Icons.File;
            IconBrush = Brushes.Black;
        }
        public string Text { get { return GetValue<string>(); } set { SetValue(value); } }

        public sh.UI.Common.Icons Icon { get { return GetValue<sh.UI.Common.Icons>(); } set { SetValue(value); } }

        public Brush IconBrush { get { return GetValue<Brush>(); } set { SetValue(value); } }

        protected virtual void Edit()
        {
            if (ConfigFile.Exists)
            {
                Process.Start(ConfigFile.FullName);
            }
        }
        
        public ICommand Cmd_Edit
        {
            get
            {
                return RegisterCommand(p => Edit());
            }
        }
        
        public ICommand Cmd_Folder
        {
            get
            {
                return RegisterCommand(p => Process.Start(ConfigFile.DirectoryName));
            }
        }
    }
}
