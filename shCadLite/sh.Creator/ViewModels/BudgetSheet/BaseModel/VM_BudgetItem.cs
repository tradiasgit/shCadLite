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
            get{ return Model.ID; }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 工程量
        /// </summary>
        public double Quantities
        {
            get { return Model.Quantities; }
            set { Model.Quantities = value; RaisePropertyChanged(); }
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
        public string Configuration
        {
            get { return Model.Configuration; }
            set { Model.Configuration = value; RaisePropertyChanged(); }
        }

        public VM_BudgetItem(BudgetItem model)
        {
            Model = model;
        }
    }

    public class BudgetItem
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        /// <summary>
        /// 工程量
        /// </summary>
        public double Quantities { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration { get; set; }
    }
}
