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
    /// shQueryEditor.xaml 的交互逻辑
    /// </summary>
    public partial class shQueryEditor : UserControl
    {
        public shQueryEditor()
        {
            InitializeComponent();
           // ViewModel = ViewModels.VM_shQueryEditor.Instance;
        }

        //public ViewModels.VM_shQueryEditor ViewModel { get { return DataContext as ViewModels.VM_shQueryEditor; } set { DataContext = value; } }


    
    }
}
