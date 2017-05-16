using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;


namespace HistogramDisplayWindow
{
    /// <summary>
    /// Interaction logic for LbpHistogramWindow.xaml
    /// </summary>
    public partial class LbpHistogramWindow : Window
    {

        public Func<double, string> YFormatter { get; set; }
        public LbpHistogramWindow(Bitmap leftImage, PointCollection leftHistogramPoints)
        {
            InitializeComponent();
            var histogramValues = new LineSeries();
            histogramValues.Values = new ChartValues<double>();
            histogramValues.LineSmoothness = 0;
            foreach (var leftHistogramPoint in leftHistogramPoints)
            {
                histogramValues.Values.Add(-leftHistogramPoint.Y);
            }
            histogramValues.PointGeometry = null;

            var seriesCollection = new SeriesCollection() { histogramValues };


            var labelsX = new List<string>();
            foreach (var leftHistogramPoint in leftHistogramPoints)
            {
                labelsX.Add(leftHistogramPoint.X.ToString());
            }
            YFormatter = value => $"{-value}";
            histogram.Series = seriesCollection;
            histogram.AxisX = new AxesCollection() { new Axis() { Title = "Brightness value", Labels = labelsX, Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF000000") } };
            histogram.AxisY = new AxesCollection() { new Axis() { Title = "Amount", ShowLabels = false, LabelFormatter = YFormatter, Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF000000") } };
            //sourceImage.Source = Imaging.CreateBitmapSourceFromHBitmap(leftImage.GetHbitmap(),
            //       IntPtr.Zero, Int32Rect.Empty,
            //       BitmapSizeOptions.FromWidthAndHeight(leftImage.Width,
            //           leftImage.Height)); ;
        }
    }
}
