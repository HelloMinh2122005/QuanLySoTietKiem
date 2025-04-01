using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Views.KhachHangViews;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Entities;
using System.Collections.ObjectModel;

namespace QuanLyDaiLy.ViewModels
{
    public class KhachHangViewModel : INotifyPropertyChanged
    {
        public ICommand ThemKhachHangCommand { get; }
        public ICommand CapNhatKhachHangCommand { get; }
        public ICommand XoaKhachHangCommand { get; }
        public ICommand CloseCommand { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IKhachHangRepo _khachHangRepo;


        public KhachHangViewModel(
            IServiceProvider serviceProvider,
            IKhachHangRepo khachHangRepo
        )
        {
            _serviceProvider = serviceProvider;
            _khachHangRepo = khachHangRepo;
            _ = LoadData();

            ThemKhachHangCommand = new RelayCommand(OpenThemKhachHang);
            CapNhatKhachHangCommand = new RelayCommand(OpenCapNhatKhachHang);
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        private KhachHang _selectedkhachHang = new();
        public KhachHang SelectedkhachHang
        {
            get => _selectedkhachHang;
            set
            {
                _selectedkhachHang = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KhachHang> _danhSachKhachHang = [];
        public ObservableCollection<KhachHang> DanhSachKhachHang
        {
            get => _danhSachKhachHang;
            set
            {
                _danhSachKhachHang = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
//                    FilterById();
                }
            }
        }


        private async Task LoadData()
        {
            try
            {
                var danhSachKhachHang = await _khachHangRepo.GetAll();

                DanhSachKhachHang.Clear();
                DanhSachKhachHang = [.. danhSachKhachHang];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenThemKhachHang()
        {
            var themKhachHang = _serviceProvider.GetRequiredService<ThemKhachHang>();
            var themKhachHangViewModel = _serviceProvider.GetRequiredService<ThemKhachHangViewModel>();
            themKhachHang.DataContext = themKhachHangViewModel;
            themKhachHang.Closed += (s, e) => _ = LoadData();
            themKhachHang.ShowDialog();
        }

        public void OpenCapNhatKhachHang()
        {
            if (string.IsNullOrEmpty(SelectedkhachHang.CMND))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để cập nhật!");
                return;
            }
            var capNhatKhachHangViewModel = _serviceProvider.GetRequiredService<CapNhatKhachHangViewModel>();
            capNhatKhachHangViewModel.SetData(SelectedkhachHang);


            capNhatKhachHangViewModel.CapNhatEvent += (sender, khachHang) =>
            {
                _ = LoadData();
            };

            var capnhatKhachHang = new CapNhatKhachHang(capNhatKhachHangViewModel);

            capnhatKhachHang.ShowDialog();
        }

        public async Task XoaKhachHang()
        {
            if (string.IsNullOrEmpty(SelectedkhachHang.CMND))
            {
                MessageBox.Show("Vui lòng chọn KH để xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa KH {SelectedkhachHang.TenKhachHang}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _khachHangRepo.Delete(SelectedkhachHang.CMND);
                    await LoadData();
                    MessageBox.Show("Xóa KH thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa KH: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
