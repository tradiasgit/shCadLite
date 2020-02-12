using System;
using System.Collections.Generic;
using System.Text;

namespace sh.BudgetTableEditor.Models
{
    /// <summary>
    /// 预算表
    /// </summary>
    class BudgetTable
    {
        /// <summary>
        /// 变量
        /// </summary>
        public List<BudgetVar> BudgetVars { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public List<BudgetGroup>  BudgetGroups { get; set; }
    }
}
