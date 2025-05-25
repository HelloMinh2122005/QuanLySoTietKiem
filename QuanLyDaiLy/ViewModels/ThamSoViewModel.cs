using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels;

public class ThamSoViewModel : INotifyPropertyChanged
{
    private readonly IThamSoRepo _thamSoRepo;
    private readonly ThamSo _entity;

    public ThamSoViewModel(IThamSoRepo thamSoRepo)
    {
        _thamSoRepo = thamSoRepo;
        _entity = _thamSoRepo.Get().Result ?? throw new Exception("Không tìm thấy ThamSo");

        Id = _entity.Id;
        SoTienGoiToiThieu = _entity.SoTienGoiToiThieu;
        ApDungSoTienGuiToiThieu = _entity.ApDungSoTienGuiToiThieu;
        SoTienGuiThemToiThieu = _entity.SoTienGuiThemToiThieu;
        SoNgayMoSoToiThieu = _entity.SoNgayMoToiThieu;
        SoNgayMoSoToiThieuDeCoLai = _entity.SoNgayMoToiThieuDeCoLai;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task CapNhatDatabase()
    {
        try
        { await _thamSoRepo.Update(_entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi cập nhật CSDL: " + ex.Message);
        }
    }

    // Các thuộc tính
    public string Id { get; set; }

    private decimal _soTienGoiToiThieu;
    public decimal SoTienGoiToiThieu
    {
        get => _soTienGoiToiThieu;
        set
        {
            if (_soTienGoiToiThieu != value)
            {
                _soTienGoiToiThieu = value;
                _entity.SoTienGoiToiThieu = value;
                OnPropertyChanged();
                _ = CapNhatDatabase();
            }
        }
    }

    private bool _apDungSoTienGuiToiThieu;
    public bool ApDungSoTienGuiToiThieu
    {
        get => _apDungSoTienGuiToiThieu;
        set
        {
            if (_apDungSoTienGuiToiThieu != value)
            {
                _apDungSoTienGuiToiThieu = value;
                _entity.ApDungSoTienGuiToiThieu = value;
                OnPropertyChanged();
                _ = CapNhatDatabase();
            }
        }
    }

    private decimal _soTienGuiThemToiThieu;
    public decimal SoTienGuiThemToiThieu
    {
        get => _soTienGuiThemToiThieu;
        set
        {
            if (_soTienGuiThemToiThieu != value)
            {
                _soTienGuiThemToiThieu = value;
                _entity.SoTienGuiThemToiThieu = value;
                OnPropertyChanged();
                _ = CapNhatDatabase();
            }
        }
    }

    private int _soNgayMoSoToiThieu;
    public int SoNgayMoSoToiThieu
    {
        get => _soNgayMoSoToiThieu;
        set
        {
            if (_soNgayMoSoToiThieu != value)
            {
                _soNgayMoSoToiThieu = value;
                _entity.SoNgayMoToiThieu = value;
                OnPropertyChanged();
                _ = CapNhatDatabase();
            }
        }
    }

    private int _soNgayMoSoToiThieuDeCoLai;
    public int SoNgayMoSoToiThieuDeCoLai
    {
        get => _soNgayMoSoToiThieuDeCoLai;
        set
        {
            if (_soNgayMoSoToiThieuDeCoLai != value)
            {
                _soNgayMoSoToiThieuDeCoLai = value;
                _entity.SoNgayMoToiThieuDeCoLai = value;
                OnPropertyChanged();
                _ = CapNhatDatabase();
            }
        }
    }
}
