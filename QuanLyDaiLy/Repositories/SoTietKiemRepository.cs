
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
          .Include(stk => stk.KhachHang)
          .ToListAsync();
    }

    public async Task<SoTietKiem> GetById(string id)
    {
        SoTietKiem? soTietKiem = await _dataContext.DsSoTietKiem.FindAsync(id);
        return soTietKiem ?? throw new Exception("SoTietKiem not found");
    }

    public async Task Update(SoTietKiem soTietKiem)
    {
        // Tìm entity hiện có trong database
        var existingEntity = await _dataContext.DsSoTietKiem
            .FindAsync(soTietKiem.MaSoTietKiem);

        if (existingEntity == null)
        {
            throw new Exception("Không tìm thấy sổ tiết kiệm.");
        }

        // Sao chép giá trị từ tham số vào entity đã tải
        _dataContext.Entry(existingEntity).CurrentValues.SetValues(soTietKiem);

        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Xử lý xung đột concurrency
            var entry = ex.Entries.Single();
            await entry.ReloadAsync(); // Tải lại giá trị mới từ database
            throw new Exception("Dữ liệu đã bị thay đổi. Vui lòng thử lại.");
        }
    }
    
    public async Task<IEnumerable<SoTietKiem>> Search(string? maLoaiTietKiem, string? searchText)
    {
        var query = _dataContext.DsSoTietKiem.AsQueryable();

        if (!string.IsNullOrEmpty(maLoaiTietKiem))
        {
            query = query.Where(stk => stk.MaLoaiTietKiem == maLoaiTietKiem);
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(stk =>
                stk.MaSoTietKiem.ToString().Contains(searchText) ||
                stk.KhachHang.TenKhachHang.Contains(searchText));
        }

        return await query.Include(stk => stk.KhachHang).ToListAsync();
    }
}
