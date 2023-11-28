using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class NavigationService<TViewModel> : INavigationService<TViewModel> where TViewModel : IViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly TViewModel _createViewModel;

        public NavigationService(NavigationStore navigationStore, TViewModel createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel;
        }
    }
}
