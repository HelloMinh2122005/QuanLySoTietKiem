using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels;

public class ThamSoViewModel
: INotifyPropertyChanged
{
    private string _id;
        private decimal _soTienGoiToiThieu = 100000;
        private bool _apDungSoTienGuiToiThieu = true;
        private decimal _soTienGuiThemToiThieu = 100000;
        private int soNgayMoSoToiThieu { get; set; } = 1;
    public int SoNgayMoSoToiThieu
    {
        get => soNgayMoSoToiThieu;
        set
        {
            if (soNgayMoSoToiThieu != value)
            {
                soNgayMoSoToiThieu = value;
                OnPropertyChanged();
            }
        }
    }
    
    private readonly IThamSoRepo _thamSoRepo;
    public ThamSoViewModel(IThamSoRepo thamSoRepo)
    {
        _thamSoRepo = thamSoRepo;

        var thamso = thamSoRepo.Get().Result;
        Id = thamso.Id;
        SoTienGoiToiThieu = thamso.SoTienGoiToiThieu;
        ApDungSoTienGuiToiThieu = thamso.ApDungSoTienGuiToiThieu;
        SoTienGuiThemToiThieu = thamso.SoTienGuiThemToiThieu;
        SoNgayMoSoToiThieu = thamso.SoNgayMoSoToiThieu;
        SoNgayMoSoToiThieuDeCoLai = thamso.SoNgayMoSoToiThieuDeCoLai;
    }



    private int soNgayMoSoToiThieuDeCoLai { get; set; } = 15;
    
    public int SoNgayMoSoToiThieuDeCoLai
    {
        get => soNgayMoSoToiThieuDeCoLai;
        set
        {
            if (soNgayMoSoToiThieuDeCoLai != value)
            {
                soNgayMoSoToiThieuDeCoLai = value;
                OnPropertyChanged();
            }
        }
    }

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SoTienGoiToiThieu
        {
            get => _soTienGoiToiThieu;
            set
            {
                if (_soTienGoiToiThieu != value)
                {
                    _soTienGoiToiThieu = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ApDungSoTienGuiToiThieu
        {
            get => _apDungSoTienGuiToiThieu;
            set
            {
                if (_apDungSoTienGuiToiThieu != value)
                {
                    _apDungSoTienGuiToiThieu = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SoTienGuiThemToiThieu
        {
            get => _soTienGuiThemToiThieu;
            set
            {
                if (_soTienGuiThemToiThieu != value)
                {
                    _soTienGuiThemToiThieu = value;
                    OnPropertyChanged();
                }
            }
        }

        // Constructor từ entity
        public ThamSoViewModel(ThamSo entity)
        {
            Id = entity.Id;
            SoTienGoiToiThieu = entity.SoTienGoiToiThieu;
            ApDungSoTienGuiToiThieu = entity.ApDungSoTienGuiToiThieu;
            SoTienGuiThemToiThieu = entity.SoTienGuiThemToiThieu;
        }

        // Convert lại về entity (khi lưu)
        public ThamSo ToEntity()
        {
            return new ThamSo
            {
                Id = this.Id,
                SoTienGoiToiThieu = this.SoTienGoiToiThieu,
                ApDungSoTienGuiToiThieu = this.ApDungSoTienGuiToiThieu,
                SoTienGuiThemToiThieu = this.SoTienGuiThemToiThieu
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }