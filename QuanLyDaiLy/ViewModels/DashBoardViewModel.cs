using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views;

namespace QuanLyDaiLy.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private string _message = "Hello from DashboardViewModel!";

        public DashboardViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            OpenThemSoTietKiemCommand = new RelayCommand(OpenThemSoTietKiem);
        }

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenThemSoTietKiemCommand { get; }

        private void OpenThemSoTietKiem()
        {
            var themSoTietKiem = _serviceProvider.GetRequiredService<ThemSoTietKiem>();
            themSoTietKiem.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
