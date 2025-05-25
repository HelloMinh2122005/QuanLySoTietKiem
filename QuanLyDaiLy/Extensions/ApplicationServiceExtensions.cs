using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.BaoCaoDoanhSoViews;
using QuanLyDaiLy.Views.BaoCaoDongMoViews;
using QuanLyDaiLy.Views.DashboardViews;
using QuanLyDaiLy.Views.KhachHangViews;
using QuanLyDaiLy.Views.PhieuGoiTienViews;
using QuanLyDaiLy.Views.PhieuRutTienViews;
using QuanLyDaiLy.Views.ThamSoView;

namespace QuanLyDaiLy.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Register database
        services.AddSingleton<DatabaseService>();
        
        // Đăng ký DataContext từ DatabaseService
        services.AddDbContext<DataContext>(options =>
        {
            var dbPath = DatabaseService.GetDefaultDatabasePath();
            options.UseSqlite($"Data Source={dbPath}");
        });

        // Register pages and ViewModels (transient/scoped/singleton)
        services.AddScoped<IKhachHangRepo, KhachHangRepository>();
        services.AddScoped<ILoaiTietKiemRepo, LoaiTietKiemRepository>();
        services.AddScoped<IThamSoRepo, ThamSoRepository>();
        services.AddScoped<ISoTietKiemRepo, SoTietKiemRepository>();
        services.AddScoped<IPhieuGoiTienRepo, PhieuGoiTienRepository>();

        // Register ViewModels
        services.AddTransient<ThemSoTietKiemViewModel>();
        services.AddTransient<DanhSachSoTietKiemViewModel>();
        services.AddTransient<CapNhatSoTietKiemViewModel>();

        services.AddTransient<PhieuGoiTienViewModel>();
        services.AddTransient<ThemKhachHangViewModel>();
        services.AddTransient<CapNhatKhachHangViewModel>();
        services.AddTransient<KhachHangViewModel>();
        services.AddTransient<ThemPhieuGoiTienViewModel>();

        // Register Views
        services.AddTransient<ThemSoTietKiem>();
        services.AddTransient<DanhSachSoTietKiem>();
        services.AddTransient<CapNhatSoTietKiem>();
        services.AddTransient<DashboardPage>();
        services.AddTransient<KhachHangPage>();
        services.AddTransient<ThemKhachHang>();
        services.AddTransient<CapNhatKhachHang>();
        services.AddTransient<DsPhieuGoiTien>();
        services.AddTransient<lapPhieuGoiTien>();
        services.AddTransient<BaoCaoDoanhSoPage>();
        services.AddTransient<BaoCaoDongMoPage>();
        services.AddTransient<DsPhieuRutTienPage>();
        services.AddTransient<LapPhieuRutTienWindow>();
        services.AddTransient<ThamSoPage>();
        services.AddTransient<TraCuuSoTietKiem>();

        // Register the main window (if needed)
        services.AddSingleton<MainWindow>();

        return services;
    }
}