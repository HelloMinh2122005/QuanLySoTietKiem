using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class LoaiTietKiem
{
    [Key]
    public string MaLoaiTietKiem { get; set; } = "";
    public string TenLoaiTietKiem { get; set; } = "";
    public bool NhanTienGoiThem { get; set; } = true;
    public bool ApDungSoTienGuiThemToiThieu { get; set; } = true;
    public ICollection<SoTietKiem> DsSoTietKiem { get; set; } = [];
}