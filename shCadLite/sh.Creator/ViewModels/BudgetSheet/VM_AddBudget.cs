using Newtonsoft.Json;
using sh.Cad;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_AddBudget:ViewModelBase
    {
        private List<BudgetVar> _budgetVars;

        public List<BudgetGroup> BudgetGroups { get; private set; }

        private int _selGroupIndex;
        /// <summary>
        /// 选择分组
        /// </summary>
        public int SelGroupIndex
        {
            get { return _selGroupIndex; }
            set { _selGroupIndex = value;RaisePropertyChanged(); }
        }


        private VM_BudgetItem _modelNew;
        /// <summary>
        /// 新预算
        /// </summary>
        public VM_BudgetItem ModelNew
        {
            get { return _modelNew; }
            set { _modelNew = value;RaisePropertyChanged(); }
        }

        private string _message;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value;RaisePropertyChanged(); }
        }


        private Action<VM_BudgetItem> _updateAction;

        public VM_AddBudget(Action<VM_BudgetItem> action=null)
        {
            ModelNew = new VM_BudgetItem(new BudgetItem());
            BudgetGroups = BudgetGroup.GetAll();
            _budgetVars = BudgetVar.GetAll();

            _updateAction = action;
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
                    #region 验证
                    if (BudgetGroups.Count == 0)
                    {
                        Message = "没有分组信息";
                        return;
                    }
                    ModelNew.GroupName = BudgetGroups[SelGroupIndex].Name;
                    if (BudgetGroups[SelGroupIndex].BudgetItems == null)
                        BudgetGroups[SelGroupIndex].BudgetItems = new List<BudgetItem>();
                    if (string.IsNullOrEmpty(ModelNew.Name))
                    {
                        Message = "名称没有填写";
                        return;
                    }
                    if (BudgetGroups[SelGroupIndex].BudgetItems.Exists(b => b.Name == ModelNew.Name))
                    {
                        Message = "名称重复";
                        return;
                    }
                    if (string.IsNullOrEmpty(ModelNew.Expression))
                    {
                        Message = "表达式没有填写";
                    }
                    var expression = ModelNew.Expression;
                    foreach (var bv in _budgetVars)
                    {
                        if (expression.Contains(bv.Name))
                            expression = expression.Replace(bv.Name, bv.GetQuantities());
                    }
                    // 试算
                    try
                    {
                        if (new System.Data.DataTable().Compute(expression, null).ToString() == "False")
                            throw new Exception();
                    }
                    catch
                    {
                        Message = "表达式不正确，请检查";
                        return;
                    }

                    //if (string.IsNullOrEmpty(ModelNew.Configuration))
                    //{
                    //    Message = "配置没有填写";
                    //    return;
                    //}
                    #endregion

                    BudgetGroups[SelGroupIndex].BudgetItems.Add(ModelNew.Model);
                    if (BudgetGroup.SaveAll(BudgetGroups))
                    {
                        _updateAction?.Invoke(ModelNew);
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
