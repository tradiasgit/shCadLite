using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Newtonsoft.Json;
using sh.Creator.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.Functions
{
    class SaveAs:CadViewModelBase,ICommand
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
            if (parameter is EntityInfo)
            {
                var dwgtitled = Application.GetSystemVariable("DWGTITLED");
                if (Convert.ToInt16(dwgtitled) == 0)
                {
                    throw new Exception("图纸为临时图纸，不能保存");
                }
                var ed = Application.DocumentManager.MdiActiveDocument.Editor;
                var db_source = HostApplicationServices.WorkingDatabase;
                var dir = new FileInfo(db_source.OriginalFileName).Directory;

                var op_file = new PromptSaveFileOptions("选择目标文件" + Environment.NewLine);
                op_file.InitialDirectory = $@"{dir.FullName}";
                op_file.Filter = "图形配置文件-json格式 (*.enf)|*.enf";
                var result_file = ed.GetFileNameForSave(op_file);
                if (result_file.Status == PromptStatus.OK)
                {
                    File.WriteAllText(result_file.StringResult, JsonConvert.SerializeObject(parameter));
                }
            }
        }
    }
}
