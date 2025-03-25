using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface ILoaiTietKiemRepo
{
    Task<List<LoaiTietKiem>> FindAll();
    Task<LoaiTietKiem> FindTheoMaLoaiTietKiemAsync(string maLoaiTietKiem);

}