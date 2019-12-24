using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private VM_BudgetVar _selBudgetVar;
        /// <summary>
        /// 选择
        /// </summary>
        public VM_BudgetVar SelBudgetVar
        {
            get { return _selBudgetVar; }
            set { _selBudgetVar = value; RaisePropertyChanged(); }
        }

        public VM_BudgetVarTable()
        {
            Refresh();
        }

        private void Refresh()
        {
            var budgetVarList = BudgetVar.GetAll();
            BudgetVars = new ObservableCollection<VM_BudgetVar>(budgetVarList.Select(b => new VM_BudgetVar { Model = b }));
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public ICommand Cmd_EditBudgetVar
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (SelBudgetVar == null) return;
                    var vm = new VM_EditBudgetVar(SelBudgetVar.Name, SelBudgetVar.Value,SelBudgetVar.Method);
                    vm.Show();
                    Refresh();
                });
            }
        }
    }
}
