using Autodesk.AutoCAD.DatabaseServices;
using Microsoft.Win32;
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
    /// Win_EditBudgetVar.xaml 的交互逻辑
    /// </summary>
    public partial class Win_EditBudgetVar : Window
    {
        public Win_EditBudgetVar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "请选择文件";
            dialog.Filter = "enf(*.enf)|*.enf";
            dialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support");
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                txtVar.Text = System.IO.File.ReadAllText(dialog.FileName);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbox = sender as ComboBox;
            if (cbox.SelectedIndex == 0)
                btnSelFile.Visibility = System.Windows.Visibility.Collapsed;
            else
                btnSelFile.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
