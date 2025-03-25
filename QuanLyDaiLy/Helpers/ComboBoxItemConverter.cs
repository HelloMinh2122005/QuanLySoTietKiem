using QuanLyDaiLy.Entities;
using System.Globalization;
using System.Windows.Data;

namespace QuanLyDaiLy.Helpers
{
    public class ComboBoxItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value is LoaiTietKiem loaiTietKiem)
                return loaiTietKiem.TenLoaiTietKiem;

            //if (value is  quan)
            //    return quan.TenQuan;

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This converter is only used for display, so we don't need to implement ConvertBack
            throw new NotImplementedException();
        }
    }
}
