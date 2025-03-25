using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface IKhachHangRepo
{
    Task<KhachHang> FindByCMND(string cmnd);
    Task<List<KhachHang>> FindAll();
    
}