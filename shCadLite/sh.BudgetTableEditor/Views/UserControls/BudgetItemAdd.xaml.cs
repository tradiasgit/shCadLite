using GalaSoft.MvvmLight.Ioc;
using sh.BudgetTableEditor.Tools;
using sh.BudgetTableEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sh.BudgetTableEditor.Views.UserControls
{
    /// <summary>
    /// BudgetItemAdd.xaml 的交互逻辑
    /// </summary>
    public partial class BudgetItemAdd : Window
    {
        BudgetItemAddViewModel ViewModel { get; set; } = new BudgetItemAddViewModel();

        BudgetTableFileHelper budgetTableFileHelper;

        public BudgetItemAdd()
        {
            InitializeComponent();

            budgetTableFileHelper = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();

            cboxMethod.ItemsSource = budgetTableFileHelper.BudgetGroups.Select(g => g.Name);


            this.DataContext = ViewModel;

        }
    }
}
