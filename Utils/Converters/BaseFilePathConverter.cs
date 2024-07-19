﻿using System.Globalization;
using System.Windows.Data;

namespace MaterialDemo.Utils.Converters
{
    public class BaseFilePathConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return BaseFileUtil.GetOriFilePath((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
