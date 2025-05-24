using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels;

public class BaoCaoDongMoViewModel : INotifyPropertyChanged
{
    private readonly ISoTietKiemRepo _soTietKiemRepo;
    private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
    private readonly IPhieuRutTienRepo _phieuRutTienRepo;

    public BaoCaoDongMoViewModel(
        ISoTietKiemRepo soTietKiemRepo,
        ILoaiTietKiemRepo loaiTietKiemRepo,
        IPhieuRutTienRepo phieuRutTienRepo)
    {
        _soTietKiemRepo = soTietKiemRepo;
        _loaiTietKiemRepo = loaiTietKiemRepo;
        _phieuRutTienRepo = phieuRutTienRepo;

        LoaiTietKiems = new ObservableCollection<LoaiTietKiem>();
        FilteredDanhSachBaoCao = new ObservableCollection<BaoCaoDongMoItem>();

        SelectedMonth = DateTime.Now.Month;
        SelectedYear = DateTime.Now.Year;

        _ = LoadDataAsync();
    }

    public ObservableCollection<LoaiTietKiem> LoaiTietKiems { get; }
    private LoaiTietKiem? _selectedLoaiTietKiem;
    public LoaiTietKiem? SelectedLoaiTietKiem
    {
        get => _selectedLoaiTietKiem;
        set
        {
            if (_selectedLoaiTietKiem != value)
            {
                _selectedLoaiTietKiem = value;
                OnPropertyChanged(nameof(SelectedLoaiTietKiem));
                _ = UpdateReportAsync();
            }
        }
    }

    private int _selectedMonth;
    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            if (_selectedMonth != value)
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
                _ = UpdateReportAsync();
            }
        }
    }

    private int _selectedYear;
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            if (_selectedYear != value)
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                _ = UpdateReportAsync();
            }
        }
    }

    public ObservableCollection<BaoCaoDongMoItem> FilteredDanhSachBaoCao { get; }

    private async Task LoadDataAsync()
    {
        var loaiTietKiemList = await _loaiTietKiemRepo.GetAll();
        LoaiTietKiems.Clear();
        foreach (var ltk in loaiTietKiemList)
            LoaiTietKiems.Add(ltk);

        SelectedLoaiTietKiem = LoaiTietKiems.FirstOrDefault();
    }

    private async Task UpdateReportAsync()
    {
        FilteredDanhSachBaoCao.Clear();

        if (SelectedLoaiTietKiem == null)
            return;

        var soTietKiemList = (await _soTietKiemRepo.GetAll())
            .Where(stk => stk.MaLoaiTietKiem == SelectedLoaiTietKiem.MaLoaiTietKiem)
            .ToList();

        var phieuRutTienList = (await _phieuRutTienRepo.GetAll()).ToList();

        int daysInMonth = DateTime.DaysInMonth(SelectedYear, SelectedMonth);

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(SelectedYear, SelectedMonth, day);

            // Sổ mở trong ngày
            int soMo = soTietKiemList.Count(stk => stk.NgayMoSo.Date == date.Date);

            // Sổ đóng trong ngày
            int soDong = soTietKiemList.Count(stk =>
                stk.DangMo == false &&
                phieuRutTienList
                    .Where(prt => prt.MaSoTietKiem == stk.MaSoTietKiem)
                    .OrderByDescending(prt => prt.NgayRutTien)
                    .FirstOrDefault() is { } lastRutTien &&
                lastRutTien.NgayRutTien.Date == date.Date
            );

            FilteredDanhSachBaoCao.Add(new BaoCaoDongMoItem
            {
                NgayMoDongSo = date,
                SoLuongSoMo = soMo,
                SoLuongSoDong = soDong,
                ChenhLech = Math.Abs(soMo - soDong)
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class BaoCaoDongMoItem
{
    public DateTime NgayMoDongSo { get; set; }
    public int SoLuongSoMo { get; set; }
    public int SoLuongSoDong { get; set; }
    public int ChenhLech { get; set; }
}
