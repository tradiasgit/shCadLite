using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_AddBudget:ViewModelBase
    {
        public List<BudgetGroup> BudgetGroups { get; private set; }

        private int _selgroupIndex;

        public int SelgroupIndex
        {
            get { return _selgroupIndex; }
            set { _selgroupIndex = value;RaisePropertyChanged(); }
        }



        private VM_BudgetItem _modelNew;

        public VM_BudgetItem ModelNew
        {
            get { return _modelNew; }
            set { _modelNew = value;RaisePropertyChanged(); }
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
    }
}
