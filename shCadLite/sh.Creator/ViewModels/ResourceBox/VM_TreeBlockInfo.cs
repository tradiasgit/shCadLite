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
    public class VM_TreeBlockInfo : VM_TreeEntityInfo
    {
        
        public VM_TreeBlockInfo(FileInfo file,EntityInfo info):base(file,info)
        {
        }
        public ICommand Cmd_PutBlock
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (!DatabaseManager.HasBlock(Model.BlockName))
                    {
                        var blockfile = new FileInfo(File.FullName.Replace(".enf", "_block.dwg"));
                        if (!blockfile.Exists) throw new FileNotFoundException("文件不存在", blockfile.FullName);
                        if (!DatabaseManager.HasBlock(blockfile, Model.BlockName)) throw new Exception($"文件中不包含指定的块：【{Model.BlockName}】{blockfile.FullName}");
                        DatabaseManager.ImportBlock(blockfile, Model.BlockName);
                    }
                    Model?.PutBlock();
                });
            }
        }
    }
}
