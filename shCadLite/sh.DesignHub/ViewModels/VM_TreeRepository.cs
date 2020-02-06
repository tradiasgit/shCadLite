using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.DesignHub.ViewModels
{
    class VM_TreeRepository:VM_TreeFolder
    {

        public override string ItemType => "Repo";
        public VM_TreeRepository(DirectoryInfo dir) : base(dir)
        {
            
        }
    }
}
