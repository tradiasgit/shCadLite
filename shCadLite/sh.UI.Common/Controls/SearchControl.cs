
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

    public class SearchControl : ItemsControl
    {
        static SearchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchControl), new FrameworkPropertyMetadata(typeof(SearchControl)));
        }

        public SearchControl()
        {
            Command = new DelegateCommand<object>(p=>
            {
                if (Provider != null)
                {
                    //var page = 0;
                    //if (p is int) page = (int)p;
                    //var result = Provider.Search(Keywords, page, PageSize);
                    //if (result.IsSuccess)
                    //{
                    //    ItemsCount = result.TotalCount;
                    //    ItemsSource = result.Data;
                    //    PageIndex = page;
                    //}
                }
            });
        }

        



        #region 基础依赖属性

        public string Keywords
        {
            get { return (string)GetValue(KeywordsProperty); }
            set { SetValue(KeywordsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keywords.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeywordsProperty =
            DependencyProperty.Register("Keywords", typeof(string), typeof(SearchControl), new FrameworkPropertyMetadata());






        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(SearchControl), new FrameworkPropertyMetadata(0,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));





        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(SearchControl), new PropertyMetadata(10));




        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(SearchControl), new PropertyMetadata(0));













        #endregion

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SearchControl), new PropertyMetadata());


        #region 样式依赖属性


        public Brush SearchBrush
        {
            get { return (Brush)GetValue(SearchBrushProperty); }
            set { SetValue(SearchBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchBrushProperty =
            DependencyProperty.Register("SearchBrush", typeof(Brush), typeof(SearchControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(172, 172, 172))));

        #endregion

        
      

        public ISearchProvider Provider
        {
            get { return (ISearchProvider)GetValue(ProviderProperty); }
            set { SetValue(ProviderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Provider.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.Register("Provider", typeof(ISearchProvider), typeof(SearchControl), new PropertyMetadata());
    }
    


    public interface ISearchProvider
    {
        //sh.Common.BusinessModel.ISearchResult Search(string keywords, int pageindex = 0, int pagesize = 10,List<sh.Common.Data.IExpressionCondition> conditions=null);
    }

}
