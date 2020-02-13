using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using sh.BudgetTableEditor.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace sh.BudgetTableEditor.ViewModels
{
    class BudgetGroupTableViewModel:ViewModelBase
    {
        private readonly BudgetTableFileHelper _budgetTableFileHelper;


        private ObservableCollection<BudgetGroupViewModel> _budgetGroups;

        /// <summary>
        /// 数据
        /// </summary>
        public ObservableCollection<BudgetGroupViewModel> BudgetGroups
        {
            get { return _budgetGroups; }
            set { Set(ref _budgetGroups, value); }
        }

        public BudgetGroupTableViewModel(BudgetTableFileHelper budgetTableFileHelper)
        {
            _budgetTableFileHelper = budgetTableFileHelper;
            Refresh();
            Messenger.Default.Register<BudgetGroupViewModel>(this, "BudgetGroupAddItem", BudgetGroupAddItem);
            Messenger.Default.Register<string>(this, "RefreshBudgetGroups", Refresh);
        }


        public void Refresh(string msg = "")
        {
            BudgetGroups = new ObservableCollection<BudgetGroupViewModel>(_budgetTableFileHelper.BudgetGroups.Select(v => new BudgetGroupViewModel(v)));
        }

        public void BudgetGroupAddItem(BudgetGroupViewModel model)
        {
            BudgetGroups.Add(model);
        }
    }
}
