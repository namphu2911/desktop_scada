using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.HostBuiders
{
    public static class AddViewModelsHostBuilderExtension
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<HomeNavigationViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<LineInitialSettingViewModel>();

                services.AddTransient<MachinesInLine1ViewModel>();
                services.AddTransient<StopperMachineViewModel>();
                //services.AddTransient<StopperMachineViewModel>((IServiceProvider serviceProvider) =>
                //{
                //    using var scope = serviceProvider.CreateScope();

                //    var navigationStore = scope.ServiceProvider.GetService<NavigationStore>();
                //    var homeNavigationService = scope.ServiceProvider.GetService<INavigationService<HomeNavigationViewModel>>();

                //    return new StopperMachineViewModel(
                //        navigationStore,
                //        homeNavigationService);
                //});

                services.AddTransient<MainViewModel>();

                services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            });

            return host;
        }
    }
}
