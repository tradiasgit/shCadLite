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

namespace sh.Creator.Controls
{
   
    public class AutoTagTextBox : TextBox
    {
        static AutoTagTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoTagTextBox), new FrameworkPropertyMetadata(typeof(AutoTagTextBox)));
        }




        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(AutoTagTextBox), new PropertyMetadata(""));




        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(AutoTagTextBox), new PropertyMetadata(""));






        public ControlTemplate AttachContent
        {
            get { return (ControlTemplate)GetValue(AttachContentProperty); }
            set { SetValue(AttachContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AttachContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachContentProperty =
            DependencyProperty.Register("AttachContent", typeof(ControlTemplate), typeof(AutoTagTextBox), new PropertyMetadata(null));



    }
}
