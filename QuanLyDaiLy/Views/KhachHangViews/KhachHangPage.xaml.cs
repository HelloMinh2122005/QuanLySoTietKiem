using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.ViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.KhachHangViews
{
    public partial class KhachHangPage : Page
    {
        private readonly KhachHangViewModel _viewModel;
        public KhachHangPage(KhachHangViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)     
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
