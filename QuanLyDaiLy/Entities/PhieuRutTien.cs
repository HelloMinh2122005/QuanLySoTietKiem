using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class PhieuRutTien
{
    [Key]
    public string MaPhieuRutTien { get; set; } = "";
    public DateTime NgayRut { get; set; } = DateTime.Now;
    public decimal SoTienRut { get; set; } = 0;
    public string MaSoTietKiem { get; set; } = "";
    //Navigation Properties
    public SoTietKiem SoTietKiem { get; set; } = new();
}
