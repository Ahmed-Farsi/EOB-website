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
	public class Bool_To_String_Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			switch ((string)parameter)
            {
				case "subscription":
					return (bool)value ? "Active" : "Expired";

				case "online":
					return (bool)value ? "Online" : "Offline";

				default:
					return null;
            }
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
