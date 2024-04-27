namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class DeviceSelectedStore
    {
        public string SeletedDeviceId { get; private set; } = "";
        public string LineId { get; private set; } = "";
        public void SetSeletedDevice(string seletedDeviceId, string lineId)
        {
            SeletedDeviceId = seletedDeviceId;
            LineId = lineId;
        }
    }
}
