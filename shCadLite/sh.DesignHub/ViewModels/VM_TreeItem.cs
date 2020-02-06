
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace sh.DesignHub.ViewModels
{
    public class VM_TreeItem:ViewModelBase
    {
        public virtual string ItemType=> GetType().Name;


        public VM_TreeItem(FileSystemInfo info)
        {
            Name = info.Name;
            if (info.Attributes.HasFlag(FileAttributes.Hidden)) IsHidden = true;
        }
        public string Name { get { return GetValue<string>(); } set { SetValue(value); } }


        public static VM_TreeItem New_VM_TreeItem(FileSystemInfo info)
        {
            if (info is FileInfo) return new VM_TreeFile(info as FileInfo);
            else if (info is DirectoryInfo) return new VM_TreeFolder(info as DirectoryInfo);
            else throw new Exception($"不支持的info类型:({info.GetType().Name},{info.FullName})");
        }

        //public ICommand Cmd_Folder
        //{
        //    get
        //    {
        //        return RegisterCommand(p => Process.Start(info.DirectoryName));
        //    }
        //}

        public bool IsHidden { get { return GetValue<bool>(); } set { SetValue(value); } }
    }
}
