using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIElements
{
    /// <summary>
    /// Interaction logic for ImageDisplay.xaml
    /// </summary>
    public partial class ImageDisplay : UserControl
    {
        public ImageSource TitleLogo { get; set; }
        public ImageSource ExitButtonLogo { get; set; }
        public ImageSource Image { get; set; }
        public string TitleText { get; set; }
        public string BodyText { get; set; }

        public DoubleAnimation IntroAnimation { get; set; }
        public DoubleAnimation OutroAnimation { get; set; }
        public System.Windows.Controls.Button Button { get; set; }

        public ImageDisplay(string titleText, string bodyText, ImageSource title, ImageSource content, ImageSource exitImageSource)
        {
            
            InitializeComponent();
            TitleLogo = title;
            ExitButtonLogo = exitImageSource;
            button.Background = new ImageBrush(ExitButtonLogo);
           
            Image = content;
            image.Source = title;
            sourceImage.Source = content;
            TitleText = titleText;
            BodyText = bodyText;
            titleLabel.Content = TitleText;
            textBlock.Text = BodyText;
            Button = button;
            OutroAnimation = new DoubleAnimation();
            IntroAnimation = new DoubleAnimation();
            DoIntroAnimation();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DoOutroAnimation();
        }

        private void DoIntroAnimation()
        {
            IntroAnimation.From = -300;
            IntroAnimation.To = 0;

            IntroAnimation.DecelerationRatio = 0.9;
            IntroAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            TranslateTransform tr = new TranslateTransform();
            this.RenderTransform = tr;
            tr.BeginAnimation(TranslateTransform.XProperty, IntroAnimation);
        }

        private void DoOutroAnimation()
        {
            OutroAnimation.From = 0;
            OutroAnimation.To = -300;

            OutroAnimation.DecelerationRatio = 0.9;
            OutroAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            TranslateTransform tr = new TranslateTransform();
            this.RenderTransform = tr;
            tr.BeginAnimation(TranslateTransform.XProperty, OutroAnimation);
        }
    }
}