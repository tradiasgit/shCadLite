using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_AddBudget:ViewModelBase
    {
        public List<BudgetGroup> BudgetGroups { get; private set; }

        private int _selGroupIndex;

        public int SelGroupIndex
        {
            get { return _selGroupIndex; }
            set { _selGroupIndex = value;RaisePropertyChanged(); }
        }



        private VM_BudgetItem _modelNew;

        public VM_BudgetItem ModelNew
        {
            get { return _modelNew; }
            set { _modelNew = value;RaisePropertyChanged(); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value;RaisePropertyChanged(); }
        }


        public VM_AddBudget()
        {
            ModelNew = new VM_BudgetItem(new BudgetItem());
            BudgetGroups = BudgetGroup.GetAll();
        }

        public void Show()
        {
            var win = new Win_AddBudget { DataContext = this };
            win.ShowDialog();
        }


        public ICommand Cmd_Add
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (BudgetGroups[SelGroupIndex].BudgetItems == null)
                        BudgetGroups[SelGroupIndex].BudgetItems = new List<BudgetItem>();
                    if (string.IsNullOrEmpty(ModelNew.Name))
                    {
                        Message = "名称没有填写";
                        return;
                    }
                    if(BudgetGroups[SelGroupIndex].BudgetItems.Exists(b => b.Name == ModelNew.Name))
                    {
                        Message = "名称重复";
                        return;
                    }
                    if (string.IsNullOrEmpty(ModelNew.Expression))
                    {
                        Message = "表达式没有选择";
                    }
                    if (BudgetGroups[SelGroupIndex].BudgetItems.Exists(b => b.Expression == ModelNew.Expression))
                    {
                        Message = "表达式重复";
                        return;
                    }
                    if(string.IsNullOrEmpty(ModelNew.Configuration))
                    {
                        Message = "配置没有填写";
                        return;
                    }
                    BudgetGroups[SelGroupIndex].BudgetItems.Add(ModelNew.Model);
                    if (BudgetGroup.SaveAll(BudgetGroups))
                    {
                        ModelNew = new VM_BudgetItem(new BudgetItem());
                        SelGroupIndex = 0;
                        Message = "操作成功";
                    }
                    else
                    {
                        Message = "操作失败";
                    }
                });
            }
        }
    }
}
