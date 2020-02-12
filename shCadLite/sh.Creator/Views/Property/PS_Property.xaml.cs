using Autodesk.AutoCAD.Windows;
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

namespace sh.Creator.Views.Property
{
    /// <summary>
    /// PS_Property.xaml 的交互逻辑
    /// </summary>
    public partial class PS_Property : UserControl
    {
        public PS_Property()
        {
            InitializeComponent();
            DataContext = new ViewModels.Property.VM_PropertyPalette();
        }

        public void ShowInPalette()
        {
            var ps = new PaletteSet("山河属性管理器");
            ps.Style = PaletteSetStyles.Snappable | PaletteSetStyles.ShowCloseButton;
            ps.Visible = true;
            ps.DockEnabled = DockSides.Left;
            ps.Dock = DockSides.Left;
            ps.AddVisual("山河属性管理器", this);
            ps.Visible = true;
        }
    }
}
