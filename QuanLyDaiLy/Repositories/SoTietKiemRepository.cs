
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
    
    public async Task<IEnumerable<SoTietKiem>> SearchSoTietKiem(
    string? maSoTietKiem,
    LoaiTietKiem? loaiTietKiem,
    string? cmnd,
    string? tenKhachHang,
    decimal? laiSuatTu,
    decimal? laiSuatDen,
    DateTime? ngayMoSoTu,
    DateTime? ngayMoSoDen,
    decimal? soDuTu,
    decimal? soDuDen,
    DateTime? ngayGuiPhieuGuiTienTu,
    DateTime? ngayGuiPhieuGuiTienDen,
    DateTime? ngayGuiPhieuRutTienTu,
    DateTime? ngayGuiPhieuRutTienDen,
    decimal? soTienGuiTu,
    decimal? soTienGuiDen,
    decimal? soTienRutTu,
    decimal? soTienRutDen,
    int? soLuongPhieuGuiTienTu,
    int? soLuongPhieuGuiTienDen,
    int? soLuongPhieuRutTienTu,
    int? soLuongPhieuRutTienDen)
{
    var query = _dataContext.DsSoTietKiem
        .Include(stk => stk.LoaiTietKiem)
        .Include(stk => stk.KhachHang)
        .Include(stk => stk.DsPhieuGoiTien)
        // .Include(stk => stk.DsPhieuRutTien)
        .AsQueryable();

    if (!string.IsNullOrEmpty(maSoTietKiem))
        query = query.Where(stk => stk.MaSoTietKiem.Contains(maSoTietKiem));

    string tenLoaiTietKiem = loaiTietKiem?.TenLoaiTietKiem;
    if (!string.IsNullOrEmpty(tenLoaiTietKiem))
        query = query.Where(stk => stk.LoaiTietKiem.TenLoaiTietKiem.Contains(tenLoaiTietKiem));

    if (!string.IsNullOrEmpty(cmnd))
        query = query.Where(stk => stk.KhachHang.CMND.Contains(cmnd));

    if (!string.IsNullOrEmpty(tenKhachHang))
        query = query.Where(stk => stk.KhachHang.TenKhachHang.Contains(tenKhachHang));

    if (laiSuatTu.HasValue)
        query = query.Where(stk => stk.LoaiTietKiem.LaiSuatQuyDinh >= laiSuatTu.Value);

    if (laiSuatDen.HasValue)
        query = query.Where(stk => stk.LoaiTietKiem.LaiSuatQuyDinh <= laiSuatDen.Value);

    if (ngayMoSoTu.HasValue)
        query = query.Where(stk => stk.NgayMoSo >= ngayMoSoTu.Value);

    if (ngayMoSoDen.HasValue)
        query = query.Where(stk => stk.NgayMoSo <= ngayMoSoDen.Value);

    if (soDuTu.HasValue)
        query = query.Where(stk => stk.SoTienGui >= soDuTu.Value);

    if (soDuDen.HasValue)
        query = query.Where(stk => stk.SoTienGui <= soDuDen.Value);

    if (ngayGuiPhieuGuiTienTu.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Any(pgt => pgt.NgayGoi >= ngayGuiPhieuGuiTienTu.Value));

    if (ngayGuiPhieuGuiTienDen.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Any(pgt => pgt.NgayGoi <= ngayGuiPhieuGuiTienDen.Value));

    // if (ngayGuiPhieuRutTienTu.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Any(prt => prt.NgayLapPhieu >= ngayGuiPhieuRutTienTu.Value));
    //
    // if (ngayGuiPhieuRutTienDen.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Any(prt => prt.NgayLapPhieu <= ngayGuiPhieuRutTienDen.Value));

    if (soTienGuiTu.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Any(pgt => pgt.SoTienGui >= soTienGuiTu.Value));

    if (soTienGuiDen.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Any(pgt => pgt.SoTienGui <= soTienGuiDen.Value));

    // if (soTienRutTu.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Any(prt => prt.SoTienRut >= soTienRutTu.Value));
    //
    // if (soTienRutDen.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Any(prt => prt.SoTienRut <= soTienRutDen.Value));

    if (soLuongPhieuGuiTienTu.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Count >= soLuongPhieuGuiTienTu.Value);

    if (soLuongPhieuGuiTienDen.HasValue)
        query = query.Where(stk => stk.DsPhieuGoiTien.Count <= soLuongPhieuGuiTienDen.Value);

    // if (soLuongPhieuRutTienTu.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Count >= soLuongPhieuRutTienTu.Value);
    //
    // if (soLuongPhieuRutTienDen.HasValue)
    //     query = query.Where(stk => stk.DsPhieuRutTien.Count <= soLuongPhieuRutTienDen.Value);

    return await query.ToListAsync();
}
}
