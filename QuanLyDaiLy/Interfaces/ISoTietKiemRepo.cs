using QuanLyDaiLy.Entities;

namespace QuanLyDaiLy.Interfaces;

public interface ISoTietKiemRepo
{
    Task<IEnumerable<SoTietKiem>> GetAll();
    Task<SoTietKiem> GetById(string id);
    Task Create(SoTietKiem soTietKiem);
    Task Update(SoTietKiem soTietKiem);
    Task Delete(string id);
    Task<IEnumerable<SoTietKiem>> Search(string? maLoaiTietKiem, string? searchText);

    Task<IEnumerable<SoTietKiem>> SearchSoTietKiem(
        string? maSoTietKiem,
        LoaiTietKiem? tenLoaiTietKiem,
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
        int? soLuongPhieuRutTienDen,
        bool? dangMo);
}