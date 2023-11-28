using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Services;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.HostBuiders
{
    public static class AddNavigationHostBuilderExtension
    {
        public static IHostBuilder AddNavigation(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddScoped<NavigationStore>();
                services.AddScoped<INavigationService<HomeNavigationViewModel>, NavigationService<HomeNavigationViewModel>>();
                services.AddScoped<INavigationService<StopperMachineViewModel>, NavigationService<StopperMachineViewModel>>();
            });

            return host;
        }
    }
}
