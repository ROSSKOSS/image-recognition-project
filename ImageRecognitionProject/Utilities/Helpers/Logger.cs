using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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
            TextRange rangeOfText1 = new TextRange(LogBox.Document.ContentEnd, LogBox.Document.ContentEnd);
            rangeOfText1.Text = text;
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, weight);
          
        }

    }
}
