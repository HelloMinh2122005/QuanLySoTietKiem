using QuanLyDaiLy.Views.PhieuGoiTienViews;
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
    public bool DangMo { get; set; } = true;
    public decimal LaiSuat { get; set; } = 0;
    public int SoLanDaoHan { get; set; } = 0;
    
    public ICollection<PhieuGoiTien> DsPhieuGoiTien { get; set; } = [];
    public ICollection<PhieuRutTien> DsPhieuRutTien { get; set; } = [];
    // Navigation properties
    public KhachHang KhachHang { get; set; } = new();
    public LoaiTietKiem LoaiTietKiem { get; set; } = new();

}
