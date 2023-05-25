using MvvmCross.Converters;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Engineers_App.WPF.Views.Converters
{
	public class Local_Bytes_To_Bitmap_Converter : IValueConverter
	{
		private static WriteableBitmap bitmap;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{ 
				bitmap = null;
				return null;
			}

            using var mat = Cv2.ImDecode(value as byte[], ImreadModes.Color);

            if (bitmap == null)
            {
				// allocate the bitmap once in order to get the resolution, pixel format etc.
                bitmap = WriteableBitmapConverter.ToWriteableBitmap(mat);
            }

			// unsafe method, but atleast it doesn't allocate a trillion bitmaps
            WriteableBitmapConverter.ToWriteableBitmap(mat, bitmap);

			return bitmap;
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
