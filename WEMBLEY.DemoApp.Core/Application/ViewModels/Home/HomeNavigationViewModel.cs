using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class HomeNavigationViewModel : BaseViewModel
    {
        public HomeViewModel Home { get; set; }
        public LineInitialSettingViewModel LineInitialSetting { get; set; }
        public HomeNavigationViewModel(HomeViewModel home, LineInitialSettingViewModel lineInitialSetting)
        { 
            Home = home;
            LineInitialSetting = lineInitialSetting;
        }
    }
}
