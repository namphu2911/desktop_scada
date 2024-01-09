using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Persons
{
    public class PersonDto
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public List<PersonWorkRecordDto> WorkRecords { get; set; }
        public PersonDto(string personId, string personName, List<PersonWorkRecordDto> workRecords)
        {
            PersonId = personId;
            PersonName = personName;
            WorkRecords = workRecords;
        }
    }
}
