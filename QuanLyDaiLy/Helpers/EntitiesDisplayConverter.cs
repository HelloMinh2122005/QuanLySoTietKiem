using System;
using System.Globalization;
using System.Windows.Data;

namespace QuanLyDaiLy.Helpers
{
    public class EntityDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value is null, return empty string
            if (value == null)
                return string.Empty;

            // If parameter is specified, it's a format string
            if (parameter != null)
            {
                if (value is DateTime dt)
                    return dt.ToString(parameter.ToString(), culture);
                else if (value is decimal dec)
                    return dec.ToString(parameter.ToString(), culture);
                else if (value is double dbl)
                    return dbl.ToString(parameter.ToString(), culture);
            }

            // For all other cases, return the string representation
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}