using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineMFC
{
    public class MFCNavigationViewModel : BaseViewModel
    {
        public MFCMonitorViewModel MFCMonitor { get; set; }
        public MFCSettingViewModel MFCSetting { get; set; } 
        public MFCNavigationViewModel(MFCMonitorViewModel mFCMonitor, MFCSettingViewModel mFCSetting)
        {
            MFCMonitor = mFCMonitor;
            MFCSetting = mFCSetting;
        }
    }
}
