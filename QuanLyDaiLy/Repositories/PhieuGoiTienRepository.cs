using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;
public class PhieuGoiTienRepository : IPhieuGoiTienRepo
{
    private readonly DataContext _dataContext;
    public PhieuGoiTienRepository(DatabaseService databaseService)
    {
        _dataContext = databaseService.DataContext;
        if (_dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService), "Database not initialized");
        }
    }

    public async Task<IEnumerable<PhieuGoiTien>> GetAll()
    {
        return await _dataContext.Set<PhieuGoiTien>().ToListAsync();
    }

    public async Task<PhieuGoiTien> GetById(string id)
    {
        var phieuGoiTien = await _dataContext.Set<PhieuGoiTien>().FindAsync(id);
        return phieuGoiTien ?? throw new Exception("PhieuGoiTien not found");
    }

    public async Task Create(PhieuGoiTien phieuGoiTien)
    {
        await _dataContext.Set<PhieuGoiTien>().AddAsync(phieuGoiTien);
        await _dataContext.SaveChangesAsync();
    }
    public async Task Delete(string id)
    {
        var phieuGoiTien = await GetById(id);
        _dataContext.Set<PhieuGoiTien>().Remove(phieuGoiTien);
        await _dataContext.SaveChangesAsync();
    }

}

