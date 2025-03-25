using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface ISoTietKiemRepo
{
    Task<IEnumerable<SoTietKiem>> GetAll();
    Task<SoTietKiem> GetById(string id);
    Task Create(SoTietKiem soTietKiem);
    Task Update(SoTietKiem soTietKiem);
    Task Delete(string id);
}
