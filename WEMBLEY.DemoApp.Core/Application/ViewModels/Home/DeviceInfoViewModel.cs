using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        public string DeviceId { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public Visibility SmallLotVis { get; set; }
        public ICommand DeletePersonCommand { get; set; }
        public event EventHandler? OnRemoved;
        public DeviceInfoViewModel(string deviceId, string personId, string personName, Visibility smallLotVis)
        {
            DeviceId = deviceId;
            PersonId = personId;
            PersonName = personName;
            SmallLotVis = smallLotVis;


            DeletePersonCommand = new RelayCommand(DeletePerson);
        }

        private void DeletePerson()
        {
            OnRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
