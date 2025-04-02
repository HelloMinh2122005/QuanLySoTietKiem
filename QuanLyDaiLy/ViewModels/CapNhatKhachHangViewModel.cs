using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Views;

namespace QuanLyDaiLy.ViewModels
{
    public class CapNhatKhachHangViewModel: INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        public ICommand UpdateCommand { get; }

        private readonly IKhachHangRepo _khachHangRepo;
        public event EventHandler<KhachHang>? CapNhatEvent;

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

        public CapNhatKhachHangViewModel(IKhachHangRepo khachHangRepo)
        {
            //command
            CloseCommand = new RelayCommand(ExecuteClose);
            UpdateCommand = new RelayCommand(async () => await CapNhat());

            //repo
            _khachHangRepo = khachHangRepo;

        }
        public void SetData(KhachHang khachHang)
        {
            cmnd = khachHang.CMND;
            diachi = khachHang.DiaChi;
            tenKhachHang = khachHang.TenKhachHang;
        }

        private async Task CapNhat()
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
                // Check if the entity is already being tracked
                var existingEntity = await _khachHangRepo.GetById(cmnd);
                if (existingEntity != null)
                {
                    // Detach the existing entity
                    _khachHangRepo.Detach(existingEntity);
                }

                await _khachHangRepo.Update(khachHang);
                MessageBox.Show("Update KH thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                CapNhatEvent?.Invoke(this, khachHang);
            }
            catch (Exception e)
            {
                // Log the exception details
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Stack Trace: {e.StackTrace}");
                MessageBox.Show($"Update KH thất bại. Vui lòng thử lại. Error: {e.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteClose()
        {
            Window parentWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);
            parentWindow?.Close();
        }

        private void ResetFields()
        {
            cmnd = string.Empty;
            tenKhachHang = string.Empty;
            diachi = string.Empty;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
