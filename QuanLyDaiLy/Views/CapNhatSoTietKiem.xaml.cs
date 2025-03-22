using System.Windows;
using QuanLyDaiLy.ViewModels;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for CapNhatSoTietKiem.xaml
    /// </summary>
    public partial class CapNhatSoTietKiem : Window
    {
        public CapNhatSoTietKiem(CapNhatSoTietKiemViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
