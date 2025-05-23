using System.Collections.ObjectModel;
using System.ComponentModel;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels;

public class BaoCaoDoanhSoViewModel: INotifyPropertyChanged
{
    private readonly ISoTietKiemRepo _soTietKiemRepo;
    private readonly IPhieuGoiTienRepo _phieuGoiTienRepo;
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
    
    public BaoCaoDoanhSoViewModel(ISoTietKiemRepo soTietKiemRepo, IPhieuGoiTienRepo phieuGoiTienRepo, ILoaiTietKiemRepo loaiTietKiemRepo)
    {
        _soTietKiemRepo = soTietKiemRepo;
        _phieuGoiTienRepo = phieuGoiTienRepo;
        _loaiTietKiemRepo = loaiTietKiemRepo;
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
                Console.Out.WriteLine(value);
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
    }
    
    private ObservableCollection<BaoCaoDoanhSo> _baoCaoDoanhSo;
    public ObservableCollection<BaoCaoDoanhSo> BaoCaoDoanhSo
    {
        get => _baoCaoDoanhSo;
        set
        {
            _baoCaoDoanhSo = value;
            OnPropertyChanged(nameof(BaoCaoDoanhSo));
        }
    }

    public void LapBaoCaoDoanhSo()
    {
        var loaiTietKiems = _loaiTietKiemRepo.GetAll().Result;
        var soTietKiems = _soTietKiemRepo.GetAll().Result;
        var phieuGoiTiens = _phieuGoiTienRepo.GetAll().Result.Where(pg => pg.NgayGoi.Date == SelectedDate.Date);            
        var phieuRuts = _phieuGoiTienRepo.GetAll().Result.Where(pr => pr.NgayGoi.Date == SelectedDate.Date);

        var baoCaoList = loaiTietKiems.Select(loaiTietKiem =>
        {
            var soTietKiemCuaLoai = soTietKiems.Where(stk => stk.MaLoaiTietKiem.Equals(loaiTietKiem.MaLoaiTietKiem));
            var tongThu = phieuGoiTiens
                .Where(pg => soTietKiemCuaLoai.Any(stk => stk.MaSoTietKiem.Equals(pg.MaSoTietKiem)))
                .Sum(pg => pg.SoTienGui);
            var tongChi = phieuRuts
                .Where(pr => soTietKiemCuaLoai.Any(stk => stk.MaSoTietKiem.Equals(pr.MaSoTietKiem)))
                .Sum(pr => pr.SoTienGui);

            return new BaoCaoDoanhSo
            {
                LoaiTietKiem = loaiTietKiem.TenLoaiTietKiem,
                TongThu = tongThu,
                TongChi = tongChi
            };
        }).ToList();

        BaoCaoDoanhSo = new ObservableCollection<BaoCaoDoanhSo>(baoCaoList);
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
}