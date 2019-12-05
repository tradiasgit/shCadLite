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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sh.Creator.Views
{
    /// <summary>
    /// shResourceBox.xaml 的交互逻辑
    /// </summary>
    public partial class shResourceBox : UserControl
    {
        public shResourceBox()
        {
            InitializeComponent();
            var vm =new ViewModels.VM_shResourceBox();
            sh.Cad.EventManager.RegisterSelectionListener(vm);
            DataContext = vm;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //dynamic data  = ((TreeViewItem)sender)?.DataContext;
            //data.Command?.Execute();
        }
    }
}
