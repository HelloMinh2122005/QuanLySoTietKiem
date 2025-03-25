using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class LoaiTietKiem
{
    [Key]
    public string MaLoaiTietKiem { get; set; } = "";
    public LoaiTietKiemEnum? TenLoaiTietKiem { get; set; } = LoaiTietKiemEnum.KhongKyHan;
    public ICollection<SoTietKiem> DsSoTietKiem { get; set; } = [];
}

public enum LoaiTietKiemEnum
{
    KhongKyHan,
    BaThang,
    SauThang
}
