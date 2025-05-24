using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Views;

namespace QuanLyDaiLy.ViewModels;

public class TraCuuSoTietKiemViewModel : INotifyPropertyChanged
{
    private readonly ISoTietKiemRepo _soTietKiemRepo;
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
    public Action<List<SoTietKiem>>? SearchCompleted { get; set; }

    public ICommand CloseCommand { get; }
    public ICommand SearchCommand { get; }
    

    public TraCuuSoTietKiemViewModel(ISoTietKiemRepo soTietKiemRepo, ILoaiTietKiemRepo loaiTietKiemRepo)
    {
        _soTietKiemRepo = soTietKiemRepo;
        _loaiTietKiemRepo = loaiTietKiemRepo;
        CloseCommand = new RelayCommand(ExecuteClose);
        SearchCommand = new RelayCommand(search);
        LoadLoaiTietKiem();
    }


    private String? _maSoTietKiem;

    public string? MaSoTietKiem
    {
        get => _maSoTietKiem;
        set
        {
            if (_maSoTietKiem != value)
            {
                _maSoTietKiem = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<LoaiTietKiem> _dsLoaiTietKiem;

    public ObservableCollection<LoaiTietKiem> DsLoaiTietKiem
    {
        get => _dsLoaiTietKiem;
        set
        {
            if (_dsLoaiTietKiem != value)
            {
                _dsLoaiTietKiem = value;
                OnPropertyChanged();
            }
        }
    }

    private LoaiTietKiem? _loaiTietKiemSelected;

    public LoaiTietKiem? LoaiTietKiemSelected
    {
        get => _loaiTietKiemSelected;
        set
        {
            if (_loaiTietKiemSelected != value)
            {
                _loaiTietKiemSelected = value;
                OnPropertyChanged();
            }
        }
    }

    private bool? _tinhTrangSo;

    public bool? TinhTrangSo
    {
        get => _tinhTrangSo;
        set
        {
            if (_tinhTrangSo != value)
            {
                _tinhTrangSo = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _cmmnd;

    public string? CMMND
    {
        get => _cmmnd;
        set
        {
            if (_cmmnd != value)
            {
                _cmmnd = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _tenKhachHang;

    public string? TenKhachHang
    {
        get => _tenKhachHang;
        set
        {
            if (_tenKhachHang != value)
            {
                _tenKhachHang = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _laiSuatTu;

    public decimal? LaiSuatTu
    {
        get => _laiSuatTu;
        set
        {
            if (_laiSuatTu != value)
            {
                _laiSuatTu = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _laiSuatDen;

    public decimal? LaiSuatDen
    {
        get => _laiSuatDen;
        set
        {
            if (_laiSuatDen != value)
            {
                _laiSuatDen = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _ngayMoSoTu;

    public DateTime? NgayMoSoTu
    {
        get => _ngayMoSoTu;
        set
        {
            if (_ngayMoSoTu != value)
            {
                _ngayMoSoTu = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _ngayMoSoDen;

    public DateTime? NgayMoSoDen
    {
        get => _ngayMoSoDen;
        set
        {
            if (_ngayMoSoDen != value)
            {
                _ngayMoSoDen = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _soDuTu;

    public decimal? SoDuTu
    {
        get => _soDuTu;
        set
        {
            if (_soDuTu != value)
            {
                _soDuTu = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _soDuDen;

    public decimal? SoDuDen
    {
        get => _soDuDen;
        set
        {
            if (_soDuDen != value)
            {
                _soDuDen = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _ngayGuiPhieuGuiTienTu;

    public DateTime? NgayGuiPhieuGuiTienTu
    {
        get => _ngayGuiPhieuGuiTienTu;
        set
        {
            if (_ngayGuiPhieuGuiTienTu != value)
            {
                _ngayGuiPhieuGuiTienTu = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _ngayGuiPhieuGuiTienDen;

    public DateTime? NgayGuiPhieuGuiTienDen
    {
        get => _ngayGuiPhieuGuiTienDen;
        set
        {
            if (_ngayGuiPhieuGuiTienDen != value)
            {
                _ngayGuiPhieuGuiTienDen = value;
                OnPropertyChanged();
            }
        }
    }
    
    private DateTime? _ngayGuiPhieuRutTienTu;
    public DateTime? NgayGuiPhieuRutTienTu
    {
        get => _ngayGuiPhieuRutTienTu;
        set
        {
            if (_ngayGuiPhieuRutTienTu != value)
            {
                _ngayGuiPhieuRutTienTu = value;
                OnPropertyChanged();
            }
        }
    }
    private DateTime? _ngayGuiPhieuRutTienDen;
    public DateTime? NgayGuiPhieuRutTienDen
    {
        get => _ngayGuiPhieuRutTienDen;
        set
        {
            if (_ngayGuiPhieuRutTienDen != value)
            {
                _ngayGuiPhieuRutTienDen = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _soTienGuiTu;

    public decimal? SoTienGuiTu
    {
        get => _soTienGuiTu;
        set
        {
            if (_soTienGuiTu != value)
            {
                _soTienGuiTu = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal? _soTienGuiDen;

    public decimal? SoTienGuiDen
    {
        get => _soTienGuiDen;
        set
        {
            if (_soTienGuiDen != value)
            {
                _soTienGuiDen = value;
                OnPropertyChanged();
            }
        }
    }
    private decimal? _soTienRutTu;
    public decimal? SoTienRutTu
    {
        get => _soTienRutTu;
        set
        {
            if (_soTienRutTu != value)
            {
                _soTienRutTu = value;
                OnPropertyChanged();
            }
        }
    }
    private decimal? _soTienRutDen;
    public decimal? SoTienRutDen
    {
        get => _soTienRutDen;
        set
        {
            if (_soTienRutDen != value)
            {
                _soTienRutDen = value;
                OnPropertyChanged();
            }
        }
    }

    private int? _soLuongPhieuGuiTienTu;

    public int? SoLuongPhieuGuiTienTu
    {
        get => _soLuongPhieuGuiTienTu;
        set
        {
            if (_soLuongPhieuGuiTienTu != value)
            {
                _soLuongPhieuGuiTienTu = value;
                OnPropertyChanged();
            }
        }
    }

    private int? _soLuongPhieuGuiTienDen;

    public int? SoLuongPhieuGuiTienDen
    {
        get => _soLuongPhieuGuiTienDen;
        set
        {
            if (_soLuongPhieuGuiTienDen != value)
            {
                _soLuongPhieuGuiTienDen = value;
                OnPropertyChanged();
            }
        }
    }

    private int? _soLuongPhieuRutTienTu;

    public int? SoLuongPhieuRutTienTu
    {
        get => _soLuongPhieuRutTienTu;
        set
        {
            if (_soLuongPhieuRutTienTu != value)
            {
                _soLuongPhieuRutTienTu = value;
                OnPropertyChanged();
            }
        }
    }

    private int? _soLuongPhieuRutTienDen;

    public int? SoLuongPhieuRutTienDen
    {
        get => _soLuongPhieuRutTienDen;
        set
        {
            if (_soLuongPhieuRutTienDen != value)
            {
                _soLuongPhieuRutTienDen = value;
                OnPropertyChanged();
            }
        }
    }
    
    private void ExecuteClose()
    {
        Application.Current.Windows.OfType<TraCuuSoTietKiem>().FirstOrDefault()?.Close();
    }

    private void LoadLoaiTietKiem()
    {
        DsLoaiTietKiem = new ObservableCollection<LoaiTietKiem>(_loaiTietKiemRepo.GetAll().Result);
    }

    void search()
    {
        var tmp = _soTietKiemRepo.SearchSoTietKiem(
            MaSoTietKiem,
            LoaiTietKiemSelected,
            CMMND,
            TenKhachHang,
            LaiSuatTu,
            LaiSuatDen,
            NgayMoSoTu,
            NgayMoSoDen,
            SoDuTu,
            SoDuDen,
            NgayGuiPhieuGuiTienTu,
            NgayGuiPhieuGuiTienDen,
            null, // NgayGuiPhieuRutTienTu (not implemented in the repository)
            null, // NgayGuiPhieuRutTienDen (not implemented in the repository)
            SoTienGuiTu,
            SoTienGuiDen,
            null, // SoTienRutTu (not implemented in the repository)
            null, // SoTienRutDen (not implemented in the repository)
            SoLuongPhieuGuiTienTu,
            SoLuongPhieuGuiTienDen,
            null, // SoLuongPhieuRutTienTu (not implemented in the repository)
            null // SoLuongPhieuRutTienDen (not implemented in the repository)
        ).Result;
        
        SearchCompleted?.Invoke(tmp.ToList()); // 🔥 Trả kết quả về ViewModel cha
        ExecuteClose(); // Đóng view
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}