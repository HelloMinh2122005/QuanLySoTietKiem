using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class KhachHangRepository : IKhachHangRepo
{
    private readonly DataContext dataContext;
    public KhachHangRepository(DatabaseService databaseService)
    {
        dataContext = databaseService.DataContext;
        if (dataContext == null)
        {
            throw new ArgumentNullException(nameof(databaseService.DataContext), "Database not initialized");
        }
    }
    public async Task Create(KhachHang khachHang)
    {
        dataContext.DsKhachHang.Add(khachHang);
        await dataContext.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var khachHang = await dataContext.DsKhachHang.FindAsync(id);
        if (khachHang != null)
        {
            dataContext.DsKhachHang.Remove(khachHang);
            await dataContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<KhachHang>> GetAll()
    {
        return await dataContext.DsKhachHang
            .Include(kh => kh.DsSoTietKiem)
            .ToListAsync();
    }

    public async Task<KhachHang> GetById(string id)
    {
        KhachHang? khachHang = await dataContext.DsKhachHang.Include(kh => kh.DsSoTietKiem).FirstOrDefaultAsync(kh => kh.CMND == id);
        return khachHang ?? throw new Exception("KhachHang not found");
    }

    public async Task Update(KhachHang khachHang)
    {
        dataContext.Entry(khachHang).State = EntityState.Modified;
        await dataContext.SaveChangesAsync();
    }
    public void Detach(KhachHang khachHang)
    {
        dataContext.Entry(khachHang).State = EntityState.Detached;
    }
}
