using System.Collections.ObjectModel;
using QuanLyDaiLy.Views;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels
{
    public class CapNhatSoTietKiemViewModel: INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        
        public ICommand CapNhatCommand { get; }
        
        public event EventHandler<SoTietKiem>? CapNhatEvent;

        
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
        
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly ISoTietKiemRepo _soTietKiemRepo;
        private readonly IThamSoRepo _thamSoRepo;

        private ObservableCollection<LoaiTietKiem> _dsLoaiTietKiem = [];
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

        private LoaiTietKiem? _loaiTietKiemDuocChon = new();
        public LoaiTietKiem? LoaiTietKiemDuocChon
        {
            get => _loaiTietKiemDuocChon;
            set
            {
                if (value != _loaiTietKiemDuocChon)
                {
                    _loaiTietKiemDuocChon = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _diaChi = "";
        public string DiaChi
        {
            get => _diaChi;
            set
            {
                if (_diaChi != value)
                {
                    _diaChi = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private string _tenKhachHang = "";
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
        private string _cmnd = "";
        public string Cmnd
        {
            get => _cmnd;
            set
            {
                if (_cmnd != value)
                {
                    _cmnd = value;
                    OnPropertyChanged();
                    _ = TimKiemKhachHangAsync(value);
                }
            }
        }
        private decimal _soTienGui = 0;
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
        
        private string _maSoTietKiem = "";
        public string MaSoTietKiem
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
        
        private DateTime _ngayMoSo = DateTime.Now;
        public DateTime NgayMoSo
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
        
        

        public void SetData(SoTietKiem soTietKiem)
        {
            SoTienGui = soTietKiem.SoTienGui;
            MaSoTietKiem = soTietKiem.MaSoTietKiem;
            NgayMoSo = soTietKiem.NgayMoSo;
            Cmnd = soTietKiem.CMND;
            
            LoaiTietKiemDuocChon = DsLoaiTietKiem.Where(item => item.MaLoaiTietKiem == soTietKiem.MaLoaiTietKiem).FirstOrDefault();
        }

        public CapNhatSoTietKiemViewModel(IKhachHangRepo khachHangRepo,ILoaiTietKiemRepo LoaiTietKiemRepo,ISoTietKiemRepo SoTietKiemRepo, IThamSoRepo ThamSoRepo)
        {
            //command
            CloseCommand = new RelayCommand(ExecuteClose);
            CapNhatCommand = new RelayCommand(CapNhat);
            
            //repo
            _khachHangRepo = khachHangRepo;
            _loaiTietKiemRepo = LoaiTietKiemRepo;
            _soTietKiemRepo = SoTietKiemRepo;
            _thamSoRepo = ThamSoRepo;
            
            LoadLoaiTietKiem();

        }
        
        private async Task TimKiemKhachHangAsync(string cmnd)
        {
            var data = await _khachHangRepo.GetById(cmnd);
            DiaChi = data.DiaChi;
            TenKhachHang = data.TenKhachHang;

        }
        
        private async void LoadLoaiTietKiem()
        {
            var danhSach = await _loaiTietKiemRepo.GetAll();
            DsLoaiTietKiem = [.. danhSach];
        }

        private void ExecuteClose()
        {
            Application.Current.Windows.OfType<CapNhatSoTietKiem>().FirstOrDefault()?.Close();
        }
        
        private async void CapNhat()
        {
            // Kiểm tra các trường
            string validationError = await ValidateFields();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (LoaiTietKiemDuocChon == null)
            {
                MessageBox.Show("Vui lòng chọn loại tiết kiệm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var loaiTietKiem = await _loaiTietKiemRepo.GetById(LoaiTietKiemDuocChon.MaLoaiTietKiem);
            var khachHang = await _khachHangRepo.GetById(Cmnd);
            // Tạo đối tượng SoTietKiem từ dữ liệu hiện có
            var soTietKiem = new SoTietKiem
            {
                MaSoTietKiem = MaSoTietKiem,
                LoaiTietKiem = loaiTietKiem,
                MaLoaiTietKiem = LoaiTietKiemDuocChon.MaLoaiTietKiem,
                SoTienGui = SoTienGui,
                NgayMoSo = NgayMoSo,
                KhachHang = khachHang,
                CMND = Cmnd
            };

            // Gọi repository để lưu vào CSDL
            try
            {
                await _soTietKiemRepo.Update(soTietKiem);
                MessageBox.Show("Cập nhật sổ tiết kiệm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                CapNhatEvent?.Invoke(this, soTietKiem);
                ExecuteClose();
            }
            catch
            {
                MessageBox.Show("Cập nhật sổ tiết kiệm thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async Task<string> ValidateFields()
        {
            // Kiểm tra CMND
            if (string.IsNullOrWhiteSpace(Cmnd))
            {
                return "Vui lòng nhập số CMND.";
            }

            // Kiểm tra khách hàng có tồn tại không
            if (String.IsNullOrEmpty(DiaChi) || String.IsNullOrEmpty(TenKhachHang))
            {
                return "Không tìm thấy khách hàng với số CMND đã nhập.";
            }

            // Kiểm tra loại tiết kiệm được chọn
            if (LoaiTietKiemDuocChon == null)
            {
                return "Vui lòng chọn loại tiết kiệm.";
            }

            var thamSo = await _thamSoRepo.Get();
            // Kiểm tra số tiền gởi
            if (SoTienGui < thamSo.SoTienGoiToiThieu)
            {
                Console.WriteLine(SoTienGui);

                return $"Số tiền gởi tối thiểu là {thamSo.SoTienGoiToiThieu}.";
            }

            // Kiểm tra ngày mở sổ
            if (NgayMoSo == default || NgayMoSo > DateTime.Today)
            {
                return "Ngày mở sổ không hợp lệ.";
            }

            // Nếu tất cả đều hợp lệ, trả về null
            return null!;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}