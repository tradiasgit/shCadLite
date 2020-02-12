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

namespace sh.Creator.Views.Repository
{
    /// <summary>
    /// PS_Repository.xaml 的交互逻辑
    /// </summary>
    public partial class PS_Repository : UserControl
    {
        public PS_Repository()
        {
            InitializeComponent();
            VM = new ViewModels.Repository.VM_RepositoryPalette();
            DataContext = VM;
        }

        ViewModels.Repository.VM_RepositoryPalette VM;



        public void ShowInPalette()
        {
            var ps = new PaletteSet("山河仓库资源管理器");
            ps.Style = PaletteSetStyles.Snappable| PaletteSetStyles.ShowCloseButton;
            ps.Visible = true;
            ps.DockEnabled = DockSides.Left;
            ps.Dock = DockSides.Left;
            ps.AddVisual("山河仓库资源管理器", this  );
            ps.Visible = true;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            VM.OnSelectedItemChanged(e.NewValue as ViewModels.Repository.VM_TreeItem);
        }
    }
}
