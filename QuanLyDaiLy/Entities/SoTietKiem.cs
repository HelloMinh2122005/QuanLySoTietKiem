using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.Entities;

public class SoTietKiem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long MaSoTietKiem { get; set; }
    private string _cmnd = string.Empty;

    public string CMND
    {
        get => _cmnd; 
        set
        {
            if (_cmnd != value)
            {
                _cmnd = value; 
                OnPropertyChanged(); 
            }
        }
    }
    public string MaLoaiTietKiem { get; set; } = "";
    public decimal SoTienGui { get; set; } = 0;
    public DateTime NgayMoSo { get; set; } = DateTime.Now;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
