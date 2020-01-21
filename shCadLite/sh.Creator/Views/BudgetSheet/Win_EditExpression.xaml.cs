using sh.Creator.ViewModels.BudgetSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sh.Creator.Views
{
    /// <summary>
    /// Win_EditExpression.xaml 的交互逻辑
    /// </summary>
    public partial class Win_EditExpression : Window
    {
        public Win_EditExpression()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListBox listBox)
                if (listBox.SelectedItem is BudgetVar budgetVar)
                    txtExpression.Text += budgetVar.Name;
        }
    }
}
