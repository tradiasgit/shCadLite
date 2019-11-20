
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sh.UI.Common
{
    public class WaterMarkTextBox : TextBox
    {

        static WaterMarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaterMarkTextBox), new FrameworkPropertyMetadata(typeof(WaterMarkTextBox)));
        }
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WaterMarkTextBox), new PropertyMetadata("请输入关键字..."));



        #region Label


        public object Label
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }


        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(object), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnLabelChanged));

        private static void OnLabelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnLabelChanged(oldValue, newValue);
        }



        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public DataTemplate LabelTemplate
        {
            get { return (DataTemplate)GetValue(LabelTemplateProperty); }
            set { SetValue(LabelTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnLabelTemplateChanged));

        private static void OnLabelTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnLabelTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnLabelChanged(object oldValue, object newValue) { }

        protected virtual void OnLabelTemplateChanged(DataTemplate oldValue, DataTemplate newValue) { }



        #endregion

        #region Attach

        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public object Attach
        {
            get { return (DataTemplate)GetValue(AttachProperty); }
            set { SetValue(AttachProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.Register("Attach", typeof(object), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnAttachChanged));

        private static void OnAttachChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnAttachChanged(oldValue, newValue);
        }


        protected virtual void OnAttachChanged(object oldValue, object newValue) { }






        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public DataTemplate AttachTemplate
        {
            get { return (DataTemplate)GetValue(AttachTemplateProperty); }
            set { SetValue(AttachTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AttachTemplateProperty =
            DependencyProperty.Register("AttachTemplate", typeof(DataTemplate), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnAttachTemplateChanged));

        private static void OnAttachTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnAttachTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnAttachTemplateChanged(DataTemplate oldValue, DataTemplate newValue) { }

        #endregion


        public IDelegateCommand Cmd_ClearText
        {
            get
            {
                return new DelegateCommand<object>(p=>Text="");
            }
        }




        public ObservableCollection<string> Keys
        {
            get { return (ObservableCollection<string>)GetValue(KeysProperty); }
            set { SetValue(KeysProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeysProperty =
            DependencyProperty.Register("Keys", typeof(ObservableCollection<string>), typeof(WaterMarkTextBox), new PropertyMetadata(new ObservableCollection<string>() { "沙发", "300*500", "位" }));








        public bool ShowPopup
        {
            get { return (bool)GetValue(ShowPopupProperty); }
            set { SetValue(ShowPopupProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowPopup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPopupProperty =
            DependencyProperty.Register("ShowPopup", typeof(bool), typeof(WaterMarkTextBox), new PropertyMetadata(false));





        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public object Popup
        {
            get { return (DataTemplate)GetValue(PopupProperty); }
            set { SetValue(PopupProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PopupProperty =
            DependencyProperty.Register("Popup", typeof(object), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnPopupChanged));

        private static void OnPopupChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnPopupChanged(oldValue, newValue);
        }


        protected virtual void OnPopupChanged(object oldValue, object newValue) { }


        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public DataTemplate PopupTemplate
        {
            get { return (DataTemplate)GetValue(PopupTemplateProperty); }
            set { SetValue(PopupTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PopupTemplateProperty =
            DependencyProperty.Register("PopupTemplate", typeof(DataTemplate), typeof(WaterMarkTextBox), new PropertyMetadata(null, OnPopupTemplateChanged));

        private static void OnPopupTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            WaterMarkTextBox target = obj as WaterMarkTextBox;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnPopupTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnPopupTemplateChanged(DataTemplate oldValue, DataTemplate newValue) { }


    }
}
