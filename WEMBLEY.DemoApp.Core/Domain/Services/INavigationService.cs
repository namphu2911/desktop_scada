using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface INavigationService
    {
        public BaseViewModel CurrentViewModel { get; }

        void NavigateTo<T>() where T : BaseViewModel;
    }
}
