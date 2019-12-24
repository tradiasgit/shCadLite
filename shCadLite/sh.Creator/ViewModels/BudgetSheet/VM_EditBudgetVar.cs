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
        private string _oldName;

        private List<BudgetVar> budgetVars;

        private ObservableCollection<string> _methodList;
        /// <summary>
        /// 集合
        /// </summary>
        public ObservableCollection<string> MethodList
        {
            get { return _methodList; }
            set { _methodList = value; RaisePropertyChanged(); }
        }

        private string _selMethod;

        public string SelMethod
        {
            get { return _selMethod; }
            set { _selMethod = value;RaisePropertyChanged(); }
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

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }


        public VM_EditBudgetVar()
        {
            MethodList = new ObservableCollection<string>();
            MethodList.Add("Value");
            MethodList.Add("Count");
            MethodList.Add("Length");
            MethodList.Add("Area");
        }

        public VM_EditBudgetVar(string name,string value,string method)
        {
            MethodList = new ObservableCollection<string>();
            MethodList.Add("Value");
            MethodList.Add("Count");
            MethodList.Add("Length");
            MethodList.Add("Area");

            Name = _oldName = name;
            Value = value;
            SelMethod = method;
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


                    if (string.IsNullOrEmpty(_oldName))
                        Add();
                    else
                        Edit();
                    
                });
            }
        }

        private void Add()
        {
            BudgetVar budgetVar;
            budgetVars = BudgetVar.GetAll();
            if (budgetVars.Exists(b => b.Name == Name))
            {
                Message = "变量名称重复";
                return;
            }
            if(SelMethod=="Value")
            {
                if (Double.TryParse(Value, out var varDouble))
                {
                    budgetVar = new BudgetVarDouble { Name = Name, Constant = varDouble,Method=SelMethod };
                }
                else
                {
                    Message = "变量值格式不正确";
                    return;
                }
            }
            else
            {
                try
                {
                    var jsonObject = JObject.Parse(Value);
                    budgetVar = new BudgetVarString { Name = Name, EcjJsonString = Value, Method = SelMethod };
                }
                catch 
                {
                    Message = "变量值格式不正确";
                    return;
                }
            }
            budgetVars.Add(budgetVar);
            if(BudgetVar.SaveAll(budgetVars))
            {
                Message = "操作成功";
                Name = Value = "";
            }
        }

        private void Edit()
        {
            
            budgetVars = BudgetVar.GetAll();
            var index = budgetVars.FindIndex(b => b.Name == _oldName);
            budgetVars.RemoveAll(b => b.Name == _oldName);
            if (budgetVars.Exists(b => b.Name == Name))
            {
                Message = "变量名称重复";
                return;
            }

            BudgetVar budgetVar;
            if (SelMethod == "Value")
            {
                if (Double.TryParse(Value, out var varDouble))
                {
                    budgetVar = new BudgetVarDouble { Name = Name, Constant = varDouble, Method = SelMethod };
                }
                else
                {
                    Message = "变量值格式不正确";
                    return;
                }
            }
            else
            {
                try
                {
                    var jsonObject = JObject.Parse(Value);
                    budgetVar = new BudgetVarString { Name = Name, EcjJsonString = Value, Method = SelMethod };
                }
                catch
                {
                    Message = "变量值格式不正确";
                    return;
                }
            }
            budgetVars.Insert(index,budgetVar);
            if(BudgetVar.SaveAll(budgetVars))
            {
                Message = "操作成功";
                _oldName = Name;
            }
        }
    }
}
