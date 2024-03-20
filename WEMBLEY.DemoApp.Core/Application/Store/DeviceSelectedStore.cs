namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class DeviceSelectedStore
    {
        public string SeletedDeviceId { get; private set; } = "";
        public void SetSeletedDevice(string seletedDeviceId)
        {
            SeletedDeviceId = seletedDeviceId;
        }
    }
}
