using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    class VM_AddGroup:ViewModelBase
    {
        private List<BudgetGroup> _budgetGroupList;

        private string _path;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;RaisePropertyChanged(); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }


        public VM_AddGroup()
        {
            _path = Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\budgetsheet\budgetsheet.json");
            if (!File.Exists(_path)) return;
            try
            {
                _budgetGroupList = JsonConvert.DeserializeObject<List<BudgetGroup>>(File.ReadAllText(_path));
            }
            catch
            {
                _budgetGroupList = new List<BudgetGroup>();
            }
        }

        public void Show()
        {
            var win = new Win_AddGroup { DataContext = this };
            win.ShowDialog();
        }

        public ICommand Cmd_Add
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if(string.IsNullOrEmpty(Name))
                    {
                        Message = "名字没有填写";
                        return;
                    }
                    if(_budgetGroupList.Exists(g=>g.Name==Name))
                    {
                        Message = "名字重复";
                        return;
                    }
                    _budgetGroupList.Add(new BudgetGroup { Name = Name });
                    File.WriteAllText(_path, JsonConvert.SerializeObject(_budgetGroupList));
                    Message = "操作成功";
                });
            }
        }
    }
}
