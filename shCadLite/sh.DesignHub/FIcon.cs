using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sh.DesignHub
{
    public class FIcon : Control
    {
        static FIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FIcon), new FrameworkPropertyMetadata(typeof(FIcon)));
            
        }

        #region FIcon

        [AttachedPropertyBrowsableForType(typeof(TextBlock))]
        [AttachedPropertyBrowsableForType(typeof(Label))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(ToggleButton))]
        public static string GetFIcon(DependencyObject obj)
        {
            return (string)obj.GetValue(FIconProperty);
        }

        public static void SetFIcon
            (DependencyObject obj, string value)
        {
            obj.SetValue(FIconProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FIconProperty =
            DependencyProperty.RegisterAttached("FIcon", typeof(string), typeof(FIcon), new PropertyMetadata(""));


        #endregion

        #region FIconSize
        [AttachedPropertyBrowsableForType(typeof(TextBlock))]
        [AttachedPropertyBrowsableForType(typeof(Label))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(ToggleButton))]
        public static int GetFIconSize(DependencyObject obj)
        {
            return (int)obj.GetValue(FIconSizeProperty);
        }

        public static void SetFIconSize(DependencyObject obj, int value)
        {
            obj.SetValue(FIconSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for FIconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FIconSizeProperty =
            DependencyProperty.RegisterAttached("FIconSize", typeof(int), typeof(FIcon), new PropertyMetadata(18));


        #endregion
        #region AllowsAnimation


        //[AttachedPropertyBrowsableForType(typeof(TextBlock))]
        //public static bool GetAllowsAnimation(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(AllowsAnimationProperty);
        //}

        //public static void SetAllowsAnimation(DependencyObject obj, bool value)
        //{
        //    obj.SetValue(AllowsAnimationProperty, value);
        //}

        //// Using a DependencyProperty as the backing store for AllowsAnimation.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AllowsAnimationProperty =
        //    DependencyProperty.RegisterAttached("AllowsAnimation", typeof(bool), typeof(FIcon), new PropertyMetadata(true));


        #endregion

        #region FIconMargin






        [AttachedPropertyBrowsableForType(typeof(TextBlock))]
        [AttachedPropertyBrowsableForType(typeof(Label))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(ToggleButton))]
        public static Thickness GetFIconMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(FIconMarginProperty);
        }

        public static void SetFIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(FIconMarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for FIconMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FIconMarginProperty =
            DependencyProperty.RegisterAttached("FIconMargin", typeof(Thickness), typeof(FIcon), new PropertyMetadata(new Thickness(0,0,0,0)));

        #endregion

    }
}
