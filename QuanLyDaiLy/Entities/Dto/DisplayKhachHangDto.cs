using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.Entities.Dto
{
    public class DisplayKhachHangDto : INotifyPropertyChanged
    {
        private KhachHang _khachHang = null!;
        public KhachHang KhachHang
        {
            get => _khachHang;
            set
            {
                if (_khachHang != value)
                {
                    _khachHang = value;
                    OnPropertyChanged();

                    if (_khachHang?.DsSoTietKiem != null)
                    {
                        SoDu = CalculateSoDu();
                    }
                    else
                    {
                        SoDu = 0;
                    }
                }
            }
        }

        private decimal _soDu;
        public decimal SoDu
        {
            get => _soDu;
            set
            {
                if (_soDu != value)
                {
                    _soDu = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal CalculateSoDu()
        {
            if (KhachHang?.DsSoTietKiem == null || !KhachHang.DsSoTietKiem.Any())
            {
                return 0;
            }

            return KhachHang.DsSoTietKiem.Sum(stk => stk.SoTienGui);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
