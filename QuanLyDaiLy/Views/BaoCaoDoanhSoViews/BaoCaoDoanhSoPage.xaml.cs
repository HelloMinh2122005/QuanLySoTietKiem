﻿using System;
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
using QuanLyDaiLy.ViewModels;

namespace QuanLyDaiLy.Views.BaoCaoDoanhSoViews
{
    /// <summary>
    /// Interaction logic for BaoCaoDoanhSoPage.xaml
    /// </summary>
    public partial class BaoCaoDoanhSoPage : Page
    {
        public BaoCaoDoanhSoPage(BaoCaoDoanhSoViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
