using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public class HerapinCapMFC
    {
        public double? S8OffsetTR1 { get; set; }
        public double? S8MinHeightTR1 { get; set; }
        public double? S8MaxHeightTR1 { get; set; }

        public double? S8OffsetTR3 { get; set; }
        public double? S8MinHeightTR3 { get; set; }
        public double? S8MaxHeightTR3 { get; set; }

        public double? S9OffsetTR2 { get; set; }
        public double? S9MinHeightTR2 { get; set; }
        public double? S9MaxHeightTR2 { get; set; }

        public double? S9OffsetTR4 { get; set; }
        public double? S9MinHeightTR4 { get; set; }
        public double? S9MaxHeightTR4 { get; set; }

        public HerapinCapMFC(double? s8OffsetTR1, double? s8MinHeightTR1, double? s8MaxHeightTR1, double? s8OffsetTR3, double? s8MinHeightTR3, double? s8MaxHeightTR3, double? s9OffsetTR2, double? s9MinHeightTR2, double? s9MaxHeightTR2, double? s9OffsetTR4, double? s9MinHeightTR4, double? s9MaxHeightTR4)
        {
            S8OffsetTR1 = s8OffsetTR1;
            S8MinHeightTR1 = s8MinHeightTR1;
            S8MaxHeightTR1 = s8MaxHeightTR1;
            S8OffsetTR3 = s8OffsetTR3;
            S8MinHeightTR3 = s8MinHeightTR3;
            S8MaxHeightTR3 = s8MaxHeightTR3;
            S9OffsetTR2 = s9OffsetTR2;
            S9MinHeightTR2 = s9MinHeightTR2;
            S9MaxHeightTR2 = s9MaxHeightTR2;
            S9OffsetTR4 = s9OffsetTR4;
            S9MinHeightTR4 = s9MinHeightTR4;
            S9MaxHeightTR4 = s9MaxHeightTR4;
        }
    }
}
