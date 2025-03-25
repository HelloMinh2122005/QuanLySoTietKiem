using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface ISoTietKiemRepo
{
    Task<bool> ThemSoTietKiemAsync(SoTietKiem soTietKiem);
    Task<List<SoTietKiem>> FindAllAsync();
    Task<List<SoTietKiem>> FindAllByLoaiTietKiemAsync(string maLoaiTietKiem, string maSearchText);
    Task XoaSoTietKiem(long maSoTietKiem);

    Task<bool> CapNhatTheoMaTietKiem(long maSoTietKiem, SoTietKiem soTietKiemMoi);
}