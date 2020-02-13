using GalaSoft.MvvmLight.Ioc;
using HandyControl.Data;
using sh.BudgetTableEditor.Tools;
using sh.BudgetTableEditor.ViewModels;
using System;
using System.Collections.Generic;
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
    

    public partial class BudgetGroupAdd : System.Windows.Window
    {
        BudgetGroupAddViewModel ViewModel { get; set; } = new BudgetGroupAddViewModel();

        public BudgetGroupAdd()
        {
            InitializeComponent();

            txtName.VerifyFunc += VerifyName;

            DataContext = ViewModel;
        }

     

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.VerifyData())
                ViewModel.SaveBudgetGroup.Execute(null);
        }

        #region 验证
        public OperationResult<bool> VerifyName(string text)
        {
            OperationResult<bool> result = OperationResult.Success();
            if (string.IsNullOrWhiteSpace(text))
                return OperationResult.Failed("不能为空");
            // 唯一验证
            var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
            if (btfh.BudgetGroupExists(text))
                return OperationResult.Failed("名称重复");
            return result;
        }

       
        #endregion
    }
}
