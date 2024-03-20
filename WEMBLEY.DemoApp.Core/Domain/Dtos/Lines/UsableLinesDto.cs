using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Lines
{
    public class UsableLinesDto
    {
        public string LineId { get; set; }
        public string LineName { get; set; }
        public ELineType LineType { get; set; }
        public UsableLinesDto(string lineId, string lineName, ELineType lineType)
        {
            LineId = lineId;
            LineName = lineName;
            LineType = lineType;
        }
    }
}
