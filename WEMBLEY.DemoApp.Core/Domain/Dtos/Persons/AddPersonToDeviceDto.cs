using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Persons
{
    public class AddPersonToDeviceDto
    {
        public List<string> PersonIds { get; set; }
        public AddPersonToDeviceDto(List<string> personIds)
        {
            PersonIds = personIds;
        }
    }
}
