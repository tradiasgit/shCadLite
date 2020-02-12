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
        private FileInfo _file;
        public VM_TreeFile(FileInfo file)
        {
            _file = file;
            Text = file.Name;
        }

        public override void OnSelect()
        {
            Property.VM_PropertyPalette.Inctance.OnSelectedTreeItemChanged(_file);
        }

    }
}
