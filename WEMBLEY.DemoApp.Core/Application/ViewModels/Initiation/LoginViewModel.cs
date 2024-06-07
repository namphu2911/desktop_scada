using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Initiation
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDatabaseSynchronizationService _databaseSynchronizationService;
        private readonly RoleEnableStore _roleEnableStore;
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
        public IBrowser? Browser { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IApiService apiService, IAuthenticationService authenticationService, IDatabaseSynchronizationService databaseSynchronizationService, RoleEnableStore roleEnableStore, INavigationService navigationService)
        {
            _apiService = apiService;
            _authenticationService = authenticationService;
            _databaseSynchronizationService = databaseSynchronizationService;
            _roleEnableStore = roleEnableStore;
            NavigationService = navigationService;

            LoginCommand = new RelayCommand(Login);
            _authenticationService.UserLoggedIn += OnLogin;
        }

        private void OnLogin(object? sender, string e)
        {
            _navigationService.NavigateTo<HomeNavigationViewModel>();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(e) as JwtSecurityToken;
            var role = tokenS?.Claims.First(claim => claim.Type == "role").Value;
            if (role != null)
            {
                _roleEnableStore.SetRole(role);
            }
        }

        private async void Login()
        {
            if (Browser is null)
            {
                throw new InvalidOperationException();
            }

            string token = await _authenticationService.LoginAsync(Browser);

            //_apiService.SetToken(token);

            //await Task.WhenAll(
            //    _databaseSynchronizationService.SynchronizeItemsData(),
            //    _databaseSynchronizationService.SynchronizeEmployeesData()
            //    );
        }
    }
}
