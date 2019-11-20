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
    public class PropertyRenderer : ContentControl
    {
        static PropertyRenderer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyRenderer), new FrameworkPropertyMetadata(typeof(PropertyRenderer)));
        }


        public PropertyRenderer()
        {
            this.Focusable = true;
        }




        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PropertyRenderer), new PropertyMetadata("Header"));





        public int HeaderWidth
        {
            get { return (int)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(int), typeof(PropertyRenderer), new PropertyMetadata(100));





        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register("HorizontalHeaderAlignment", typeof(HorizontalAlignment), typeof(PropertyRenderer), new PropertyMetadata(HorizontalAlignment.Center));






        public Brush FocusBackground
        {
            get { return (Brush)GetValue(FocusBackgroundProperty); }
            set { SetValue(FocusBackgroundProperty, value); }
        }
        public static readonly DependencyProperty FocusBackgroundProperty =
            DependencyProperty.Register("FocusBackground", typeof(Brush), typeof(PropertyRenderer), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));




    }
}
