
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
    /// Win_AddBudget.xaml 的交互逻辑
    /// </summary>
    public partial class Win_AddBudget : Window
    {
        public Win_AddBudget()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "请选择文件";
            dialog.Filter = "xml(*.xml)|*.xml";
            dialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\field");
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                txtExpression.Text = System.IO.Path.GetFileName(dialog.FileName);
            }
        }
    }
}
