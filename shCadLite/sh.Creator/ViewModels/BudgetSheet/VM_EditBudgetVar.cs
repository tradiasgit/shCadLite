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
    class VM_EditBudgetVar: ViewModelBase
    {
        private ObservableCollection<string> _methodList;
        /// <summary>
        /// 集合
        /// </summary>
        public ObservableCollection<string> MethodList
        {
            get { return _methodList; }
            set { _methodList = value; RaisePropertyChanged(); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }

        #region 属性
        private string _selMethod;

        public string SelMethod
        {
            get { return _selMethod; }
            set { _selMethod = value; RaisePropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; ; RaisePropertyChanged(); }
        }
        #endregion

        private VM_BudgetVar _vM_BudgetVar;

        public VM_EditBudgetVar(VM_BudgetVar vM_BudgetVar)
        {
            MethodList = new ObservableCollection<string>(BudgetVar.GetMethodList());

            _vM_BudgetVar = vM_BudgetVar;
            Name = vM_BudgetVar.Name;
            Value = vM_BudgetVar.Value;
            SelMethod = vM_BudgetVar.Method;
        }

        public void Show()
        {
            var win = new Views.Win_EditBudgetVar { DataContext = this };
            win.ShowDialog();
        }

        public ICommand Cmd_Save
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    #region 验证
                    if (string.IsNullOrEmpty(Name))
                    {
                        Message = "变量名称没有填写";
                        return;
                    }
                    var budgetVars = BudgetVar.GetAll();
                    if (budgetVars.Exists(b => b.Name == Name&&b.Name!= _vM_BudgetVar.Name))
                    {
                        Message = "变量名称重复";
                        return;
                    }
                    if (string.IsNullOrEmpty(Value))
                    {
                        Message = "变量值没有填写";
                        return;
                    }
                    if (string.IsNullOrEmpty(SelMethod))
                    {
                        Message = "方法没有选择";
                        return;
                    }

                    #endregion

                    var index = budgetVars.FindIndex(b => b.Name == _vM_BudgetVar.Name);
                    try
                    {
                        if (SelMethod == "Value")
                        {
                            var varDouble = Double.Parse(Value);
                            budgetVars[index] = new BudgetVarDouble { Name = Name, Constant = varDouble, Method = SelMethod };
                        }
                        else
                        {
                            var jsonObject = JObject.Parse(Value);
                            budgetVars[index] = new BudgetVarString { Name = Name, EcjJsonString = Value, Method = SelMethod };
                        }
                    }
                    catch
                    {
                        Message = "变量值格式不正确";
                        return;
                    }
                    if (BudgetVar.SaveAll(budgetVars))
                    {
                        Message = "操作成功";
                        _vM_BudgetVar.Name = Name;
                        _vM_BudgetVar.Value = Value;
                        _vM_BudgetVar.Method = SelMethod;
                    }
                });
            }
        }
    }
}
