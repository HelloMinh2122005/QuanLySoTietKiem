using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Views.KhachHangViews;

namespace QuanLyDaiLy.ViewModels
{
    public class KhachHangViewModel : INotifyPropertyChanged
    {
        public ICommand ThemKhachHangCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand CapNhatKhachHangCommand { get; }

        private readonly IServiceProvider _serviceProvider;

        public KhachHangViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            ThemKhachHangCommand = new RelayCommand(OpenThemKhachHang);
            CapNhatKhachHangCommand = new RelayCommand(OpenCapNhatKhachHang);
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        public void OpenThemKhachHang()
        {
            var themKhachHang = _serviceProvider.GetRequiredService<ThemKhachHang>();
            themKhachHang.ShowDialog();
        }

        public void OpenCapNhatKhachHang()
        {
            var capNhatKhachHang = _serviceProvider.GetRequiredService<CapNhatKhachHang>();
            capNhatKhachHang.ShowDialog();
        }

        private void ExecuteClose()
        {
            Window parentWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);
            parentWindow?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
