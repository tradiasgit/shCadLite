using System;
using System.Collections.Generic;
using System.Text;

namespace sh.BudgetTableEditor.Models
{
    /// <summary>
    /// 预算项
    /// </summary>
    class BudgetItem
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 预算名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工程量表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public BudgetItemConfiguration Configuration { get; set; }

        /// <summary>
        /// 0.000001  比例
        /// </summary>
        public double Ratio { get; set; } = 1;

        /// <summary>
        /// {0:f2}平米 工程量格式化
        /// </summary>
        public string Format { get; set; }
    }
}
