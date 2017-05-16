using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBinaryPattern
{
    public class LBP
    {
        public void Calculate(object sender, DoWorkEventArgs e)
        {
            var source = (Bitmap)e.Argument;
            int threshold = 0;
           
            var result = new List<int[,]>();
            var array = new int[3, 3];
            var bg = sender as BackgroundWorker;
            int percent = 0;
            int count = 0;
            int pace = (source.Height * source.Width) / 100;
            int separator = 0;
            for (int x = 1; x < source.Width - 1; x++)
            {
                for (int y = 1; y < source.Height - 1; y++)
                {
                    var pixel = source.GetPixel(x, y);
                    threshold = pixel.R;
                    count++;
                    if (count > pace)
                    {
                        count = 0;
                        separator++;
                        if (separator == 1)
                        {
                            separator = 0;
                            bg.ReportProgress(percent < 99 ? percent += 1 : percent = 100);
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {

                            var currentPixel = source.GetPixel(x - 1 + j, y - 1 + i);

                            if (currentPixel.R >= threshold)
                            {
                                array[i, j] = 1;
                            }
                            else
                            {
                                array[i, j] = 0;
                            }

                        }
                    }
                    result.Add(array);
                    array = new int[3, 3];
                }
            }
            e.Result = result;
        }
        public void GrayScale(object sender, DoWorkEventArgs e)
        {
            Bitmap source = ((Bitmap)e.Argument);
            var bg = sender as BackgroundWorker;
            int percent = 0;
            int count = 0;
            int pace = (source.Height * source.Width) / 100;
            int separator = 0;
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    count++;
                    if (count > pace)
                    {
                        count = 0;
                        separator++;
                        if (separator == 1)
                        {
                            separator = 0;
                            bg.ReportProgress(percent < 99 ? percent += 1 : percent = 100);
                        }
                    }
                    var pixel = source.GetPixel(x, y);
                    var average = (pixel.R + pixel.G + pixel.B) / 3;
                    source.SetPixel(x, y, Color.FromArgb(average, average, average));
                }
            }
            e.Result = source;
        }

    }
}
