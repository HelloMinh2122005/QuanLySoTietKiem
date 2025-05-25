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
    public class ThemPhieuGoiTienViewModel: INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        public ICommand LapPhieuCommand { get;  } 

        public event EventHandler<PhieuGoiTien>? LapPhieuEvent;
        private readonly IPhieuGoiTienRepo _phieuGoiTienRepo;
        private readonly ISoTietKiemRepo _soTietKiemRepo;
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly IThamSoRepo _thamSoRepo;
        private readonly ILoaiTietKiemRepo _loaitietkiemRepo;

        private string _maPhieuGoiTien;
        public string MaPhieuGoiTien
        {
            get => _maPhieuGoiTien;
            set
            {
                if (_maPhieuGoiTien != value)
                {
                    _maPhieuGoiTien = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _ngayGoi;
        public DateTime NgayGoi
        {
            get => _ngayGoi;
            set
            {
                if (_ngayGoi != value)
                {
                    _ngayGoi = value;
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
                    LoadFields(); //update UI
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
                IsFieldsEnabled = false; // Enable fields for user input
                return;
            }

            try
            {
                // Fetch SoTietKiem
                var soTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
                if (soTietKiem == null)
                {
                    MessageBox.Show("Không tìm thấy sổ tiết kiệm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    IsFieldsEnabled = true; // Enable fields for user input
                    return;
                }

                // Fetch KhachHang
                var khachHang = await _khachHangRepo.GetById(soTietKiem.CMND);
                TenKhachHang = khachHang?.TenKhachHang ?? "Không xác định";

                // Fetch LoaiTietKiem
                var loaiTietKiem = await _loaitietkiemRepo.GetById(soTietKiem.MaLoaiTietKiem);
                TenLoaiTietKiem = loaiTietKiem?.TenLoaiTietKiem ?? "Không xác định";
                // validate LoaiTietKiem
                IsFieldsEnabled = loaiTietKiem?.TenLoaiTietKiem == "Không kỳ hạn";
                if (!IsFieldsEnabled)
                {
                    MessageBox.Show("Không thể gửi thêm vào loại tiết kiệm này!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin sổ tiết kiệm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private bool _isFieldsEnabled = false;
        public bool IsFieldsEnabled
        {
            get => _isFieldsEnabled;
            set
            {
                if (_isFieldsEnabled != value)
                {
                    _isFieldsEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FieldStatusText));
                }
            }
        }

        public string FieldStatusText
        {
            get => IsFieldsEnabled ? "Đang áp dụng" : string.Empty;
        }

        public ThemPhieuGoiTienViewModel(IPhieuGoiTienRepo phieuGoiTienRepo, ISoTietKiemRepo soTietKiemRepo, IKhachHangRepo khachHangRepo, IThamSoRepo thamSoRepo, ILoaiTietKiemRepo loaitietkiemRepo)
        {
            //repo
            _phieuGoiTienRepo = phieuGoiTienRepo;
            _soTietKiemRepo = soTietKiemRepo;
            _khachHangRepo = khachHangRepo;
            _thamSoRepo = thamSoRepo;
            _loaitietkiemRepo = loaitietkiemRepo;
            //command
            CloseCommand = new RelayCommand(ExecuteClose);
            LapPhieuCommand = new RelayCommand(LapPhieu);

            //load fields
            NgayGoi = DateTime.Now;
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

            //create new PhieuGoiTien
            var SoTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
            if (SoTietKiem == null)
            {
                MessageBox.Show("Không tìm thấy sổ tiết kiệm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var phieuGoiTien = new PhieuGoiTien
            {
                MaPhieuGoiTien = IdGenerator.GenerateId<PhieuGoiTien>(),
                NgayGoi = NgayGoi,
                SoTienGui = SoTienGui,
                MaSoTietKiem = MaSoTietKiem,
                SoTietKiem = SoTietKiem
            };

            try
            {
                await _phieuGoiTienRepo.Create(phieuGoiTien);

                //update SoTietKiem
                SoTietKiem.SoTienGui += SoTienGui;
                await _soTietKiemRepo.Update(SoTietKiem);

                MessageBox.Show("Lập phiếu gửi tiền thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LapPhieuEvent?.Invoke(this, phieuGoiTien);
                MaPhieuGoiTien = phieuGoiTien.MaPhieuGoiTien;
                //reset fields
                SoTienGui = 0;
                MaSoTietKiem = string.Empty;
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi lập phiếu gửi tiền: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> ValidateFields()
        {
            var thamSo = await _thamSoRepo.Get();
            
            if (SoTienGui < thamSo.SoTienGuiThemToiThieu)
            {
                Console.WriteLine(SoTienGui);

                return $"Số tiền gửi thêm tối thiểu là {thamSo.SoTienGuiThemToiThieu}.";
            }
            // only add LTK "Khong ky han" khi quy dinh ap dung duoc bat
            var soTietKiem = await _soTietKiemRepo.GetById(MaSoTietKiem);
            var loaiTietKiem = await _loaitietkiemRepo.GetById(soTietKiem.MaLoaiTietKiem);
            if (soTietKiem.MaLoaiTietKiem != "LTK0830010523001" && loaiTietKiem.ApDungSoTienGuiThemToiThieu == true)
            {
                Console.WriteLine(soTietKiem.MaLoaiTietKiem);
                return "Chỉ được gửi thêm vào loại tiết kiệm không kỳ hạn.";
            }

            return null;
        }

        private void ExecuteClose()
        {
            Application.Current.Windows.OfType<lapPhieuGoiTien>().FirstOrDefault()?.Close();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
