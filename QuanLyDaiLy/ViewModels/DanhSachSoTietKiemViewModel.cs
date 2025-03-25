using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Entities;
using QuanLyDaiLy.Interfaces;

namespace QuanLyDaiLy.ViewModels
{
    public class DanhSachSoTietKiemViewModel : INotifyPropertyChanged
    {
        public ICommand ThemSoTietKiemCommand { get; }
        public ICommand CapNhatSoTietKiemCommand { get; }
        public ICommand XoaSoTietKiemCommand { get; }
        


        
        private readonly ISoTietKiemRepo _soTietKiemRepo;
        
        private readonly ILoaiTietKiemRepo _loaiTietKiemRepo;


        private readonly IServiceProvider _serviceProvider;
        
        private ObservableCollection<SoTietKiem> _danhSachSoTietKiem;
        public ObservableCollection<SoTietKiem> DanhSachSoTietKiem
        {
            get => _danhSachSoTietKiem;
            set
            {
                _danhSachSoTietKiem = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<LoaiTietKiem> _danhSachLoaiTietKiem;
        public ObservableCollection<LoaiTietKiem> DanhSachLoaiTietKiem
        {
            get => _danhSachLoaiTietKiem;
            set
            {
                _danhSachLoaiTietKiem = value;
                OnPropertyChanged();
            }
        }
        
        private LoaiTietKiem? _loaiTietKiemDuocChon;
        public LoaiTietKiem? LoaiTietKiemDuocChon
        {
            get => _loaiTietKiemDuocChon;
            set
            {
                _loaiTietKiemDuocChon = value;
                OnPropertyChanged();
                LoadDanhSachSoTietKiemAsync();
            }
        }
        
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // PlaceholderText = string.IsNullOrEmpty(_searchText) ? "Tìm kiếm sổ tiết kiệm" : "";
                LoadDanhSachSoTietKiemAsync();
                
            }
        }
        
        // private string _placeholderText = "Tìm kiếm sổ tiết kiệm";
        // public string PlaceholderText
        // {
        //     get => _placeholderText;
        //     set
        //     {
        //         _placeholderText = value;
        //         OnPropertyChanged();
        //     }
        // }
        
        private List<long> _dsCanXoa = new();
        public List<long> DsCanXoa
        {
            get => _dsCanXoa;
            set
            {
                _dsCanXoa = value;
                OnPropertyChanged();
            }
        }
        
        public void XuLyCheckBox(bool isChecked, long id)
        {
            if (isChecked)
            {
                if (!DsCanXoa.Contains(id))
                    DsCanXoa.Add(id);
            }
            else
            {
                DsCanXoa.Remove(id);
            }
        }
        
        private SoTietKiem? _soTietKiemDuocChon;
        public SoTietKiem? SoTietKiemDuocChon
        {
            get => _soTietKiemDuocChon;
            set
            {
                _soTietKiemDuocChon = value;
                OnPropertyChanged();
            }
        }
        
       

        public DanhSachSoTietKiemViewModel(IServiceProvider serviceProvider,  ISoTietKiemRepo soTietKiemRepo,ILoaiTietKiemRepo LoaiTietKiemRepo)
        {
            _serviceProvider = serviceProvider;

            XoaSoTietKiemCommand = new RelayCommand(XoaSoTietKiem);
            ThemSoTietKiemCommand = new RelayCommand(OpenThemSoTietKiem);
            CapNhatSoTietKiemCommand = new RelayCommand(CapNhatSoTietKiem);
            
            _soTietKiemRepo = soTietKiemRepo;
            _loaiTietKiemRepo = LoaiTietKiemRepo;

            LoadLoaiTietKiem();

            LoadDanhSachSoTietKiemAsync();
            
        }
        
        
        
        private async void XoaSoTietKiem()
        {
            if (DsCanXoa == null || !DsCanXoa.Any())
            {
                MessageBox.Show("Không có phần tử nào để xóa!");
                return;
            }

            // Xóa các item có ID trong danh sách
            DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>(
                DanhSachSoTietKiem.Where(stk => !DsCanXoa.Contains(stk.MaSoTietKiem))
            );
            
            // Xóa trong database
            foreach (var item in _dsCanXoa)
            {
                await _soTietKiemRepo.XoaSoTietKiem(item);
            }
            
            // reset
            DsCanXoa.Clear();
        }
        
        public async Task LoadDanhSachSoTietKiemAsync()
        {
            try
            {
                var danhSach = await _soTietKiemRepo.FindAllByLoaiTietKiemAsync(_loaiTietKiemDuocChon?.MaLoaiTietKiem, _searchText);
                DanhSachSoTietKiem = new ObservableCollection<SoTietKiem>();
                DanhSachSoTietKiem.Clear();
                foreach (var soTietKiem in danhSach)
                {
                    DanhSachSoTietKiem.Add(soTietKiem);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi khi tai danh sach so tiet kiem: {ex.Message}");
            }
        }
        
        private async void LoadLoaiTietKiem()
        {
            var danhSach = await _loaiTietKiemRepo.FindAll();
            DanhSachLoaiTietKiem = new ObservableCollection<LoaiTietKiem>();
            DanhSachLoaiTietKiem.Add(new LoaiTietKiem
            {
                TenLoaiTietKiem = null
            });

            foreach (var loaiTietKiem in danhSach)
            {
                DanhSachLoaiTietKiem.Add(loaiTietKiem);   
            }
        }


        public void OpenThemSoTietKiem()
        {
            var themSoTietKiemViewModel = _serviceProvider.GetRequiredService<ThemSoTietKiemViewModel>();
    
            // Đăng ký sự kiện
            themSoTietKiemViewModel.LapSoEvent += (sender, soTietKiem) =>
            {
                DanhSachSoTietKiem.Add(soTietKiem);
            };

            var themSoTietKiemView = new ThemSoTietKiem(themSoTietKiemViewModel);
            themSoTietKiemView.Show();
        }


        public void CapNhatSoTietKiem()
        {
            if (SoTietKiemDuocChon == null)
            {
                MessageBox.Show("Vui lòng chọn một sổ tiết kiệm để cập nhật!");
                return;
            }
     
            
            var capNhatSoTietKiemViewModel = _serviceProvider.GetRequiredService<CapNhatSoTietKiemViewModel>();
            capNhatSoTietKiemViewModel.SetData(SoTietKiemDuocChon);
            
                   
            capNhatSoTietKiemViewModel.CapNhatEvent += (sender, soTietKiem) =>
            {
                LoadDanhSachSoTietKiemAsync();
            };
            
            var capnhatSoTietKiem = new CapNhatSoTietKiem(capNhatSoTietKiemViewModel);
            
            capnhatSoTietKiem.Show();
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}