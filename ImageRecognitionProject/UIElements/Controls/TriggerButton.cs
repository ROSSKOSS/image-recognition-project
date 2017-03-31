using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace UIElements.Controls
{
    public class TriggerButton : Button
    {
        public bool State { get; set; }
        public TriggerButton(double width, double height, int roundX, int roundY, string text, int textSize, string backgroundHex, string hoverHex, string downHex, string foregroundHex, string foregroundHoverHex, string foregroundDownHex)
        {
            _brushConverter = new BrushConverter();
            State = false;
            ControlWidth = width;
            ControlHeight = height;
            RoundX = roundX;
            RoundY = roundY;
            Text = text;
            TextSize = textSize;
            BackgroundColor = (Brush)_brushConverter.ConvertFrom(backgroundHex);
            HoverBrush = (Brush)_brushConverter.ConvertFrom(hoverHex);
            BGColor = (Color)ColorConverter.ConvertFromString(backgroundHex);
            HoverColor = (Color)ColorConverter.ConvertFromString(hoverHex);
            ForeColor = (Color)ColorConverter.ConvertFromString(foregroundHex);
            HoverForeColor = (Color)ColorConverter.ConvertFromString(foregroundHoverHex);
            DownColor = (Brush)_brushConverter.ConvertFrom(downHex);
            ForegroundColor = (Brush)_brushConverter.ConvertFrom(foregroundHex);
            ForegroundHoverColor = (Brush)_brushConverter.ConvertFrom(foregroundHoverHex);
            ForegroundDownColor = (Brush)_brushConverter.ConvertFrom(foregroundDownHex);
            MouseEnter += MouseEnterMethod;
            MouseLeave += MouseLeaveMethod;
            MouseLeftButtonDown += MouseDownMethod;
            MouseLeftButtonUp += MouseUpMethod;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            IntitalizeComponent();

            
        }

        private void MouseLeaveMethod(object sender, MouseEventArgs e)
        {
            if (State == false)
            {
                ChangeColor(BGColor);
            }
        }

        public void MouseEnterMethod(object sender, MouseEventArgs e)
        {
            if (State == false)
            {
                ChangeColor(HoverColor);
            }
        }

        public void MouseUpMethod(object sender, MouseButtonEventArgs e)
        {

            if (State == false)
            {
                Body.Fill = DownColor;
                State = true;
            }
            else if (State == true)
            {
                Body.Fill = HoverBrush;
                State = false;
            }
        }
        public void MouseDownMethod(object sender, MouseButtonEventArgs e)
        {
            Body.Fill = DownColor;
            if (State == false)
            {

            }
        }

    }

}
