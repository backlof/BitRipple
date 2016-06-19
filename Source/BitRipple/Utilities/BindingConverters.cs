﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BitRipple.Utilities
{
    [ValueConversion(typeof(double), typeof(DataGridLength))]
    public class DataGridWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridLengthConverter dglc = new DataGridLengthConverter();
            double val = (double)value;
            return dglc.ConvertFrom(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DataGridLength)value).DisplayValue;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = value == null? default(DateTime) : (DateTime)value;
            String format = parameter as string;

            if (date.Equals(default(DateTime)))
            {
                return "None";
            }

            return date.ToString(format, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse((string)value, CultureInfo.CurrentCulture);
        }
    }

    [ValueConversion(typeof(double), typeof(GridLength))]
    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return new GridLength(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength val = (GridLength)value;

            return val.Value;
        }
    }
}
