using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class BitmapAdjustment
    {
        public static Bitmap BrightnessAdjustment(Bitmap source, double changeD)
        {
            int change = Convert.ToInt32(changeD);
            Bitmap temp = (Bitmap)source;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (change < -255) change = -255;
            if (change > 255) change = 255;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int cR = c.R + change;
                    int cG = c.G + change;
                    int cB = c.B + change;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bmap.SetPixel(i, j,
        Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                }
            }
            source = (Bitmap)bmap.Clone();
            return source;
        }
        public static Bitmap ContrastAdjustment(Bitmap source, float changeD)
        {
            changeD = (100.0f + changeD) / 100.0f;
            changeD *= changeD;
            Bitmap NewBitmap = (Bitmap)source.Clone();
            BitmapData data = NewBitmap.LockBits(
                new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                ImageLockMode.ReadWrite,
                NewBitmap.PixelFormat);
            int Height = NewBitmap.Height;
            int Width = NewBitmap.Width;

            unsafe
            {
                for (int y = 0; y < Height; ++y)
                {
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);
                    int columnOffset = 0;
                    for (int x = 0; x < Width; ++x)
                    {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        float Red = R / 255.0f;
                        float Green = G / 255.0f;
                        float Blue = B / 255.0f;
                        Red = (((Red - 0.5f) * changeD) + 0.5f) * 255.0f;
                        Green = (((Green - 0.5f) * changeD) + 0.5f) * 255.0f;
                        Blue = (((Blue - 0.5f) * changeD) + 0.5f) * 255.0f;

                        int iR = (int)Red;
                        iR = iR > 255 ? 255 : iR;
                        iR = iR < 0 ? 0 : iR;
                        int iG = (int)Green;
                        iG = iG > 255 ? 255 : iG;
                        iG = iG < 0 ? 0 : iG;
                        int iB = (int)Blue;
                        iB = iB > 255 ? 255 : iB;
                        iB = iB < 0 ? 0 : iB;

                        row[columnOffset] = (byte)iB;
                        row[columnOffset + 1] = (byte)iG;
                        row[columnOffset + 2] = (byte)iR;

                        columnOffset += 4;
                    }
                }
            }

            NewBitmap.UnlockBits(data);

            return NewBitmap;
        }
        public  void Binarize(object sender, DoWorkEventArgs e)
        {
            var argumentList = (List<object>)e.Argument;

            var source = (Bitmap)argumentList[0];
            var pixelAmount = source.Width * source.Height;
            var uniqueColors = (List<System.Windows.Media.Color>)argumentList[1];
            var bg = sender as BackgroundWorker;
            int percent = 0;
            int count = 0;
            int pace = (source.Height * source.Width) / 100;
            int separator = 0;

            double middleR = 0, middleG = 0, middleB = 0;

            foreach (var color in uniqueColors)
            {
                if (color.R != 255 && color.G != 255 && color.B != 255 && color.R != 0 && color.G != 0 && color.B != 0)
                {
                    middleR += Convert.ToDouble(color.R) / uniqueColors.Count;
                    middleG += Convert.ToDouble(color.G) / uniqueColors.Count;
                    middleB += Convert.ToDouble(color.B) / uniqueColors.Count;
                }

            }
            for (int i = 0; i < source.Height; i++)
            {
                for (int j = 0; j < source.Width; j++)
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

                    var pixel = source.GetPixel(j, i);
                    if (pixel.A != 0)
                    {
                        var R = pixel.R + pixel.G + pixel.B > middleR + middleB + middleG ? 255 : 0;
                        var G = pixel.R + pixel.G + pixel.B > middleR + middleB + middleG ? 255 : 0;
                        var B = pixel.R + pixel.G + pixel.B > middleR + middleB + middleG ? 255 : 0;

                        source.SetPixel(j, i, Color.FromArgb(R, G, B));
                    }



                }
            }
            e.Result = source;
        }
        public void CollectColorsFromBitmap(object sender, DoWorkEventArgs e)
        {
            List<System.Windows.Media.Color> colorArray = new List<System.Windows.Media.Color>();
            Bitmap bitmap = (Bitmap)e.Argument;
            var bg = sender as BackgroundWorker;
            double pace = Convert.ToDouble(bitmap.Width * bitmap.Height) / 100;
            int count = 0;
            int percent = 0;
            int separator = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    count++;
                    if (bg.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
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

                    var pixel = bitmap.GetPixel(j, i);
                    //var cnt = new HashSet<System.Drawing.Color>();
                    //cnt.Add(pixel);



                    var current = System.Windows.Media.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);

                    colorArray.Add(current);

                }
            }
            e.Result = colorArray.Distinct().ToList();
        }
    }
}
