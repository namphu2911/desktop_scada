using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public class LineReference
    {
        public string LineId { get; set; }
        public string ReferenceId { get; set; }
        public LineReference(string lineId, string referenceId)
        {
            LineId = lineId;
            ReferenceId = referenceId;
        }
    }
}
