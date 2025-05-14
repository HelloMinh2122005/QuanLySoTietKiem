using QuanLyDaiLy.Entities;
namespace QuanLyDaiLy.Interfaces;

public interface IPhieuGoiTienRepo
{
    Task<IEnumerable<PhieuGoiTien>> GetAll();
    Task<PhieuGoiTien> GetById(string id);
    Task Create(PhieuGoiTien phieuGoiTien);
    Task Delete(string id);
 //   Task<IEnumerable<PhieuGoiTien>> Search(string? maPhieuGoiTien, string? searchText);
}

