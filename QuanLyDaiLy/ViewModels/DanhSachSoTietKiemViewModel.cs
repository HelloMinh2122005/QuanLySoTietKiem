using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;
using System.Windows;

namespace QuanLyDaiLy.ViewModels
{
    public class DanhSachSoTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand ThemSoTietKiemCommand { get; }
        public ICommand CapNhatSoTietKiemCommand { get; }
        public ICommand XoaSoTietKiemCommand { get; }
        public ICommand ResetSoTietKiemCommand { get; }

        public ICommand MoTraCuuSoTietKiemCommand { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly ISoTietKiemRepo _soTietKiemRepo;
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;

        public DanhSachSoTietKiemViewModel(
            IServiceProvider serviceProvider,
            ISoTietKiemRepo soTietKiemRepo,
            ILoaiTietKiemRepo loaiTietKiemRepo
        )
        {
            _serviceProvider = serviceProvider;
            _soTietKiemRepo = soTietKiemRepo;
            _loaiTietKiemRepo = loaiTietKiemRepo;
            _ = LoadData();

            ThemSoTietKiemCommand = new RelayCommand(OpenThemSoTietKiem);
            CapNhatSoTietKiemCommand = new RelayCommand(CapNhatSoTietKiem);
            XoaSoTietKiemCommand = new RelayCommand(XoaSoTietKiem);
            ResetSoTietKiemCommand = new RelayCommand(async () => await LoadData());
            MoTraCuuSoTietKiemCommand = new RelayCommand(OpenSearchWindow);
        }

        private SoTietKiem _selectedSoTietKiem = new();

        public SoTietKiem SelectedSoTietKiem
        {
            get => _selectedSoTietKiem;
            set
            {
                _selectedSoTietKiem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LoaiTietKiem> _danhSachLoaiTietKiem = [];

        public ObservableCollection<LoaiTietKiem> DanhSachLoaiTietKiem
        {
            get => _danhSachLoaiTietKiem;
            set
            {
                _danhSachLoaiTietKiem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SoTietKiem> _danhSachSoTietKiem = [];

        public ObservableCollection<SoTietKiem> DanhSachSoTietKiem
        {
            get => _danhSachSoTietKiem;
            set
            {
                _danhSachSoTietKiem = value;
                OnPropertyChanged();
            }
        }

        private LoaiTietKiem _selectedLoaiTietKiem = new();

        public LoaiTietKiem SelectedLoaiTietKiem
        {
            get => _selectedLoaiTietKiem;
            set
            {
                if (SelectedLoaiTietKiem != value)
                {
                    _selectedLoaiTietKiem = value;
                    Filter();
                    OnPropertyChanged();
                }
            }
        }

        private string _searchText = "";

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }

        private async Task LoadData()
        {
            try
            {
                await LoadDanhSachSoTietKiem();
                await LoadDanhSachLoaiTietKiem();
                SelectedLoaiTietKiem = DanhSachLoaiTietKiem.Last();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task LoadDanhSachSoTietKiem()
        {
            try
            {
                var danhSachSoTietKiem = await _soTietKiemRepo.GetAll();
                DanhSachSoTietKiem.Clear();
                DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(danhSachSoTietKiem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sổ tiết kiệm: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task LoadDanhSachLoaiTietKiem()
        {
            try
            {
                var danhSachLoaiTietKiem = await _loaiTietKiemRepo.GetAll();
                DanhSachLoaiTietKiem.Clear();

                var tmpDanhSach = new List<LoaiTietKiem>(danhSachLoaiTietKiem)
                {
                    new LoaiTietKiem { MaLoaiTietKiem = "", TenLoaiTietKiem = "Tất cả" }
                };

                DanhSachLoaiTietKiem = new ObservableCollection<LoaiTietKiem>(tmpDanhSach);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại tiết kiệm: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private async void Filter()
        {
            string? maLoaiTietKiem = null;
            if (SelectedLoaiTietKiem?.MaLoaiTietKiem != "")
            {
                maLoaiTietKiem = SelectedLoaiTietKiem?.MaLoaiTietKiem;
            }
            try
            {
                var danhSachSoTietKiem = await _soTietKiemRepo.Search(
                    maLoaiTietKiem, SearchText);
                DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(danhSachSoTietKiem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void OpenThemSoTietKiem()
        {
            var themSoTietKiem = _serviceProvider.GetRequiredService<ThemSoTietKiem>();
            themSoTietKiem.Closed += (s, e) => Filter();
            themSoTietKiem.Show();
        }

        public void CapNhatSoTietKiem()
        {
            if (string.IsNullOrEmpty(SelectedSoTietKiem.CMND))
            {
                MessageBox.Show("Vui lòng chọn một sổ tiết kiệm để cập nhật!");
                return;
            }


            var capNhatSoTietKiemViewModel = _serviceProvider.GetRequiredService<CapNhatSoTietKiemViewModel>();
            capNhatSoTietKiemViewModel.SetData(SelectedSoTietKiem);


            capNhatSoTietKiemViewModel.CapNhatEvent += (sender, soTietKiem) => { _ = LoadData(); };

            var capnhatSoTietKiem = new CapNhatSoTietKiem(capNhatSoTietKiemViewModel);

            capnhatSoTietKiem.Show();
        }

        public async void XoaSoTietKiem()
        {
            if (string.IsNullOrEmpty(SelectedSoTietKiem.CMND))
            {
                MessageBox.Show("Vui lòng chọn sổ tiết kiệm để xóa", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sổ tiết kiệm {SelectedSoTietKiem.MaSoTietKiem}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _soTietKiemRepo.Delete(SelectedSoTietKiem.MaSoTietKiem);
                    await LoadData();
                    MessageBox.Show("Xóa sổ tiết kiệm thành công!", "Thông báo", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa sổ tiết kiệm: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void OpenSearchWindow()
        {
            var traCuuViewModel = _serviceProvider.GetRequiredService<TraCuuSoTietKiemViewModel>();

            // Gán callback để nhận kết quả tìm kiếm
            traCuuViewModel.SearchCompleted = (list) =>
            {
                DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(list);
            };

            // Tạo view và gán DataContext nếu cần
            var traCuuView = new TraCuuSoTietKiem(traCuuViewModel);

            traCuuView.Show();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}