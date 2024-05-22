﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaterialDemo.Utils.Converters
{
    public class BaseBoolConverter : IValueConverter
    {
        //Convert方法中的value参数，为绑定源数据，此例中为int类型
        //如果value为0，则返回false，否则返回true
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Equals((string)value, BaseConstant.SUCCESS);
        }
        //ConvertBack方法的value参数，为绑定目标数据，此例中为bool类型
        //如果value为true，则返回1，否则返回0
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? BaseConstant.SUCCESS : BaseConstant.FAIL;
        }
    }
}
