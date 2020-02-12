using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Newtonsoft.Json;
using sh.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.CadCommands
{
    class Cmd_BrushWithEnf : CadViewModelBase, ICommand
    {
        public event EventHandler CanExecuteChanged;


        public void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public bool CanExecute(object parameter)
        {
            //   throw new NotImplementedException();
            return true;
        }

        public void Execute(object parameter)
        {
            var enf = parameter as EntityInfo;
            if (enf == null)
            {
                var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                var db_source = HostApplicationServices.WorkingDatabase;
                var dir = new FileInfo(db_source.OriginalFileName).Directory;

                var op_file = new PromptOpenFileOptions("选择目标文件" + Environment.NewLine);
                op_file.InitialDirectory = $@"{dir.FullName}";
                op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";
                var result_file = ed.GetFileNameForOpen(op_file);
                if (result_file.Status == PromptStatus.OK)
                {
                    enf = EntityInfo.Get(new FileInfo(result_file.StringResult)) as EntityInfo;
                }
                else return;
            }
            
            enf.BrushImplied();
            var sel = new EntitySelection(doc.Editor.SelectImplied());
            ViewModels.Property.VM_PropertyPalette.Inctance.OnSelectionChanged(sel);
        }
    }
}
