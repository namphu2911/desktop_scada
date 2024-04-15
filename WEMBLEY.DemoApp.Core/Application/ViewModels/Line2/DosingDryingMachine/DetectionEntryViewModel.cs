using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DetectionEntryViewModel
    {
        public int? Number { get; set; }
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
        public DetectionEntryViewModel(int? number, long? a, long? b, long? c, long? d, long? e, long? f, long? g, long? h, long? j, long? k)
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
        }   
        
    }
}
