using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Views.KhachHangViews;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Entities;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Entities.Dto;

namespace QuanLyDaiLy.ViewModels
{
    public class KhachHangViewModel : INotifyPropertyChanged
    {
        public ICommand ThemKhachHangCommand { get; }
        public ICommand CapNhatKhachHangCommand { get; }
        public ICommand XoaKhachHangCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ResetKhachHangCommand { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;

        public KhachHangViewModel(
            IServiceProvider serviceProvider,
            IKhachHangRepo khachHangRepo,
            ILoaiTietKiemRepo loaiTietKiemRepo
        )
        {
            _serviceProvider = serviceProvider;
            _khachHangRepo = khachHangRepo;
            _loaiTietKiemRepo = loaiTietKiemRepo;

            _ = LoadData();

            ThemKhachHangCommand = new RelayCommand(OpenThemKhachHang);
            CapNhatKhachHangCommand = new RelayCommand(OpenCapNhatKhachHang);
            CloseCommand = new RelayCommand(ExecuteClose);
            XoaKhachHangCommand = new RelayCommand(async () => await XoaKhachHang());
            ResetKhachHangCommand = new RelayCommand(async () => await LoadData());
        }

        private DisplayKhachHangDto _selectedKhachHangDto = null!;
        public DisplayKhachHangDto SelectedKhachHangDto
        {
            get => _selectedKhachHangDto;
            set
            {
                if (_selectedKhachHangDto != value)
                {
                    _selectedKhachHangDto = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<KhachHang> _danhSachKhachHang = [];

        private ObservableCollection<DisplayKhachHangDto> _danhSachKhachHangDto = [];
        public ObservableCollection<DisplayKhachHangDto> DanhSachKhachHangDto
        {
            get => _danhSachKhachHangDto;
            set
            {
                _danhSachKhachHangDto = value;
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
                    _ = FilterKhachHang();
                }
            }
        }


        private ObservableCollection<LoaiTietKiem> _danhSachLoaiTietKiem = [];
        public ObservableCollection<LoaiTietKiem> DanhSachLoaiTietKiem
        {
            get => _danhSachLoaiTietKiem;
            set
            {
                _danhSachLoaiTietKiem = value;
                OnPropertyChanged();
            }
        }

        private LoaiTietKiem _selectedLoaiTietKiem = null!;
        public LoaiTietKiem SelectedLoaiTietKiem
        {
            get => _selectedLoaiTietKiem;
            set
            {
                if (_selectedLoaiTietKiem != value)
                {
                    _selectedLoaiTietKiem = value;
                    OnPropertyChanged();
                    _ = FilterKhachHang();
                }
            }
        }

        private async Task FilterKhachHang()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchText) && SelectedLoaiTietKiem == null)
                {
                    DanhSachKhachHangDto.Clear();
                    foreach (var khachHang in _danhSachKhachHang)
                    {
                        DanhSachKhachHangDto.Add(new DisplayKhachHangDto
                        {
                            KhachHang = khachHang,
                            SoDu = khachHang.DsSoTietKiem?.Sum(stk => stk.SoTienGui) ?? 0
                        });
                    }
                }
                else
                {
                    var filteredList = _danhSachKhachHang.Where(kh =>
                        (string.IsNullOrEmpty(SearchText) || kh.TenKhachHang.Contains(SearchText)) &&
                        (SelectedLoaiTietKiem == null || kh.DsSoTietKiem.Any(stk => stk.MaLoaiTietKiem == SelectedLoaiTietKiem.MaLoaiTietKiem))
                    ).ToList();
                    DanhSachKhachHangDto.Clear();
                    foreach (var khachHang in filteredList)
                    {
                        DanhSachKhachHangDto.Add(new DisplayKhachHangDto
                        {
                            KhachHang = khachHang,
                            SoDu = khachHang.DsSoTietKiem?.Sum(stk => stk.SoTienGui) ?? 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadData()
        {
            try
            {
                _danhSachKhachHang.Clear();
                DanhSachKhachHangDto.Clear();
                DanhSachLoaiTietKiem.Clear();

                DanhSachLoaiTietKiem = [.. (await _loaiTietKiemRepo.GetAll())];

                _danhSachKhachHang = [.. (await _khachHangRepo.GetAll())];
                foreach (var khachHang in _danhSachKhachHang)
                {
                    DanhSachKhachHangDto.Add(new DisplayKhachHangDto
                    {
                        KhachHang = khachHang,
                        SoDu = khachHang.DsSoTietKiem?.Sum(stk => stk.SoTienGui) ?? 0
                    });
                }
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
            if (string.IsNullOrEmpty(SelectedKhachHangDto?.KhachHang.CMND))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để cập nhật!");
                return;
            }
            var capNhatKhachHangViewModel = _serviceProvider.GetRequiredService<CapNhatKhachHangViewModel>();

            capNhatKhachHangViewModel.SetData(SelectedKhachHangDto.KhachHang);


            capNhatKhachHangViewModel.CapNhatEvent += (sender, khachHang) =>
            {
                _ = LoadData();
            };

            var capnhatKhachHang = new CapNhatKhachHang(capNhatKhachHangViewModel);

            capnhatKhachHang.ShowDialog();
        }

        public async Task XoaKhachHang()
        {
            if (string.IsNullOrEmpty(SelectedKhachHangDto?.KhachHang.CMND))
            {
                MessageBox.Show("Vui lòng chọn KH để xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa Khách hàng {SelectedKhachHangDto.KhachHang.TenKhachHang}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _khachHangRepo.Delete(SelectedKhachHangDto.KhachHang.CMND);
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
            Window? parentWindow = Application.Current.Windows
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
