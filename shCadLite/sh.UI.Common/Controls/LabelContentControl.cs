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

namespace sh.UI.Common
{
 
    public class LabelContentControl : HeaderedContentControl
    {
        static LabelContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelContentControl), new FrameworkPropertyMetadata(typeof(LabelContentControl)));
        }


        public int ContentColumnSpan
        {
            get { return (int)GetValue(ContentColumnSpanProperty); }
            set { SetValue(ContentColumnSpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentColumnSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentColumnSpanProperty =
            DependencyProperty.Register("ContentColumnSpan", typeof(int), typeof(LabelContentControl), new PropertyMetadata(2));





        public int LabelWidth
        {
            get { return (int)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(int), typeof(LabelContentControl), new PropertyMetadata(100));




        public Brush HeaderBorderBrush
        {
            get { return (Brush)GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register("HeaderBorderBrush", typeof(Brush), typeof(LabelContentControl), new PropertyMetadata(null));




    }
}
