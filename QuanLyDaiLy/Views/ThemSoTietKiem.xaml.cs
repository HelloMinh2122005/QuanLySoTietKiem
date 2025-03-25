using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for ThemSoTietKiem.xaml
    /// </summary>
    public partial class ThemSoTietKiem : Window
    {
        public ThemSoTietKiem(ThemSoTietKiemViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
