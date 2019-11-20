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
    /// <summary>
    /// HeaderdTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderdTextBox : UserControl
    {
        public HeaderdTextBox()
        {
            InitializeComponent();


            //var adlayer = AdornerLayer.GetAdornerLayer(this);
            //adlayer.Add(new BorderAdorner(this));
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HeaderdTextBox), new PropertyMetadata(""));







        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeaderdTextBox), new PropertyMetadata("Header"));





        public int HeaderWidth
        {
            get { return (int)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(int), typeof(HeaderdTextBox), new PropertyMetadata(100));





        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalHeaderAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register("HorizontalHeaderAlignment", typeof(HorizontalAlignment), typeof(HeaderdTextBox), new PropertyMetadata(HorizontalAlignment.Center));

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            //AdornerLayer.GetAdornerLayer(this).Add(new Common.Controls.BorderAdorner(this));
        }
    }
}
