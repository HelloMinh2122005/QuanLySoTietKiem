using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class SoTietKiemRepository : ISoTietKiemRepo
{
    private readonly DataContext _dataContext;
    public SoTietKiemRepository(DatabaseService databaseService)
    {
        _dataContext = databaseService.DataContext;
        if (_dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService), "Database not initialized");
        }
    }
    public async Task Create(SoTietKiem soTietKiem)
    {
        _dataContext.DsSoTietKiem.Add(soTietKiem);
        await _dataContext.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var existingSoTietKiem = await _dataContext.DsSoTietKiem.FindAsync(id);
        if (existingSoTietKiem != null)
        {
            _dataContext.DsSoTietKiem.Remove(existingSoTietKiem);
            await _dataContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<SoTietKiem>> GetAll()
    {
        return await _dataContext.DsSoTietKiem
            .ToListAsync();
    }

    public async Task<SoTietKiem> GetById(string id)
    {
        SoTietKiem? soTietKiem = await _dataContext.DsSoTietKiem.FindAsync(id);
        return soTietKiem ?? throw new Exception("SoTietKiem not found");
    }

    public async Task Update(SoTietKiem soTietKiem)
    {
        _dataContext.Entry(Update).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }
}
