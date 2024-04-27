using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Mapping;
using WEMBLEY.DemoApp.Core.Application.Services;
using WEMBLEY.DemoApp.Core.Application.Store;
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
                services.AddAutoMapper(typeof(DtoToModelProfile));
                services.AddSingleton<INavigationService, NavigationService>(); 
                services.AddSingleton<ISignalRClient, SignalRClient>();

                services.AddSingleton<ProductStore>();
                services.AddSingleton<ReferenceStore>();
                services.AddSingleton<StationStore>();
                services.AddSingleton<EmployeeStore>();
                services.AddSingleton<HomeDataStore>();
                services.AddSingleton<IdTransferStore>();
                services.AddSingleton<DeviceSelectedStore>();
                services.AddSingleton<RoleEnableStore>();

                services.AddSingleton<IApiService, ApiService>();
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IDatabaseSynchronizationService, DatabaseSynchronizationService>();
            });
            return host;
        }
    }
}
