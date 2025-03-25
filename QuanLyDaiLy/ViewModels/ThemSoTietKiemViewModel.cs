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

namespace QuanLyDaiLy.ViewModels
{
    public class ThemSoTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        
        public ICommand LapSoCommand { get; }
        
        public ICommand ThemSoCommand { get; }
        public event EventHandler<SoTietKiem>? LapSoEvent;
        
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;
        
        private readonly IKhachHangRepo _khachHangRepo;
        
        private readonly ISoTietKiemRepo _soTietKiemRepo;

        private readonly IThamSoRepo _thamSoRepo;



        private DateTime _ngayMoSo;
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

        private ObservableCollection<LoaiTietKiem> _dsLoaiTietKiem;
        public ObservableCollection<LoaiTietKiem> DsLoaiTietKiem
        {
            get => _dsLoaiTietKiem;
            set
            {
                _dsLoaiTietKiem = value;
                OnPropertyChanged();
            }
        }
        
        private LoaiTietKiem? _loaiTietKiemDuocChon;
        public LoaiTietKiem? LoaiTietKiemDuocChon
        {
            get => _loaiTietKiemDuocChon;
            set
            {
                _loaiTietKiemDuocChon = value;
                OnPropertyChanged();
            }
        }

        
        private KhachHang _khachHang;
        public KhachHang KhachHang
        {
            get => _khachHang;
            set
            {
                if (_khachHang != value)
                {
                    _khachHang = value;
                    OnPropertyChanged();
                }
               
            }
        }
        
        private string _cmnd;
        public string Cmnd
        {
            get => _cmnd;
            set
            {
                if (_cmnd != value)
                {
                    _cmnd = value;
                    OnPropertyChanged();
                    TimKiemKhachHangAsync(value);
                }
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

        public ThemSoTietKiemViewModel(IKhachHangRepo khachHangRepo,ILoaiTietKiemRepo LoaiTietKiemRepo,ISoTietKiemRepo SoTietKiemRepo, IThamSoRepo ThamSoRepo)
        {
            //command
            CloseCommand = new RelayCommand(ExecuteClose);
            LapSoCommand = new RelayCommand(LapSo);
            ThemSoCommand = new RelayCommand(ResetFields);
            
            //repo
            _khachHangRepo = khachHangRepo;
            _loaiTietKiemRepo = LoaiTietKiemRepo;
            _soTietKiemRepo = SoTietKiemRepo;
            _thamSoRepo = ThamSoRepo;

            //field
            NgayMoSo = DateTime.Today;
            LoadLoaiTietKiem();
        }
        
        private void ResetFields()
        {
            NgayMoSo = DateTime.Today;
            Cmnd = string.Empty;
            KhachHang = null;
            LoaiTietKiemDuocChon = null;
            SoTienGui = 0;
        }

        private async void LoadLoaiTietKiem()
        {
            var danhSach = await _loaiTietKiemRepo.GetAll();
            DsLoaiTietKiem = new ObservableCollection<LoaiTietKiem>(danhSach);
        }
        
        private async void LapSo()
        {
            // Kiểm tra các trường
            string validationError = await ValidateFields();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var loaiTietKiem = await _loaiTietKiemRepo.GetById(LoaiTietKiemDuocChon.MaLoaiTietKiem);
            var khachHang = await _khachHangRepo.GetById(Cmnd);
            // Tạo đối tượng SoTietKiem từ dữ liệu hiện có
            var soTietKiem = new SoTietKiem
            {
                MaSoTietKiem = IdGenerator.GenerateId<SoTietKiem>(),
                MaLoaiTietKiem = LoaiTietKiemDuocChon.MaLoaiTietKiem,
                SoTienGui = SoTienGui,
                KhachHang = khachHang,
                LoaiTietKiem = loaiTietKiem,
                NgayMoSo = NgayMoSo,
                CMND = Cmnd
            };

            // Gọi repository để lưu vào CSDL
            try
            {
                await _soTietKiemRepo.Create(soTietKiem);
                MessageBox.Show("Lập sổ tiết kiệm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LapSoEvent?.Invoke(this, soTietKiem);
                ExecuteClose();
            }
            catch (Exception e)
            {
                MessageBox.Show("Lập sổ tiết kiệm thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (KhachHang == null)
            {
                return "Không tìm thấy khách hàng với số CMND đã nhập.";
            }

            // Kiểm tra loại tiết kiệm được chọn
            if (LoaiTietKiemDuocChon == null)
            {
                return "Vui lòng chọn loại tiết kiệm.";
            }

            var thamSo = await _thamSoRepo.Get();
            // Kiểm tra số tiền gửi
            if (SoTienGui < thamSo.SoTienGoiToiThieu)
            {
                Console.WriteLine(SoTienGui);

                return $"Số tiền gửi tối thiểu là {thamSo.SoTienGoiToiThieu}.";
            }


            // Kiểm tra ngày mở sổ
            if (NgayMoSo == default || NgayMoSo > DateTime.Today)
            {
                return "Ngày mở sổ không hợp lệ.";
            }

            // Nếu tất cả đều hợp lệ, trả về null
            return null;
        }
        
        private void ExecuteClose()
        {
            Application.Current.Windows.OfType<ThemSoTietKiem>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private async Task TimKiemKhachHangAsync(string cmnd)
        {
            KhachHang = await _khachHangRepo.GetById(cmnd);
        }
    }
} 