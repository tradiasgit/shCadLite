using GalaSoft.MvvmLight;
using sh.BudgetTableEditor.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using sh.BudgetTableEditor.Views.UserControls;

namespace sh.BudgetTableEditor.ViewModels
{
    /// <summary>
    /// 变量表
    /// </summary>
    class BudgetVarTableViewModel:ViewModelBase
    {
        private readonly BudgetTableFileHelper _budgetTableFileHelper;


        private ObservableCollection<BudgetVarViewModel> _budgetVars;

        /// <summary>
        /// 数据
        /// </summary>
        public ObservableCollection<BudgetVarViewModel> BudgetVars
        {
            get { return _budgetVars; }
            set { Set(ref _budgetVars, value); }
        }

        public BudgetVarTableViewModel(BudgetTableFileHelper budgetTableFileHelper)
        {
            _budgetTableFileHelper = budgetTableFileHelper;
            Refresh();
            Messenger.Default.Register<BudgetVarViewModel>(this, "BudgetVarAddItem", BudgetVarAddItem);
            Messenger.Default.Register<string>(this, "Refresh", Refresh);
        }


        public void Refresh(string msg="")
        {
            BudgetVars = new ObservableCollection<BudgetVarViewModel>(_budgetTableFileHelper.BudgetVars.Select(v => new BudgetVarViewModel(v)));
        }

        public void BudgetVarAddItem(BudgetVarViewModel model)
        {
            BudgetVars.Add(model);
        }
        
    }
}
