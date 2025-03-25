using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class LoaiTietKiem
{
    [Key]
    public string MaLoaiTietKiem { get; set; } = "";
    public string TenLoaiTietKiem { get; set; } = "";
    public ICollection<SoTietKiem> DsSoTietKiem { get; set; } = [];
}