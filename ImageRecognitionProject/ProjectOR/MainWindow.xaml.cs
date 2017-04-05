using Emgu.CV;
using Emgu.CV.Structure;
using FileOpener;
using LocalBinaryPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UIElements;
using UIElements.Controls;
using Utilities;
using Utilities.Helpers;
using Button = UIElements.Button;
using ContextMenu = UIElements.ContextMenu;

namespace ProjectOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private variables

        private ImageDisplay _imgDisplay;
        public static System.Windows.Controls.Image ImageDisplay { get; set; }

        // Describes current Context menu state
        private bool _contextMenuOpened = false;
        private List<int> valuesFromBitmap;
        private ImageAdjustmentMenu _adjustmentMenu;
        private Bitmap initialBitmap;
        private Bitmap _sourceImage;
        private Bitmap _tempBitmap;
        private UIElements.Trigger _trigger;
        public RichTextBox Log { get; set; }
        private Logger _logger;
        public static ProgressBar ProgressDisplay { get; set; }
        // Describes current logger state
        private bool _loggerHidden = true;
        public System.Windows.Shapes.Polygon Histogram { get; set; }
        #endregion private variables

        public MainWindow()
        {
            InitializeComponent();
            ProgressDisplay = progressBar;
            ImageDisplay = imageDetailedView;
            Histogram = histogram;
            LogRow.Height = new GridLength(0);
            Log = log;
            Log.Document.Blocks.Clear();
            LoggerColors.StartProcessColor = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF08B800");
            LoggerColors.MiscProcessColor = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF818181");
            LoggerColors.MiscProcessColor = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF03A9F4");
            LoggerColors.TimeColor = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FFD8D8D8");
            _logger = new Logger(Log);
            _logger.Report("Application started", LoggerColors.StartProcessColor, FontWeights.Medium);

            // Adding 'Add image' button to the Menu bar
            var addImageButton = new UIElements.Button(Double.NaN, menuBar.Height - 10, 3, 3, "Add Image", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(10, 3, 3, 3),
            };
            addImageButton.Content.Margin = new Thickness(5, 1, 5, 1);
            addImageButton.MouseLeftButtonUp += AddImageClick;
            menuBar.Children.Add(addImageButton);

            // Adding 'Help' button to the menu bar
            var button = new UIElements.Button(Double.NaN, menuBar.Height - 10, 3, 3, $"Help", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(5, 3, 3, 3),
            };
            button.Content.Margin = new Thickness(5, 1, 5, 1);
            menuBar.Children.Add(button);

            // Adding 'Logger' button to the menu bar
            var logButton = new UIElements.Button(Double.NaN, menuBar.Height - 10, 3, 3, $"Log", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(5, 3, 3, 3),
            };
            logButton.Content.Margin = new Thickness(5, 1, 5, 1);
            logButton.MouseLeftButtonUp += LogButtonMouseLeftButtonClick;
            menuBar.Children.Add(logButton);
        }



        // Event handler for 'Add image' button click event
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
            _tempBitmap = _sourceImage;
        }

        #region Image display

        private void SetUpImageDisplay()
        {
            Bitmap sourceBmp = BitmapOpener.Open();
            _logger.Report("File located", LoggerColors.MiscProcessColor, FontWeights.Medium);
            //var element = new Image<Gray, Byte>(BitmapOpener.FilePath);




            //imageDetailedView.Source =
            //    BitmapConverter.ToImageSource(FeatureMatching.FeatureMatcher.Match(element, scene));

            var titleSource = BitmapConverter.ToImageSource(Properties.Resources.titleLogo);
            var bodySource = BitmapConverter.ToImageSource(sourceBmp);
            var exitSource = BitmapConverter.ToImageSource(Properties.Resources.exitLogo);

            _imgDisplay = new ImageDisplay(BitmapOpener.FileName,
                $"File path: {BitmapOpener.FilePath.Replace(@"\\", "/")}.\nWidth: {sourceBmp.Width}px.\nHeight: {sourceBmp.Height}px.\nPixel amount: {sourceBmp.Width * sourceBmp.Height}.",
                titleSource, bodySource, exitSource)
            { Margin = new Thickness(0, 10, 0, 10) };

            _logger.Report("Image loaded", LoggerColors.MiscProcessColor, FontWeights.Medium);

            imageDetailedView.Source = BitmapConverter.ToImageSource(sourceBmp);

            imageDetailedView.Width = sourceBmp.Width;
            imageDetailedView.Height = sourceBmp.Height;
            _imgDisplay.MouseRightButtonUp += ImgDisplayRightMouseUp;
            _sourceImage = sourceBmp;

            imageGrid.Children.Add(_imgDisplay);
            _logger.Report("Image display spawned", LoggerColors.MiscProcessColor, FontWeights.Medium);
            _imgDisplay.OutroAnimation.Completed += CloseImageDisplay;
        }

        private void CloseImageDisplay(object sender, EventArgs e)
        {
            _logger.Report("Image closed", LoggerColors.MiscProcessColor, FontWeights.Medium);
            imageGrid.Children.Clear();
            imageDetailedView.Source = null;
            _adjustmentMenu.DoOutroAnimation();
            histogram.Points = null;
        }

        private void ImgDisplayRightMouseUp(object sender, MouseButtonEventArgs e)
        {
            var model = new List<Button>();
            Panel.SetZIndex(contextMenuHost, Int32.MaxValue);

            var button = new UIElements.Button(150, 25, 0, 0, "Find on a scene", 14, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(0, 0, 0, 0)
            };
            model.Add(button);

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
            var titleSource = BitmapConverter.ToImageSource(Properties.Resources.menuLogo);
            _adjustmentMenu = new ImageAdjustmentMenu("Menu", titleSource)
            {
                Width = 302,
                Height = Double.NaN,
            };
            adjustmentMenuHost.Children.Add(_adjustmentMenu);
            _adjustmentMenu.ApplyButton.MouseLeftButtonUp += AdjustmentMenuApplyButtonLeftButtonUp;
            _adjustmentMenu.ResetButton.MouseLeftButtonUp += AdjustmentMenuResetButtonLeftButtonUp;
            _adjustmentMenu.OutroAnimation.Completed += CloseAdjustmentMenu;
            SetUpAdjustmentControls();
        }

        private void AdjustmentMenuResetButtonLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _tempBitmap = _sourceImage;
            imageDetailedView.Source = BitmapConverter.ToImageSource(_tempBitmap);
        }

        private void AdjustmentMenuApplyButtonLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _sourceImage = (Bitmap)_tempBitmap.Clone();
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
            //var contrastControl = new AdjustmentControl("Contrast",
            //    BitmapConverter.ToImageSource(Properties.Resources.contrast_titleLogo));
            //contrastControl.Slider.ValueChanged += ContrastChanged;
            //_adjustmentMenu.Host.Children.Add(contrastControl);

            //var brightnessControl = new AdjustmentControl("Brightness",
            //    BitmapConverter.ToImageSource(Properties.Resources.brightness_titleLogo));
            //brightnessControl.Slider.ValueChanged += BrightnessChanged;
            //_adjustmentMenu.Host.Children.Add(brightnessControl);

            //_trigger = new UIElements.Trigger();
            //_trigger.Width = 40;
            //_trigger.Height = 20;   
            var titleSource = BitmapConverter.ToImageSource(Properties.Resources.settingsLogo);
            var imageAdjustmentSubMenu = new UIElements.Controls.SubMenu("Image adjustment", titleSource)
            {
                Margin = new Thickness(5, 10, 5, 3)
            };
            titleSource = BitmapConverter.ToImageSource(Properties.Resources.recognitionLogo);
            var imageRecognitionSubMenu = new UIElements.Controls.SubMenu("Image recognition", titleSource)
            {
                Margin = new Thickness(5, 10, 5, 3)
            };
            _adjustmentMenu.Host.Children.Add(imageAdjustmentSubMenu);
            _adjustmentMenu.Host.Children.Add(imageRecognitionSubMenu);

            var openCvObjectDetectionTrigger = new UIElements.Button(100, 50, 3, 3, "Open CV object\ndetection", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(5, 5, 5, 5)

            };
            imageRecognitionSubMenu.Container.Children.Add(openCvObjectDetectionTrigger);
            openCvObjectDetectionTrigger.MouseLeftButtonUp += openCvObjectDetectionTrigger_MouseLeftButtonUp;

            var contrastIncreaseTrigger = new UIElements.Controls.TriggerButton(80, 50, 3, 3, "Increase\nContrast", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(5, 5, 5, 5),
            };
            contrastIncreaseTrigger.MouseLeftButtonUp += contrastIncreaseTrigger_MouseLeftButtonUp;
            imageAdjustmentSubMenu.Container.Children.Add(contrastIncreaseTrigger);


            var binarizeTrigger = new UIElements.Button(80, 50, 3, 3, "Binarize\nimage", 13, ButtonColors.ButtonBackgroundColor,
                ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(0, 5, 5, 5),
            };
            binarizeTrigger.MouseLeftButtonUp += binarizeTrigger_MouseLeftButtonUp;
            imageAdjustmentSubMenu.Container.Children.Add(binarizeTrigger);

            var grayscaleTrigger = new UIElements.Button(80, 50, 3, 3, "Convert to\ngrayscale", 13, ButtonColors.ButtonBackgroundColor,
               ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(0, 5, 5, 5),
            };
            grayscaleTrigger.MouseLeftButtonUp += GrayscaleTrigger_MouseLeftButtonUp;
            imageAdjustmentSubMenu.Container.Children.Add(grayscaleTrigger);
            var buildHistogram = new UIElements.Button(80, 50, 3, 3, "Build\nhistogram", 13, ButtonColors.ButtonBackgroundColor,
             ButtonColors.ButtonHoverColor, ButtonColors.ButtonDownColor, ButtonColors.ButtonForegroundColor, ButtonColors.ButtonForegroundHoverColor, ButtonColors.ButtonForegroundDownColor)
            {
                Margin = new Thickness(5, 0, 5, 5),
            };
            buildHistogram.MouseLeftButtonUp += buildHistogram_MouseLeftButtonUp;
            imageAdjustmentSubMenu.Container.Children.Add(buildHistogram);

            // trigger.Margin = new Thickness(20, 50, 10, 10);
            //blckAndWhite.Host.Children.Add(_trigger);
            //_trigger.MouseLeftButtonUp += TriggerTriggered;
        }

        private void buildHistogram_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid.SetZIndex(histogram, Int32.MaxValue);
            new Histogram().CreateValuesDictionary(valuesFromBitmap, ProgressDisplay, Histogram);
        }

        private void GrayscaleTrigger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var threadOne = new BackgroundWorker();
            threadOne.WorkerReportsProgress = true;
            threadOne.WorkerSupportsCancellation = true;
            threadOne.DoWork += new LBP().GrayScale;
            threadOne.ProgressChanged += GrayscaleProgressChanged;
            threadOne.RunWorkerCompleted += GrayscaleCompleted;
            threadOne.RunWorkerAsync(_tempBitmap);
            _logger.Report("Converting image to grayscale.", LoggerColors.StartProcessColor, FontWeights.Medium);

        }

        private void GrayscaleCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imageDetailedView.Source = BitmapConverter.ToImageSource((Bitmap)e.Result);
            _tempBitmap = (Bitmap)e.Result;
            progressBar.Value = 0;

            var threadOne = new BackgroundWorker();
            threadOne.WorkerReportsProgress = true;
            threadOne.WorkerSupportsCancellation = true;
            threadOne.DoWork += new LBP().Calculate;
            threadOne.ProgressChanged += CalculateLBPProgressChanged;
            threadOne.RunWorkerCompleted += CalculateLBPCompleted;
            threadOne.RunWorkerAsync(_tempBitmap);
            _logger.Report("Calculating LBP.", LoggerColors.StartProcessColor, FontWeights.Medium);



        }

        private void CalculateLBPCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            var result = (List<int[,]>)e.Result;
            List<string> binaryValues = new List<string>();
            foreach (var array in result)
            {
                string binary = String.Empty;
                binary += array[0, 0];
                binary += array[0, 1];
                binary += array[0, 2];
                binary += array[1, 2];
                binary += array[2, 2];
                binary += array[2, 1];
                binary += array[2, 0];
                binary += array[1, 0];
                binaryValues.Add(binary);
            }

            var resultValues = new List<int>();
            foreach (var item in binaryValues)
            {
                resultValues.Add(Convert.ToInt32(item, 2));
            }
            var distinct = resultValues.Distinct().ToList();
            valuesFromBitmap = resultValues;
            _logger.Report("Calculating LBP finished.", LoggerColors.EndProcessColor, FontWeights.Medium);
        }

        private void CalculateLBPProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void GrayscaleProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void contrastIncreaseTrigger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _tempBitmap = BitmapAdjustment.ContrastAdjustment(_tempBitmap, 15.0f);
            imageDetailedView.Source = BitmapConverter.ToImageSource(_tempBitmap);
        }

        private void binarizeTrigger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var threadOne = new BackgroundWorker();
            threadOne.WorkerReportsProgress = true;
            threadOne.WorkerSupportsCancellation = true;
            threadOne.DoWork += new BitmapAdjustment().CollectColorsFromBitmap;
            threadOne.ProgressChanged += CollectColorsProgressChanged;
            threadOne.RunWorkerCompleted += CollectColorsCompleted;
            threadOne.RunWorkerAsync(_tempBitmap);
            _logger.Report("Collecting colors", LoggerColors.StartProcessColor, FontWeights.Medium);


        }

        private void openCvObjectDetectionTrigger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var element = new Image<Gray, Byte>(BitmapOpener.FilePath);
                Bitmap kekBmp = BitmapOpener.Open();
                var scene = new Image<Gray, Byte>(BitmapOpener.FilePath);
                imageDetailedView.Source =
                    BitmapConverter.ToImageSource(FeatureMatching.FeatureMatcher.Match(element, scene));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Open CV has thrown an exception. Check your image and try again.\nOriginal error:" + ex.Message);
            }
        }

        #region Binarize image

        private void TriggerTriggered(object sender, MouseButtonEventArgs e)
        {
            _sourceImage = (Bitmap)_tempBitmap.Clone();

            if (_trigger.State)
            {
                var threadOne = new BackgroundWorker();
                threadOne.WorkerReportsProgress = true;
                threadOne.WorkerSupportsCancellation = true;
                threadOne.DoWork += new BitmapAdjustment().CollectColorsFromBitmap;
                threadOne.ProgressChanged += CollectColorsProgressChanged;
                threadOne.RunWorkerCompleted += CollectColorsCompleted;
                threadOne.RunWorkerAsync(_tempBitmap);
                _logger.Report("Collecting colors", LoggerColors.StartProcessColor, FontWeights.Medium);
            }
            else if (!_trigger.State)
            {
                _logger.Report("Revert", LoggerColors.MiscProcessColor, FontWeights.Medium);

                imageDetailedView.Source = BitmapConverter.ToImageSource(_sourceImage);
            }
        }

        private void CollectColorsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (List<System.Windows.Media.Color>)e.Result;
            var model = new List<object> { _tempBitmap, result };
            var threadOne = new BackgroundWorker();
            threadOne.WorkerReportsProgress = true;
            threadOne.WorkerSupportsCancellation = true;
            threadOne.DoWork += new BitmapAdjustment().Binarize;
            threadOne.ProgressChanged += BinarizeProgressChanged;
            threadOne.RunWorkerCompleted += BinarizeWorkCompleted;
            threadOne.RunWorkerAsync(model);
        }

        private void CollectColorsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void BinarizeWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imageDetailedView.Source = BitmapConverter.ToImageSource((Bitmap)e.Result);
            _tempBitmap = (Bitmap)e.Result;
            progressBar.Value = 0;
        }

        private void BinarizeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        #endregion Binarize image

        //private void ContrastChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    float change = float.Parse(Convert.ToString(e.NewValue - e.OldValue));
        //    _tempBitmap = BitmapAdjustment.ContrastAdjustment(_tempBitmap, change);
        //    imageDetailedView.Source = BitmapConverter.ToImageSource(_tempBitmap);
        //}

        //private void BrightnessChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    double change = e.NewValue - e.OldValue;
        //    _tempBitmap = BitmapAdjustment.BrightnessAdjustment(_tempBitmap, change);
        //    imageDetailedView.Source = BitmapConverter.ToImageSource(_tempBitmap);
        //}

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

        private void LogButtonMouseLeftButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (_loggerHidden)
            {
                _logger.Report("Log displayed", LoggerColors.MiscProcessColor, FontWeights.Normal);
                logBody.Visibility = Visibility.Visible;
                LogRow.Height = new GridLength(204);
                _loggerHidden = false;
            }
            else if (!_loggerHidden)
            {
                _logger.Report("Log hidden", LoggerColors.MiscProcessColor, FontWeights.Medium);
                logBody.Visibility = Visibility.Hidden;
                LogRow.Height = new GridLength(0);
                _loggerHidden = true;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                }
            }
        }
    }
}