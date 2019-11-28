using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels
{
    public class VM_BudgetSheet: ViewModelBase
    {
        // 名称 工程量 金额 配置 string
        
    }

    public class Budget
    {
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

    public class Room
    {
        public string Name { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public List<Budget> Budgets { get; set; }
    }


}
