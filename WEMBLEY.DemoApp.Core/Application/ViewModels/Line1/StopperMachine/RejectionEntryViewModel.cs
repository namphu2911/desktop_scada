namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine
{
    public class RejectionEntryViewModel
    {
        public string StationName { get; set; }
        public long? Track1Value { get; set; }
        public long? Track2Value { get; set; }
        public long? Track3Value { get; set; }
        public long? Track4Value { get; set; }
        public RejectionEntryViewModel(string stationName, long? track1Value, long? track2Value, long? track3Value, long? track4Value)
        {
            StationName = stationName;
            Track1Value = track1Value;
            Track2Value = track2Value;
            Track3Value = track3Value;
            Track4Value = track4Value;
        }
    }
}
