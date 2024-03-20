namespace WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences
{
    public class ComparedMFC
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double? RealValue { get; set; }
        public bool IsAlarmed => !((RealValue >= MinValue) && (RealValue <= MaxValue));
        public ComparedMFC(string name, double value, double minValue, double maxValue, double? realValue)
        {
            Name = name;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            RealValue = realValue;
        }
    }
}
