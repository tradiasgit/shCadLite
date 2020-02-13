﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using sh.BudgetTableEditor.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class MainWindowViewModel: ViewModelBase
    {
        private readonly Tools.BudgetTableFileHelper _budgetTableFileHelper;

        private ObservableCollection<LeftMenuItem> _leftMenuItems;

        public ObservableCollection<LeftMenuItem> LeftMenuItems
        {
            get { return _leftMenuItems; }
            set { Set(ref _leftMenuItems, value); }
        }

        private object _subContent;

        public object SubContent
        {
            get { return _subContent; }
            set { Set(ref _subContent, value); }
        }

        private string _contentTitle;

        public string ContentTitle
        {
            get { return _contentTitle; }
            set { Set(ref _contentTitle, value); }
        }


        void ExecuteClickCommnd()
        {
           
        }

        public MainWindowViewModel(Tools.BudgetTableFileHelper budgetTableFileHelper)
        {
            LeftMenuItems = new ObservableCollection<LeftMenuItem>();
            LeftMenuItems.Add(new LeftMenuItem { Name = "预算",ImagePath= "\xf1ec" });
            LeftMenuItems.Add(new LeftMenuItem { Name = "变量", ImagePath = "\xf27d" });
            LeftMenuItems.Add(new LeftMenuItem { Name = "分组", ImagePath = "\xf1a0" });

            _budgetTableFileHelper = budgetTableFileHelper;
        }

        public RelayCommand<object> SwitchContent
        {
            get
            {
                return new RelayCommand<object>(p => 
                {
                    if(p is LeftMenuItem lmi)
                    {
                        switch (lmi.Name)
                        {
                            case "预算":
                                ContentTitle = "预算表";
                                SubContent = new BudgetItemTable();
                                break;
                            case "变量":
                                ContentTitle = "变量表";
                                SubContent = new BudgetVarTable();
                                break;
                            case "分组":
                                ContentTitle = "分组表";
                                SubContent = new BudgetGroupTable();
                                break;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        public RelayCommand AddBudgetVar
        {
            get
            {
                return new RelayCommand( () =>
                {
                    var win = new BudgetVarAdd();
                    win.ShowDialog();
                });
            }
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        public RelayCommand AddBudgetGroup
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var win = new BudgetGroupAdd();
                    win.ShowDialog();
                });
            }
        }

        /// <summary>
        /// 添加预算
        /// </summary>
        public RelayCommand AddBudgetItem
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var win = new BudgetItemAdd();
                    win.ShowDialog();
                });
            }
        }

    }

    class LeftMenuItem
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }
    }

}
