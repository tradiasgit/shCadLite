using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    class VM_BudgetGroup : ViewModelBase<BudgetGroup>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; RaisePropertyChanged(); }
        }

        public VM_BudgetGroup(BudgetGroup model)
        {
            Model = model;
        }
    }
}
