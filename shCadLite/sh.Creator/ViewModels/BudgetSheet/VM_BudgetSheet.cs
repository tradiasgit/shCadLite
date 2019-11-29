using Newtonsoft.Json;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_BudgetSheet: ViewModelBase
    {
        // 名称 工程量 金额 配置 string
        public VM_BudgetSheet()
        {
            
        }

        public void Show()
        {
            var win = new Window();
            win.Content = new UC_BudgetSheet();
            win.Show();
        }
    }

    public class BudgetGroup
    {
        public string Name { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public List<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public Guid ID { get; set; }

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
