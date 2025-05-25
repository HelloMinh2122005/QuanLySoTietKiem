using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDaiLy.Entities.Dto
{
    public class PhieuRutTienDTO: INotifyPropertyChanged
    {
        public string MaPhieuRutTien { get; set; } = "";
        public DateTime NgayRut { get; set; } = DateTime.Now;
        public decimal SoTienRut { get; set; } = 0;
        public string MaSoTietKiem { get; set; } = "";
        //display DTO
        public string LoaiTietKiem { get; set; } = "";
        public string TenKhachHang { get; set; } = "";
        public DateTime NgayMoSo { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
