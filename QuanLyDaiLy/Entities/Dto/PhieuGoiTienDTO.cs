using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDaiLy.Entities.Dto
{
    public class PhieuGoiTienDTO : INotifyPropertyChanged
    {
        public string MaPhieuGoiTien { get; set; } = "";
        public DateTime NgayGoi { get; set; } = DateTime.Now;
        public decimal SoTienGui { get; set; } = 0;
        public string MaSoTietKiem { get; set; } = "";
        //display DTO
        public string LoaiTietKiem { get; set; } = "";
        public string TenKhachHang { get; set; } = "";


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
