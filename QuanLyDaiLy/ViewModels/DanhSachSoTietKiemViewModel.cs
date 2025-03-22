using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace QuanLyDaiLy.ViewModels
{
    public class DanhSachSoTietKiemViewModel : INotifyPropertyChanged
    {
        public class SoTietKiem
        {
            public int STT { get; set; }
            public string MaSo { get; set; }
            public string LoaiTietKiem { get; set; }
            public string KhachHang { get; set; }
            public decimal SoDu { get; set; }
            public bool IsSelected { get; set; }
        }

        private ObservableCollection<SoTietKiem> _danhSachSoTietKiem;

        public DanhSachSoTietKiemViewModel()
        {
            var danhSach = new List<SoTietKiem>
            {
                new SoTietKiem { IsSelected = false, MaSo = "23520906", LoaiTietKiem = "Không kỳ hạn", KhachHang = "Hứa Văn Lý", SoDu = 15756.800m },
                new SoTietKiem { IsSelected = false, MaSo = "23520923", LoaiTietKiem = "3 tháng", KhachHang = "Hồ Nguyên Minh", SoDu = 21854.320m },
                new SoTietKiem { IsSelected = false, MaSo = "23520922", LoaiTietKiem = "6 tháng", KhachHang = "Trần Thị Mộng Trúc Ngân", SoDu = 17356.480m },
                new SoTietKiem { IsSelected = false, MaSo = "23520950", LoaiTietKiem = "Không kỳ hạn", KhachHang = "Nguyễn Tấn Trần Minh Khang", SoDu = 100739645.000m },
                new SoTietKiem { IsSelected = false, MaSo = "12345678", LoaiTietKiem = "3 tháng", KhachHang = "Huỳnh Thị Hồ Mộng Trinh", SoDu = 19375698.000m }
            };

            for (int i = 0; i < danhSach.Count; i++)
            {
                danhSach[i].STT = i + 1;
            }

            DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(danhSach);
        }

        public ObservableCollection<SoTietKiem> DanhSachSoTietKiem
        {
            get => _danhSachSoTietKiem;
            set
            {
                _danhSachSoTietKiem = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}