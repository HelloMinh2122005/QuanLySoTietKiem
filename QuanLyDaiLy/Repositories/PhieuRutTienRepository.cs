using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class PhieuRutTienRepository: IPhieuRutTienRepo
{
    private readonly DataContext _dataContext;

    public PhieuRutTienRepository(DatabaseService databaseService)
    {
        _dataContext = databaseService.DataContext;
        if (_dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService), "Database not initialized");
        }
    }

    public async Task<IEnumerable<PhieuRutTien>> GetAll()
    {
        return await _dataContext.Set<PhieuRutTien>().ToListAsync();
    }

    public async Task<PhieuRutTien> GetById(string id)
    {
        var phieuRutTien = await _dataContext.Set<PhieuRutTien>().FindAsync(id);
        return phieuRutTien ?? throw new Exception("PhieuRutTien not found");
    }

    public async Task Create(PhieuRutTien phieuRutTien)
    {
        await _dataContext.Set<PhieuRutTien>().AddAsync(phieuRutTien);
        await _dataContext.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var phieuRutTien = await GetById(id);
        _dataContext.Set<PhieuRutTien>().Remove(phieuRutTien);
        await _dataContext.SaveChangesAsync();
    }
}
