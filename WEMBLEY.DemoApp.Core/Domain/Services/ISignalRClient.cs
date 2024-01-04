using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface ISignalRClient
    {
        event Action<string>? OnTagChanged;
        Task ConnectAsync();
        Task<List<TagChangedNotification>> GetBufferList();
        Task<TagChangedNotification> GetBuffer(string tagId);
        Task<object?> GetBufferValue(string tagId);
        bool GetState();
    }
}
