using QuanLyDaiLy.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Views.PhieuGoiTienViews;
using QuanLyDaiLy.Views.PhieuRutTienViews;
using Microsoft.Extensions.DependencyInjection;

namespace QuanLyDaiLy.ViewModels
{
    public class ThemPhieuRutTienViewModel: INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; set; }
        public ICommand LapPhieuCommand { get; set; }
        public ICommand TraCuuSoCommand { get; }

        public EventHandler<PhieuRutTien>? LapPhieuEvent;
        private readonly IPhieuRutTienRepo _phieuRutTienRepo;
        private readonly ISoTietKiemRepo _soTietKiemRepo;
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly IThamSoRepo _thamSoRepo;
        private readonly ILoaiTietKiemRepo _loaitietkiemRepo;
        private readonly IServiceProvider _serviceProvider;

        private string _maPhieuRutTien;
        public string MaPhieuRutTien
        {
            get => _maPhieuRutTien;
            set
            {
                if (_maPhieuRutTien != value)
                {
                    _maPhieuRutTien = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _ngayRut;
        public DateTime NgayRut
        {
            get => _ngayRut;
            set
            {
                if (_ngayRut != value)
                {
                    _ngayRut = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maSoTietKiem;
        public string MaSoTietKiem
        {
            get => _maSoTietKiem;
            set
            {
                if (_maSoTietKiem != value)
                {
                    _maSoTietKiem = value;
                    OnPropertyChanged();
                    LoadFields(); //UI update
                }
            }
        }

        private async void LoadFields()
        {
            if (string.IsNullOrEmpty(MaSoTietKiem))
            {
                // Clear fields if MaSoTietKiem is empty
                TenKhachHang = string.Empty;
                TenLoaiTietKiem = string.Empty;
                KyHan = 0;
                SoTienRut = 0;
                SoLanDaoHan = 0;
                SoTienGui = 0;
                TienLai = 0;
                NgayMoSo = null;
                return;
            }

            try
            {
                var soTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
                if (soTietKiem == null)
                {
                    MessageBox.Show("Số tiết kiệm không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TenKhachHang = soTietKiem.KhachHang.TenKhachHang;
                TenLoaiTietKiem = soTietKiem.LoaiTietKiem.TenLoaiTietKiem;
                KyHan = soTietKiem.LoaiTietKiem.KyHan;
                SoTienRut = 0; //reset to 0
                SoLanDaoHan = soTietKiem.SoLanDaoHan;
                SoTienGui = soTietKiem.SoTienGui;

                //Tinh LaiSuat

                if (soTietKiem.LoaiTietKiem.KyHan != 0 && soTietKiem.SoTienGui > 0)
                {
                    TienLai = soTietKiem.SoTienGui * soTietKiem.LoaiTietKiem.LaiSuatQuyDinh * soTietKiem.SoLanDaoHan * soTietKiem.LoaiTietKiem.KyHan;
                }
                else if (soTietKiem.LoaiTietKiem.KyHan == 0 && soTietKiem.SoTienGui > 0)
                {
                    TienLai = soTietKiem.SoTienGui * soTietKiem.LoaiTietKiem.LaiSuatQuyDinh;
                }
                else
                {
                    TienLai = 0;
                }

                DangMo = soTietKiem.DangMo;
                NgayMoSo = soTietKiem.NgayMoSo;
                LoaiTietKiem = soTietKiem.LoaiTietKiem;
                QuyDinhRutHetTien = LoaiTietKiem.QuyDinhRutHetTien;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin sổ tiết kiệm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private decimal _soTienRut;
        public decimal SoTienRut
        {
            get => _soTienRut;
            set
            {
                if (_soTienRut != value)
                {
                    _soTienRut = value;
                    OnPropertyChanged();
                }
            }
        }

        private LoaiTietKiem _loaitietkiem;
        public LoaiTietKiem LoaiTietKiem
        {
            get => _loaitietkiem;
            set
            {
                if (_loaitietkiem != value)
                {
                    _loaitietkiem = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tenKhachHang;
        public string TenKhachHang
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

        private string _tenLoaiTietKiem;
        public string TenLoaiTietKiem
        {
            get => _tenLoaiTietKiem;
            set
            {
                if (_tenLoaiTietKiem != value)
                {
                    _tenLoaiTietKiem = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _dangmo;
        public bool DangMo
        {
            get => _dangmo;
            set
            {
                if (_dangmo != value)
                {
                    _dangmo = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrangThai));
                }
            }
        }

        public DateTime? _ngayMoSo;
        public DateTime? NgayMoSo
        {
            get => _ngayMoSo;
            set
            {
                if (_ngayMoSo != value)
                {
                    _ngayMoSo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TrangThai
        {
            get
            {
                if (DangMo)
                    return "Đang mở";
                else
                    return "Đã đóng";
            }
        }

        private int _kyHan;
        public int KyHan
        {
            get => _kyHan;
            set
            {
                if (_kyHan != value)
                {
                    _kyHan = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _soLanDaoHan;
        public int SoLanDaoHan
        {
            get => _soLanDaoHan;
            set
            {
                if (_soLanDaoHan != value)
                {
                    _soLanDaoHan = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _quyDinhRutHetTien;
        public bool QuyDinhRutHetTien
        {
            get => _quyDinhRutHetTien;
            set
            {
                if (_quyDinhRutHetTien != value)
                {
                    _quyDinhRutHetTien = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TrangThaiQuyDinh
        {
            get
            {
                if (QuyDinhRutHetTien)
                    return "Đang áp dụng";
                else
                    return "Không áp dụng";
            }
        }

        private decimal _soTienGui;
        public decimal SoTienGui
        {
            get => _soTienGui;
            set
            {
                if (_soTienGui != value)
                {
                    _soTienGui = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _tienLai;
        public decimal TienLai
        {
            get => _tienLai;
            set
            {
                if (_tienLai != value)
                {
                    _tienLai = value;
                    OnPropertyChanged();
                }
            }
        }

        public ThemPhieuRutTienViewModel(IPhieuRutTienRepo phieuRutTienRepo, ISoTietKiemRepo soTietKiemRepo, IKhachHangRepo khachHangRepo, IThamSoRepo thamSoRepo, ILoaiTietKiemRepo loaiTietKiemRepo, IServiceProvider serviceProvider)
        {
            //repo 
            _phieuRutTienRepo = phieuRutTienRepo;
            _soTietKiemRepo = soTietKiemRepo;
            _khachHangRepo = khachHangRepo;
            _thamSoRepo = thamSoRepo;
            _loaitietkiemRepo = loaiTietKiemRepo;
            
            //service
            _serviceProvider = serviceProvider;

            //commands
            CloseCommand = new RelayCommand(ExecuteClose);
            LapPhieuCommand = new RelayCommand(LapPhieu);
            TraCuuSoCommand = new RelayCommand(ExecuteCloseAndNavigate);

            //load fields 
            NgayRut = DateTime.Now;
            _serviceProvider = serviceProvider;
        }

        private void ExecuteCloseAndNavigate()
        {
            // Close the current lapPhieuGoiTien window
            var currentWindow = Application.Current.Windows.OfType<LapPhieuRutTienWindow>().FirstOrDefault();
            currentWindow?.Close();

            // Resolve the required dependencies
            var viewModel = _serviceProvider.GetRequiredService<DanhSachSoTietKiemViewModel>();
            var danhSachSoTietKiemWindow = new DanhSachSoTietKiem(viewModel, _serviceProvider);

            //Navigate
            var currentWindows = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (currentWindows != null)
            {
                currentWindows.Content = danhSachSoTietKiemWindow.Content;
            }

            // Call OpenTraCuuSoTietKiem function
            if (viewModel is DanhSachSoTietKiemViewModel danhSachViewModel)
            {
                danhSachViewModel.OpenSearchWindow();
            }
        }

        private async void LapPhieu()
        {
            //validation
            string validationError = await ValidateFields();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //create new PhieuRutTien
            var soTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
            if (soTietKiem == null)
            {
                MessageBox.Show("Số tiết kiệm không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var phieuRutTien = new PhieuRutTien
            {
                MaPhieuRutTien = IdGenerator.GenerateId<PhieuRutTien>(),
                NgayRut = NgayRut,
                SoTienRut = SoTienRut,
                MaSoTietKiem = MaSoTietKiem,
                SoTietKiem = soTietKiem
            };

            //save to db
            try
            {
                await _phieuRutTienRepo.Create(phieuRutTien);

                //update SoTietKiem
                soTietKiem.SoTienGui -= SoTienRut;
                if (soTietKiem.SoTienGui <= 0 )
                {
                    soTietKiem.DangMo = false; //đóng sổ tiết kiệm nếu rút hết tiền
                }
                await _soTietKiemRepo.Update(soTietKiem);

                MessageBox.Show("Lập phiếu rút tiền thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LapPhieuEvent?.Invoke(this, phieuRutTien);
                MaPhieuRutTien = phieuRutTien.MaPhieuRutTien;

                //reset fields
                SoTienRut = 0;
                MaSoTietKiem = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập phiếu rút tiền: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> ValidateFields()
        {
            if (MaSoTietKiem == null || MaSoTietKiem.Trim() == "")
            {
                return "Vui lòng chọn số tiết kiệm để rút tiền.";
            }

            var soTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
            if (soTietKiem == null)
            {
                return "Số tiết kiệm không tồn tại.";
            }
            if (soTietKiem.DangMo == false)
            {
                return "Số tiết kiệm đã đóng, không thể rút tiền.";
            }

            var daysSinceOpened = (DateTime.Now - soTietKiem.NgayMoSo).TotalDays;

            var thamSo = await _thamSoRepo.Get();
            if (daysSinceOpened < thamSo.SoNgayMoToiThieu)
            {
                return "Số tiết kiệm chưa đủ 15 ngày để rút tiền.";
            }

            var loaiTietKiem = await _loaitietkiemRepo.GetById(soTietKiem.MaLoaiTietKiem);
            //kiem tra ngay rut phai sau ngay dao han
            var expiredDay = soTietKiem.NgayMoSo.AddDays(loaiTietKiem.KyHan * 30);
            if (NgayRut < expiredDay && loaiTietKiem.KyHan != 0)
            {
                return $"Ngày rút tiền phải sau ngày đáo hạn ({expiredDay.ToShortDateString()}).";
            }

            //loai tiet kiem co ky han phai rut het tien
            if (loaiTietKiem.KyHan != 0 && loaiTietKiem.QuyDinhRutHetTien == true)
            {
                //lai suat co ky han
                var profit = soTietKiem.SoLanDaoHan * loaiTietKiem.LaiSuatQuyDinh * loaiTietKiem.KyHan * soTietKiem.SoTienGui;
                if (SoTienRut != soTietKiem.SoTienGui + profit)
                {
                    return $"Cần rút hết tiền trong sổ tiết kiệm có kỳ hạn. Bạn đang có lãi suất {profit}.";
                }
            }

            //loai tiet kiem khong co ky han
            if (loaiTietKiem.KyHan == 0 && daysSinceOpened >= 30)
            {
                var profit = loaiTietKiem.LaiSuatQuyDinh * soTietKiem.SoTienGui;
                if (SoTienRut > soTietKiem.SoTienGui + profit)
                {
                    return $"Bạn chỉ có thể rút tối đa {soTietKiem.SoTienGui + profit} từ sổ tiết kiệm không kỳ hạn này.";
                }
            }

            //validation passed
            return string.Empty;
        }
        private void ExecuteClose()
        {
            Application.Current.Windows.OfType<LapPhieuRutTienWindow>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
