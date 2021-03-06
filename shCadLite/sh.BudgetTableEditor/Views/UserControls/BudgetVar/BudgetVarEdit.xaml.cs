﻿using GalaSoft.MvvmLight.Ioc;
using HandyControl.Data;
using Microsoft.Win32;
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
    /// <summary>
    /// BudgetVarEdit.xaml 的交互逻辑
    /// </summary>
    public partial class BudgetVarEdit : Window
    {
        BudgetVarEditViewModel ViewModel { get; set; } = new BudgetVarEditViewModel();

        public BudgetVarEdit()
        {
            InitializeComponent();

            var methodList = new Dictionary<string, string>
            {
                { "常量", "Value" },
                { "数量", "Count" },
                { "长度", "Length" },
                { "面积", "Area" }
            };
            cboxMethod.ItemsSource = methodList;
            cboxMethod.DisplayMemberPath = "Key";
            cboxMethod.SelectedValuePath = "Value";

            txtName.VerifyFunc += VerifyName;
            txtValue.VerifyFunc += VerifyValue;

            DataContext = ViewModel;
        }

        private void cboxMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSelectFile.IsEnabled = cboxMethod.SelectedIndex != 0;
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "选择文件";
            dialog.Filter = "enf(*.enf)|*.enf";
            if (dialog.ShowDialog() == true)
                txtValue.Text = System.IO.File.ReadAllText(dialog.FileName);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var v1 = txtName.VerifyData();
            var v2 = cboxMethod.VerifyData();
            var v3 = txtValue.VerifyData();
            if (v1 && v2 && v3)
                ViewModel.SaveBudgetVar.Execute(null);
        }

        #region 验证
        public OperationResult<bool> VerifyName(string text)
        {
            OperationResult<bool> result = OperationResult.Success();
            if (string.IsNullOrWhiteSpace(text))
                return OperationResult.Failed("不能为空");
            // 唯一验证
            if(text!=ViewModel.OldName)
            {
                var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
                if (btfh.BudgetVarExists(text))
                    return OperationResult.Failed("名称重复");
            }
            return result;
        }

        public OperationResult<bool> VerifyValue(string text)
        {
            OperationResult<bool> result = OperationResult.Success();
            if (string.IsNullOrWhiteSpace(text))
                return OperationResult.Failed("不能为空");
            if (cboxMethod.SelectedIndex == 0 && !double.TryParse(text, out var res))
                return OperationResult.Failed("格式不正确，方法《常量》时应为数字");

            return result;
        }
        #endregion
    }
}
