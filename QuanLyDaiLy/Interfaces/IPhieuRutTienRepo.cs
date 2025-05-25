using QuanLyDaiLy.Entities;
namespace QuanLyDaiLy.Interfaces;

public interface IPhieuRutTienRepo
{
    Task<IEnumerable<PhieuRutTien>> GetAll();
    Task<PhieuRutTien> GetById(string id);
    Task Create(PhieuRutTien phieuRutTien);
    Task Delete(string id);
}
