using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels
{
    public class DanhSachSoTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand ThemSoTietKiemCommand { get; }
        public ICommand CapNhatSoTietKiemCommand { get; }
        private readonly IServiceProvider _serviceProvider;
        private readonly ISoTietKiemRepo _soTietKiemRepo;


        public DanhSachSoTietKiemViewModel(
            IServiceProvider serviceProvider,
            ISoTietKiemRepo soTietKiemRepo
        ) {
            //DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(danhSach);
            _serviceProvider = serviceProvider;
            _soTietKiemRepo = soTietKiemRepo;

            ThemSoTietKiemCommand = new RelayCommand(OpenThemSoTietKiem);
            CapNhatSoTietKiemCommand = new RelayCommand(CapNhatSoTietKiem);
        }

        private ObservableCollection<SoTietKiem> _danhSachSoTietKiem = [];
        public ObservableCollection<SoTietKiem> DanhSachSoTietKiem
        {
            get => _danhSachSoTietKiem;
            set
            {
                _danhSachSoTietKiem = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadData()
        {
            
        }

        public void OpenThemSoTietKiem()
        {
            var themSoTietKiem = _serviceProvider.GetRequiredService<ThemSoTietKiem>();
            themSoTietKiem.Show();
        }

        public void CapNhatSoTietKiem()
        {
            var capnhatSoTietKiem = _serviceProvider.GetRequiredService<CapNhatSoTietKiem>();
            capnhatSoTietKiem.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}  