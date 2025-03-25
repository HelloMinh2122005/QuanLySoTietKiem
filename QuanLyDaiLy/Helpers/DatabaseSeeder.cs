using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Helpers
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(ModelBuilder modelBuilder)
        {
            SeedThamSo(modelBuilder);
            SeedLoaiTietKiem(modelBuilder);
            SeedKhachHang(modelBuilder);
            SeedSoTietKiem(modelBuilder);
        }

        private static void SeedThamSo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThamSo>().HasData(
                new ThamSo { Id = "1", SoTienGoiToiThieu = 100000 }
            );
        }

        private static void SeedLoaiTietKiem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiTietKiem>().HasData(
                new { MaLoaiTietKiem = "LTK0830010523001", TenLoaiTietKiem = "Không kỳ hạn" },
                new { MaLoaiTietKiem = "LTK0831010523002", TenLoaiTietKiem = "3 tháng" },
                new { MaLoaiTietKiem = "LTK0832010523003", TenLoaiTietKiem = "6 tháng" },
                new { MaLoaiTietKiem = "LTK0833010523004", TenLoaiTietKiem = "12 tháng" }
            );
        }

        private static void SeedKhachHang(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KhachHang>().HasData(
                new { CMND = "123456789", TenKhachHang = "Nguyễn Văn A", DiaChi = "123 Lê Lợi, Quận 1, TP.HCM" },
                new { CMND = "987654321", TenKhachHang = "Trần Thị B", DiaChi = "456 Nguyễn Huệ, Quận 1, TP.HCM" },
                new { CMND = "246813579", TenKhachHang = "Lê Văn C", DiaChi = "789 Đồng Khởi, Quận 1, TP.HCM" },
                new { CMND = "135792468", TenKhachHang = "Phạm Thị D", DiaChi = "101 Võ Văn Tần, Quận 3, TP.HCM" },
                new { CMND = "112233445", TenKhachHang = "Hoàng Văn E", DiaChi = "202 Nguyễn Thị Minh Khai, Quận 3, TP.HCM" }
            );
        }

        private static void SeedSoTietKiem(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<SoTietKiem>().HasData(
                new
                {
                    MaSoTietKiem = "STK0830010523001",
                    CMND = "123456789",
                    MaLoaiTietKiem = "LTK0830010523001",
                    SoTienGui = 5000000m,
                    NgayMoSo = baseDate.AddDays(-30)
                },
                new
                {
                    MaSoTietKiem = "STK0831010523002",
                    CMND = "123456789",
                    MaLoaiTietKiem = "LTK0831010523002",
                    SoTienGui = 10000000m,
                    NgayMoSo = baseDate.AddDays(-60)
                },
                new
                {
                    MaSoTietKiem = "STK0832010523003",
                    CMND = "987654321",
                    MaLoaiTietKiem = "LTK0832010523003",
                    SoTienGui = 20000000m,
                    NgayMoSo = baseDate.AddDays(-90)
                },
                new
                {
                    MaSoTietKiem = "STK0833010523004",
                    CMND = "246813579",
                    MaLoaiTietKiem = "LTK0833010523004",
                    SoTienGui = 50000000m,
                    NgayMoSo = baseDate.AddDays(-120)
                },
                new
                {
                    MaSoTietKiem = "STK0834010523005",
                    CMND = "135792468",
                    MaLoaiTietKiem = "LTK0831010523002",
                    SoTienGui = 15000000m,
                    NgayMoSo = baseDate.AddDays(-45)
                },
                new
                {
                    MaSoTietKiem = "STK0835010523006",
                    CMND = "112233445",
                    MaLoaiTietKiem = "LTK0832010523003",
                    SoTienGui = 25000000m,
                    NgayMoSo = baseDate.AddDays(-75)
                },
                new
                {
                    MaSoTietKiem = "STK0836010523007",
                    CMND = "987654321",
                    MaLoaiTietKiem = "LTK0830010523001",
                    SoTienGui = 3000000m,
                    NgayMoSo = baseDate.AddDays(-15)
                },
                new
                {
                    MaSoTietKiem = "STK0837010523008",
                    CMND = "246813579",
                    MaLoaiTietKiem = "LTK0831010523002",
                    SoTienGui = 8000000m,
                    NgayMoSo = baseDate.AddDays(-50)
                }
            );
        }
    }
}