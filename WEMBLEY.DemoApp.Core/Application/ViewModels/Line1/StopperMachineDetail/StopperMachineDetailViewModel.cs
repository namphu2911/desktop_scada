using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail
{
    public class StopperMachineDetailViewModel : BaseViewModel
    {
        public StopperMachineMonitorViewModel StopperMachineMonitor {  get; set; }
        public FaultHistoryViewModel StopperMachineFault { get; set; }
        public StopperMachineDetailViewModel(StopperMachineMonitorViewModel stopperMachineMonitor, FaultHistoryViewModel stopperMachineFault) 
        {
            StopperMachineMonitor = stopperMachineMonitor;
            StopperMachineFault = stopperMachineFault;
        }
    }
}
   