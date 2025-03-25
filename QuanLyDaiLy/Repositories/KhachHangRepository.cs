using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class KhachHangRepository(DatabaseService databaseService) : IKhachHangRepo
{
    public async Task<KhachHang> FindByCMND(string cmnd)
    {
        return await databaseService.DataContext.DsKhachHang
            .FirstOrDefaultAsync(kh => kh.CMND == cmnd);
    }  
    
    public async Task<List<KhachHang>> FindAll()
    {
        return await databaseService.DataContext.DsKhachHang.ToListAsync();
    }
}