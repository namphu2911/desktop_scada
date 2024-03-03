using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineReport
{
    public class ReportNavigationViewModel : BaseViewModel
    {
        public int SeletedTabIndex { get; set; }
        public ReportLongTimeViewModel ReportLongTime { get; set; }
        public ReportForShiftViewModel ReportForShift { get; set; }
        public ReportNavigationViewModel(ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift) 
        {
            ReportLongTime = reportLongTime;
            ReportForShift = reportForShift;

            ReportLongTime.Changed += TabChanged;
        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
        }
    }
}
