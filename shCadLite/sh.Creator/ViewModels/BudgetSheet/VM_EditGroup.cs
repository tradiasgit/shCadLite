using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_EditGroup:ViewModelBase
    {
        private ObservableCollection <VM_BudgetGroup> _budgetGroups;

        public ObservableCollection<VM_BudgetGroup> BudgetGroups
        {
            get { return _budgetGroups; }
            set { _budgetGroups = value;RaisePropertyChanged(); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }

        private VM_BudgetGroup _selBudgetGroup;

        public VM_BudgetGroup SelBudgetGroup
        {
            get { return _selBudgetGroup; }
            set { _selBudgetGroup = value;RaisePropertyChanged(); }
        }

        public VM_EditGroup()
        {
            var list = BudgetGroup.GetAll();
            BudgetGroups = new ObservableCollection<VM_BudgetGroup>(list.Select(b => new VM_BudgetGroup(b)).ToList());
        }

        public void Show()
        {
            var win = new Win_EditGroup { DataContext = this };
            win.ShowDialog();
        }

        public ICommand Cmd_Save
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if(SelBudgetGroup==null)
                    {
                        Message = "旧名字没有选择";
                        return;
                    }
                    if (string.IsNullOrEmpty(Name))
                    {
                        Message = "新名字没有填写";
                        return;
                    }
                    if(SelBudgetGroup.Name==Name)
                    {
                        Message = "新旧名字不能一样";
                        return;
                    }
                    if (BudgetGroups.Where(g => g.Name == Name).Count()!=0)
                    {
                        Message = "名字重复";
                        return;
                    }
                    SelBudgetGroup.Name = Name;
                    Name = string.Empty;
                    SelBudgetGroup = null;
                    if (BudgetGroup.SaveAll(BudgetGroups.Select(b => b.Model)))
                        Message = "操作成功";
                    else
                        Message = "操作失败";
                });
            }
        }

    }
}
