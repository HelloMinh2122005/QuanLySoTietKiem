using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Animation;

namespace QuanLyDaiLy.Entities;

public class LoaiTietKiem
{
    [Key]
    public string MaLoaiTietKiem { get; set; } = "";
    public string TenLoaiTietKiem { get; set; } = "";
    public bool NhanTienGoiThem { get; set; } = true;
    public bool ApDungSoTienGuiThemToiThieu { get; set; } = true;
    public decimal LaiSuatQuyDinh { get; set; } = 0;
    public bool QuyDinhRutHetTien { get; set; } = true;
    public int KyHan { get; set; } = 0;
    public ICollection<SoTietKiem> DsSoTietKiem { get; set; } = [];
}