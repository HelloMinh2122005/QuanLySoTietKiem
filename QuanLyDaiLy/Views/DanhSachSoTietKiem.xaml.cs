using QuanLyDaiLy.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views.BaoCaoDoanhSoViews;
using QuanLyDaiLy.Views.BaoCaoDongMoViews;
using QuanLyDaiLy.Views.CustomAnimation;
using QuanLyDaiLy.Views.KhachHangViews;
using QuanLyDaiLy.Views.PhieuGoiTienViews;
using QuanLyDaiLy.Views.PhieuRutTienViews;
using QuanLyDaiLy.Views.ThamSoView;

namespace QuanLyDaiLy.Views
{
    public partial class DanhSachSoTietKiem : Window
    {
        private readonly double collapsedWidth = 60;
        private readonly double expandedWidth = 200;
        private readonly IServiceProvider _serviceProvider;

        public DanhSachSoTietKiem(DanhSachSoTietKiemViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            DataContext = viewModel;

            // configure the window
            WindowState = WindowState.Maximized;

            // Ensure NavColumn is properly set
            if (NavColumn != null)
            {
                NavColumn.Width = new GridLength(collapsedWidth);
            }

            Loaded += (s, e) =>
            {
                NavigateToPage("SoTietKiem");
            };
        }

        private void MainContent_ContentRendered(object sender, EventArgs e)
        {
            // This handler is called when content is rendered in MainContent frame
            // You can add logic here if needed
        }

        private void NavigationRail_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateNavDrawerWidth(expandedWidth);
        }

        private void NavigationRail_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateNavDrawerWidth(collapsedWidth);
        }

        private void AnimateNavDrawerWidth(double targetWidth)
        {
            // Check if NavColumn exists before animating
            if (NavColumn == null)
                return;

            var duration = new Duration(TimeSpan.FromMilliseconds(300));

            double currentWidth = NavColumn.ActualWidth;

            var animation = new GridLengthAnimation
            {
                Duration = duration,
                From = new GridLength(currentWidth),
                To = new GridLength(targetWidth)
            };

            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.Tag is string pageName)
                {
                    NavigateToPage(pageName);
                }
            }
        }

        private void NavigateToPage(string pageName)
        {
            // Ensure MainContent exists
            if (MainContent == null)
                return;

            MainContent.Visibility = Visibility.Collapsed;

            switch (pageName)
            {
                case "SoTietKiem":
                    MainContent.Content = null;
                    break;
                case "KhachHang":
                    var khachHangPage = _serviceProvider.GetRequiredService<KhachHangPage>();
                    MainContent.Navigate(khachHangPage);
                    break;
                case "PhieuGoiTien":
                    var DsPhieuGoiTien = _serviceProvider.GetRequiredService<DsPhieuGoiTien>();
                    MainContent.Navigate(DsPhieuGoiTien);
                    break;
                case "PhieuRutTien":
                    var DsPhieuRutTien = _serviceProvider.GetRequiredService<DsPhieuRutTienPage>();
                    MainContent.Navigate(DsPhieuRutTien);
                    break;
                case "BaoCaoDoanhSo":
                    var baoCaoDoanhSoPage = _serviceProvider.GetRequiredService<BaoCaoDoanhSoPage>();
                    MainContent.Navigate(baoCaoDoanhSoPage);
                    break;
                case "BaoCaoDongMo":
                    var baoCaoDongMoPage = _serviceProvider.GetRequiredService<BaoCaoDongMoPage>();
                    MainContent.Navigate(baoCaoDongMoPage);
                    break;
                case "ThamSo":
                    var thamSoPage = _serviceProvider.GetRequiredService<ThamSoPage>();
                    MainContent.Navigate(thamSoPage);
                    break;
                default:
                    break;
            }

            // Now set it back to visible
            MainContent.Visibility = Visibility.Visible;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}