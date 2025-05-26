using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using System.Windows;
using QuanLyDaiLy.Entities.Dto;

namespace QuanLyDaiLy.ViewModels
{
    public class PhieuGoiTienViewModel : INotifyPropertyChanged
    {
        public ICommand LapPhieuGoiTienCommand { get; } 
        public ICommand CloseCommand { get; }         
        public ICommand XoaPhieuGoiTienCommand {get; }
        public ICommand ResetPhieuGoiTienCommand { get; }
        public ICommand CapNhatPhieuGoiTienCommand { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IPhieuGoiTienRepo _phieuGoiTienRepo;
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly ISoTietKiemRepo _soTietKiemRepo;

        public PhieuGoiTienViewModel(
            IServiceProvider serviceProvider,
            IPhieuGoiTienRepo phieuGoiTienRepo,
            ILoaiTietKiemRepo loaiTietKiemRepo,
            IKhachHangRepo khachHangRepo,
            ISoTietKiemRepo soTietKiemRepo
            )
        {

            _serviceProvider = serviceProvider;
            _khachHangRepo = khachHangRepo;
            _phieuGoiTienRepo = phieuGoiTienRepo;
            _loaiTietKiemRepo = loaiTietKiemRepo;
            _soTietKiemRepo = soTietKiemRepo;
            _ = LoadData();

            LapPhieuGoiTienCommand = new RelayCommand(OpenLapPhieuGoiTien);
            XoaPhieuGoiTienCommand = new RelayCommand(XoaPhieuGoiTien);
            ResetPhieuGoiTienCommand = new RelayCommand(async () => await LoadData());            
            CloseCommand = new RelayCommand(ExecuteClose);
        } 

        private ObservableCollection<PhieuGoiTienDTO> _danhSachPhieuGoiTien = new();
        public ObservableCollection<PhieuGoiTienDTO> DanhSachPhieuGoiTien
        {
            get => _danhSachPhieuGoiTien;
            set
            {
                _danhSachPhieuGoiTien = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PhieuGoiTienDTO> _filteredDanhSachPhieuGoiTien = new();
        public ObservableCollection<PhieuGoiTienDTO> FilteredDanhSachPhieuGoiTien
        {
            get => _filteredDanhSachPhieuGoiTien;
            set
            {
                _filteredDanhSachPhieuGoiTien = value;
                OnPropertyChanged();
            }
        }

        private PhieuGoiTienDTO _selectedPhieuGoiTien = new();
        public PhieuGoiTienDTO SelectedPhieuGoiTien
        {
            get => _selectedPhieuGoiTien;
            set
            {
                _selectedPhieuGoiTien = value;
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
                    FilterDanhSachPhieuGoiTien();
                }
            }
        }
        private void FilterDanhSachPhieuGoiTien()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If the search text is empty, show the full list
                FilteredDanhSachPhieuGoiTien = new ObservableCollection<PhieuGoiTienDTO>(DanhSachPhieuGoiTien);
            }
            else
            {
                // Filter the list based on TenKhachHang or MaPhieuGoiTien
                var lowerSearchText = SearchText.ToLower();
                var filteredList = DanhSachPhieuGoiTien.Where(phieu =>
                    (phieu.TenKhachHang?.ToLower().Contains(lowerSearchText) ?? false) ||
                    (phieu.MaPhieuGoiTien?.ToLower().Contains(lowerSearchText) ?? false)
                );

                FilteredDanhSachPhieuGoiTien = new ObservableCollection<PhieuGoiTienDTO>(filteredList);
            }
        }

        private async Task LoadData()
        {
            try
            {
                var danhSachPhieuGoiTien = await _phieuGoiTienRepo.GetAll();
                var danhSachLoaiTietKiem = await _loaiTietKiemRepo.GetAll();
                var danhSachKhachHang = await _khachHangRepo.GetAll();
                var danhSachSoTietKiem = await _soTietKiemRepo.GetAll();

                var danhSachPhieuGoiTienDTO = danhSachPhieuGoiTien.Select(phieu =>
                {
                    var soTietKiem = danhSachSoTietKiem.FirstOrDefault(stk => stk.MaSoTietKiem == phieu.MaSoTietKiem);
                    var loaiTietKiem = danhSachLoaiTietKiem.FirstOrDefault(loai => loai.MaLoaiTietKiem == soTietKiem?.MaLoaiTietKiem);
                    var khachHang = danhSachKhachHang.FirstOrDefault(kh => kh.DsSoTietKiem.Any(stk => stk.MaSoTietKiem == phieu.MaSoTietKiem));

                    return new PhieuGoiTienDTO
                    {
                        MaPhieuGoiTien = phieu.MaPhieuGoiTien,
                        NgayGoi = phieu.NgayGoi,
                        SoTienGui = phieu.SoTienGui,
                        MaSoTietKiem = phieu.MaSoTietKiem,
                        LoaiTietKiem = loaiTietKiem?.TenLoaiTietKiem ?? "Unknown",
                        TenKhachHang = khachHang?.TenKhachHang ?? "Unknown"
                    };
                });

                DanhSachPhieuGoiTien = new ObservableCollection<PhieuGoiTienDTO>(danhSachPhieuGoiTienDTO);
                FilteredDanhSachPhieuGoiTien = new ObservableCollection<PhieuGoiTienDTO>(danhSachPhieuGoiTienDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenLapPhieuGoiTien()
        {
            var lapPhieuGoiTien = _serviceProvider.GetRequiredService<lapPhieuGoiTien>();
            lapPhieuGoiTien.Closed += (s, e) =>
            {
                    _ = LoadData();
            };
            lapPhieuGoiTien.ShowDialog(); 
        }

        private void XoaPhieuGoiTien()
        {
            if (SelectedPhieuGoiTien != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu gởi tiền này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _ = _phieuGoiTienRepo.Delete(SelectedPhieuGoiTien.MaPhieuGoiTien);
                    _ = LoadData();
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
