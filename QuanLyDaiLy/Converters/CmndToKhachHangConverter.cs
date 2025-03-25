using System.Globalization;
using System.Windows.Data;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;

public class CmndToKhachHangConverter : IValueConverter
{
    private IKhachHangRepo _khachHangRepo;

    // Default constructor for XAML
    public CmndToKhachHangConverter() { }

    // Constructor for DI
    public CmndToKhachHangConverter(IKhachHangRepo khachHangRepo)
    {
        _khachHangRepo = khachHangRepo;
    }

    public IKhachHangRepo KhachHangRepo
    {
        get => _khachHangRepo;
        set => _khachHangRepo = value;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (_khachHangRepo == null)
        {
            var databaseService = new DatabaseService();
            databaseService.Initialize(); // Make sure this method exists
            _khachHangRepo = new KhachHangRepository(databaseService);
        }

        if (value is string cmnd)
        {
            var khachHang = _khachHangRepo.FindByCMND(cmnd).Result;
            return khachHang?.TenKhachHang ?? "Không xác định";
        }
        return "Không xác định";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}