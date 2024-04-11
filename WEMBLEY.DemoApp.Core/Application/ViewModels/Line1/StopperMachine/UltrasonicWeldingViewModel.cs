namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine
{
    public class UltrasonicWeldingViewModel
    {
        public long? Cycle { get; set; }
        public double? RunTime { get; set; }
        public double? PkPwr { get; set; }
        public double? Energy { get; set; }
        public double? WeldAbs { get; set; }
        public double? WeldCol { get; set;}
        public double? TotalCol { get; set; }
        public long? TrigForce { get; set; }
        public long? WeldForce { get; set; }
        public long? FreqChg { get; set; }
        public long? SetAMPA { get; set; }
        public long? Velocity { get; set; }

        public UltrasonicWeldingViewModel(long? cycle, double? runTime, double? pkPwr, double? energy, double? weldAbs, double? weldCol, double? totalCol, long? trigForce, long? weldForce, long? freqChg, long? setAMPA, long? velocity)
        {
            Cycle = cycle;
            RunTime = runTime;
            PkPwr = pkPwr;
            Energy = energy;
            WeldAbs = weldAbs;
            WeldCol = weldCol;
            TotalCol = totalCol;
            TrigForce = trigForce;
            WeldForce = weldForce;
            FreqChg = freqChg;
            SetAMPA = setAMPA;
            Velocity = velocity;
        }
    }
}
