using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Repositories;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<SoTietKiem> DsSoTietKiem { get; set; } = null!;
    public DbSet<LoaiTietKiem> DsLoaiTietKiem { get; set; } = null!;
    public DbSet<KhachHang> DsKhachHang { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<KhachHang>()
            .HasMany(kh => kh.DsSoTietKiem)
            .WithOne()
            .HasForeignKey(stk => stk.CMND)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoTietKiem>()
            .HasOne<LoaiTietKiem>()
            .WithMany()
            .HasForeignKey(stk => stk.MaLoaiTietKiem)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SoTietKiem>()
            .HasOne<LoaiTietKiem>()
            .WithMany(ltk => ltk.DsSoTietKiem)
            .HasForeignKey(stk => stk.MaLoaiTietKiem)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public void SeedData()
    {
        if (!DsLoaiTietKiem.Any())
        {
            DsLoaiTietKiem.AddRange(
                new LoaiTietKiem { MaLoaiTietKiem = "1", TenLoaiTietKiem = LoaiTietKiemEnum.KhongKyHan},
                new LoaiTietKiem { MaLoaiTietKiem = "2", TenLoaiTietKiem = LoaiTietKiemEnum.BaThang },
                new LoaiTietKiem {MaLoaiTietKiem = "3", TenLoaiTietKiem = LoaiTietKiemEnum.SauThang}
            );
            SaveChanges();
        }
        else
        {
            Console.WriteLine("Da co data ds loai tiet kiem");
        }

        if (!DsKhachHang.Any())
        {
            DsKhachHang.AddRange(new KhachHang { CMND = "1", TenKhachHang = "Nguyễn Văn Tèo", DiaChi = "Hà Nội" },
                new KhachHang { CMND = "2", TenKhachHang = "Trần Thị Nở", DiaChi = "Hồ Chí Minh" },
                new KhachHang { CMND = "3", TenKhachHang = "Lê Văn Cưng", DiaChi = "Đà nẵng" });
            SaveChanges();
        }
        else
        {
            Console.WriteLine("Da co data ds khach hang");
        }


    }
}