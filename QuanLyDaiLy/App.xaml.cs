﻿using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Extensions;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using System.Windows;

namespace QuanLyDaiLy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        services.ConfigureServices(); 
        ServiceProvider = services.BuildServiceProvider();

        var dbService = ServiceProvider.GetRequiredService<DatabaseService>();
        await dbService.Initialize();

        //var dashboard = ServiceProvider.GetRequiredService<Dashboard>();
        //dashboard.Show();

        var danhSachSoTietKiem = ServiceProvider.GetRequiredService<DanhSachSoTietKiem>();
        danhSachSoTietKiem.Show();
    }
}
