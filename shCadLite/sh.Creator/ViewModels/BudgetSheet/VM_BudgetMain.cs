using sh.Creator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetMain
    {
        public void Show()
        {
            var tab  = new System.Windows.Controls.TabControl();

            var tab1 = new System.Windows.Controls.TabItem();
            tab1.Header = "变量";
            tab1.Content = new UC_BudgetVarTable { DataContext = new VM_BudgetVarTable() };
            tab.Items.Add(tab1);

            var tab2 = new System.Windows.Controls.TabItem();
            tab2.Header = "预算";
            tab2.Content = new UC_BudgetSheet { DataContext = new VM_BudgetSheet() };
            tab.Items.Add(tab2);

            var win = new Window { Content = tab };

            win.Show();
        }
    }
}
