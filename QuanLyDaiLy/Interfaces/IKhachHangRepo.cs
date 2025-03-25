using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface IKhachHangRepo
{
    Task<IEnumerable<KhachHang>> GetAll();
    Task<KhachHang> GetById(string id);
    Task Create(KhachHang khachHang);
    Task Update(KhachHang khachHang);
    Task Delete(string id);
}
