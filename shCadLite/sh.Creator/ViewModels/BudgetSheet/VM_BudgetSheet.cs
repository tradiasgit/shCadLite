using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using sh.XmlResourcesParsing.Fields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetSheet : ViewModelBase
    {
        private ObservableCollection<VM_BudgetItem> _budgetItems;

        public ObservableCollection<VM_BudgetItem> BudgetItems
        {
            get { return _budgetItems; }
            set { _budgetItems = value; RaisePropertyChanged(); }
        }


        // 名称 工程量 金额 配置 string
        public VM_BudgetSheet()
        {
            var list = BudgetGroup.GetAll();
            BudgetItems = new ObservableCollection<VM_BudgetItem>();
            foreach (var itemGroup in list)
            {
                if (itemGroup.BudgetItems == null) continue;
                foreach (var item in itemGroup.BudgetItems)
                {
                    BudgetItems.Add(new VM_BudgetItem(item) { GroupName = itemGroup.Name });
                }
            }

            var vw = CollectionViewSource.GetDefaultView(BudgetItems);
            vw.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
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

        public ICommand Cmd_Calculate
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    foreach (var item in BudgetItems)
                    {
                        //var path= Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), $@"support\field\{item.Expression}");
                        //if (!File.Exists(path)) continue;
                        //var doc = new XmlDocument();
                        //doc.Load(path);
                        //var qf = new QueryField(doc.DocumentElement);
                        //item.QuantitieString = qf.GetText();
                    }
                });
            }
        }
    }
}
