using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

public class RowIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DataGridRow row)
        {
            return row.GetIndex() + 1; // +1 to start from 1 instead of 0
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}