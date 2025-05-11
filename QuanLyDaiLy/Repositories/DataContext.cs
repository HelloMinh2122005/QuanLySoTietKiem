using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Helpers;

namespace QuanLyDaiLy.Repositories;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<SoTietKiem> DsSoTietKiem { get; set; } = null!;
    public DbSet<LoaiTietKiem> DsLoaiTietKiem { get; set; } = null!;
    public DbSet<KhachHang> DsKhachHang { get; set; } = null!;
    public DbSet<ThamSo> ThamSo { get; set; } = null!;
    public DbSet<PhieuGoiTien> DsPhieuGoiTien { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure KhachHang relationship
        modelBuilder.Entity<KhachHang>()
            .HasMany(kh => kh.DsSoTietKiem)
            .WithOne(stk => stk.KhachHang)
            .HasForeignKey(stk => stk.CMND)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoTietKiem>()
            .HasOne(stk => stk.LoaiTietKiem)
            .WithMany(ltk => ltk.DsSoTietKiem)
            .HasForeignKey(stk => stk.MaLoaiTietKiem)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PhieuGoiTien>()
            .HasOne(pgt => pgt.SoTietKiem)
            .WithMany(stk => stk.DsPhieuGoiTien)
            .HasForeignKey(pgt => pgt.MaSoTietKiem)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed the database
        DatabaseSeeder.SeedDatabase(modelBuilder);
    }
}