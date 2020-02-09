using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetVarTable : ViewModelBase
    {
        private ObservableCollection<VM_BudgetVar> _budgetVars;
        /// <summary>
        /// 集合
        /// </summary>
        public ObservableCollection<VM_BudgetVar> BudgetVars
        {
            get { return _budgetVars; }
            set { _budgetVars = value;RaisePropertyChanged(); }
        }

        public VM_BudgetVarTable()
        {
            Cmd_RefreshBudgetVar.Execute(null);
        }


        /// <summary>
        /// 添加
        /// </summary>
        public ICommand Cmd_AddBudgetVar
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var vm = new VM_AddBudgetVar((v) => 
                    { 
                        BudgetVars.Add(v); 
                    });
                    vm.Show();
                });
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public ICommand Cmd_RefreshBudgetVar
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var budgetVarList = BudgetVar.GetAll();
                    BudgetVars = new ObservableCollection<VM_BudgetVar>(budgetVarList.Select(b => new VM_BudgetVar { Model = b }));
                });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public ICommand Cmd_RemoveBudgetVar
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if(p is VM_BudgetVar rModel)
                    {
                        if (MessageBox.Show("此操作不可逆，确定删除变量吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;
                        BudgetVars.Remove(rModel);
                        BudgetVar.SaveAll(BudgetVars.Select(b => b.Model).ToList());
                    }
                });
            }
        }

        
    }
}
