using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories;

public class LoaiTietKiemRepository(DatabaseService databaseService) : ILoaiTietKiemRepo
{
    public async Task<List<LoaiTietKiem>> FindAll()
    {
        return await databaseService.DataContext.DsLoaiTietKiem.ToListAsync();
    }
    
    public async Task<LoaiTietKiem> FindTheoMaLoaiTietKiemAsync(string maLoaiTietKiem)
    {
        try
        {
            var result = await databaseService.DataContext.DsLoaiTietKiem
                .FirstOrDefaultAsync(ltk => ltk.MaLoaiTietKiem == maLoaiTietKiem);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tìm sổ theo mã loại tiết kiệm: {ex.Message}");
            return new LoaiTietKiem();
        }
    }

}