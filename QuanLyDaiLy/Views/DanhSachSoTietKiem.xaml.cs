using QuanLyDaiLy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for DanhSachSoTietKiem.xaml
    /// </summary>
    public partial class DanhSachSoTietKiem : Window
    {
        public DanhSachSoTietKiem(DanhSachSoTietKiemViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
