using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DetectionEntryViewModel
    {
        public string? Number { get; set; }
        public long? A { get; set; }
        public long? B { get; set; }
        public long? C { get; set; }
        public long? D { get; set; }
        public long? E { get; set; }
        public long? F { get; set; }
        public long? G { get; set; }
        public long? H { get; set; }
        public long? J { get; set; }
        public long? K { get; set; }
        public long? Min { get; set; }
        public long? Max { get; set; }
        public bool IsAlarmedA => ((A >= Min) && (A <= Max));
        public bool IsAlarmedB => ((B >= Min) && (B <= Max));
        public bool IsAlarmedC => ((C >= Min) && (C <= Max));
        public bool IsAlarmedD => ((D >= Min) && (D <= Max));
        public bool IsAlarmedE => ((E >= Min) && (E <= Max));
        public bool IsAlarmedF => ((F >= Min) && (F <= Max));
        public bool IsAlarmedG => ((G >= Min) && (G <= Max));
        public bool IsAlarmedH => ((H >= Min) && (H <= Max));
        public bool IsAlarmedJ => ((J >= Min) && (J <= Max));
        public bool IsAlarmedK => ((K >= Min) && (K <= Max));
        public DetectionEntryViewModel(string? number, long? a, long? b, long? c, long? d, long? e, long? f, long? g, long? h, long? j, long? k, long? min, long? max)
        {
            Number = number;
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
            J = j;
            K = k;
            Min = min;
            Max = max;
        }

    }
}
