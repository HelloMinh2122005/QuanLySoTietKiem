using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace QuanLyDaiLy.ViewModels
{
    public class LoaiTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand LapPhieuGoiTienCommand { get; } 
        public ICommand CloseCommand { get; }         

        private readonly IServiceProvider _serviceProvider;

        public LoaiTietKiemViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LapPhieuGoiTienCommand = new RelayCommand(OpenLapPhieuGoiTien);
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        private void OpenLapPhieuGoiTien()
        {
            var lapPhieuGoiTien = _serviceProvider.GetRequiredService<lapPhieuGoiTien>();
            lapPhieuGoiTien.ShowDialog(); 
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
