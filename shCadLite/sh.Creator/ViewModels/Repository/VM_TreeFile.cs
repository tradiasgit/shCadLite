using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.Repository
{
    class VM_TreeFile : VM_TreeItem
    {
        public FileInfo File { get; set; }
        public VM_TreeFile(FileInfo file)
        {
            File = file;
            Text = file.Name;
        }


    }
}
