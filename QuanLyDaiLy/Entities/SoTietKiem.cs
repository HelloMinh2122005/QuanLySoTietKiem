using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class SoTietKiem
{
    [Key]
    public string MaSoTietKiem { get; set; } = "";
    public string CMND { get; set; } = "";
    public string MaLoaiTietKiem { get; set; } = "";
    public decimal SoTienGui { get; set; } = 0;
    public DateTime NgayMoSo { get; set; } = DateTime.Now;

    // Navigation properties
    public KhachHang KhachHang { get; set; } = new();
    public LoaiTietKiem LoaiTietKiem { get; set; } = new();
}