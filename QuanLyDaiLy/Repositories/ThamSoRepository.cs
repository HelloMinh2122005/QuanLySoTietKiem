using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories; 

public class ThamSoRepository : IThamSoRepo
{
    private readonly DataContext _dataContext;
    public ThamSoRepository(DatabaseService databaseService)
    {
        _dataContext = databaseService.DataContext;
        if (_dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService), "Database not initialized");
        }
    }

    public async Task<ThamSo> Get()
    {
        var thamSo = await _dataContext.Set<ThamSo>().FirstOrDefaultAsync();
        return thamSo ?? throw new Exception("ThamSo not found");
    }

    public async Task Update(ThamSo thamSo)
    {
        _dataContext.Entry(thamSo).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }
}
