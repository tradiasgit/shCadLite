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
    /// UC_Data.xaml 的交互逻辑
    /// </summary>
    public partial class UC_DataBox : UserControl
    {
        public UC_DataBox()
        {
            InitializeComponent();
            var vm = new ViewModels.VM_DataBox();
            sh.Cad.EventManager.RegisterSelectionListener(vm);
            this.DataContext = vm;
        }
    }
}
