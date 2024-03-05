using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;

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

                //LINES
                services.AddTransient<MachinesInLine1ViewModel>();
                services.AddTransient<MachinesInLine2ViewModel>();

                //Line 1 MAIN
                services.AddTransient<StopperMachineViewModel>();
                
                services.AddTransient<StopperMachineMonitorViewModel>();
                services.AddTransient<FaultHistoryViewModel>();

                services.AddTransient<MFCMonitorViewModel>();
                services.AddTransient<MFCSettingViewModel>();

                services.AddTransient<ReportLongTimeViewModel>();
                services.AddTransient<ReportForShiftViewModel>();

                services.AddTransient<MachineStatusViewModel>();
                ///

                //Line 2 MAIN
                services.AddTransient<DosingDryingMachineViewModel>();
                services.AddTransient<DosingDryingMonitorViewModel>();
                services.AddTransient<DosingDryingParameterMonitorViewModel>();
                //

                services.AddTransient<MainViewModel>();
                services.AddSingleton<MainWindow>((IServiceProvider serviceProvider) => new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainViewModel>()
                });
            });

            return host;
        }
    }
}
