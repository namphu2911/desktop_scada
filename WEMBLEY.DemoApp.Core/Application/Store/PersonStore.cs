using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Persons;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class PersonStore
    {
        public List<PersonDto> Persons { get; private set; }
        public ObservableCollection<string> PersonIds { get; private set; }
        public ObservableCollection<string> PersonNames { get; private set; }
        public PersonStore()
        {
            Persons = new List<PersonDto>();
            PersonIds = new ObservableCollection<string>();
            PersonNames = new ObservableCollection<string>();
        }

        public void SetPerson(IEnumerable<PersonDto> persons)
        {
            Persons = persons.ToList();
            PersonIds = new ObservableCollection<string>(Persons.Select(i => i.PersonId).OrderBy(s => s));
            PersonNames = new ObservableCollection<string>(Persons.Select(i => i.PersonName).OrderBy(s => s));
        }
    }
}
