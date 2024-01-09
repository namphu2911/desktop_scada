using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Persons
{
    public class PersonWorkingDto
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public PersonWorkingDto(string personId, string personName)
        {
            PersonId = personId;
            PersonName = personName;
        }
    }
}
