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
        private shResourceBox()
        {
            InitializeComponent();
            ViewModel = ViewModels.VM_shResourceBox.Instance;
        }

        public ViewModels.VM_shResourceBox ViewModel { get { return DataContext as ViewModels.VM_shResourceBox; } set { DataContext = value; } }


        public Cad.PaletteConfig GetPaletteConfig()
        {
            return new Cad.PaletteConfig { Title = "资源盒子", View = this };
        }

        private static shResourceBox _Instance;
        public static shResourceBox Instance
        {
            get
            {
                if (_Instance == null) _Instance = new shResourceBox();
                return _Instance;
            }
        }
    }
}
