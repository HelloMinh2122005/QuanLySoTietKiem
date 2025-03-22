﻿using QuanLyDaiLy.Views;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels
{
    public class CapNhatSoTietKiemViewModel
    {
        public ICommand CloseCommand { get; }

        private DateTime _ngayMoSo;
        public DateTime NgayMoSo
        {
            get => _ngayMoSo;
            set
            {
                if (_ngayMoSo != value)
                {
                    _ngayMoSo = value;
                    OnPropertyChanged();
                }
            }
        }

        public CapNhatSoTietKiemViewModel()
        {
            CloseCommand = new RelayCommand(ExecuteClose);
            NgayMoSo = DateTime.Today;
        }

        private void ExecuteClose()
        {
            Application.Current.Windows.OfType<CapNhatSoTietKiem>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
