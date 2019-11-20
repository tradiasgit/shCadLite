//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace sh.UI.Common
//{
   
//    public class HeaderedTextBox : TextBox
//    {
//        static HeaderedTextBox()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedTextBox), new FrameworkPropertyMetadata(typeof(HeaderedTextBox)));
//        }

//        public object Header
//        {
//            get { return (object)GetValue(HeaderProperty); }
//            set { SetValue(HeaderProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty HeaderProperty =
//            DependencyProperty.Register("Header", typeof(object), typeof(HeaderedTextBox), new PropertyMetadata(null, OnHeaderChanged));

//        private static void OnHeaderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
//        {
//            HeaderedTextBox target = obj as HeaderedTextBox;
//            object oldValue = (object)args.OldValue;
//            object newValue = (object)args.NewValue;
//            if (oldValue != newValue)
//                target.OnHeaderChanged(oldValue, newValue);
//        }



//        /// <summary>
//        /// 获取或设置HeaderTemplate的值
//        /// </summary>  
//        public DataTemplate HeaderTemplate
//        {
//            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
//            set { SetValue(HeaderTemplateProperty, value); }
//        }

//        /// <summary>
//        /// 标识 HeaderTemplate 依赖属性。
//        /// </summary>
//        public static readonly DependencyProperty HeaderTemplateProperty =
//            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedTextBox), new PropertyMetadata(null, OnHeaderTemplateChanged));

//        private static void OnHeaderTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
//        {
//            HeaderedTextBox target = obj as HeaderedTextBox;
//            DataTemplate oldValue = (DataTemplate)args.OldValue;
//            DataTemplate newValue = (DataTemplate)args.NewValue;
//            if (oldValue != newValue)
//                target.OnHeaderTemplateChanged(oldValue, newValue);
//        }

//        protected virtual void OnHeaderChanged(object oldValue, object newValue) { }

//        protected virtual void OnHeaderTemplateChanged(DataTemplate oldValue, DataTemplate newValue) { }

//    }
//}
