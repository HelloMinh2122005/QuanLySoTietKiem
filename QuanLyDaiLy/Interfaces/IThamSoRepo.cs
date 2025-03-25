using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface IThamSoRepo
{
    Task<ThamSo> Get();
    Task Update(ThamSo thamSo);
}
