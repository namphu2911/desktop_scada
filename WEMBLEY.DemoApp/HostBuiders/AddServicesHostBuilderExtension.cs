using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Services;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.HostBuiders
{
    public static class AddServicesHostBuilderExtension
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                //services.AddSingleton<NavigationStore>();

                //services.AddSingleton<INavigationService<HomeNavigationViewModel>>(s =>
                //{
                //    return new NavigationService<HomeNavigationViewModel>(
                //    s.GetRequiredService<NavigationStore>(),
                //    s.GetRequiredService<HomeNavigationViewModel>());
                //});

                //services.AddTransient<HomeNavigationViewModel>();
                //services.AddTransient<HomeViewModel>();
                //services.AddTransient<LineInitialSettingViewModel>();

                //services.AddTransient<MachinesInLine1ViewModel>(s => new MachinesInLine1ViewModel(
                //    s.GetRequiredService<NavigationStore>(),
                //    new NavigationService<StopperMachineViewModel>(
                //        s.GetRequiredService<NavigationStore>(),
                //        s.GetRequiredService<StopperMachineViewModel>())));

                //services.AddTransient<StopperMachineViewModel>(s => new StopperMachineViewModel(
                //    s.GetRequiredService<NavigationStore>(),
                //    new NavigationService<HomeNavigationViewModel>(
                //        s.GetRequiredService<NavigationStore>(),
                //        s.GetRequiredService<HomeNavigationViewModel>())));

                //services.AddSingleton<MainViewModel>();

                //services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            });
            return host;
        }
    }
}
