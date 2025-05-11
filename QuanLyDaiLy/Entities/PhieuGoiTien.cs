using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Entities;

public class PhieuGoiTien
{
    [Key]
    public string MaPhieuGoiTien { get; set; } = "";
    public DateTime NgayGoi { get; set; } = DateTime.Now;
    public decimal SoTienGui { get; set; } = 0;
    public string MaSoTietKiem { get; set; } = "";
    //navigation properties
    public SoTietKiem SoTietKiem { get; set; } = new();
}

