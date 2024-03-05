using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared
{
    public class OEEEntryViewModel
    {
        public double? OEE { get; set; }
        public double? A { get; set; }
        public double? P { get; set; }
        public double? Q { get; set; }
        public OEEEntryViewModel(double? oEE, double? a, double? p, double? q)
        {
            OEE = oEE;
            A = a;
            P = p;
            Q = q;
        }
    }
}
