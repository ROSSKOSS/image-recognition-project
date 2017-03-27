using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FancyButton
{
    partial class CustomButton : UserControl, INotifyPropertyChanged
    {
        private BrushConverter _bc = new BrushConverter();

        //INotifyPropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        private FontFamily _font = null;
        //Brush color when mouse enter
        private Brush _secondColor = null;
        public Brush SecondColor
        {
            get { return _secondColor; }
            set
            {
                _secondColor = value;
            }
        }

        //Label font family
        public FontFamily Font
        {
            get { return _font; }
            set
            {
                _font = value;
                OnPropertyChanged("Font");
            }
        }
        //Button radius
        private int _roundX = 0;
        private int _roundY = 0;

        public int RoundX
        {
            get { return _roundX; }
            set
            {
                _roundX = value;
                OnPropertyChanged("RoundX");
            }
        }
        public int RoundY
        {
            get { return _roundY; }
            set
            {
                _roundY = value;
                OnPropertyChanged("RoundY");
            }
        }
        //private brushes
        private Brush _fillColor = null;
        private Brush _strokeColor = null;
        private Brush _foregroundColor = null;

        private string _text = "Button";
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        public Brush FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                OnPropertyChanged("FillColor");
            }
        }
        public Brush StrokeColor
        {
            get { return _strokeColor; }
            set
            {
                _strokeColor = value;
                OnPropertyChanged("StrokeColor");
            }
        }

        public Brush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                OnPropertyChanged("ForegroundColor");
            }
        }

        public Brush HoverColor { get; set; } = null;
        public Brush DownColor { get; set; } = null;


        public CustomButton()
        {

            InitializeComponent();
            #region Initial preferences
            Width = 100;
            Height = 50;
            buttonBody.Width = 0;
            buttonBody.Height = 0;
            PropertyChanged += new PropertyChangedEventHandler(SomethingChanged);
            MouseEnter += CustomButton_MouseEnter;
            MouseLeave += CustomButton_MouseLeave;
            MouseDown += CustomButton_MouseDown;
            MouseUp += CustomButton_MouseUp;
            name.Content = Text;
            name.Margin = new Thickness(0, 0, 0, 0);
            FillColor = (Brush)_bc.ConvertFrom("#FFF4F4F5");
            StrokeColor = (Brush)_bc.ConvertFrom("#FF000000");
            name.HorizontalContentAlignment = HorizontalAlignment.Center;
            name.VerticalContentAlignment = VerticalAlignment.Center;
            #endregion
        }

        private void CustomButton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonBody.Fill = HoverColor;

        }
        private void CustomButton_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonBody.Fill = FillColor;
            name.Foreground = ForegroundColor;
        }
        private void CustomButton_MouseDown(object sender, MouseEventArgs e)
        {
            buttonBody.Fill = DownColor;
            name.Foreground = _secondColor;
        }
        private void CustomButton_MouseUp(object sender, MouseEventArgs e)
        {
            buttonBody.Fill = HoverColor;
            name.Foreground = ForegroundColor;
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            buttonBody.Width = Width;
            buttonBody.Height = Height;
            name.Width = Width;
            name.Height = Height;
            name.Margin = new Thickness(0, 0, 0, 0);
        }
        #region INotifyPropertyChanged methods
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SomethingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FillColor")
            {
                buttonBody.Fill = _fillColor;
            }
            if (e.PropertyName == "StrokeColor")
            {
                buttonBody.Stroke = _strokeColor;
            }
            if (e.PropertyName == "Text")
            {
                name.Content = _text;
            }
            if (e.PropertyName == "ForegroundColor")
            {
                name.Foreground = _foregroundColor;
            }
            if (e.PropertyName == "RoundX")
            {
                buttonBody.RadiusX = _roundX;
            }
            if (e.PropertyName == "RoundY")
            {
                buttonBody.RadiusY = _roundY;
            }
            if (e.PropertyName == "Font")
            {
                name.FontFamily = _font;
            }

        }
        #endregion
    }
}