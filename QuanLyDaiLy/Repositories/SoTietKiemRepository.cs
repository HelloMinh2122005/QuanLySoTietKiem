    using System.Windows;
    using Microsoft.EntityFrameworkCore;
    using QuanLyDaiLy.Entities;
    using QuanLyDaiLy.Interfaces;
    using QuanLyDaiLy.Services;

    namespace QuanLyDaiLy.Repositories;

    public class SoTietKiemRepository(DatabaseService databaseService) : ISoTietKiemRepo
    {
        public async Task<bool> ThemSoTietKiemAsync(SoTietKiem soTietKiem)
        {
            try
            {
                
                databaseService.DataContext.DsSoTietKiem.Add(soTietKiem);
                await databaseService.DataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm sổ tiết kiệm: {ex.Message}");
                return false;
            }
        }
        
        public async Task<List<SoTietKiem>> FindAllAsync()
        {
            try
            {
                if (databaseService == null)
                {
                    Console.WriteLine("databaseService is null");
                    return new List<SoTietKiem>();
                }

                if (databaseService.DataContext == null)
                {
                    Console.WriteLine("DataContext is null");
                    return new List<SoTietKiem>();
                }

                return await databaseService.DataContext.DsSoTietKiem.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi khi lay danh sach so tiet kiem: {ex.Message}");
                return new List<SoTietKiem>();
            }
        }

        public async Task<List<SoTietKiem>> FindAllByLoaiTietKiemAsync(string maLoaiTietKiem,string maSearchText)
        {
            try
            {
                if (databaseService == null)
                {
                    Console.WriteLine("databaseService is null");
                    return new List<SoTietKiem>();
                }

                if (databaseService.DataContext == null)
                {
                    Console.WriteLine("DataContext is null");
                    return new List<SoTietKiem>();
                }

                var query = databaseService.DataContext.DsSoTietKiem.AsQueryable();

                if (!string.IsNullOrEmpty(maLoaiTietKiem))
                {
                    query = query.Where(stk => stk.MaLoaiTietKiem == maLoaiTietKiem);
                }

                if (!string.IsNullOrEmpty(maSearchText))
                {
                    int transformedMaSearchText = Int32.Parse(maSearchText);
                    query = query.Where(stk => stk.MaSoTietKiem.Equals(transformedMaSearchText));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi khi lay danh sach so tiet kiem: {ex.Message}");
                return new List<SoTietKiem>();
            }
        }
        
        public async Task XoaSoTietKiem(long maSoTietKiem)
        {
            try
            {
                // Tìm bản ghi cần xóa
                var loaiTietKiem = await databaseService.DataContext.DsSoTietKiem.FirstOrDefaultAsync(l => l.MaSoTietKiem == maSoTietKiem);

                if (loaiTietKiem != null)
                {
                    // Xóa bản ghi
                    databaseService.DataContext.DsSoTietKiem.Remove(loaiTietKiem);
                    await databaseService.DataContext.SaveChangesAsync();
                    MessageBox.Show("Xoa thanh cong!");
                }
                else
                {
                    MessageBox.Show("Khong tim thay loai tiet kiem can xoa!");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Loi khi xoa so tiet kiem: {ex.Message}");
            }
        }

        public async Task<bool> CapNhatTheoMaTietKiem(long maSoTietKiem, SoTietKiem soTietKiemMoi)
        {
            try
            {
                // Tìm bản ghi cần cập nhật
                var soTietKiem = await databaseService.DataContext.DsSoTietKiem
                    .FirstOrDefaultAsync(stk => stk.MaSoTietKiem == maSoTietKiem);

                if (soTietKiem == null)
                {
                    Console.WriteLine("Không tìm thấy sổ tiết kiệm cần cập nhật.");
                    return false;
                }

                // Cập nhật thông tin
                soTietKiem.CMND = soTietKiemMoi.CMND;
                soTietKiem.MaLoaiTietKiem = soTietKiemMoi.MaLoaiTietKiem;
                soTietKiem.SoTienGui = soTietKiemMoi.SoTienGui;
                soTietKiem.NgayMoSo = soTietKiemMoi.NgayMoSo;

                // Lưu thay đổi
                await databaseService.DataContext.SaveChangesAsync();

                Console.WriteLine("Cập nhật thành công!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật sổ tiết kiệm: {ex.Message}");
                return false;
            }
        }

    }