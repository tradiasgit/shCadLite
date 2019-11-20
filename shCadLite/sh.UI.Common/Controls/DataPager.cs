
using sh.UI.Common.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sh.UI.Common
{
    

    public class DataPager : Control
    {
        #region 构造及数据源
        static DataPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPager), new FrameworkPropertyMetadata(typeof(DataPager)));
        }

        public DataPager()
        {
            Cmd_NextPage = new DelegateCommand<object>(p =>
            {
                PageIndex++;
            }, p =>
             {
                 return PageIndex + 1 < PageCount;
             });
            Cmd_PreviousPage = new DelegateCommand<object>(p =>
            {
                PageIndex--;
            }, p =>
             {
                 return PageIndex > 0;
             });


        }
        #endregion




        public void GotoPage()
        {
            Command?.Execute(PageIndex);
            RaiseCommandAvailable();
            
        }

        #region 分页字段


        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,  (sender, e) => ((DataPager)sender).GotoPage()));





        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new PropertyMetadata(1));




        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((DataPager)sender).PageIndex=0));



        protected void CountPageCount()
        {
            PageCount = ItemsCount / PageSize;
            if (ItemsCount % PageSize > 0) PageCount++;
            RaiseCommandAvailable();
        }



        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }


        //PageCount = ItemsCount / PageSize;
    //            if (ItemsCount % PageSize > 0) PageCount++;
        // Using a DependencyProperty as the backing store for ItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(DataPager), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((DataPager)sender).CountPageCount()));


        #endregion



        #region Commands

        public void RaiseCommandAvailable()
        {
            Cmd_PreviousPage.RaiseCanExecuteChanged();
            Cmd_NextPage.RaiseCanExecuteChanged();
        }


        public IDelegateCommand Cmd_PreviousPage
        {
            get { return (IDelegateCommand)GetValue(Cmd_PreviousPageProperty); }
            private set { SetValue(Cmd_PreviousPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cmd_PreviousPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Cmd_PreviousPageProperty =
            DependencyProperty.Register("Cmd_PreviousPage", typeof(IDelegateCommand), typeof(DataPager), new PropertyMetadata());





        public IDelegateCommand Cmd_NextPage
        {
            get { return (IDelegateCommand)GetValue(Cmd_NextPageProperty); }
            private set { SetValue(Cmd_NextPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cmd_NextPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Cmd_NextPageProperty =
            DependencyProperty.Register("Cmd_NextPage", typeof(IDelegateCommand), typeof(DataPager), new PropertyMetadata());

        #endregion



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DataPager), new PropertyMetadata());





    }
}
