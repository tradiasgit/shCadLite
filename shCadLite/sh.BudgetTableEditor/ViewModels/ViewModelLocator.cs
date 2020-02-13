using GalaSoft.MvvmLight.Ioc;
using MessageBox= HandyControl.Controls.MessageBox;
using Microsoft.Extensions.Configuration;
using sh.BudgetTableEditor.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace sh.BudgetTableEditor.ViewModels
{
    class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // 默认在这里注册就可以了;
            SimpleIoc.Default.Register<BudgetTableFileHelper>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<BudgetVarTableViewModel>();
            SimpleIoc.Default.Register<BudgetGroupTableViewModel>();
        }

        /// <summary>
        /// 这里对应的是View中DataContext需要的依赖属性;
        /// 就是这个Main属性，关联起了View和ViewModel;
        /// DataContext="{Binding Source={StaticResource Locator}, Path=Main}
        /// </summary>
        public MainWindowViewModel Main => SimpleIoc.Default.GetInstance<MainWindowViewModel>();

        public BudgetVarTableViewModel BudgetVarTable => SimpleIoc.Default.GetInstance<BudgetVarTableViewModel>();

        public BudgetGroupTableViewModel BudgetGroupTable => SimpleIoc.Default.GetInstance<BudgetGroupTableViewModel>();




        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
