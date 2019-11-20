using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace sh.UI.Common
{
    public class DropDownMenuButton: Button
    {
        static DropDownMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownMenuButton), new FrameworkPropertyMetadata(typeof(DropDownMenuButton)));
        }

        public DropDownMenuButton()
        {
            
        }

        private ContextMenu menu;


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            menu = ContextMenu;
            ContextMenu = null;
        }

        protected override void OnClick()
        {
            base.OnClick();
            //目标
            menu.PlacementTarget = this;
            //位置
            menu.Placement = PlacementMode.Bottom;
            //显示菜单
            menu.IsOpen = true;
        }

    
    }
}
