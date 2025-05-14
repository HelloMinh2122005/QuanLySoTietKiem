using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyDaiLy.Views.PhieuGoiTienViews
{
    /// <summary>
    /// Interaction logic for DsPhieuGoiTien.xaml
    /// </summary>
    public partial class DsPhieuGoiTien : Page
    {
        private readonly PhieuGoiTienViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;
        public DsPhieuGoiTien(PhieuGoiTienViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _serviceProvider = serviceProvider;
            DataContext = _viewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
