using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.Views;

namespace QuanLyDaiLy.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Register database
        services.AddSingleton<DatabaseService>();

        // Register pages and ViewModels (transient/scoped/singleton)
        services.AddScoped<IKhachHangRepo, KhachHangRepository>();
        services.AddScoped<ILoaiTietKiemRepo, LoaiTietKiemRepository>();
        services.AddScoped<IThamSoRepo, ThamSoRepository>();
        services.AddScoped<ISoTietKiemRepo, SoTietKiemRepository>();

        // Register ViewModels
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<ThemSoTietKiemViewModel>();
        services.AddTransient<DanhSachSoTietKiemViewModel>();
        services.AddTransient<CapNhatSoTietKiemViewModel>();

        // Register Views
        services.AddTransient<Dashboard>();
        services.AddTransient<ThemSoTietKiem>();
        services.AddTransient<DanhSachSoTietKiem>();
        services.AddTransient<CapNhatSoTietKiem>();

        // Register the main window (if needed)
        services.AddSingleton<MainWindow>();

        return services;
    }
}