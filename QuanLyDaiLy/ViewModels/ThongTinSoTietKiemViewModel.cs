using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels
{
    public class ThongTinSoTietKiemViewModel : INotifyPropertyChanged
    {
        private string _message = "Hello from ThongTinSoTietKiemViewModel!";

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}