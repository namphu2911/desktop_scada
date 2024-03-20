namespace WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences
{
    public class MFCDto
    {
        public string MFCName { get; set; }
        public double Value { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public MFCDto(string mFCName, double value, double minValue, double maxValue)
        {
            MFCName = mFCName;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
