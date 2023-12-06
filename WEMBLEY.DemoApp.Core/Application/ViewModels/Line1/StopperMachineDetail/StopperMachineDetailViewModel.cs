using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail
{
    public class StopperMachineDetailViewModel : BaseViewModel
    {
        public StopperMachineMonitorViewModel StopperMachineMonitor {  get; set; }
        public StopperMachineFaultHistoryViewModel StopperMachineFault { get; set; }
        public StopperMachineDetailViewModel(StopperMachineMonitorViewModel stopperMachineMonitor, StopperMachineFaultHistoryViewModel stopperMachineFault) 
        {
            StopperMachineMonitor = stopperMachineMonitor;
            StopperMachineFault = stopperMachineFault;
        }
    }
}
   