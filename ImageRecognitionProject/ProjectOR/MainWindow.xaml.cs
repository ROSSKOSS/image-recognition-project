using FileOpener;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using UIElements;
using Utilities;
using Button = UIElements.Button;
using ContextMenu = UIElements.ContextMenu;

namespace ProjectOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageDisplay _imgDisplay;
        private bool _contextMenuOpened = false;
        private ImageAdjustmentMenu _adjustmentMenu;
        private Bitmap _sourceImage;

        public MainWindow()
        {
            InitializeComponent();
            var addImageButton = new UIElements.Button(Double.NaN, menuBar.Height - 10, 3, 3, "Add Image", 13, "#FF0288D1",
                "#4fc3f7", "#0277bd", "#FFFFFFFF", "#FF000000", "#FFFFFFFF")
            {
                Margin = new Thickness(10, 3, 3, 3)
            };
            addImageButton.MouseLeftButtonUp += AddImageClick;
            menuBar.Children.Add(addImageButton);

            var button = new UIElements.Button(Double.NaN, menuBar.Height - 10, 3, 3, $"Help", 13, "#FF0288D1",
                "#4fc3f7", "#0277bd", "#FFFFFFFF", "#FF000000", "#FFFFFFFF")
            {
                Margin = new Thickness(5, 3, 3, 3),
            };

            menuBar.Children.Add(button);
        }

        private void AddImageClick(object sender, MouseButtonEventArgs e)
        {
            imageGrid.Children.Clear();
            adjustmentMenuHost.Children.Clear();
            imageDetailedView.Source = null;
            try
            {
                SetUpImageDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Image couldn't be set up.\nOriginal error:" + ex.Message);
                return;
            }
            try
            {
                SetUpAdjustmentMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adjustment menu has thrown an exception. Check your image and try again.\nOriginal error:" + ex.Message);
                return;
            }
            label2.Content = null;
        }

        #region Image display

        private void SetUpImageDisplay()
        {
            Bitmap sourceBmp = BitmapOpener.Open();
            var titleSource = BitmapConverter.ToImageSource(Properties.Resources.titleLogo);
            var bodySource = BitmapConverter.ToImageSource(sourceBmp);
            var exitSource = BitmapConverter.ToImageSource(Properties.Resources.exitLogo);
            _imgDisplay = new ImageDisplay(BitmapOpener.FileName,
                $"File path: {BitmapOpener.FilePath.Replace(@"\\", "/")}.\nWidth: {sourceBmp.Width}px.\nHeight: {sourceBmp.Height}px.\nPixel amount: {sourceBmp.Width * sourceBmp.Height}.",
                titleSource, bodySource, exitSource)
            { Margin = new Thickness(0, 10, 0, 10) };
            imageDetailedView.Source = BitmapConverter.ToImageSource(sourceBmp);
            imageDetailedView.Width = sourceBmp.Width;
            imageDetailedView.Height = sourceBmp.Height;
            _imgDisplay.MouseRightButtonUp += ImgDisplayRightMouseUp;
            _sourceImage = sourceBmp;
            imageGrid.Children.Add(_imgDisplay);
            _imgDisplay.OutroAnimation.Completed += CloseImageDisplay;
        }

        private void CloseImageDisplay(object sender, EventArgs e)
        {
            imageGrid.Children.Clear();
            imageDetailedView.Source = null;
            _adjustmentMenu.DoOutroAnimation();
        }

        private void ImgDisplayRightMouseUp(object sender, MouseButtonEventArgs e)
        {
            var model = new List<Button>();
            Panel.SetZIndex(contextMenuHost, Int32.MaxValue);
            for (int i = 0; i < 10; i++)
            {
                var button = new UIElements.Button(150, 25, 0, 0, $"Option #{i}", 14, "#FF0288D1",
               "#FFFFFFFF", "#0277bd", "#FFFFFFFF", "#FF000000", "#FFFFFFFF")
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };
                model.Add(button);
            }

            var contextMenu = new ContextMenu(model)
            {
                Margin = new Thickness(Mouse.GetPosition(contextMenuHost).X, Mouse.GetPosition(contextMenuHost).Y, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 150,
                Height = 300
            };
            Grid.SetZIndex(contextMenu, Int32.MaxValue);
            contextMenuHost.Children.Clear();
            contextMenuHost.Children.Add(contextMenu);
        }

        #endregion Image display

        #region Adjustment menu

        private void SetUpAdjustmentMenu()
        {
            var titleSource = BitmapConverter.ToImageSource(Properties.Resources.settingsLogo);
            _adjustmentMenu = new ImageAdjustmentMenu("Image adjustment", titleSource)
            {
                Width = 302,
                Height = Double.NaN,
            };
            adjustmentMenuHost.Children.Add(_adjustmentMenu);
            _adjustmentMenu.OutroAnimation.Completed += CloseAdjustmentMenu;
            SetUpAdjustmentControls();
        }

        private void CloseAdjustmentMenu(object sender, EventArgs e)
        {
            adjustmentMenuHost.Children.Clear();
            contextMenuHost.Children.Clear();
            label2.Content = "Nothing to display";
        }

        #endregion Adjustment menu

        #region Filling the adjustment menu

        private void SetUpAdjustmentControls()
        {
            var contrastControl = new AdjustmentControl("Contrast",
                BitmapConverter.ToImageSource(Properties.Resources.contrast_titleLogo));
            contrastControl.Slider.ValueChanged += ContrastChanged;
            _adjustmentMenu.Host.Children.Add(contrastControl);
            var brightnessControl = new AdjustmentControl("Brightness",
                BitmapConverter.ToImageSource(Properties.Resources.brightness_titleLogo));
            brightnessControl.Slider.ValueChanged += BrightnessChanged;
            _adjustmentMenu.Host.Children.Add(brightnessControl);
            UIElements.Trigger trigger = new UIElements.Trigger();
            trigger.Width = 40;
            trigger.Height = 20;
            var blckAndWhite = new BlackAndWhiteControl("B&W",
                BitmapConverter.ToImageSource(Properties.Resources.bw_titleLogo));

            _adjustmentMenu.Host.Children.Add(blckAndWhite);
            blckAndWhite.Host.Children.Add(trigger);
        }

        private void ContrastChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float change = float.Parse(Convert.ToString(e.NewValue - e.OldValue));
            _sourceImage = BitmapAdjustment.ContrastAdjustment(_sourceImage, change);
            imageDetailedView.Source = BitmapConverter.ToImageSource(_sourceImage);
        }

        private void BrightnessChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double change = e.NewValue - e.OldValue;
            _sourceImage = BitmapAdjustment.BrightnessAdjustment(_sourceImage, change);
            imageDetailedView.Source = BitmapConverter.ToImageSource(_sourceImage);
        }

        #endregion Filling the adjustment menu

        #region Context menu closing actions

        private void mainHost_MouseUp(object sender, MouseButtonEventArgs e)
        {
            contextMenuHost.Children.Clear();
        }

        private void contextMenuHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            contextMenuHost.Children.Clear();
        }

        private void Father_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            contextMenuHost.Children.Clear();
        }

        #endregion Context menu closing actions
    }
}