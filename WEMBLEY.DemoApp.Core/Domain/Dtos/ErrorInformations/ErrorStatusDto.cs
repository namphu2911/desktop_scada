using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations
{
    public class ErrorStatusDto
    {
        public string ErrorId { get; set; }
        public string ErrorName { get; set; }
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public ErrorStatusDto(string errorId, string errorName, DateTime date, int shiftNumber, DateTime timestamp)
        {
            ErrorId = errorId;
            ErrorName = errorName;
            Date = date;
            ShiftNumber = shiftNumber;
            Timestamp = timestamp;
        }
    }
}
