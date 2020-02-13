using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.Configuration;
using sh.BudgetTableEditor.Tools;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        /// <summary>
        /// 这里对应的是View中DataContext需要的依赖属性;
        /// 就是这个Main属性，关联起了View和ViewModel;
        /// DataContext="{Binding Source={StaticResource Locator}, Path=Main}
        /// </summary>
        public MainWindowViewModel Main => SimpleIoc.Default.GetInstance<MainWindowViewModel>();


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
