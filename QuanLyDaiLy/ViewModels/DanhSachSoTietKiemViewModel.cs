using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuanLyDaiLy.Commands;

namespace QuanLyDaiLy.ViewModels
{
    public class DanhSachSoTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand ThemSoTietKiemCommand { get; }
        public ICommand CapNhatSoTietKiemCommand { get; }
        private readonly IServiceProvider _serviceProvider;

        public DanhSachSoTietKiemViewModel(IServiceProvider serviceProvider)
        {
            //DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(danhSach);
            _serviceProvider = serviceProvider;
            ThemSoTietKiemCommand = new RelayCommand(OpenThemSoTietKiem);
            CapNhatSoTietKiemCommand = new RelayCommand(CapNhatSoTietKiem);
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