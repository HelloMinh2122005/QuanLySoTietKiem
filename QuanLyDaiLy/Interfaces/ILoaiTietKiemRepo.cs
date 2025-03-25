using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface ILoaiTietKiemRepo
{
    Task<IEnumerable<LoaiTietKiem>> GetAll();
    Task<LoaiTietKiem> GetById(string id);
    Task Create(LoaiTietKiem soTietKiem);
    Task Update(LoaiTietKiem soTietKiem);
    Task Delete(string id);
}