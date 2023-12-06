using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineReport
{
    public class ReportForShiftViewModel : BaseViewModel
    {
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        public ICommand LoadReportLongTimeCommand { get; set; }
        public ICommand LoadReportAllCommand { get; set; }
        public ICommand LoadReportOEECommand { get; set; }
        public ICommand LoadReportACommand { get; set; }
        public ICommand LoadReportPCommand { get; set; }
        public ICommand LoadReportQCommand { get; set; }
        public double OEE { get; set; }
        public double A { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public int ShiftNumber { get; set; }
        public int Output { get; set; }
        public int StandarOutput { get; set; }
        public ReportForShiftViewModel() 
        {
        
        }
    }
}
