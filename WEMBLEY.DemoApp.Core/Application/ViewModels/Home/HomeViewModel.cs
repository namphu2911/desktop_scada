using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public MachinesInLine1ViewModel MachinesInLine1 { get; set; }
        public HomeViewModel(MachinesInLine1ViewModel machinesInLine1)
        {
            MachinesInLine1 = machinesInLine1;
        }
    }
}
