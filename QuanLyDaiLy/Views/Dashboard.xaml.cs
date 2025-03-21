using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard(DashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
