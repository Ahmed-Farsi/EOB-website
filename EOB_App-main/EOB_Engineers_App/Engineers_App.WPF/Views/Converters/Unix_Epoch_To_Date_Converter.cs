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
	public class Unix_Epoch_To_Date_Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var date = DateTimeOffset.FromUnixTimeMilliseconds((long)value).DateTime;

			if (date == DateTime.UnixEpoch)
				return "Never";
			else
				return date.ToShortDateString();
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
