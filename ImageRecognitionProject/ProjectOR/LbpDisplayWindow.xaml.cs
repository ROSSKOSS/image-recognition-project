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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using Utilities;
using Point = System.Windows.Point;

namespace ProjectOR
{
    /// <summary>
    /// Interaction logic for LbpDisplayWindow.xaml
    /// </summary>
    public partial class LbpDisplayWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public LbpDisplayWindow()
        {

        }
        public LbpDisplayWindow(Bitmap leftImage, Polygon leftHistogram)
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<Double>() {34,12,5,64},
                    PointGeometry = null
                }
            };
         
            var labelsX = new List<string>() { "11", "12", "13", "14" };
            histogram.Series = SeriesCollection;
            histogram.AxisX = new AxesCollection() { new Axis() { Title = "Brightness value", Labels = labelsX, Foreground =(System.Windows.Media.Brush)new BrushConverter().ConvertFrom("00000") } };
            histogram.AxisY = new AxesCollection() { new Axis() { Title = "Brightness value", Labels = labelsX, Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("00000") } };
            sourceImage.Source = BitmapConverter.ToImageSource(leftImage);
        }
    }
}
