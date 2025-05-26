using System.Collections.ObjectModel;
using System.ComponentModel;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels;

public class BaoCaoDoanhSoViewModel: INotifyPropertyChanged
{
    private readonly ISoTietKiemRepo _soTietKiemRepo;
    private readonly IPhieuGoiTienRepo _phieuGoiTienRepo;
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
    private readonly IPhieuRutTienRepo _phieuRutTienRepo;
    
    public BaoCaoDoanhSoViewModel(ISoTietKiemRepo soTietKiemRepo, 
        IPhieuGoiTienRepo phieuGoiTienRepo, ILoaiTietKiemRepo loaiTietKiemRepo, IPhieuRutTienRepo phieuRutTienRepo)
    {
        _soTietKiemRepo = soTietKiemRepo;
        _phieuGoiTienRepo = phieuGoiTienRepo;
        _loaiTietKiemRepo = loaiTietKiemRepo;
        _phieuRutTienRepo = phieuRutTienRepo;
        SelectedDate = DateTime.Now;
    }
    
    private DateTime _selectedDate;

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                LapBaoCaoDoanhSo();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
    }
    
    private Decimal _tongDoanhSo;
    
    public Decimal TongDoanhSo
    {
        get => _tongDoanhSo;
        set
        {
            _tongDoanhSo = value;
            OnPropertyChanged(nameof(TongDoanhSo));
        }
    }
    
    private ObservableCollection<BaoCaoDoanhSo> _dsBaoCaoDoanhSo;
    public ObservableCollection<BaoCaoDoanhSo> DSBaoCaoDoanhSo
    {
        get => _dsBaoCaoDoanhSo;
        set
        {
            _dsBaoCaoDoanhSo = value;
            OnPropertyChanged(nameof(DSBaoCaoDoanhSo));
        }
    }

    public void LapBaoCaoDoanhSo()
    {
        var loaiTietKiems = _loaiTietKiemRepo.GetAll().Result;
        var soTietKiems = _soTietKiemRepo.GetAll().Result;
        var phieuGoiTiens = _phieuGoiTienRepo.GetAll().Result.Where(pg => pg.NgayGoi.Date == SelectedDate.Date);            
        var phieuRuts = _phieuRutTienRepo.GetAll().Result.Where(pr => pr.NgayRut.Date == SelectedDate.Date);

        var baoCaoList = loaiTietKiems.Select(loaiTietKiem =>
        {
            var soTietKiemCuaLoai = soTietKiems.Where(stk => stk.MaLoaiTietKiem.Equals(loaiTietKiem.MaLoaiTietKiem));
            var tongThu = phieuGoiTiens
                .Where(pg => soTietKiemCuaLoai.Any(stk => stk.MaSoTietKiem.Equals(pg.MaSoTietKiem)))
                .Sum(pg => pg.SoTienGui);
            var tongChi = phieuRuts
                .Where(pr => soTietKiemCuaLoai.Any(stk => stk.MaSoTietKiem.Equals(pr.MaSoTietKiem)))
                .Sum(pr => pr.SoTienRut);

            return new BaoCaoDoanhSo
            {
                LoaiTietKiem = loaiTietKiem.TenLoaiTietKiem,
                TongThu = tongThu,
                TongChi = tongChi
            };
        }).ToList();
        DSBaoCaoDoanhSo = new ObservableCollection<BaoCaoDoanhSo>(baoCaoList);
        TongDoanhSo = DSBaoCaoDoanhSo.Sum(b => b.ChenhLech);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class BaoCaoDoanhSo
{
    public string LoaiTietKiem { get; set; }
    public decimal TongThu { get; set; }
    public decimal TongChi { get; set; }
    public decimal ChenhLech => TongThu - TongChi;
}