using System.Globalization;
using System.Windows.Data;

namespace QuanLyDaiLy.Helpers
{
    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOpen)
            {
                return isOpen ? "Đang mở" : "Đang đóng";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status == "Đang mở";
            }

            return false;
        }
    }
}