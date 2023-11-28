using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class InitialHomeNavigationService<TViewModel> : NavigationService<TViewModel> where TViewModel : IViewModel
    {
        public InitialHomeNavigationService(NavigationStore navigationStore, TViewModel createViewModel) : base(navigationStore, createViewModel)
        {
        }
    }
}
