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
                .WithUrl("https://wembleymedicalscada.azurewebsites.net/NotificationHub")
                .WithAutomaticReconnect()
                .Build();
        }
        public async Task ConnectAsync()
        {
            connection.On<string>("OnTagChanged", (json) => OnTagChanged?.Invoke(json));
            await connection.StartAsync();
            var a = connection.State;
            await connection.InvokeAsync("UpdateTopics", new List<string>() { "WembleyMedical/HCM/IE-F2-HCA01/Desktop",
                                                                              "WembleyMedical/BTM/IE-F3-BLO06/Desktop",
                                                                              "WembleyMedical/BTM/IE-F3-BLO01/Desktop",
                                                                              "WembleyMedical/BTM/IE-F3-BLO02/Desktop"});
        }

        public async Task<List<TagChangedNotification>> GetBufferList()
        {
            var respone = await connection.InvokeAsync<string>("SendAll");
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

        public async Task<object?> GetBufferValue(string tagId)
        {
            string respone = await connection.InvokeAsync<string>("SendAll");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }

            var tag = tags.LastOrDefault(i => i.TagId == tagId);
            if(tag is not null)
            {
                return tag.TagValue;
            }
            else return null;
        }

        public async Task<object?> GetBufferValue(string stationId, string tagId)
        {
            string respone = await connection.InvokeAsync<string>("SendAll");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }

            var tag = tags.LastOrDefault(i => ((i.StationId == stationId) && (i.TagId == tagId)));
            if (tag is not null)
            {
                return tag.TagValue;
            }
            else return null;
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
