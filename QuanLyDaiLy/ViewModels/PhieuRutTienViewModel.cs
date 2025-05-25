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
using QuanLyDaiLy.Views.PhieuRutTienViews;

namespace QuanLyDaiLy.ViewModels;
public class PhieuRutTienViewModel: INotifyPropertyChanged
{
    public ICommand LapPhieuRutTienCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand ResetPhieuRutTienCommand { get; }
    public ICommand CapNhatPhieuRutTienCommand { get; }

    private readonly IServiceProvider _serviceProvider;
    private readonly IPhieuRutTienRepo _phieuRutTienRepo;
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
    private readonly IKhachHangRepo _khachHangRepo;
    private readonly ISoTietKiemRepo _soTietKiemRepo;

    public PhieuRutTienViewModel(
        IServiceProvider serviceProvider,
        IPhieuRutTienRepo phieuRutTienRepo,
        ILoaiTietKiemRepo loaiTietKiemRepo,
        IKhachHangRepo khachHangRepo,
        ISoTietKiemRepo soTietKiemRepo
        )
    {
        _serviceProvider = serviceProvider;
        _khachHangRepo = khachHangRepo;
        _phieuRutTienRepo = phieuRutTienRepo;
        _loaiTietKiemRepo = loaiTietKiemRepo;
        _soTietKiemRepo = soTietKiemRepo;
        _ = LoadData();

        LapPhieuRutTienCommand = new RelayCommand(OpenLapPhieuRutTien);
        ResetPhieuRutTienCommand = new RelayCommand(async () => await LoadData());
        CloseCommand = new RelayCommand(ExecuteClose);
    }   

    public ObservableCollection<PhieuRutTienDTO> _danhSachPhieuRutTien = new();
    public ObservableCollection<PhieuRutTienDTO> DanhSachPhieuRutTien
    {
        get => _danhSachPhieuRutTien;
        set
        {
            _danhSachPhieuRutTien = value;
            OnPropertyChanged();
        }
    }

    private PhieuGoiTienDTO _selectedPhieuRutTien = new();
    public PhieuGoiTienDTO SelectedPhieuRutTien
    {
        get => _selectedPhieuRutTien;
        set
        {
            _selectedPhieuRutTien = value;
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
//                FilterDanhSachPhieuRutTien();
            }
        }
    }

    private async Task LoadData()
    {
        try
        {
            var danhSachPhieuRutTien = await _phieuRutTienRepo.GetAll();
            var danhSachLoaiTietKiem = await _loaiTietKiemRepo.GetAll();
            var danhSachKhachHang = await _khachHangRepo.GetAll();
            var danhSachSoTietKiem = await _soTietKiemRepo.GetAll();

            var danhSachPhieuRutTienDTO = danhSachPhieuRutTien.Select(phieu =>
            {
                var soTietKiem = danhSachSoTietKiem.FirstOrDefault(stk => stk.MaSoTietKiem == phieu.MaSoTietKiem);
                var loaiTietKiem = danhSachLoaiTietKiem.FirstOrDefault(loai => loai.MaLoaiTietKiem == soTietKiem?.MaLoaiTietKiem);
                var khachHang = danhSachKhachHang.FirstOrDefault(kh => kh.DsSoTietKiem.Any(stk => stk.MaSoTietKiem == phieu.MaSoTietKiem));

                return new PhieuRutTienDTO
                {
                    MaPhieuRutTien = phieu.MaPhieuRutTien,
                    NgayRut = phieu.NgayRut,
                    SoTienRut = phieu.SoTienRut,
                    MaSoTietKiem = phieu.MaSoTietKiem,
                    LoaiTietKiem = loaiTietKiem?.TenLoaiTietKiem ?? "Unknown",
                    TenKhachHang = khachHang?.TenKhachHang ?? "Unknown"
                };
            });

            DanhSachPhieuRutTien = new ObservableCollection<PhieuRutTienDTO>(danhSachPhieuRutTienDTO);
//            FilteredDanhSachPhieuRutTien = new ObservableCollection<PhieuGoiTienDTO>(danhSachPhieuRutTienDTO);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OpenLapPhieuRutTien()
    {
        var lapPhieuRutTien = _serviceProvider.GetRequiredService<LapPhieuRutTienWindow>();
        lapPhieuRutTien.Closed += (s, e) =>
        {
            _ = LoadData();
        };
        lapPhieuRutTien.ShowDialog();
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
