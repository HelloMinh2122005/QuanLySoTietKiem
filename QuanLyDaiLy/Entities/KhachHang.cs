using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class KhachHang
{
    [Key]
    public string CMND { get; set; } = "";
    public string TenKhachHang { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public ICollection<SoTietKiem> DsSoTietKiem { get; set; } = [];
}