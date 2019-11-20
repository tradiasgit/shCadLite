using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace sh.UI.Common
{
    public class BorderAdorner : Adorner
    {
        public BorderAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            var uc = adornedElement as UserControl;
            uc.GotFocus += (s1, e1) => InvalidateVisual();
            uc.LostFocus += (s1, e1) => InvalidateVisual();
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //找出控件所围成的矩形区域
            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

            var uc = AdornedElement as UserControl;
            Brush brush = new SolidColorBrush(Colors.Red);

            if (uc != null)
            {
                //3399ff
                if (uc.IsMouseOver)
                {
                    brush = new SolidColorBrush(Color.FromRgb(51, 153, 255));

                    Pen renderPen = new Pen(brush, 1);
                    var tl = adornedElementRect.TopLeft; tl.Offset(1, -1);
                    var tr = adornedElementRect.TopRight; tr.Offset(-1, -1);
                    var br = adornedElementRect.BottomRight;tr.Offset(-1, 0);
                    var bl = adornedElementRect.BottomLeft; tr.Offset(1,0);
                    drawingContext.DrawLine(renderPen, tl, tr);
                    drawingContext.DrawLine(renderPen, tr, br);
                    drawingContext.DrawLine(renderPen, br, bl);
                    drawingContext.DrawLine(renderPen, bl, tl);

                }

                else //8b8b8b
                    brush = new SolidColorBrush(Color.FromRgb(139, 139, 139));
            }

            


        }
    }
}
