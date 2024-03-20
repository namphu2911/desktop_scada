namespace WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences
{
    public class DeviceReferenceDto
    {
        public string StationId { get; set; }
        public List<MFCDto> MFCs { get; set; }
        public DeviceReferenceDto(string stationId, List<MFCDto> mFCs)
        {
            StationId = stationId;
            MFCs = mFCs;
        }
    }
}
