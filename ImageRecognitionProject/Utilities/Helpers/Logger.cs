using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Utilities.Helpers;

namespace Utilities
{
    public class Logger
    {
        public RichTextBox LogBox { get; set; }
        public Logger(RichTextBox logBox)
        {
            LogBox = logBox;
        }
        public void Report(string text, Brush color, FontWeight weight)
        {
            TextRange rangeOfText0 = new TextRange(LogBox.Document.ContentEnd, LogBox.Document.ContentEnd);
            rangeOfText0.Text = $"[{ DateTime.Now.TimeOfDay}]: ";
            rangeOfText0.ApplyPropertyValue(TextElement.ForegroundProperty, LoggerColors.TimeColor);
            rangeOfText0.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

            TextRange rangeOfText1 = new TextRange(LogBox.Document.ContentEnd, LogBox.Document.ContentEnd);
            rangeOfText1.Text = $"{text}\n";
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, weight);
          
        }

    }
}
