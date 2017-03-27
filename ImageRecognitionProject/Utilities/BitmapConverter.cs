using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Utilities
{
    public class BitmapConverter
    {
        public static ImageSource ToImageSource(Bitmap bmp)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                   IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromWidthAndHeight(bmp.Width,
                       bmp.Height));
        }
    }
}