using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Repositories;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<SoTietKiem> DsSoTietKiem { get; set; } = null!;
    public DbSet<LoaiTietKiem> DsLoaiTietKiem { get; set; } = null!;
    public DbSet<KhachHang> DsKhachHang { get; set; } = null!;
    public DbSet<ThamSo> ThamSo { get; set; } = null!;

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
}