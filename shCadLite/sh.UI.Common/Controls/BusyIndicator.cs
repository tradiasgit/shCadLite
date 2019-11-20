using System;
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
using System.Windows.Threading;

namespace sh.UI.Common
{

    [TemplatePart(Name = "CNT", Type = typeof(Control))]
    [TemplatePart(Name = "MASK", Type = typeof(Border))]
    public class BusyIndicator : ContentControl
    {

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BusyIndicator), new PropertyMetadata("Waiting..."));
        public static DependencyProperty MaskTypeProperty = DependencyProperty.Register("MaskType", typeof(MaskTypes), typeof(BusyIndicator), new PropertyMetadata(MaskTypes.Adorned));
        public static DependencyProperty ContentControlTemplateProperty = DependencyProperty.Register("ContentControlTemplate", typeof(ControlTemplate), typeof(BusyIndicator));

        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public MaskTypes MaskType
        {
            get
            {
                return (MaskTypes)this.GetValue(MaskTypeProperty);
            }
            set
            {
                this.SetValue(MaskTypeProperty, value);
            }
        }

        public ControlTemplate ContentControlTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ContentControlTemplateProperty);
            }
            set
            {
                this.SetValue(ContentControlTemplateProperty, value);
            }
        }

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            this.DataContext = this;
        }
        public override void OnApplyTemplate()
        {
            var mask = this.Template.FindName("MASK", this) as Border;
            mask.Visibility = this.MaskType != MaskTypes.None ? Visibility.Visible : Visibility.Collapsed;
            if (this.ContentControlTemplate != null)
            {
                var tp = this.Template.FindName("CNT", this) as Control;
                tp.Template = this.ContentControlTemplate;
            }
        }
    }




    public enum MaskTypes
    {
        None,
        Adorned,
        Window
    }
    public class Busy
    {

        #region Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(Busy), new PropertyMetadata(TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Update((FrameworkElement)d);
        }

        public static void SetText(FrameworkElement target, string value)
        {
            target.SetValue(TextProperty, value);
        }

        public static string GetText(FrameworkElement target)
        {
            return (string)target.GetValue(TextProperty);
        }
        #endregion

        #region MaskType
        public static readonly DependencyProperty MaskTypeProperty = DependencyProperty.RegisterAttached("MaskType", typeof(MaskTypes), typeof(Busy), new PropertyMetadata(MaskTypes.Adorned, MaskTypeChanged));

        private static void MaskTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Update((FrameworkElement)d);
        }

        public static void SetMaskType(FrameworkElement target, MaskTypes type)
        {
            target.SetValue(MaskTypeProperty, type);
        }

        public static MaskTypes GetMaskType(FrameworkElement target)
        {
            return (MaskTypes)target.GetValue(MaskTypeProperty);
        }
        #endregion

        #region Show
        public static readonly DependencyProperty ShowProperty = DependencyProperty.RegisterAttached("Show", typeof(bool), typeof(Busy), new PropertyMetadata(false, ShowChanged));
        private static void ShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Update((FrameworkElement)d);
        }

        public static void SetShow(FrameworkElement target, bool value)
        {
            target.SetValue(ShowProperty, value);
        }

        public static bool GetShow(FrameworkElement target)
        {
            return (bool)target.GetValue(ShowProperty);
        }
        #endregion

        #region adorner
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof(BusyIndicatorAdorner), typeof(Busy));

        public static void SetAdorner(FrameworkElement target, BusyIndicatorAdorner adorner)
        {
            target.SetValue(AdornerProperty, adorner);
        }

        public static BusyIndicatorAdorner GetAdorner(FrameworkElement target)
        {
            return (BusyIndicatorAdorner)target.GetValue(AdornerProperty);
        }

        #endregion

        #region
        public static readonly DependencyProperty ContentControlTemplateProperty = DependencyProperty.Register("ContentControlTemplate", typeof(ControlTemplate), typeof(Busy));

        public static ControlTemplate GetContentControlTemplate(FrameworkElement target)
        {
            return (ControlTemplate)target.GetValue(ContentControlTemplateProperty);
        }

        public static void SetContentControlTemplate(FrameworkElement target, ControlTemplate ctrl)
        {
            target.SetValue(ContentControlTemplateProperty, ctrl);
        }

        #endregion

        private static void Update(FrameworkElement target)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            if (layer == null)
            {
                //Dispatcher.CurrentDispatcher.BeginInvoke(new Action<FrameworkElement>(o => Update(o)), DispatcherPriority.Loaded, target);
                return;
            }

            var text = GetText(target);
            var show = GetShow(target);
            var maskType = GetMaskType(target);
            var template = GetContentControlTemplate(target);

            var adorner = GetAdorner(target);

            if (show)
            {
                if (adorner == null)
                {
                    adorner = new BusyIndicatorAdorner(target)
                    {
                        MaskType = maskType,
                        ContentControlTemplate = template,
                        Text = text
                    };
                    layer.Add(adorner);
                    SetAdorner(target, adorner);
                }
                else
                {
                    adorner.MaskType = maskType;
                    adorner.ContentControlTemplate = template;
                    adorner.Text = text;
                }
                //adorner.Visibility = Visibility.Visible;
            }
            else
            {
                if (adorner != null)
                {
                    layer.Remove(adorner);
                    //如果不 Remove 并设置为 null, 在 使用AvalonDock的程序里，切换标签会使 adorner 的 Parent 丢失
                    //如果设置为 null ，会在再一次显示的时候，重建
                    //adorner.Visibility = Visibility.Collapsed;
                    SetAdorner(target, null);
                }
            }
        }
    }
    public class BusyIndicatorAdorner : Adorner
    {

        #region maskType
        private MaskTypes maskType;
        public MaskTypes MaskType
        {
            get
            {
                return this.maskType;
            }
            set
            {
                this.maskType = value;
                this.Indicator.MaskType = value;
            }
        }
        #endregion

        #region Text
        private string text;
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.Indicator.Text = value;
            }
        }
        #endregion

        #region contentControlTemplate
        private ControlTemplate contentControlTemplate;
        public ControlTemplate ContentControlTemplate
        {
            get
            {
                return this.contentControlTemplate;
            }
            set
            {
                this.contentControlTemplate = value;
                this.Indicator.ContentControlTemplate = value;
            }
        }
        #endregion

        private BusyIndicator Indicator;

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            return this.Indicator;
        }

        public BusyIndicatorAdorner(FrameworkElement adornered)
            : base(adornered)
        {

            this.Indicator = new BusyIndicator();
            this.AddVisualChild(this.Indicator);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size s = Size.Empty;
            switch (this.MaskType)
            {
                case MaskTypes.None:
                case MaskTypes.Adorned:
                    this.AdornedElement.Measure(constraint);
                    var ele = this.AdornedElement as FrameworkElement;
                    s = new Size(ele.ActualWidth, ele.ActualHeight);
                    //s = this.AdornedElement.DesiredSize;
                    break;
                case MaskTypes.Window:
                    var w = GetParentWindow(this.AdornedElement);
                    s = new Size(w.ActualWidth, w.ActualHeight - ((SystemParameters.ResizeFrameHorizontalBorderHeight * 2) + SystemParameters.WindowCaptionHeight));
                    break;
            }
            return s;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Indicator.Measure(finalSize);
            this.Indicator.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (this.MaskType == MaskTypes.Window)
            {
                //变换Adorner 的起点
                var w = GetParentWindow(this.AdornedElement);
                var a = this.AdornedElement.TransformToAncestor(w);
                var b = a.Transform(new Point(0, 0));

                TransformGroup group = new TransformGroup();
                var t = new TranslateTransform(-b.X, -b.Y + ((SystemParameters.ResizeFrameHorizontalBorderHeight * 2) + SystemParameters.WindowCaptionHeight));
                group.Children.Add(t);

                group.Children.Add(transform as Transform);
                return group;
            }
            else
            {
                return base.GetDesiredTransform(transform);
            }
        }

        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }
    }

}
