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
            var vm =new ViewModels.VM_ResourceBox();
            sh.Cad.EventManager.RegisterSelectionListener(vm);
            DataContext = vm;
        }

   




        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(UC_ResourceBox), new PropertyMetadata(null));

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
