using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Services;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Initiation;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class HomeNavigationViewModel : BaseViewModel
    {
        public HomeViewModel Home { get; set; }
        public LineInitialSettingViewModel LineInitialSetting { get; set; }

        private INavigationService? _navigationService;

        public INavigationService? NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; set; }
        public HomeNavigationViewModel(HomeViewModel home, LineInitialSettingViewModel lineInitialSetting, INavigationService navigationService)
        { 
            Home = home;
            LineInitialSetting = lineInitialSetting;
            NavigationService = navigationService;
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Logout()
        {
            if (MessageBox.Show("Xác nhận đăng xuất", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NavigationService.NavigateTo<LoginViewModel>();
            }
            else { }
        }
    }
}
