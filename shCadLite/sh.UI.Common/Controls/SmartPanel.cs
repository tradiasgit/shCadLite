using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sh.UI.Common
{
    public class SmartPanel : StackPanel
    {
        static SmartPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmartPanel), new FrameworkPropertyMetadata(typeof(SmartPanel)));
        }

        public SmartPanel()
        {
        }




        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(SmartPanel), new PropertyMetadata(0));










        //protected override Size ArrangeOverride(Size arrangeSize)
        //{
        //    if (double.IsInfinity(arrangeSize.Height) || double.IsInfinity(arrangeSize.Width))
        //    {
        //        throw new InvalidOperationException("容器的宽和高必须是确定值");
        //    }

        //    if (Children.Count > 0)
        //    {
        //        double childAverageWidth = arrangeSize.Width / Children.Count;
        //        for (int childIndex = 0; childIndex < InternalChildren.Count; childIndex++)
        //        {
        //            // 计算子元素将被安排的布局区域
        //            var rect = new Rect(childIndex * childAverageWidth, 0, childAverageWidth, arrangeSize.Height);
        //            InternalChildren[childIndex].Arrange(rect);
        //        }
        //    }

        //    return base.ArrangeOverride(arrangeSize);
        //}

        //protected override Size MeasureOverride(Size constraint)
        //{
        //    foreach (UIElement child in InternalChildren)
        //    {
        //        child.Measure(constraint);   // 测量子元素期望布局尺寸(child.DesiredSize)
        //    }
        //    return base.MeasureOverride(constraint);
        //}
    }
}
