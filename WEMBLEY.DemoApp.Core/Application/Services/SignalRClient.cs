using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class SignalRClient : ISignalRClient
    {
        private HubConnection connection;
        public event Action<string>? OnTagChanged;
        public SignalRClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://wembleyscadaapi.azurewebsites.net/notificationHub")
                .WithAutomaticReconnect()
                .Build();
        }
        public async Task ConnectAsync()
        {
            connection.On<string>("TagChanged", (json) => OnTagChanged?.Invoke(json));
            await connection.StartAsync();
            var a = connection.State;
        }

        public async Task<List<TagChangedNotification>> GetBufferList()
        {
            string respone = await connection.InvokeAsync<string>("SendAll");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                return new List<TagChangedNotification>();
            }
            return tags;
        }

        public async Task<TagChangedNotification> GetBuffer(string tagId)
        {
            string respone = await connection.InvokeAsync<string>("SendAll");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            return tags.Last(i => i.TagId == tagId);
        }

        public async Task<object> GetBufferValue(string tagId)
        {
            string respone = await connection.InvokeAsync<string>("SendAll");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            return tags.Last(i => i.TagId == tagId).TagValue;
        }

        public bool GetState()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                return true;
            }
            else return false;
        }
    }
}
