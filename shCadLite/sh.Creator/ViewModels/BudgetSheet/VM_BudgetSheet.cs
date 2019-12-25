using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.Cad;
using sh.Creator.Views;
using sh.UI.Common.MVVM;
using sh.XmlResourcesParsing.Fields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ICollectionView collectionView;
        private void Initialize()
        {
            // 所有预算和所有变量
            var list = BudgetGroup.GetAll();
            var budgetVars = BudgetVar.GetAll();

            BudgetItems = new ObservableCollection<VM_BudgetItem>();

            foreach (var itemGroup in list)
            {
                if (itemGroup.BudgetItems == null) continue;
                foreach (var item in itemGroup.BudgetItems)
                {
                    var vb = new VM_BudgetItem(item) { GroupName = itemGroup.Name };
                    BudgetItems.Add(vb);

                    var expression = item.Expression;
                    budgetVars.ForEach(bv =>
                    {
                        if (expression.Contains(bv.Name))
                            expression = expression.Replace(bv.Name, bv.GetQuantities());
                    });
                    try
                    {
                        var gcl = new System.Data.DataTable().Compute(expression, null).ToString();
                        if(gcl!="False")
                            vb.QuantitieString = gcl;
                    }
                    catch 
                    {

                    }
                }
            }

            collectionView = CollectionViewSource.GetDefaultView(BudgetItems);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
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
                    var vm = new VM_AddBudget(m=> {
                        BudgetItems.Add(m);
                    });
                    vm.Show();
                });
            }
        }

        public ICommand Cmd_Refresh
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Initialize();
                });
            }
        }
    }
}
