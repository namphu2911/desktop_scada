using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Initiation;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.NonStopperCappingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.StopperCappingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.ValueConverters;

namespace WEMBLEY.DemoApp.HostBuiders
{
    public static class AddViewModelsHostBuilderExtension
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<HomeViewModel>();
                services.AddTransient<LineInitialSettingViewModel>();
                services.AddTransient<HomeNavigationViewModel>();

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

                services.AddTransient<StopperCappingMachineViewModel>();
                services.AddTransient<StopperCappingMonitorViewModel>();
                services.AddTransient<StopperCappingParameterViewModel>();

                services.AddTransient<NonStopperCappingMachineViewModel>();
                services.AddTransient<NonStopperCappingMonitorViewModel>();
                services.AddTransient<NonStopperCappingParameterViewModel>();
                //

                services.AddTransient<MainViewModel>();
                services.AddTransient<LoginViewModel>();
                services.AddSingleton<MainWindow>((IServiceProvider serviceProvider) => new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainViewModel>()
                });
            });

            return host;
        }
    }
}
