using sh.Creator.Views;
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
    class VM_EditExpression:ViewModelBase
    {
        private Window _win;

        private ObservableCollection<BudgetVar> _budgetVars;

        public ObservableCollection<BudgetVar> BudgetVars
        {
            get { return _budgetVars; }
            set { _budgetVars = value;RaisePropertyChanged(); }
        }

        private string _expressionString;

        public string ExpressionString
        {
            get { return _expressionString; }
            set { _expressionString = value; RaisePropertyChanged(); }
        }

        private string _quantityString;

        public string QuantityString
        {
            get { return _quantityString; }
            set { _quantityString = value; RaisePropertyChanged(); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value;RaisePropertyChanged(); }
        }



        

        public VM_EditExpression(string exp)
        {
            ExpressionString = exp;
            BudgetVars = new ObservableCollection<BudgetVar>(BudgetVar.GetAll());
        }

        public bool ShowWindow()
        {
            if(_win==null)
                _win = new Win_EditExpression { DataContext = this };
            return  _win.ShowDialog().Value;
        }

        /// <summary>
        /// 试算
        /// </summary>
        public ICommand Cmd_Trial
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    validation(out var quantity);
                });
            }
        }

        public ICommand Cmd_Ok
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (validation(out var quantity))
                    {
                        QuantityString = quantity;
                        _win.DialogResult = true;
                    }
                });
            }
        }

        private bool validation(out string quantity)
        {
            quantity = string.Empty;
            if (string.IsNullOrEmpty(ExpressionString))
            {
                Message = "表达式没有填写";
                return false;
            }
            var expression = ExpressionString;
            foreach (var bv in _budgetVars)
            {
                if (expression.Contains(bv.Name))
                    expression = expression.Replace(bv.Name, bv.GetQuantities());
            }
            // 试算
            try
            {
                quantity = new System.Data.DataTable().Compute(expression, null).ToString();
                if (quantity == "False")
                    throw new Exception();
            }
            catch
            {
                Message = "表达式不正确，请检查";
                return false;
            }
            Message = "表达式正确";
            return true;
        }
    }
}
