using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        public string DeviceId { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public bool IsButtonEnable => !(PersonId == "");
        public ICommand DeletePersonCommand { get; set; }
        public event EventHandler? OnRemoved;
        public DeviceInfoViewModel(string deviceId, string personId, string personName)
        {
            DeviceId = deviceId;
            PersonId = personId;
            PersonName = personName;

            DeletePersonCommand = new RelayCommand(DeletePerson);
        }

        private void DeletePerson()
        {
            OnRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
