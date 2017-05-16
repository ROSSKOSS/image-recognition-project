using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HistogramDisplayWindow;

namespace Utilities
{
    public class Histogram
    {
        private ProgressBar ProgressReporter { get; set; }
        private System.Windows.Shapes.Polygon HistogramDisplay { get; set; }
        private Bitmap SourceImage { get; set; }
        public void CreateValuesDictionary(List<int> values,Bitmap sourceImage, ProgressBar reporter, System.Windows.Shapes.Polygon histogram)
        {
            var threadOne = new BackgroundWorker();
            SourceImage = (Bitmap)sourceImage.Clone();
            ProgressReporter = reporter;
            HistogramDisplay = histogram;
            threadOne.WorkerReportsProgress = true;
            threadOne.WorkerSupportsCancellation = true;
            threadOne.DoWork += CreateValuesDictionaryDoWork;
            threadOne.ProgressChanged += CreateValuesDictionaryProgressChanged;
            threadOne.RunWorkerCompleted += CreateValuesDictionaryCompleted;
            threadOne.RunWorkerAsync(values);
        }

        public void CreateValuesDictionaryDoWork(object sender, DoWorkEventArgs e)
        {
            var values = (List<int>)e.Argument;
            Dictionary<int, int> histogramValues = new Dictionary<int, int>();
            var count = 0;
            var distinct = values.Distinct().ToList();
            for (int i = 0; i < distinct.Count; i++)
            {
                for (int j = 0; j < values.Count; j++)
                {
                    if (distinct[i] == values[j])
                    {
                        count++;
                    }
                }
                histogramValues.Add(distinct[i], count);
                count = 0;
            }

            e.Result = histogramValues;
        }

        private void CreateValuesDictionaryCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var values = (Dictionary<int, int>)e.Result;
            var sortedDict = from entry in values orderby entry.Key ascending select entry;
            int max = values.Values.Max();

            PointCollection points = new PointCollection();
            PointCollection detailedPoints = new PointCollection();
            // first point (lower-left corner)
            points.Add(new System.Windows.Point(0, HistogramDisplay.Height));
            detailedPoints.Add(new System.Windows.Point(0, HistogramDisplay.Height));
            // middle points
            foreach (var value in sortedDict)
            {
                var heightDifference = value.Value;
                double heightPercent = Convert.ToDouble(heightDifference) / Convert.ToDouble(max);
                points.Add(new System.Windows.Point(value.Key, (HistogramDisplay.Height-(HistogramDisplay.Height * heightPercent))));
                detailedPoints.Add(new System.Windows.Point(value.Key, (HistogramDisplay.Height - (HistogramDisplay.Height * heightPercent))));
            }
            // last point (lower-right corner)
            points.Add(new System.Windows.Point(values.Count - 1, HistogramDisplay.Height));
            detailedPoints.Add(new System.Windows.Point(values.Count - 1, HistogramDisplay.Height));
            HistogramDisplay.Points = points;


           new LbpHistogramWindow(SourceImage, detailedPoints).Show();
        }

        private void CreateValuesDictionaryProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressReporter.Value = e.ProgressPercentage;
        }
    }
}