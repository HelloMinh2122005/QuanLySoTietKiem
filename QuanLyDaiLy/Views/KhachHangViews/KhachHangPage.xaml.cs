using System.Windows.Controls;

namespace QuanLyDaiLy.Views.KhachHangViews
{
    public partial class KhachHangPage : Page
    {
        public KhachHangPage()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
