using System;
using System.Collections.Generic;
using System.Text;

namespace sh.BudgetTableEditor.Models
{
    /// <summary>
    /// 预算分组
    /// </summary>
    class BudgetGroup
    {
        public string Name { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public List<BudgetItem> BudgetItems { get; set; }
    }
}
