using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UIElements
{
    public class Button : UserControl
    {
        public double ControlWidth { get; set; }
        public Rectangle Body { get; set; }
        public TextBlock Content { get; set; }
        public double ControlHeight { get; set; }
        public int RoundX { get; set; }
        public int RoundY { get; set; }
        public string Text { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush ForegroundColor { get; set; }
        public Brush HoverBrush { get; set; }
        public Color HoverColor { get; set; }
        public Color BGColor { get; set; }
        public Color HoverForeColor { get; set; }
        public Color ForeColor { get; set; }
        public Brush DownColor { get; set; }
        public int TextSize { get; set; }
        public Brush ForegroundHoverColor { get; set; }
        public Brush ForegroundDownColor { get; set; }

        private BrushConverter _brushConverter;

        public Button(double width, double height, int roundX, int roundY, string text, int textSize, string backgroundHex, string hoverHex, string downHex, string foregroundHex, string foregroundHoverHex, string foregroundDownHex)
        {
            _brushConverter = new BrushConverter();
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
            IntitalizeComponent();
        }

        private void MouseUpMethod(object sender, MouseButtonEventArgs e)
        {
            ChangeColor(HoverColor);
            ChangeForegroundColor(HoverForeColor);
        }

        private void MouseLeaveMethod(object sender, MouseEventArgs e)
        {
            ChangeColor(BGColor);
            ChangeForegroundColor(ForeColor);
        }

        private void ChangeColor(Color to)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();

            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Body.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void ChangeForegroundColor(Color to)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();

            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Content.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void MouseDownMethod(object sender, MouseButtonEventArgs e)
        {
            Body.Fill = DownColor;
            Content.Foreground = ForegroundDownColor;
        }

        private void MouseEnterMethod(object sender, MouseEventArgs e)
        {
            ChangeColor(HoverColor);
            ChangeForegroundColor(HoverForeColor);
        }

        private void IntitalizeComponent()
        {
            var grid = new System.Windows.Controls.Grid()
            {
                Width = ControlWidth,
                Height = ControlHeight,
                Background = null
            };

            grid.Children.Add(SetUpRectangle());
            grid.Children.Add(SetUpTextBlock());
            AddChild(grid);
        }

        private Rectangle SetUpRectangle()
        {
            var brushConverter = new BrushConverter();
            Body = new Rectangle()
            {
                Width = ControlWidth,
                Height = ControlHeight,
                Stroke = null,
                Fill = BackgroundColor,
                RadiusX = RoundX,
                RadiusY = RoundY
            };

            return Body;
        }

        private TextBlock SetUpTextBlock()
        {
            Content = new TextBlock()
            {
                Width = ControlWidth,
                Height = ControlHeight,
                Foreground = ForegroundColor,
                Background = null,
                Text = Text,

                TextAlignment = TextAlignment.Center,

                Margin = new Thickness(5, 1, 5, 1),
                FontSize = TextSize,
                FontFamily = new FontFamily("Malgun Gothic Semilight")
            };

            return Content;
        }
    }
}