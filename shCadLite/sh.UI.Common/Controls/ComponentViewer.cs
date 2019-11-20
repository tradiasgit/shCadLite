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
 
    public class ComponentViewer : ItemsControl
    {
        static ComponentViewer()
        {
            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComponentViewer), new FrameworkPropertyMetadata(typeof(ComponentViewer)));
        }



        public void Init()
        {
        }

        public string VMKey
        {
            get { return (string)GetValue(VMKeyProperty); }
            set { SetValue(VMKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VMKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VMKeyProperty =
            DependencyProperty.Register("VMKey", typeof(string), typeof(ComponentViewer), new PropertyMetadata());


    }
}
