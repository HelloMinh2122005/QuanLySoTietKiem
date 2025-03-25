using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class LoaiTietKiemRepository : ILoaiTietKiemRepo
{
    private readonly DataContext dataContext;
    public LoaiTietKiemRepository(DatabaseService databaseService)
    {
        dataContext = databaseService.DataContext;
        if (dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService.DataContext), "Database not initialized");
        }
    }

    public async Task Create(LoaiTietKiem loaiTietKiem)
    {
        dataContext.DsLoaiTietKiem.Add(loaiTietKiem);
        await dataContext.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var loaiTietKiem = await dataContext.DsLoaiTietKiem.FindAsync(id);
        if (loaiTietKiem != null)
        {
            dataContext.DsLoaiTietKiem.Remove(loaiTietKiem);
            await dataContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<LoaiTietKiem>> GetAll()
    {
        return await dataContext.DsLoaiTietKiem
            .Include(ltk => ltk.DsSoTietKiem)
            .ToListAsync();
    }

    public async Task<LoaiTietKiem> GetById(string id)
    {
        LoaiTietKiem? loaiTietKiem = await dataContext.DsLoaiTietKiem
            .Include(ltk => ltk.DsSoTietKiem)
            .FirstOrDefaultAsync(ltk => ltk.MaLoaiTietKiem == id);
        return loaiTietKiem ?? throw new Exception("LoaiTietKiem not found");
    }

    public async Task Update(LoaiTietKiem loaiTietKiem)
    {
        dataContext.Entry(loaiTietKiem).State = EntityState.Modified;
        await dataContext.SaveChangesAsync();
    }
}