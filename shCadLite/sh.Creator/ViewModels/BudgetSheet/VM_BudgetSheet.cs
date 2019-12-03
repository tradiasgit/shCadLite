using Newtonsoft.Json;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace sh.Creator.ViewModels.BudgetSheet
{
    public class VM_BudgetSheet : ViewModelBase
    {
        // 名称 工程量 金额 配置 string
        public VM_BudgetSheet()
        {

        }

        public void Show()
        {
            var win = new Window { Content = new UC_BudgetSheet(), DataContext = this };
            win.Show();
        }


        /// <summary>
        /// 添加分组
        /// </summary>
        public ICommand Cmd_AddGroup
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var vm = new VM_AddGroup();
                    vm.Show();
                });
            }
        }

        /// <summary>
        /// 编辑分组
        /// </summary>
        public ICommand Cmd_EditGroup
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var vm = new VM_EditGroup();
                    vm.Show();
                });
            }
        }

        /// <summary>
        /// 添加预算
        /// </summary>
        public ICommand Cmd_AddBudget
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var vm = new VM_AddBudget();
                    vm.Show();
                });
            }
        }
    }
}
