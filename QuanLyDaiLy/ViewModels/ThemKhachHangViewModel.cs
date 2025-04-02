using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace QuanLyDaiLy.ViewModels
{
    public class ThemKhachHangViewModel : INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        public ICommand LapPhieuCommand { get; }
        public ICommand ThemPhieuCommand { get; }

        private readonly IKhachHangRepo _khachHangRepo;
        public event EventHandler<KhachHang>? LapPhieuEvent;

        private string _cmnd;
        public string cmnd
        {
            get => _cmnd;
            set
            {
                if (_cmnd != value)
                {
                    _cmnd = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tenKhachHang;
        public string tenKhachHang
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

        private string _diachi;
        public string diachi
        {
            get => _diachi;
            set
            {
                if (_diachi != value)
                {
                    _diachi = value;
                    OnPropertyChanged();
                }
            }
        }

        public ThemKhachHangViewModel(IKhachHangRepo khachHangRepo)
        {
            //command
            CloseCommand = new RelayCommand(ExecuteClose);
            LapPhieuCommand = new RelayCommand(LapPhieu);
            ThemPhieuCommand = new RelayCommand(ResetFields);

            //repo
            _khachHangRepo = khachHangRepo;

        }

        public async void LapPhieu()
        {
            // Kiểm tra các trường
            if (string.IsNullOrEmpty(cmnd) || string.IsNullOrEmpty(diachi) || string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("validationError", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var khachHang = new KhachHang
            {
                CMND = cmnd,
                TenKhachHang = tenKhachHang,
                DiaChi = diachi,
            };

            // Gọi repository để lưu vào CSDL
            try
            {
                await _khachHangRepo.Create(khachHang);
                MessageBox.Show("Thêm khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LapPhieuEvent?.Invoke(this, khachHang);
                cmnd = khachHang.CMND;
            }
            catch (Exception e)
            {
                // Log the exception details
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Stack Trace: {e.StackTrace}");
                MessageBox.Show($"Lap phieu KH thất bại. Vui lòng thử lại. Error: {e.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ResetFields()
        {
            cmnd = string.Empty;
            tenKhachHang = string.Empty;
            diachi = string.Empty;
        }
        private void ExecuteClose()
        {
            Window parentWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);
            parentWindow?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
