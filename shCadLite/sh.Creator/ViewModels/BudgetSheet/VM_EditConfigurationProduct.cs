using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_EditConfigurationProduct:ViewModelBase
    {
        private Window _win;

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }

        private VM_BudgetItemConfiguration _budgetItemConfiguration;

        public VM_BudgetItemConfiguration BudgetItemConfiguration
        {
            get { return _budgetItemConfiguration; }
            set { _budgetItemConfiguration = value;RaisePropertyChanged(); }
        }


        public VM_EditConfigurationProduct(BudgetItemConfiguration budgetItemConfiguration)
        {
            BudgetItemConfiguration = new VM_BudgetItemConfiguration { Model = budgetItemConfiguration };
        }

        public bool ShowWindow()
        {
            if (_win == null)
                _win = new Win_EditConfigurationProduct { DataContext = this };
            return _win.ShowDialog().Value;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public ICommand Cmd_Get
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if (!InternetGetConnectedState(out int i, 0))
                    {
                        Message = "未联网";
                        return;
                    }
                    Message = "功能未开放";
                    //var result = string.Empty;
                    //Uri server = new Uri(BudgetItemConfiguration.Url);
                    //using (var httpClient = new HttpClient())
                    //{
                    //    result = httpClient.GetAsync(server).;
                    //}

                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand Cmd_Ok
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    if(!IsURL(BudgetItemConfiguration.Url))
                    {
                        Message = "URL格式无效";
                        return;
                    }
                    if(string.IsNullOrEmpty(BudgetItemConfiguration.Name))
                    {
                        Message = "商品名称没有填写";
                        return;
                    }
                    if(string.IsNullOrEmpty(BudgetItemConfiguration.Price.ToString()))
                    {
                        Message = "商品价格没有填写";
                        return;
                    }

                    _win.DialogResult = true;
                });
            }
        }

        public static bool IsURL(string str)
        {
            string pattern = @"^(https?|ftp|file|ws)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            var reg = new Regex(pattern);
            if (string.IsNullOrEmpty(str))
                return false;
            return reg.IsMatch(str);
        }


        #region MyRegion
        [DllImport("wininet")]

        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        #endregion


    }
}
