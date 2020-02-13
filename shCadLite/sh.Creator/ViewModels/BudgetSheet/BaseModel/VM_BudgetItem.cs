using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetItem : ViewModelBase<BudgetItem>
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID
        {
            get { return Model.ID; }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; RaisePropertyChanged(); }
        }

        

        private string _quantitieString;
        /// <summary>
        /// 工程量
        /// </summary>
        public string QuantitieString
        {
            get { return _quantitieString; }
            set { _quantitieString = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression
        {
            get { return Model.Expression; }
            set { Model.Expression = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public BudgetItemConfiguration Configuration
        {
            get { return Model.Configuration; }
            set { Model.Configuration = value; RaisePropertyChanged(); }
        }

        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 比例
        /// </summary>
        public double Ration
        {
            get { return Model.Ratio; }
            set { Model.Ratio = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        public string Format
        {
            get { return Model.Format; }
            set { Model.Format = value; RaisePropertyChanged(); }
        }



        public VM_BudgetItem(BudgetItem model)
        {
            Model = model;
        }


        public ICommand Cmd_EditExpression
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var editVMm = new VM_EditExpression(Expression, Ration, Format);
                    if (editVMm.ShowWindow())
                    {
                        // 保存
                        var budgetGroups = BudgetGroup.GetAll();
                        var thisModel = budgetGroups.FirstOrDefault(g => g.Name == GroupName).BudgetItems.FirstOrDefault(i => i.ID == ID);
                        thisModel.Expression = editVMm.ExpressionString;
                        BudgetGroup.SaveAll(budgetGroups);

                        Expression = editVMm.ExpressionString;
                        QuantitieString = editVMm.QuantityString;
                    }

                });
            }
        }

        public ICommand Cmd_EditName
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var budgetGroups = BudgetGroup.GetAll();
                    var thisModel = budgetGroups.FirstOrDefault(g => g.Name == GroupName).BudgetItems.FirstOrDefault(i => i.ID == ID);
                    thisModel.Name = Name;
                    BudgetGroup.SaveAll(budgetGroups);
                });
            }
        }

        /// <summary>
        /// 编辑配置
        /// </summary>
        public ICommand Cmd_EditConfiguration
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var editVMm = new VM_EditConfigurationProduct(Configuration);
                    if (editVMm.ShowWindow())
                    {
                        // 保存
                        var budgetGroups = BudgetGroup.GetAll();
                        var thisModel = budgetGroups.FirstOrDefault(g => g.Name == GroupName).BudgetItems.FirstOrDefault(i => i.ID == ID);
                        thisModel.Configuration = editVMm.BudgetItemConfiguration.Model ;
                        BudgetGroup.SaveAll(budgetGroups);

                        Configuration = editVMm.BudgetItemConfiguration.Model;
                    }
                });
            }
        }


    }

    public class BudgetItem
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public BudgetItemConfiguration Configuration { get; set; }

        /// <summary>
        /// 0.000001
        /// </summary>
        public double Ratio { get; set; } = 1;

        /// <summary>
        /// {0:f2}平米
        /// </summary>
        public string Format { get; set; }

    }
}
