using System;
using System.Collections.Generic;
using System.Text;

namespace sh.BudgetTableEditor.Models
{
    /// <summary>
    /// 预算配置   20-02-12：填写淘宝、京东商品信息
    /// </summary>
    class BudgetItemConfiguration
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public double Price { get; set; }
    }
}
