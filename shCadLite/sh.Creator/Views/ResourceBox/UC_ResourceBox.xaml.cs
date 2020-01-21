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
    public partial class UC_ResourceBox : UserControl
    {
        public UC_ResourceBox()
        {
            InitializeComponent();
            VM = new ViewModels.VM_ResourceBox();
            sh.Cad.EventManager.RegisterSelectionListener(VM);
            DataContext = VM;
        }

        ViewModels.VM_ResourceBox VM;

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = e.NewValue as ViewModels.VM_TreeItem;
            if (item != null)
                VM.SelectedItem = item;
            else VM.SelectedItem = null;

        }
    }
}
