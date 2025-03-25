using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy;
using QuanLyDaiLy.Interfaces;

public class MaLoaiToTenConverter : IValueConverter
{
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;

    public MaLoaiToTenConverter()
    {
        // Lấy ServiceProvider từ Application.Current
        var serviceProvider = (IServiceProvider)App.Current.Resources["ServiceProvider"];
        _loaiTietKiemRepo = serviceProvider.GetService<ILoaiTietKiemRepo>();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string maLoai && _loaiTietKiemRepo != null)
        {
            var result = _loaiTietKiemRepo.FindTheoMaLoaiTietKiemAsync(maLoai).Result;
            return result.TenLoaiTietKiem;
        }
        return "N/A";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}