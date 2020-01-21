using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.Cad;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
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
            Initialize();
        }

        public void Show()
        {
            var win = new Window { Content = new UC_BudgetSheet(), DataContext = this };
            win.Show();
        }


        private void Initialize()
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
                    Initialize();
                });
            }
        }

        public ICommand Cmd_Calculate
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var budgetVars = BudgetVar.GetAll();

                    foreach (var item in BudgetItems)
                    {
                        var expression = item.Expression;
                        foreach (var bv in budgetVars)
                        {
                            if (expression.Contains(bv.Name))
                            {
                                string v = Getbiaodashizhi(bv);
                                expression = expression.Replace(bv.Name, v);
                            }
                        }
                        if(double.TryParse(new System.Data.DataTable().Compute(expression, null).ToString(), out var cr))
                        {
                            item.QuantitieString = Math.Round(cr, 2, MidpointRounding.AwayFromZero).ToString();
                        }
                    }
                });
            }
        }


        private static string Getbiaodashizhi(BudgetVar bv)
        {
            var v = string.Empty;
            if (bv.Method == "Value")
            {
                v = bv.GetValue();
            }
            else
            {
                var query = sh.Cad.EntityQuery.Compute(JsonConvert.DeserializeObject<EntityInfo>(bv.GetValue()));
                switch (bv.Method)
                {
                    case "Count":
                        v = query.Count.ToString();
                        break;
                    case "Length":
                        v = query.SumLength.ToString();
                        break;
                    case "Area":
                        v = query.SumArea.ToString();
                        break;
                }
            }

            return v;
        }
    }
}
