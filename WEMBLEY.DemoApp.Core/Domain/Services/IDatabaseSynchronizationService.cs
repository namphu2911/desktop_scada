using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IDatabaseSynchronizationService
    {
        Task SynchronizeReferencesData();
        Task SynchronizeDevicesData();
        Task SynchronizeHomeData();
        Task SynchronizePersonsData();
    }
}
